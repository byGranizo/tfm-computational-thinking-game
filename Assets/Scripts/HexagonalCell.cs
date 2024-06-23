using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonalCell : MonoBehaviour
{
    [SerializeField]
    private Vector2Int boardCoordinates;

    private Tile tile;

    private GameObject placeholder;
    private GameObject cellCollider;

    //Attribute for the cell's neighbors
    private HexagonalCell north;
    private HexagonalCell northEast;
    private HexagonalCell southEast;
    private HexagonalCell south;
    private HexagonalCell southWest;
    private HexagonalCell northWest;

    private BoardController boardController;

    public Tile Tile
    {
        get { return tile; }
    }

    public Vector2Int Coordinates
    {
        get { return boardCoordinates; }
        set { boardCoordinates = value; }
    }

    public HexagonalCell North
    {
        get { return north; }
        set { north = value; }
    }

    public HexagonalCell NorthEast
    {
        get { return northEast; }
        set { northEast = value; }
    }

    public HexagonalCell SouthEast
    {
        get { return southEast; }
        set { southEast = value; }
    }

    public HexagonalCell South
    {
        get { return south; }
        set { south = value; }
    }

    public HexagonalCell SouthWest
    {
        get { return southWest; }
        set { southWest = value; }
    }

    public HexagonalCell NorthWest
    {
        get { return northWest; }
        set { northWest = value; }
    }


    void Awake()
    {
        placeholder = GetComponentInChildren<EmptyCellRenderer>().gameObject;
        placeholder.SetActive(false);

        cellCollider = GetComponentInChildren<CellCollider>().gameObject;
        cellCollider.SetActive(false);



    }

    void Start()
    {
        boardController = GetComponentInParent<BoardController>();
    }

    public void PlaceTile(Tile tile)
    {
        this.tile = tile;
        UpdateColliderAndPlaceholder();
        UpdateNeighbors();

        boardController?.CheckModifiedBiomes(this);
    }

    private void UpdateNeighbors()
    {
        foreach (var neighbor in GetAllNeightbours())
        {
            neighbor?.UpdateColliderAndPlaceholder();
        }
    }

    public void UpdateColliderAndPlaceholder()
    {
        bool isActive = tile == null;
        cellCollider.SetActive(isActive);
        placeholder.SetActive(isActive);
    }
    public HexagonalCell GetNeighbor(TileCoordinates direction)
    {
        return direction switch
        {
            TileCoordinates.North => North,
            TileCoordinates.NorthEast => NorthEast,
            TileCoordinates.SouthEast => SouthEast,
            TileCoordinates.South => South,
            TileCoordinates.SouthWest => SouthWest,
            TileCoordinates.NorthWest => NorthWest,
            _ => null,
        };
    }

    public HexagonalCell[] GetAllNeightbours()
    {
        return new HexagonalCell[] { North, NorthEast, SouthEast, South, SouthWest, NorthWest };
    }

    public bool AreAllNeighborsOccupied()
    {
        foreach (HexagonalCell neighbor in GetAllNeightbours())
        {
            if (neighbor != null && neighbor.Tile == null)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanPlaceTile(TileType tileType)
    {
        return IsBiomeCompatible(North, tileType.BiomeNorth, t => t.BiomeSouth) &&
               IsBiomeCompatible(NorthEast, tileType.BiomeNorthEast, t => t.BiomeSouthWest) &&
               IsBiomeCompatible(SouthEast, tileType.BiomeSouthEast, t => t.BiomeNorthWest) &&
               IsBiomeCompatible(South, tileType.BiomeSouth, t => t.BiomeNorth) &&
               IsBiomeCompatible(SouthWest, tileType.BiomeSouthWest, t => t.BiomeNorthEast) &&
               IsBiomeCompatible(NorthWest, tileType.BiomeNorthWest, t => t.BiomeSouthEast);
    }

    private bool IsBiomeCompatible(HexagonalCell neighbor, BiomeType tileBiome, System.Func<TileType, BiomeType> getNeighborBiome)
    {
        if (neighbor == null || neighbor.Tile == null)
        {
            return true;
        }

        BiomeType neighborBiome = getNeighborBiome(neighbor.Tile.TileType);
        return neighborBiome == BiomeType.Wildcard || tileBiome == BiomeType.Wildcard || neighborBiome == tileBiome;
    }

}