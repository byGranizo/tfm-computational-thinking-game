using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardType))]
public class CardTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector (fields from the ScriptableObject)
        DrawDefaultInspector();

        // Cast the target of the inspector to your ScriptableObject type
        CardType yourScriptableObject = (CardType)target;

        // Display the albedo sprite of the material
        if (yourScriptableObject.Texture != null)
        {
            EditorGUILayout.LabelField("Albedo Texture");
            float aspectRatio = (float)yourScriptableObject.Texture.width / yourScriptableObject.Texture.height;

            // Calculate the size of the preview texture rectangle while maintaining the aspect ratio
            Rect rect = GUILayoutUtility.GetAspectRect(aspectRatio, GUILayout.Width(300), GUILayout.Height(300));
            EditorGUI.DrawPreviewTexture(rect, yourScriptableObject.Texture);
        }
        else
        {
            EditorGUILayout.HelpBox("Assign a Material with a texture to see its properties.", MessageType.Info);
        }
    }
}