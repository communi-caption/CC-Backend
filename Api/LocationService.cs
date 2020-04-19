using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommunicaptionBackend.Api
{
    public class LocationService
    {
        private MainContext mainContext;

        public LocationService(MainContext mainContext)
        {
            this.mainContext = mainContext;
        }

        public int[] LocationBasedRecommendation(float latitude, float longitude)
        {
            var info = mainContext.Arts.Select(x => new {x.Id, x.Latitude, x.Longitude}).ToList();
            List<double[]> sortedList = new List<double[]>();

            for(int i = 0; i < info.Count; i++)
            {
                double distance = Math.Pow(latitude - info[i].Latitude, 2) + Math.Pow(longitude - info[i].Longitude, 2);
                sortedList.Add(new double[]{info[i].Id,distance});
            }

            sortedList.OrderBy(x => x[1]);
            return sortedList.Take(5).Select(x => Convert.ToInt32(x[0])).ToArray();
        }
    }
}
