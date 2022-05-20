using System.Collections.ObjectModel;
using Sim.Core;

namespace Pool.Engine;

public class LoggerToMemory
    : Logger
        , ILoggerToMemory
{
    public ObservableCollection<string> LogContent { get; private set; }

    public LoggerToMemory()
    {
        LogContent = new ObservableCollection<string>();
    }


    public override void Log(string logMessage)
    {
        ThreadHelper.SendingToTheDispatcherThread(() =>
            LogContent.Add(logMessage));
    }
}