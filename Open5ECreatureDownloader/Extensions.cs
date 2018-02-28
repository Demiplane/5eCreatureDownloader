using System.Collections.Generic;
using System.Linq;

namespace Open5ECreatureDownloader
{
    public static class Extensions
    {
        public static Creature DownloadCreature(this ICreatureDownloader downloader, string uri) =>
            downloader.DownloadCreatures(uri).FirstOrDefault();

        public static IEnumerable<Creature> DownloadCreatures(this ICreatureDownloader downloader) => 
            downloader.DownloadCreatures(downloader.ListMonsters().Values.ToArray());
    }
}
