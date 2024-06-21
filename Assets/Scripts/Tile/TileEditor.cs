using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Tile tile = (Tile)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Display the biome values of the tileType
        if (tile.TileType != null)
        {
            EditorGUILayout.LabelField("Biome North", tile.TileType.BiomeNorth.ToString());
            EditorGUILayout.LabelField("Biome North East", tile.TileType.BiomeNorthEast.ToString());
            EditorGUILayout.LabelField("Biome South East", tile.TileType.BiomeSouthEast.ToString());
            EditorGUILayout.LabelField("Biome South", tile.TileType.BiomeSouth.ToString());
            EditorGUILayout.LabelField("Biome South West", tile.TileType.BiomeSouthWest.ToString());
            EditorGUILayout.LabelField("Biome North West", tile.TileType.BiomeNorthWest.ToString());
        }

        if (tile.TileType.BiomeGroups != null)
        {
            foreach (var biomeGroup in tile.TileType.BiomeGroups)
            {
                EditorGUILayout.LabelField("Biome Group");
                EditorGUILayout.LabelField("Biome: ", biomeGroup.Biome.ToString());
                EditorGUILayout.LabelField("Value: ", biomeGroup.Value.ToString());

                EditorGUILayout.LabelField("Links: ");
                foreach (var link in biomeGroup.Links)
                {
                    EditorGUILayout.LabelField(link.ToString());
                }
            }
        }
    }
}