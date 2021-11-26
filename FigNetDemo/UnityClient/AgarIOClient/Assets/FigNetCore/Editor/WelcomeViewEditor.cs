using UnityEditor;
using UnityEngine;

namespace FigNet.EditorUI
{
    public class WelcomeViewEditor : ISettingView
    {
        public WelcomeViewEditor(FigNetEditorWindow window)
        {
            this.ParentView = window;
        }
        public FigNetEditorWindow ParentView { get; set; }
        private static Texture2D BackgroundImage;
        public void DisplayView()
        {
            GUILayout.Space(15);
            if (BackgroundImage == null)
            {
                string[] paths = AssetDatabase.FindAssets("FigNetGradient t:Texture2D");
                if (paths != null && paths.Length > 0)
                {
                    BackgroundImage = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(paths[0]));
                }
            }
            UiTitleBox("FigNet Core", BackgroundImage);

            GUILayout.BeginHorizontal("box");

            if (GUILayout.Button("Documentation", GUILayout.Height(100)))
            {
                Application.OpenURL("https://m-ahmed310.gitbook.io/fignet/");
            }

            //if (GUILayout.Button("Blog", GUILayout.Height(100)))
            //{
            //    Application.OpenURL("https://eeprogrammer.wordpress.com/2020/10/31/fignet-core/");
            //}
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Join Discord", GUILayout.Height(25)))
            {
                Application.OpenURL("https://discord.gg/5ScrQaH");
            }
            GUILayout.EndHorizontal();
        }

        public void ResetView()
        {
        }

        private void UiTitleBox(string title, Texture2D bgIcon)
        {
            GUIStyle bgStyle = EditorGUIUtility.isProSkin ? new GUIStyle(GUI.skin.GetStyle("Label")) : new GUIStyle(GUI.skin.GetStyle("WhiteLabel"));
            bgStyle.padding = new RectOffset(10, 10, 10, 10);
            bgStyle.fontSize = 22;
            bgStyle.fontStyle = FontStyle.Bold;
            if (bgIcon != null)
            {
                bgStyle.normal.background = bgIcon;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            Rect scale = GUILayoutUtility.GetLastRect();
            scale.height = 44;

            GUI.Label(scale, title, bgStyle);
            GUILayout.Space(scale.height + 5);
        }
    }
}

