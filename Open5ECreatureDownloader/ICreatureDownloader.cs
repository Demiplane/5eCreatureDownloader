using System.Collections.Generic;

namespace Open5ECreatureDownloader
{
    public interface ICreatureDownloader
    {
        IEnumerable<Creature> DownloadCreatures(params string[] monsterUris);
        IDictionary<string, string> ListMonsters();
    }
}