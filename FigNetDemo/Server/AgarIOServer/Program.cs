using FigNet.Core;
using FigNet.Server;
using System.Threading;
using System.Diagnostics;
using AgarIOGame.Game;

namespace AgarIOServer
{
    class Program
    {
        private static int frameMilliseconds;
        private static float deltaTime = 0;
        static void Main(string[] args)
        {
           // TcpProvider.Module.Load();
            ENetProvider.Module.Load();
           // WebSocketProvider.Module.Load();
            LiteNetLibProvider.Module.Load();

            IServer serverApp = new ServerApplication();

            FN.OnSettingsLoaded = () =>
            {
                var logger = new DefaultLogger();
                logger.SetUp(FN.Settings.EnableFileLogs, "ServerLogs");
                FN.Logger = logger;

                FN.Logger.Info("@Main config Loaded");
                
                
            };

            //var connectionManager = new ConnectionManager();
            //FN.BindServerListner(connectionManager);
            //var module = new GameplayModule();
            //serverApp.AddModule(module);
            serverApp.SetUp();

            
           // Default_Serializer.RegisterPayloads();
           // Default_Serializer.RegisterHandlers();

            Run(serverApp);
            serverApp.TearDown();
        }

        private static void Run(IServer server)
        {
            frameMilliseconds = 1000 / FN.Settings.FrameRate;

            Stopwatch stopwatch = new Stopwatch();
            int overTime = 0;


            while (true)
            {
                stopwatch.Restart();

                deltaTime = (frameMilliseconds + overTime) * 0.001f;
                server.Process(deltaTime);
                stopwatch.Stop();

                int stepTime = (int)stopwatch.ElapsedMilliseconds;
                if (stepTime <= frameMilliseconds)
                {
                    Thread.Sleep(frameMilliseconds - stepTime);
                    overTime = 0;
                }
                else
                {
                    overTime = stepTime - frameMilliseconds;
                }
            }
        }
    }
}
