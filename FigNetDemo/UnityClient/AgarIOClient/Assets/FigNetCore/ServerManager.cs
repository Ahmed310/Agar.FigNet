using System;
using FigNet.Core;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace FigNet
{
    public class ServerManager : MonoBehaviour, IServer
    {
        readonly List<IModule> Modules = new List<IModule>();
        readonly Dictionary<string, Action> commands = new Dictionary<string, Action>();
        public bool AutoConnectOnAwake = true;

        private void Awake()
        {
            if (AutoConnectOnAwake)
            {
                SetUp();
            }
        }
        private void OnDestroy()
        {
            TearDown();
        }
        private void Update()
        {
            Process(Time.deltaTime);
        }

        #region IServer_Implementation

        public bool AddCommand(string id, Action procedure)
        {
            bool sucess = false;
            if (!commands.ContainsKey(id))
            {
                sucess = true;
                commands.Add(id, procedure);
            }
            return sucess;
        }
        public bool ExecuteCommand(string id)
        {
            bool sucess = false;
            if (commands.ContainsKey(id))
            {
                sucess = true;
                commands[id].Invoke();
            }
            return sucess;
        }
        public List<string> ListCommands()
        {
            List<string> _commands = null;
            if (commands.Count > 0)
            {
                _commands = commands.Keys.ToList();
            }
            return _commands;
        }
        public void AddModule(IModule module)
        {
            module.Load(this);
            Modules.Add(module);
        }

        public bool ContainsModule(IModule module)
        {
            return Modules.Contains(module);
        }

        public IPeer GetPeer(uint peerId)
        {
            return FN.PeerCollection.GetPeerByID(peerId);
        }

        public bool InitializeModules()
        {
            return false;
        }

        public void SetUp()
        {
            var config = Resources.Load<TextAsset>("ServerConfig").text;
            var settings = config.DeserializeFromXml<Configuration>();
            FN.Logger = new DefaultLogger();
            FN.Initilize(settings);
            FN.LoadModules(this);
            AddModule(new Server.Modules.BuiltInModule());

            //Application.targetFrameRate = FN.Settings.FrameRate;
        }

        public void TearDown()
        {
            FN.Deinitialize();
        }

        T IServer.GetModule<T>()
        {
            return Modules.Find(m => m.GetType() == typeof(T)) as T;
        }

        public void Process(float deltaTime)
        {
            for (int i = 0; i < FN.Servers?.Count; i++)
            {
                FN.Servers[i].Process();
            }

            for (int i = 0; i < FN.Connections?.Count; i++)
            {
                FN.Connections[i].Process();
            }

            for (int i = 0; i < Modules?.Count; i++)
            {
                Modules[i].Process(deltaTime);
            }

            TimerScheduler.Tick(deltaTime);
        }
        #endregion
    }

}

