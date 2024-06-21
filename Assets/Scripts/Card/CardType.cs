using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardType : ScriptableObject
{
    [SerializeField]
    private Texture2D _texture;

    [Space(10)]
    [SerializeField]
    private BiomeType _biome;

    [SerializeField]
    private int _value;

    [SerializeField]
    private CardMissionType _missionType;

    [SerializeField]
    private CardMissionDifficulty cardMissionDifficulty;

    private int turnStart;
    private int turnEnd;
    private bool completedWithWildCard = false;

    public Texture2D Texture
    {
        get
        {
            return _texture;
        }
    }

    public BiomeType Biome
    {
        get
        {
            return _biome;
        }
    }

    public int Value
    {
        get
        {
            return _value;
        }
    }

    public CardMissionType MissionType
    {
        get
        {
            return _missionType;
        }
    }

    public CardMissionDifficulty CardMissionDifficulty
    {
        get
        {
            return cardMissionDifficulty;
        }
    }

    public int TurnStart
    {
        get
        {
            return turnStart;
        }
        set
        {
            turnStart = value;
        }
    }

    public int TurnEnd
    {
        get
        {
            return turnEnd;
        }
        set
        {
            turnEnd = value;
        }
    }

    public bool CompletedWithWildCard
    {
        get
        {
            return completedWithWildCard;
        }
        set
        {
            completedWithWildCard = value;
        }
    }

    public CardType()
    {
        _texture = null;
        _biome = BiomeType.Wildcard;
        _value = 0;
        _missionType = CardMissionType.Equals;
        cardMissionDifficulty = CardMissionDifficulty.Easy;
    }
}
