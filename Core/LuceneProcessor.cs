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

namespace CommunicaptionBackend.Core
{
    public class LuceneProcessor
    {
        private StandardAnalyzer analyzer;
        private IndexWriterConfig indexConfig;
        private IndexWriter writer;
        private IndexReader reader;
        private SpellChecker spellChecker;

        public LuceneProcessor()
        {
            // Ensures index backwards compatibility
            var AppLuceneVersion = LuceneVersion.LUCENE_48;

            var indexLocation = @"C:\Index";
            var dir = FSDirectory.Open(indexLocation);

            //create an analyzer to process the text
            analyzer = new StandardAnalyzer(AppLuceneVersion);

            //create an index writer
            indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
            writer = new IndexWriter(dir, indexConfig);
            spellChecker = new SpellChecker(dir);
        }

        public void AddToTheIndex()
        {
            var source = new
            {
                Name = "Kermit the Frog",
                FavoritePhrase = "The quick brown fox jumps over the lazy dog"
            };

            Document doc = new Document
            {
            // StringField indexes but doesn't tokenize
            new StringField("name",
                source.Name,
                Field.Store.YES),
            new TextField("favoritePhrase",
                source.FavoritePhrase,
                Field.Store.YES)
            };

            writer.AddDocument(doc);
            writer.Flush(triggerMerge: false, applyAllDeletes: false);
        }

        public void FetchResults(string json)
        {
            var searchRequest = JsonConvert.DeserializeObject<SearchRequest>(json);
            var keyword = searchRequest.keyword;

            if(searchRequest.spellCheck == true)
            {
                keyword = suggestSimilar(searchRequest.keyword);
            }

            // search with a phrase
            var phrase = new MultiPhraseQuery();
            phrase.Add(new Term("keyword", keyword));

            // re-use the writer to get real-time updates
            var searcher = new IndexSearcher(writer.GetReader(applyAllDeletes: true));
            var hits = searcher.Search(phrase, 20 /* top 20 */).ScoreDocs;
            foreach (var hit in hits)
            {
                var foundDoc = searcher.Doc(hit.Doc);

            }

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
