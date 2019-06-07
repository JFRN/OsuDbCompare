using System;
using System.Collections.Generic;
using System.Linq;
using osu_database_reader.BinaryFiles;
using osu_database_reader.Components.Beatmaps;

namespace OsuDbCompare
{
    /// <summary>This program compares two osu! database files to get the list of mapset IDs the first databsae doesn't have</summary>
    class Program
    {
        static void Main(string[] args)
        {
            OsuDb db1 = OsuDb.Read(@"D:\Dev\OsuDbCompare\testdbs\osu!.db");
            OsuDb db2 = OsuDb.Read(@"D:\Dev\OsuDbCompare\testdbs\osu!2.db");

            PrintDbInfo(db1);
            PrintDbInfo(db2);

            var db1Mapsets = ConvertBeatmapListToMapsetSet(db1.Beatmaps);
            var db2Mapsets = ConvertBeatmapListToMapsetSet(db2.Beatmaps);
            var difference = DiffMapsetSets(db1Mapsets, db2Mapsets);

            Console.WriteLine("The first database has all of the second database mapsets except : ");
            Console.WriteLine("Diff count: " + difference.Count);
            Console.WriteLine(String.Join(", ", difference));
        }

        static void PrintDbInfo(OsuDb db) {
            Console.WriteLine($"{db.AccountName}'s osu!.db");
            Console.WriteLine("osu! version: " + db.OsuVersion);
            Console.WriteLine("Beatmap count: " + db.Beatmaps.Count);
            Console.WriteLine($"Account name: \"{db.AccountName}\" (account {(db.AccountUnlocked ? "not locked" : "locked, unlocked at " + db.AccountUnlockDate)})");
            Console.WriteLine("Account rank: " + db.AccountRank);

            //var beatmapSets = convertBeatmapListToMapsetSet(db.Beatmaps);
            //Console.WriteLine("Beatmap set count: " + beatmapSets.Count);
            Console.WriteLine();
        }

        static HashSet<int> ConvertBeatmapListToMapsetSet(List<BeatmapEntry> beatmaps) {
            return beatmaps.Select(b => b.BeatmapSetId).ToHashSet();
        }

        static HashSet<int> DiffMapsetSets(ISet<int> p1Set, ISet<int> p2Set) {
            var diff = p2Set.Except(p1Set);
            return diff.ToHashSet();
        }
    }
}
