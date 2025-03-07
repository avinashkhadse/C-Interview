using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public interface ILogger
{
    Task<LogResult> LogAsync(string message, string userId);
}
public class FileLogger : ILogger
{
    private readonly string file;
    public FileLogger()
    {
        string solutionDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
        file = Path.Combine(solutionDirectory, "loggerGenerated.txt");
    }
    public async Task<LogResult> LogAsync(string message, string userId)
    {
        try
        {
            string logMessage = $"{DateTime.Now} | UserID: {userId ?? "Unknown"} | {message}{Environment.NewLine}";
            using (StreamWriter writer = new StreamWriter(file, true, Encoding.UTF8))
            {
                await writer.WriteAsync(logMessage);
            }
            return new LogResult(true, null);
        }
        catch (Exception ex)
        {
            return new LogResult(false, ex.Message);
        }
    }
}
public class ConsoleLogger : ILogger
{
    public async Task<LogResult> LogAsync(string message, string userId)
    {
        return await Task.Run(() =>
        {
            try
            {
                Console.WriteLine($"{DateTime.Now} | UserID: {userId ?? "Unknown"} | {message}");
                return new LogResult(true, null);
            }
            catch (Exception ex)
            {
                return new LogResult(false, ex.Message);
            }
        });
    }
}
public class CompositeLogger : ILogger
{
    private readonly List<ILogger> _loggers;
    public CompositeLogger(List<ILogger> loggers)
    {
        _loggers = loggers;
    }
    public async Task<LogResult> LogAsync(string message, string userId)
    {
        List<Task<LogResult>> logTasks = new List<Task<LogResult>>();
        foreach (var logger in _loggers)
        {
            logTasks.Add(logger.LogAsync(message, userId));
        }
        var results = await Task.WhenAll(logTasks);
        bool allSuccess = results.All(r => r.Success);
        string errors = string.Join(" | ", results.Where(r => !r.Success).Select(r => r.ErrorMessage));
        return new LogResult(allSuccess, allSuccess ? null : errors);
    }
}