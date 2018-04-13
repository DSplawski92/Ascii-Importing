using System;
using System.Windows;
using NLog;

namespace DS.DataImporter
{
    public partial class App : Application
    {

        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public App()
        {
            Current.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            System.AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, System.UnhandledExceptionEventArgs e)
        {
            logger.Debug(sender.GetType().Name + "|" + (e.ExceptionObject as Exception).Message);
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Debug(sender.GetType().Name + "|" + e.Exception.Message);
        }
    }
}
