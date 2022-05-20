using System.Windows;

namespace Pool.Engine;

public static class ThreadHelper
{
    public static void SendingToTheDispatcherThread(Action action)
    {
        var dispatcher = Application.Current.Dispatcher;
        if (!dispatcher.CheckAccess())
        {
            dispatcher.BeginInvoke((Action)(() =>
            {
                action();
            }));
        }
        else
        {
            action();
        }
    }
}