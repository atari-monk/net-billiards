using System.IO;
using Sim.Core;

namespace Pool.Engine;

public class LoggerToFile : LoggerDisposeable
{
    private const string Path = "log.txt";

    private readonly StreamWriter streamWriter = File.AppendText(Path);

    public override void Log(string logMessage)
    {
        streamWriter.Write($"{Environment.NewLine}Log Entry : ");
        streamWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
        streamWriter.WriteLine($" :{Environment.NewLine}{logMessage}");
        streamWriter.WriteLine("-------------------------------");
    }

    public override void Dispose() => streamWriter.Dispose();
}