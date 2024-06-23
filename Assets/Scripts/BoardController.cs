using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField, Tooltip("Built for odd numbers only. If an even number is entered, it will be converted to the next odd number.")]
    private int boardSize = 3;
    private HexagonalCell[,] boardGrid;

    [SerializeField]
    private float cellSize = 1.0f;

    [SerializeField]
    private float cellSpacing = 0.1f;

    [SerializeField]
    private HexagonalCell cellPrefab;

    [SerializeField]
    private bool isDebug = false;

    private GameManager gameManager;
    HexagonalCell startingCell;


    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void InitBoardGrid()
    {
        if (boardSize % 2 == 0)
        {
            boardSize++;
        }
        boardGrid = new HexagonalCell[boardSize, boardSize];

        int boardIndexOffset = boardSize / 2;

        for (int i = -boardIndexOffset; i <= boardIndexOffset; i++)
        {
            for (int j = -boardIndexOffset; j <= boardIndexOffset; j++)
            {
                float yOffset = (i % 2 == 0) ? -(cellSize + cellSpacing) / 2 : 0;
                HexagonalCell cell = Instantiate<HexagonalCell>(cellPrefab, new Vector3((i * ((cellSize + cellSpacing) * 0.866025404f)), 0, j * (cellSize + cellSpacing) + yOffset), Quaternion.identity);
                cell.transform.localScale = new Vector3(cellSize, 1, cellSize);
                cell.transform.parent = transform;

                Vector2Int coords = new Vector2Int(i + boardIndexOffset, j + boardIndexOffset);
                cell.Coordinates = coords;

                boardGrid[coords.x, coords.y] = cell;

                if (i == 0 && j == 0)
                {
                    startingCell = cell;
                }

                if (isDebug) CreateCellDebugging(coords.x, coords.y, cell);
            }
        }


        GenerateCellsStructure();

        ConfigureTileAndPlaceTile(startingCell);
    }

    private void GenerateCellsStructure()
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                HexagonalCell cell = boardGrid[i, j];

                if (j < boardSize - 1)
                {
                    cell.North = boardGrid[i, j + 1];
                }

                if (j > 0)
                {
                    cell.South = boardGrid[i, j - 1];
                }

                if (i % 2 == 0)
                {
                    SetCellNeighborsEven(cell, i, j);
                }
                else
                {
                    SetCellNeighborsOdd(cell, i, j);
                }
            }
        }
    }

    private void SetCellNeighborsEven(HexagonalCell cell, int i, int j)
    {
        if (i < boardSize - 1)
        {
            cell.NorthEast = boardGrid[i + 1, j];
        }

        if (i < boardSize - 1 && j > 0)
        {
            cell.SouthEast = boardGrid[i + 1, j - 1];
        }

        if (i > 0 && j > 0)
        {
            cell.SouthWest = boardGrid[i - 1, j - 1];
        }

        if (i > 0)
        {
            cell.NorthWest = boardGrid[i - 1, j];
        }
    }

    private void SetCellNeighborsOdd(HexagonalCell cell, int i, int j)
    {
        if (i < boardSize - 1 && j < boardSize - 1)
        {
            cell.NorthEast = boardGrid[i + 1, j + 1];
        }

        if (i < boardSize - 1)
        {
            cell.SouthEast = boardGrid[i + 1, j];
        }

        if (i > 0)
        {
            cell.SouthWest = boardGrid[i - 1, j];
        }

        if (i > 0 && j < boardSize - 1)
        {
            cell.NorthWest = boardGrid[i - 1, j + 1];
        }
    }

    private void ConfigureTileAndPlaceTile(HexagonalCell cell)
    {
        gameManager.InstantiateInitialTile(cell);
    }


    public void CheckModifiedBiomes(HexagonalCell cell)
    {
        bool[,] visited = new bool[boardSize, boardSize];
        visited[cell.Coordinates.x, cell.Coordinates.y] = true;

        List<(BiomeType, int)> biomeGroups = new List<(BiomeType, int)>();

        List<InTileBiome> initBiomeGroups = cell.Tile.TileType.BiomeGroups;


        for (int i = 0; i < initBiomeGroups.Count; i++)
        {
            InTileBiome biomeGroup = initBiomeGroups[i];

            if (biomeGroup.Biome == BiomeType.Wildcard) continue;

            (BiomeType, int) biomeCount = (biomeGroup.Biome, biomeGroup.Value);

            for (int j = 0; j < biomeGroup.Links.Count; j++)
            {
                TileCoordinates neighborCoord = biomeGroup.Links[j];
                HexagonalCell neighbor = cell.GetNeighbor(neighborCoord);

                if (neighbor != null && neighbor.Tile != null && neighbor.Tile.TileType.BiomeGroups[0].Biome != BiomeType.Wildcard)
                {
                    CheckPathBiomes(neighbor, ref biomeCount, ref visited, TileCoordinatesExtensions.GetOpposite(neighborCoord));
                }
            }
            biomeGroups.Add(biomeCount);
        }

        int wildcardCompleted = CheckWildcard(cell);

        if (wildcardCompleted > 0)
        {
            biomeGroups.Add((BiomeType.Wildcard, wildcardCompleted));
        }

        gameManager.CheckMissionsAtPlace(biomeGroups);
    }

    private void CheckPathBiomes(HexagonalCell cell, ref (BiomeType, int) biomeCount, ref bool[,] visited, TileCoordinates entryDirection)
    {

        if (visited[cell.Coordinates.x, cell.Coordinates.y]) return;

        visited[cell.Coordinates.x, cell.Coordinates.y] = true;

        List<InTileBiome> cellBiomeGroups = cell.Tile.TileType.BiomeGroups;
        InTileBiome biomeGroup = cellBiomeGroups.Find(x => x.Links.Contains(entryDirection));

        if (biomeGroup == null || biomeGroup.Biome != biomeCount.Item1) return;

        biomeCount.Item2 += biomeGroup.Value;

        for (int i = 0; i < biomeGroup.Links.Count; i++)
        {
            TileCoordinates neighborCoord = biomeGroup.Links[i];
            HexagonalCell neighbor = cell.GetNeighbor(neighborCoord);

            if (neighbor != null && neighbor.Tile != null && neighbor.Tile.TileType.BiomeGroups[0].Biome != BiomeType.Wildcard)
            {
                CheckPathBiomes(neighbor, ref biomeCount, ref visited, TileCoordinatesExtensions.GetOpposite(neighborCoord));
            }
        }
    }

    private int CheckWildcard(HexagonalCell cell)
    {
        int completedWildcards = 0;

        if (cell.Tile.TileType.BiomeGroups[0].Biome == BiomeType.Wildcard && cell.AreAllNeighborsOccupied())
        {
            completedWildcards++;
        }

        HexagonalCell[] neighbors = cell.GetAllNeightbours();
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (neighbors[i] != null && neighbors[i].Tile != null && neighbors[i].Tile.TileType.BiomeGroups[0].Biome == BiomeType.Wildcard && neighbors[i].AreAllNeighborsOccupied())
            {
                completedWildcards++;
            }
        }

        return completedWildcards;
    }




    /**** DEBUGGING FUNCTIONS ****/
    private void CreateCellDebugging(int i, int j, HexagonalCell cell)
    {
        float halfSize = cellSize / 2;

        // Instantiate a text facing y positive object to show the cell index
        GameObject text = new GameObject("Text");
        text.transform.parent = cell.transform;
        text.transform.localPosition = new Vector3(0, 0.1f, 0);
        text.transform.rotation = Quaternion.Euler(90, 0, 0);
        TextMesh textMesh = text.AddComponent<TextMesh>();
        textMesh.text = i + "," + j;
        textMesh.fontSize = 40;
        textMesh.characterSize = 0.1f;
        textMesh.color = Color.black;
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;

        // Instantiate a GameObject for the hexagon and add a LineRenderer to it
        GameObject hexagon = new GameObject("Hexagon");
        hexagon.transform.parent = cell.transform;
        hexagon.transform.localPosition = Vector3.zero;
        LineRenderer hexagonLineRenderer = hexagon.AddComponent<LineRenderer>();
        hexagonLineRenderer.useWorldSpace = false;
        hexagonLineRenderer.positionCount = 7;
        hexagonLineRenderer.loop = true;
        hexagonLineRenderer.widthMultiplier = 0.01f; // Adjust this value to change the border width
        hexagonLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        hexagonLineRenderer.startColor = hexagonLineRenderer.endColor = Color.black;

        // Set the positions of the LineRenderer to the vertices of the hexagon
        float hexagonRadius = halfSize / Mathf.Cos(Mathf.PI / 6); // Calculate the radius of the hexagon
        for (int k = 0; k < 6; k++)
        {
            float angle = k * 60 * Mathf.Deg2Rad;
            hexagonLineRenderer.SetPosition(k, new Vector3(hexagonRadius * Mathf.Cos(angle), 0, hexagonRadius * Mathf.Sin(angle)));
        }
        hexagonLineRenderer.SetPosition(6, hexagonLineRenderer.GetPosition(0));
    }
}
