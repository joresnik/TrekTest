using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrekTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://trekhiringassignments.blob.core.windows.net/interview/bikes.json";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(url).Result;

            var result = response.Content.ReadAsStringAsync().Result;
            var bike = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(result);

            List<Bikes> BikesList = new List<Bikes>();
            
            foreach (var family in bike)
            {
                int cnt = 0;
                foreach (var famBikes in family["bikes"])
                {
                    string name = famBikes;


                    if (BikesList.Any(Bikes => Bikes.name == name))
                    {
                        BikesList.Find(Bikes => Bikes.name == name).count += 1;

                    }
                    else
                    {
                        Bikes bikes = new Bikes();
                        bikes.name = name;
                        bikes.count = 1;
                        BikesList.Add(bikes);

                    }
                }
            } 
            
            BikesList.Sort(delegate (Bikes x, Bikes y) { return y.count.CompareTo(x.count); });
            for (int cnt = 1; cnt < 21; cnt++)
            {
                Console.WriteLine("#" + cnt.ToString() + ": " + BikesList[cnt].name + ", Count: " + BikesList[cnt].count);
            }

            Console.ReadLine();
        }
    }
}
