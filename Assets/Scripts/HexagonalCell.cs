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

        if (boardController != null)
        {
            boardController.CheckModifiedBiomes(this);
        }

    }

    private void UpdateNeighbors()
    {
        if (north != null)
        {
            north.UpdateColliderAndPlaceholder();
        }

        if (northEast != null)
        {
            northEast.UpdateColliderAndPlaceholder();
        }

        if (southEast != null)
        {
            southEast.UpdateColliderAndPlaceholder();
        }

        if (south != null)
        {
            south.UpdateColliderAndPlaceholder();
        }

        if (southWest != null)
        {
            southWest.UpdateColliderAndPlaceholder();
        }

        if (northWest != null)
        {
            northWest.UpdateColliderAndPlaceholder();
        }
    }

    public void UpdateColliderAndPlaceholder()
    {
        if (tile != null)
        {
            cellCollider.SetActive(false);
            placeholder.SetActive(false);
        }
        else
        {
            cellCollider.SetActive(true);
            placeholder.SetActive(true);
        }
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
        if (North != null && North.Tile != null)
        {
            BiomeType biomeAtNorth = North.Tile.TileType.BiomeSouth;
            if (biomeAtNorth != BiomeType.Wildcard && tileType.BiomeNorth != BiomeType.Wildcard && biomeAtNorth != tileType.BiomeNorth)
            {
                return false;
            }
        }

        if (NorthEast != null && NorthEast.Tile != null)
        {
            BiomeType biomeAtNorthEast = NorthEast.Tile.TileType.BiomeSouthWest;
            if (biomeAtNorthEast != BiomeType.Wildcard && tileType.BiomeNorthEast != BiomeType.Wildcard && biomeAtNorthEast != tileType.BiomeNorthEast)
            {
                return false;
            }
        }

        if (SouthEast != null && SouthEast.Tile != null)
        {
            BiomeType biomeAtSouthEast = SouthEast.Tile.TileType.BiomeNorthWest;
            if (biomeAtSouthEast != BiomeType.Wildcard && tileType.BiomeSouthEast != BiomeType.Wildcard && biomeAtSouthEast != tileType.BiomeSouthEast)
            {
                return false;
            }
        }

        if (South != null && South.Tile != null)
        {
            BiomeType biomeAtSouth = South.Tile.TileType.BiomeNorth;
            if (biomeAtSouth != BiomeType.Wildcard && tileType.BiomeSouth != BiomeType.Wildcard && biomeAtSouth != tileType.BiomeSouth)
            {
                return false;
            }
        }

        if (SouthWest != null && SouthWest.Tile != null)
        {
            BiomeType biomeAtSouthWest = SouthWest.Tile.TileType.BiomeNorthEast;
            if (biomeAtSouthWest != BiomeType.Wildcard && tileType.BiomeSouthWest != BiomeType.Wildcard && biomeAtSouthWest != tileType.BiomeSouthWest)
            {
                return false;
            }
        }

        if (NorthWest != null && NorthWest.Tile != null)
        {
            BiomeType biomeAtNorthWest = NorthWest.Tile.TileType.BiomeSouthEast;
            if (biomeAtNorthWest != BiomeType.Wildcard && tileType.BiomeNorthWest != BiomeType.Wildcard && biomeAtNorthWest != tileType.BiomeNorthWest)
            {
                return false;
            }
        }

        return true;
    }

}