using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTableStoragePSR
{
    class Program
    {
        public static async Task Main(string[] args)
        {


            CloudTable table = await CreateTableAsync("Teams");
            int quit = 0;
            while (quit == 0)
            {

                Console.WriteLine("wybierz akcje");
                Console.WriteLine("-------------------------");
                Console.WriteLine("1 - Wyswietl wszystko");
                Console.WriteLine("2 - wyswietl druzynę");
                Console.WriteLine("3 - Dodaj");
                Console.WriteLine("4 - edytuj");
                Console.WriteLine("5 - usun");
                Console.WriteLine("6 - liczba druzyn");
                Console.WriteLine("7 - Quit");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":

                        Console.WriteLine("wszyscy: ");
                        IQueryable<Team> teams = table.CreateQuery<Team>().Where(x => x.PartitionKey == "Teams").Select(x => new Team
                        {
                            RowKey = x.RowKey,
                            Name = x.Name,
                            Wins = x.Wins,
                            Draws = x.Draws,
                            Looses = x.Looses
                        });
                        foreach (var e in teams)
                        {
                            Console.WriteLine(e.RowKey + " => " + e.ToString());
                        }
                        break;
                    case "2":
                        Console.WriteLine("podaj nazwe drużyny");
                        string name23 = Console.ReadLine();
                        List<Team> teams2 = table.CreateQuery<Team>().Where(x => x.PartitionKey == "Teams" && x.Name == name23).Select(x => new Team
                        {
                            RowKey = x.RowKey,
                            Name = x.Name,
                            Wins = x.Wins,
                            Draws = x.Draws,
                            Looses = x.Looses
                        }).ToList();
                        foreach (var e in teams2)
                        {
                            Console.WriteLine(e.RowKey + " => " + e.ToString());
                        }
                        
                        break;
                    case "3":
                        Guid key1 = Guid.NewGuid();
                        Console.WriteLine("podaj nazwe klubu ");
                        string name = Console.ReadLine();
                        Console.WriteLine("wygrane:");
                        int wins = int.Parse(Console.ReadLine());
                        Console.WriteLine("remisy:");
                        int draws = int.Parse(Console.ReadLine());
                        Console.WriteLine("przegrane");
                        int looses = int.Parse(Console.ReadLine());
                        Team Team1 = new Team()
                        {
                            RowKey = key1.ToString(),
                            Name = name,
                            Wins = wins,
                            Draws = draws,
                            Looses = looses
                        };
                        Console.WriteLine("PUT " + key1 + " => " + Team1);

                        TableOperation insertOperation = TableOperation.InsertOrMerge(Team1);
                        await table.ExecuteAsync(insertOperation);

                        break;
                    case "4":
                        Console.WriteLine("wpisz id do edycji");
                        string idToEdit = Console.ReadLine();
                        Team objectToEdit = table.CreateQuery<Team>().Where(x => x.RowKey == idToEdit).FirstOrDefault();
                        int endEdit = 0;
                        while (endEdit == 0)
                        {

                            Console.WriteLine("co chcesz edytować");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("1 - nazwa");
                            Console.WriteLine("2 - wygrane");
                            Console.WriteLine("3 - remisy");
                            Console.WriteLine("4 - przegrane");
                            Console.WriteLine("5 - zakończ edycje");

                            string choice2 = Console.ReadLine();

                            switch (choice2)
                            {
                                case "1":

                                    Console.WriteLine("podaj nowe imie");

                                    objectToEdit.Name = Console.ReadLine();
                                    break;
                                case "2":
                                    Console.WriteLine("wygrane");
                                    objectToEdit.Wins = int.Parse(Console.ReadLine());
                                    break;
                                case "3":
                                    Console.WriteLine("remisy");
                                    objectToEdit.Draws = int.Parse(Console.ReadLine());
                                    break;
                                case "4":
                                    Console.WriteLine("przegrane");
                                    objectToEdit.Looses =  int.Parse(Console.ReadLine());
                                    break;
                                case "5":
                                    endEdit = 1;
                                    break;
                                default:
                                    Console.WriteLine("zły numer");
                                    break;
                            }

                        }

                        TableOperation editOperation = TableOperation.InsertOrReplace(objectToEdit);
                        await table.ExecuteAsync(editOperation);

                        break;
                    case "5":

                        Console.WriteLine("wpisz id do usuniecia");
                        string id1 = Console.ReadLine();

                        Team objectTodelete = table.CreateQuery<Team>().Where(x => x.RowKey == id1).FirstOrDefault();
                        TableOperation deleteOperation = TableOperation.Delete(objectTodelete);
                        TableResult deleteResult = await table.ExecuteAsync(deleteOperation);
                        break;
                    case "6":
                        List<Team> teams5 = table.CreateQuery<Team>().Where(x => x.PartitionKey == "Teams").Select(x => new Team
                        {
                            RowKey = x.RowKey,
                            Name = x.Name,
                            Wins = x.Wins,
                            Draws = x.Draws,
                            Looses = x.Looses
                        }).ToList();
                        Console.WriteLine($"liczba druzyn: {teams5.Count}");
                        break;
                    case "7":
                        quit = 1;
                        Console.WriteLine("Wyjscie");
                        break;
                    default:
                        Console.WriteLine("zły numer");
                        break;
                }
            }
            return;

        }



        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            string storageConnectionString = AppSettings.LoadAppSettings().StorageConnectionString;

            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(storageConnectionString);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            Console.WriteLine("Create a Table for the demo");
            CloudTable table = tblclient.GetTableReference(tableName);
            if (await table.CreateIfNotExistsAsync())
            {
                Console.WriteLine("Created Table named: {0}", tableName);
            }
            else
            {
                Console.WriteLine("Table {0} already exists", tableName);
            }

            Console.WriteLine();
            return table;
        }
    }
}
