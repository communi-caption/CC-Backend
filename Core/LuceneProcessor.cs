using CommunicaptionBackend.Wrappers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Newtonsoft.Json;
using Lucene.Net.Search.Spell;
using System.IO;
using System;
using System.Collections.Generic;
using CommunicaptionBackend.Entities;
using System.Text;
using System.Globalization;

namespace CommunicaptionBackend.Core
{
    public class LuceneProcessor
    {
        private StandardAnalyzer analyzer;
        private IndexWriterConfig indexConfig;
        private IndexWriter writer;
        private IndexReader reader;
        private FSDirectory dir;
        private SpellChecker spellChecker;

        [Obsolete]
        public LuceneProcessor()
        {
            // Ensures index backwards compatibility
            var AppLuceneVersion = LuceneVersion.LUCENE_48;

            //var indexLocation = @"C:\Users\catal\Desktop\try";
            System.IO.Directory.CreateDirectory("index");
            var path = System.IO.Directory.GetCurrentDirectory() + @"/index";
            dir = FSDirectory.Open(path);
            IndexWriter.Unlock(dir);

            //create an analyzer to process the text
            analyzer = new StandardAnalyzer(AppLuceneVersion);

            //create an index writer
            indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            writer = new IndexWriter(dir, indexConfig);
            //spellChecker = new SpellChecker(dir);
        }

        public List<SearchArtTextRequest> getArtList(List<TextEntity> textEntityList)
        {
            List<SearchArtTextRequest> newArtTextList = new List<SearchArtTextRequest>();
            foreach (var item in textEntityList)
            {
                newArtTextList.Add(new SearchArtTextRequest()
                {
                    ArtId = item.ArtId.ToString(),
                    Text = item.Text
                });
            }
            return newArtTextList;
        }

        [Obsolete]
        public void AddToTheIndex(List<SearchArtTextRequest> artObject)
        {
            foreach(var obj in artObject)
            {
                Document doc = new Document
                {
                // StringField indexes but doesn't tokenize
                new StringField("artId",
                    obj.ArtId,
                    Field.Store.YES),
                new TextField("text",
                    obj.Text,
                    Field.Store.YES)
                };

                writer.AddDocument(doc);
            }
            
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public string FetchResults(string json)
        {
            List<SearchArtTextRequest> resultList = new List<SearchArtTextRequest>();

            var searchRequest = JsonConvert.DeserializeObject<SearchRequest>(json);
            var keyword = searchRequest.keyword;

            // search with a phrase
            var phrase = new MultiPhraseQuery();
            phrase.Add(new Term("text", keyword));

            // re-use the writer to get real-time updates
            var searcher = new IndexSearcher(writer.GetReader(applyAllDeletes: true));
            var hits = searcher.Search(phrase, 20 /* top 20 */).ScoreDocs;
            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);
                resultList.Add(new SearchArtTextRequest { 
                    ArtId = foundDoc.Get("artId"), Text = HighlightText(searchRequest.keyword, foundDoc.Get("text"))
                });     
            }
            return JsonConvert.SerializeObject(resultList);
        }

        private static string HighlightText(string keyword, string text) {
            keyword = keyword.Trim();
            if (keyword.Length < 2)
                return text;

            if (text.Length > 50) {
                int index = text.IndexOf(keyword);
                index = Math.Max(index - 10, 0);
                text = text.Substring(index);
                int length = Math.Min(text.Length, 50);
                text = text.Substring(0, length);
            }

            var lowerKey = keyword.ToLowerInvariant();
            var upperKey = keyword.ToUpperInvariant();
            var titleKey = new CultureInfo("en-US").TextInfo.ToTitleCase(keyword);

            text = text.Replace(lowerKey, $"<color=yellow>{lowerKey}</color>");
            text = text.Replace(upperKey, $"<color=yellow>{upperKey}</color>");
            text = text.Replace(titleKey, $"<color=yellow>{titleKey}</color>");

            return text;
        }

        public string suggestSimilar(string query)
        {
            reader = writer.GetReader(true);
            spellChecker.IndexDictionary(new LuceneDictionary(reader, "keyword"), indexConfig, true);
            // To index a file containing words:
            string[] suggestions = spellChecker.SuggestSimilar(query, 5);
          
            return suggestions[0];
        }
    }
}
