using System;
using System.Threading;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Program
    {

        private static bool isRunning = false;
        private const float updatePeriod = 1000f / Terminal.updatesPerSecond;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;
            try
            {
                Console.Title = "Server Console";
                isRunning = true;
                Thread mainThread = new Thread(new ThreadStart(MainThread));
                mainThread.Start();
                Server.Start(Terminal.maxPlayers, Terminal.portNumber);
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        private static void MainThread()
        {
            DateTime nextLoop = DateTime.Now;
            while (isRunning)
            {
                while (nextLoop < DateTime.Now)
                {
                    Terminal.Update();
                    Threading.UpdateMain();
                    nextLoop = nextLoop.AddMilliseconds(updatePeriod);
                    if (nextLoop > DateTime.Now)
                    {
                        Thread.Sleep((int)Math.Clamp((nextLoop - DateTime.Now).TotalMilliseconds, 0, Int32.MaxValue));
                    }
                }
            }
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = default(Exception);
            ex = (Exception)e.ExceptionObject;
            Tools.LogError(ex.Message, ex.StackTrace, "Unhandled");
        }

    }
}