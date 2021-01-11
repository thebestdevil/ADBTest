using Neo4j.Driver;
using System;
using System.Linq;

namespace ConsoleApp1
{

    class Program
    {        
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            using (var greeter = new HelloWorldExample("neo4j://localhost:7687", "asd", "1234"))
            {
                greeter.PrintGreeting("hello, world");
            }
        }

        public class DriverLifecycleExample : IDisposable
        {
            public IDriver Driver { get; }

            public DriverLifecycleExample(string uri, string user, string password)
            {
                Driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            }

            public void Dispose()
            {
                Driver?.Dispose();
            }
        }




        public class HelloWorldExample : IDisposable
        {
            private readonly IDriver _driver;

            public HelloWorldExample(string uri, string user, string password)
            {
                _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
            }

            public void PrintGreeting(string message)
            {
                using (var session = _driver.Session())
                {
                    var greeting = session.WriteTransaction(tx =>
                    {
                        var result = tx.Run("MATCH (n:Movie) RETURN n LIMIT 1",
                            new { message });
                        return result.Id.As<string>();
                    });
                    Console.WriteLine(greeting);
                }
            }

            public void Dispose()
            {
                _driver?.Dispose();
            }
        }


    }

   


}
