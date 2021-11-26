using FigNet.Core;
using UnityEditor;
using UnityEngine;

namespace FigNet.EditorUI
{
    public class FigNetEditorWindow : EditorWindow
    {
        private WelcomeViewEditor WelcomeView;
        private ConfigViewEditor ConfigView;

        int selectedModule = -1;

        public FigNetConfiguration configuration;
        private enum ViewState { Welcome, Config, Count }
        ViewState view_state = ViewState.Welcome;

        //public enum TransportLayer { ENet, LiteNetLib, WS, TCP, WEBGL }
        public Providers Transport = Providers.ENet;

        public enum LoggingLevel { NONE, INFO, DEBUG, ALL }

        [MenuItem("FigNet/Settings")]
        public static void Init()
        {
            FigNetEditorWindow window = EditorWindow.GetWindow<FigNetEditorWindow>();
            window.titleContent = new UnityEngine.GUIContent("FigNetCore", "FigNet Settings");
            window.Show();
            //LoadDatabaseFromXML(DB_NAME);
        }

        void OnEnable()
        {
            selectedModule = -1;

            configuration = Resources.Load<FigNetConfiguration>("FigNet_Configuration");

            ConfigView = new ConfigViewEditor(this);
            WelcomeView = new WelcomeViewEditor(this);

            //Transport = (TransportLayer)Enum.Parse(typeof(TransportLayer), configuration.Config.TransportLayer);
        }

        private void OnDestroy()
        {
            AssetDatabase.SaveAssets();
        }

        private void OnGUI()
        {
            DisplayMainView();
        }


        void DisplayMainView()
        {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical("box", GUILayout.Width(180));
            GUILayout.Label("Modules", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();

            GUILayout.EndHorizontal();

            ShowList();
            GUILayout.Space(15);

            EditorGUILayout.HelpBox("Write changes to disk.", MessageType.Warning);

            if (GUILayout.Button("Save Changes", GUILayout.Width(150)))
            {
                EditorUtility.SetDirty(configuration);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }


            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("FigNetCore Settings", EditorStyles.boldLabel, GUILayout.Width(300));

            GUILayout.Space(25);

            switch (view_state)
            {
                case ViewState.Welcome:
                    WelcomeView.DisplayView();
                    break;
                case ViewState.Config:
                    ConfigView.DisplayView();
                    break;
            }

            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }


        Vector2 scrollCharacters;

        void ShowList()
        {
            //return;
            scrollCharacters = GUILayout.BeginScrollView(scrollCharacters, GUILayout.Width(175));

            for (int i = 0; i < (int)ViewState.Count; i++)
            {
                GUILayout.BeginHorizontal();
                if (selectedModule == i) GUI.color = new Color(0.7f, 0.7f, 0.7f);
                if (GUILayout.Button(((ViewState)i).ToString(), GUILayout.ExpandWidth(true)))
                {
                    // here display unique stats
                    selectedModule = i;

                    view_state = (ViewState)i;
                }
                GUI.color = Color.white;
                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();

        }
        public void ShowNotification(string message)
        {
            this.ShowNotification(new GUIContent(message));
        }
    }

    public interface ISettingView
    {
        FigNetEditorWindow ParentView { get; set; }
        void ResetView();
        void DisplayView();
    }
}
