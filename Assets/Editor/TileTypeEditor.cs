using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileType))]
public class TileTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector (fields from the ScriptableObject)
        DrawDefaultInspector();

        // Cast the target of the inspector to your ScriptableObject type
        TileType yourScriptableObject = (TileType)target;

        // Display the albedo sprite of the material
        if (yourScriptableObject.Material != null && yourScriptableObject.Material.mainTexture != null)
        {
            EditorGUILayout.LabelField("Albedo Texture");
            float aspectRatio = (float)yourScriptableObject.Material.mainTexture.width / yourScriptableObject.Material.mainTexture.height;

            // Calculate the size of the preview texture rectangle while maintaining the aspect ratio
            Rect rect = GUILayoutUtility.GetAspectRect(aspectRatio, GUILayout.Width(300), GUILayout.Height(300));
            EditorGUI.DrawPreviewTexture(rect, yourScriptableObject.Material.mainTexture);
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a Material with a texture to see its properties.", MessageType.Info);
        }
    }
}