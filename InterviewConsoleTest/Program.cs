//using InterviewConsoleTest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Starting application...");

        //Implement three classes, all must implement ILogger interface:
        //1. a FileLogger, which logs a message to a file
        //2. a ConsoleLogger, which logs a message to the consoler
        //3. a CompositeLogger, which accepts a number of ILogger and uses all of them

        //Instantiate a CompositeLogger, which will use a FileLogger and a CompositeLogger

        //LogResult for CompositeLogger is Successful if ALL loggers are successful. ErrorMessage is the concatenation of each ErrorMessage of each logger (if any)

        FileLogger fileLogger = new FileLogger();
        ConsoleLogger consoleLogger = new ConsoleLogger();
        List<ILogger> loggers = new List<ILogger> { fileLogger, consoleLogger };
        CompositeLogger compositeLogger = new CompositeLogger(loggers);
        await compositeLogger.LogAsync("Application started", null);
        Dictionary<string, string> users = new Dictionary<string, string>();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Login page");
            Console.Write("Enter UserID: ");
            string userId = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            if (!users.ContainsKey(userId))
            {
                users[userId] = password;
                Console.WriteLine("New user registered!");
                await Task.Delay(1000);
            }
            if (users.TryGetValue(userId, out string storedPassword) && storedPassword == password)
            {
                await compositeLogger.LogAsync("User logged in", userId);
                bool isLoggedIn = true;
                while (isLoggedIn)
                {
                    Console.Clear();
                    Console.WriteLine("Home Page after login");
                    Console.WriteLine($"Welcome, {userId}!");
                    Console.WriteLine("[1] Logout");
                    Console.WriteLine("[2] Stay Logged In");
                    Console.Write("Enter number: ");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        await compositeLogger.LogAsync("User logged out", userId);
                        Console.WriteLine("Logging out...");
                        await Task.Delay(1000);
                        isLoggedIn = false;
                    }
                    else
                    {
                        Console.WriteLine("You are still logged in. Press Enter to continue...");
                        Console.ReadLine();
                    }
                }
            }
            else
            {
                await compositeLogger.LogAsync("Failed login attempt", userId);
                Console.WriteLine("Invalid login. Press Enter to try again.");
                Console.ReadLine();
            }
        }
    }
}
