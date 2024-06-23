using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileCoordinates
{
    North,
    NorthEast,
    SouthEast,
    South,
    SouthWest,
    NorthWest,
    None
}
public static class TileCoordinatesExtensions
{
    public static TileCoordinates GetOpposite(this TileCoordinates direction)
    {
        switch (direction)
        {
            case TileCoordinates.North:
                return TileCoordinates.South;
            case TileCoordinates.NorthEast:
                return TileCoordinates.SouthWest;
            case TileCoordinates.SouthEast:
                return TileCoordinates.NorthWest;
            case TileCoordinates.South:
                return TileCoordinates.North;
            case TileCoordinates.SouthWest:
                return TileCoordinates.NorthEast;
            case TileCoordinates.NorthWest:
                return TileCoordinates.SouthEast;
            default:
                return TileCoordinates.None;
        }
    }
}

[System.Serializable]
public class InTileBiome
{
    [SerializeField]
    private BiomeType biome;
    [SerializeField]
    private int value;
    [SerializeField]
    private List<TileCoordinates> links;

    public BiomeType Biome
    {
        get
        {
            return biome;
        }
    }

    public int Value
    {
        get
        {
            return value;
        }
    }

    public List<TileCoordinates> Links
    {
        get
        {
            return links;
        }
    }

    public void RotateAntiClockwise()
    {
        Dictionary<TileCoordinates, TileCoordinates> directionsRotation = new Dictionary<TileCoordinates, TileCoordinates>
        {
            {TileCoordinates.North, TileCoordinates.NorthEast},
            {TileCoordinates.NorthEast, TileCoordinates.SouthEast},
            {TileCoordinates.SouthEast, TileCoordinates.South},
            {TileCoordinates.South, TileCoordinates.SouthWest},
            {TileCoordinates.SouthWest, TileCoordinates.NorthWest},
            {TileCoordinates.NorthWest, TileCoordinates.North},
        };

        for (int i = 0; i < links.Count; i++)
        {
            links[i] = directionsRotation[links[i]];
        }
    }

    public void RotateClockwise()
    {
        Dictionary<TileCoordinates, TileCoordinates> directionsRotation = new Dictionary<TileCoordinates, TileCoordinates>
        {
            {TileCoordinates.North, TileCoordinates.NorthWest},
            {TileCoordinates.NorthWest, TileCoordinates.SouthWest},
            {TileCoordinates.SouthWest, TileCoordinates.South},
            {TileCoordinates.South, TileCoordinates.SouthEast},
            {TileCoordinates.SouthEast, TileCoordinates.NorthEast},
            {TileCoordinates.NorthEast, TileCoordinates.North},
        };

        for (int i = 0; i < links.Count; i++)
        {
            links[i] = directionsRotation[links[i]];
        }
    }

}

[CreateAssetMenu]
public class TileType : ScriptableObject
{
    [SerializeField]
    private Material _material;

    [Space(10)]
    [SerializeField]
    private BiomeType _biomeNorth;

    [SerializeField]
    private BiomeType _biomeNorthEast;

    [SerializeField]
    private BiomeType _biomeSouthEast;

    [SerializeField]
    private BiomeType _biomeSouth;

    [SerializeField]
    private BiomeType _biomeSouthWest;

    [SerializeField]
    private BiomeType _biomeNorthWest;

    [SerializeField]
    private List<InTileBiome> biomeGroups;


    public Material Material
    {
        get
        {
            return _material;
        }
    }

    public BiomeType BiomeNorth
    {
        get
        {
            return _biomeNorth;
        }
    }

    public BiomeType BiomeNorthEast
    {
        get
        {
            return _biomeNorthEast;
        }
    }

    public BiomeType BiomeSouthEast
    {
        get
        {
            return _biomeSouthEast;
        }
    }

    public BiomeType BiomeSouth
    {
        get
        {
            return _biomeSouth;
        }
    }

    public BiomeType BiomeSouthWest
    {
        get
        {
            return _biomeSouthWest;
        }
    }

    public BiomeType BiomeNorthWest
    {
        get
        {
            return _biomeNorthWest;
        }
    }

    public List<InTileBiome> BiomeGroups
    {
        get
        {
            return biomeGroups;
        }
    }

    public void RotateClockwise()
    {
        BiomeType temp = _biomeNorth;
        _biomeNorth = _biomeNorthEast;
        _biomeNorthEast = _biomeSouthEast;
        _biomeSouthEast = _biomeSouth;
        _biomeSouth = _biomeSouthWest;
        _biomeSouthWest = _biomeNorthWest;
        _biomeNorthWest = temp;

        for (int i = 0; i < biomeGroups.Count; i++)
        {
            biomeGroups[i].RotateClockwise();
        }
    }

    public void RotateAntiClockwise()
    {
        BiomeType temp = _biomeNorth;
        _biomeNorth = _biomeNorthWest;
        _biomeNorthWest = _biomeSouthWest;
        _biomeSouthWest = _biomeSouth;
        _biomeSouth = _biomeSouthEast;
        _biomeSouthEast = _biomeNorthEast;
        _biomeNorthEast = temp;

        for (int i = 0; i < biomeGroups.Count; i++)
        {
            biomeGroups[i].RotateAntiClockwise();
        }
    }
}

