using Cassandra;
using Cassandra.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CassandraPSW
{
    class Program
    {
        static void Main(string[] args)
        {

            ICluster cluster = Cluster.Builder()
            .AddContactPoint("127.0.0.1")
            .Build();
            
            var session = cluster.Connect("psw");
            string query = "DROP TABLE Teams;";
            //session.Execute(query);
            /*session.Execute("CREATE TABLE Teams(\n" +

                "    id UUID PRIMARY KEY,\n" +
                "    name text,\n" +
                "    wins int,\n" +
                "    draws int,\n" +
                "    loses int,\n" +
                ");");*/
            MappingConfiguration.Global.Define(
           new Map<Team>()
              .TableName("Teams")
              .PartitionKey(u => u.Id)
              .Column(u => u.Id, cm => cm.WithName("id"))
              .Column(u => u.Name, cm => cm.WithName("name")));
            IMapper mapper = new Mapper(session);

            

            int quit = 0;
            while (quit == 0)
            {

                Console.WriteLine("wybierz akcje");
                Console.WriteLine("-------------------------");
                Console.WriteLine("1 - Wyswietl wszystko");
                Console.WriteLine("2 - Wyswietl liczbe druzyn");
                Console.WriteLine("3 - Dodaj");
                Console.WriteLine("4 - edytuj");
                Console.WriteLine("5 - usun");
                Console.WriteLine("6 - wyświetl druzyne");
                Console.WriteLine("7 - Quit");


                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":

                        Console.WriteLine("wszyscy: ");

                        IEnumerable<Team> teams = mapper.Fetch<Team>("SELECT * FROM Teams");
                        foreach (Team team in teams)
                        {


                            Console.WriteLine($"{team.Id} => {team.Name}     zwycięstwa: {team.Wins}, remisy: {team.Draws}, porażki: {team.Loses}");

                        }
                        break;
                    case "2":
                        List<Team> teams23 = mapper.Fetch<Team>("SELECT * FROM Teams").ToList();
                        Console.WriteLine($"liczba zatrzymanych {teams23.Count}");

                        break;
                    case "3":

                        Console.WriteLine("podaj nazwe ");
                        string name = Console.ReadLine();
                        Console.WriteLine("podaj  liczbe zwycięstw");
                        int wins = int.Parse(Console.ReadLine());
                        Console.WriteLine("podaj  liczbe remisów");
                        int draws = int.Parse(Console.ReadLine());

                        Console.WriteLine("podaj  liczbe porażek");
                        int loses = int.Parse(Console.ReadLine());

                        Team teamToAdd = new Team()
                        {
                            Id = Guid.NewGuid(),
                            Name = name,
                            Wins = wins,
                            Draws = draws,
                            Loses = loses,
                            
                        };
                        mapper.Insert(teamToAdd);


                        break;
                    case "4":
                        Console.WriteLine("wpisz id do edycji");
                        string idToEdit = Console.ReadLine();
                        Team objecttToEdit = mapper.FirstOrDefault<Team>(" WHERE id = ? ", Guid.Parse(idToEdit));
                        int endEdit = 0;
                        while (endEdit == 0)
                        {

                            Console.WriteLine("co chcesz edytować");
                            Console.WriteLine("-------------------------");
                            Console.WriteLine("1 - nazwa");
                            Console.WriteLine("2 - zwycięstwa");
                            Console.WriteLine("3 - remisy");
                            Console.WriteLine("4 - porażki");
                            Console.WriteLine("5 - zakończ edycje");

                            string choice2 = Console.ReadLine();

                            switch (choice2)
                            {
                                case "1":

                                    Console.WriteLine("podaj nowa nazwe");
                                    objecttToEdit.Name = Console.ReadLine();


                                    break;
                                case "2":
                                    Console.WriteLine("podaj nową liczbe zwycięstw");
                                    objecttToEdit.Wins = int.Parse(Console.ReadLine());


                                    break;
                                case "3":
                                    Console.WriteLine("podaj nową liczbe remisów");
                                    objecttToEdit.Draws = int.Parse(Console.ReadLine());
                                    
                                    break;
                                case "4":
                                    Console.WriteLine("podaj nową liczbe porażek");
                                    objecttToEdit.Loses = int.Parse(Console.ReadLine());
                                    break;
                                case "5":
                                    endEdit = 1;
                                    break;
                                default:
                                    Console.WriteLine("zły numer");
                                    break;
                            }

                        }
                        mapper.Update(objecttToEdit);

                        break;
                    case "5":

                        Console.WriteLine("wpisz id do usuniecia");
                        string objToDelete = Console.ReadLine();
                        var deletestatement = new SimpleStatement("DELETE FROM teams WHERE id = ? IF EXISTS; ",Guid.Parse(objToDelete));
                        session.Execute(deletestatement);
                        break;
                    case "6":
                        Console.WriteLine("podaj  nazwe drużyny");
                        string name29 = Console.ReadLine();
                        Team objecttToDisplay = mapper.FirstOrDefault<Team>("SELECT * FROM Teams WHERE name = ? ALLOW FILTERING", name29);
                        Console.WriteLine($"{objecttToDisplay.Id} => {objecttToDisplay.Name}     zwycięstwa: {objecttToDisplay.Wins}, remisy: {objecttToDisplay.Draws}, porażki: {objecttToDisplay.Loses}");
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
            //session.Execute(query);

        }
    }
}
