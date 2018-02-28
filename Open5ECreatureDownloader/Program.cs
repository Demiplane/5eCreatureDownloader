using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Open5ECreatureDownloader
{
    internal static class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    ListOperations();
                }

                var operations = new[]
                {
                    new { Operation = "downloadall", Method = new Action<string[], CreatureDownloader>(DownloadAll) },
                    new { Operation = "download", Method = new Action<string[], CreatureDownloader>(Download) },
                    new { Operation = "list", Method = new Action<string[], CreatureDownloader>(ListAll) },
                };

                var creatureDownloader = new CreatureDownloader();

                operations
                    .FirstOrDefault(o => string.Equals(o.Operation, args[0], StringComparison.OrdinalIgnoreCase))
                    ?.Method(args.Skip(1).ToArray(), creatureDownloader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ListOperations()
        {
            Console.WriteLine(@"
downloadall [filepath]
download [filepath] [uriOrName] ...
list");
        }

        private static void DownloadAll(string[] args, CreatureDownloader creatureDownloader)
        {
            if (args.Length == 0)
            {
                ListOperations();
                return;
            }

            var file = args[0];

            var creatures = creatureDownloader.DownloadCreatures();
            SaveToFiile(file, creatures);
        }

        private static void SaveToFiile<T>(string file, T objectToSave)
        {
            var json = JsonConvert.SerializeObject(objectToSave);
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            File.WriteAllText(file, json);
            Console.WriteLine(file);
        }

        private static void Download(string[] args, CreatureDownloader creatureDownloader)
        {
            if (args.Length < 2)
            {
                ListOperations();
                return;
            }

            var file = args[0];

            var monsters = creatureDownloader.ListMonsters();

            var urisOrNames = args
                .Skip(1)
                .Select(uriOrName =>
                    Uri.IsWellFormedUriString(uriOrName, UriKind.Absolute) ? 
                    uriOrName : 
                    monsters.FirstOrDefault(monster => string.Equals(uriOrName, monster.Key, StringComparison.OrdinalIgnoreCase)).Value
                    ?? throw new ArgumentException("Invalid monster name or uri."));

            var creatures = creatureDownloader.DownloadCreatures(urisOrNames.ToArray());
            SaveToFiile(file, creatures);
        }

        private static void ListAll(string[] args, CreatureDownloader creatureDownloader)
        {
            creatureDownloader.ListMonsters().ToList().ForEach(f => Console.WriteLine($"Name: {f.Key}, Uri: {f.Value}"));
        }
    }
}
