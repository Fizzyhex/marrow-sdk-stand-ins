 
 
 
 
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using SLZ.Marrow.Warehouse;

 

 

 

namespace SLZ.MarrowEditor
{
    [CustomEditor(typeof(LevelCrate))]
    [CanEditMultipleObjects]
    public class LevelCrateEditor : CrateEditor
    {
        SerializedProperty multiSceneProperty;
        SerializedProperty persistentScenesProperty;
        SerializedProperty chunkScenesProperty;
        SerializedProperty editorScenesProperty;
        protected override string AssetReferenceDisplayName
        {
            get
            {
                if (multiSceneProperty != null && multiSceneProperty.boolValue)
                {
                    return "Root Persistent Scene";
                }
                else
                {
                    return "Scene";
                }
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            multiSceneProperty = serializedObject.FindProperty("_multiScene");
            persistentScenesProperty = serializedObject.FindProperty("_persistentScenes");
            chunkScenesProperty = serializedObject.FindProperty("_chunkScenes");
            editorScenesProperty = serializedObject.FindProperty("_editorScenes");
        }

        public override void OnInspectorGUIBody()
        {
            base.OnInspectorGUIBody();
            LockedPropertyField(multiSceneProperty, false);
            if (multiSceneProperty.boolValue)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    if (additionalAssetReferencesProperty != null)
                    {
                        LockedPropertyField(additionalAssetReferencesProperty, null, true, new GUIContent("Additional Scenes [DEPRECATED]"));
                    }

                    LockedPropertyField(persistentScenesProperty, false, false, new GUIContent("Additional Persistent Scenes"));
                    LockedPropertyField(chunkScenesProperty, false);
                    LockedPropertyField(editorScenesProperty, false);
                }
            }
            

            if (GUILayout.Button(new GUIContent("Launch", "Open this level in BONELAB"), GUILayout.ExpandWidth(false)))
            {
                var exePath = $"{SDKProjectPreferences.MarrowGameInstallPaths[0]}/../BONELAB_Steam_Windows64.exe";
                var levelCrate = (LevelCrate)target;

                var startInfo = new ProcessStartInfo()
                {
                    FileName = exePath,
                    Arguments = $"-level {levelCrate.Barcode.ID}",
                    UseShellExecute = true,
                    CreateNoWindow = false,
                    RedirectStandardOutput = false
                };

                var process = new Process
                {
                    StartInfo = startInfo
                };

                process.Start();
            }
        }
    }

    [CustomPreview(typeof(LevelCrate))]
    public class LevelCratePreview : CratePreview
    {
        public override void Cleanup()
        {
            base.Cleanup();
            ClearCachedAssets();
        }
    }
}