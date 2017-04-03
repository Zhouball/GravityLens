using UnityEditor;
using UnityEngine;

namespace Platipus.Editor
{
    public class DodecahedronHierarchyMenu
    {
        public const string version = "0.5";
        private const string buckyballPath = "GameObject/Buckyball Hierarchy/";
        private const string create = "Create ";
        private const string dodecahedron = "Bucky Dodecahedron";
        
        [MenuItem("Help/About Buckyball Hierarchy Creator")]
        private static void About()
        {
            BuckyballAboutWindow.Open();
        }

        [MenuItem(buckyballPath + dodecahedron, false, 1)]
        public static void Dodecahedron()
        {
            GameObject go = new GameObject();
            Undo.RegisterCreatedObjectUndo(go, create + dodecahedron );
            go.name = dodecahedron;

            DodecahedronHierarchy dodeca = go.AddComponent<DodecahedronHierarchy>();
            dodeca.initializeDodeca();
        }
    }

    public class BuckyballAboutWindow : EditorWindow
    {
        public static void Open()
        {
            GetWindow<BuckyballAboutWindow>(true, "About Buckyball Hierarchy Creator");
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Version: " + DodecahedronHierarchyMenu.version);

            EditorGUILayout.SelectableLabel("Copyright © 2016 Platipus Software Inside");
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Platipus website"))
            {
                Application.OpenURL("http://www.platipus.nl/");
            }
        }
    }
}