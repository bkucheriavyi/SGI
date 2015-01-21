using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SGI.Core;
using SGI.Core.GameInfo;

namespace ConsoleApplication2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var gameInfoList = GetGameInfoFromFiles();
            var steamApps = GetSteamAppsList().ToList();

            Console.WriteLine("Steam apps cout : {0}", steamApps.Count);
            var gameInfoNameList = new HashSet<string>(gameInfoList.Select(x => x.Name));
            var filtered = steamApps.Where(x => gameInfoNameList.Contains(x.Name)).ToList();
            var filteredNames = new HashSet<string>(filtered.Select(x => x.Name));

            Console.WriteLine("Found: {0}", filteredNames.Count());
            foreach (var f in filteredNames)
                Console.WriteLine(f);
            Console.WriteLine("\nUNFOUND: ----------------------------------");
            var unfound = gameInfoNameList.Where(x => !filteredNames.Contains(x));
            foreach (var f in unfound)
                Console.WriteLine(f);
            var prices1 =
                GetPricesV2(steamApps.Take(100).Select(f => f.AppId.ToString()))
                    .Where(t => t.Currency != null)
                    .ToList();
            //var prices2 =
            //    GetPriceOverViews(steamApps.Take(1000).Select(f => f.AppId.ToString()))
            //        .Where(t => t.Currency != null)
            //        .ToList();
           
            Console.ReadLine();
        }

        private static IEnumerable<PriceOverview> GetPriceOverViews(IEnumerable<string> appsId)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var steamClient = new SteamClient();
            var priceOverviews = steamClient.GetPriceOverviewAsync(appsId);

            stopWatch.Stop();
            Console.WriteLine("Requests sent: {0}, Time elapsed: {1} milliseconds", appsId.Count(),
                stopWatch.ElapsedMilliseconds);
            Console.WriteLine("Avg. time spent per request: {0}", stopWatch.ElapsedMilliseconds/appsId.Count());
            return priceOverviews;
        }

        private static IEnumerable<GameInfo> GetGameInfoFromFiles()
        {
            var fileOne =
                System.IO.File.ReadAllLines(
                    @"C:\Users\Bohdan\AppData\Roaming\Skype\My Skype Received Files\steam-games.txt");
            var fileTwo =
                System.IO.File.ReadAllLines(
                    @"C:\Users\Bohdan\AppData\Roaming\Skype\My Skype Received Files\UWrKVVCa4KTyfFQ.txt");
            var merge = fileOne.Union(fileTwo).ToArray();

            Console.WriteLine("Lines in file 1:{0}", fileOne.Length);
            Console.WriteLine("Lines in file 2:{0}", fileTwo.Length);
            Console.WriteLine("Lines in merge file:{0}", merge.Length);
            var mapper = new GameInfoMapper();
            return merge.Select(mapper.TextToEntity);
        }

        private static IEnumerable<Apps> GetSteamAppsList()
        {
            var steamClient = new SteamClient();
            var steamApplications = steamClient.GetCurrentApplicationList();
            return steamApplications.AppList.Apps;
        }

        private static IEnumerable<PriceOverview> GetPricesV2(IEnumerable<string> appIds)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var steamClient = new SteamClient();
            var results = new List<PriceOverview>();
            var enumerable = appIds as string[] ?? appIds.ToArray();
            enumerable.ForEachAsync(s => steamClient.GetPriceOverview(s, "ru"), (url, t) => results.Add(t));
            stopWatch.Stop();
            Console.WriteLine("Requests sent: {0}, Time elapsed: {1} milliseconds", enumerable.Count(),
                stopWatch.ElapsedMilliseconds);
            Console.WriteLine("Avg. time spent per request: {0}", stopWatch.ElapsedMilliseconds / enumerable.Count());
            return results;
        }
    }

}
