using System;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace client
{
    class Program
    {
        static HttpClient client;
        static List<int> ids = new List<int>();

        static async Task<bool> MakeRequestAsync(string url)
        {
            try
            {
                using (var response = await client.GetAsync(url))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine($"{response.StatusCode}: {response.ReasonPhrase}");
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }

        static async Task RunTestAsync(string url, int count)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var gets = ids.Take(count).Select(id => MakeRequestAsync($"{url}/{id}"));
            var results = await Task.WhenAll(gets);
            var total = results.Count();
            var success = results.Where(x => x).Count();
            var error = results.Where(x => !x).Count();
            Console.WriteLine("Time\tTotal\tSuccess\tError");
            Console.WriteLine($"{stopWatch.ElapsedMilliseconds/1000.0} s\t{total}\t{success}\t{error}");
        }

        static async Task GetIds()
        {
            using (var response = await client.GetAsync("http://localhost:5000/api/anime"))
            {
                ids = JsonConvert.DeserializeObject<List<int>>(await response.Content.ReadAsStringAsync());
            }
        }

        static async Task Main(string[] args)
        {
            var which = args[0];
            var count = Int32.Parse(args[1]);
            using (client = new HttpClient())
            {
                await GetIds();
                Console.WriteLine(which);
                var url = $"http://localhost:5000/api/{(which == "Sync" ? "anime" : "anime/async")}";
                await RunTestAsync(url, count);
            }
        }
    }
}
