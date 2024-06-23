using System;

[Serializable]
public class GameType
{

  private string id;
  private DateTime startDateTime;
  private DateTime endDateTime;
  private int nTurns;
  private int nTiles;
  private int nMissions;
  private EndGameUIState result;

  public GameType()
  {
    this.startDateTime = DateTime.Now;
  }

  public void EndGame(int nTurns, int nTiles, int nMissions, EndGameUIState result)
  {
    this.endDateTime = DateTime.Now;
    this.nTurns = nTurns;
    this.nTiles = nTiles;
    this.nMissions = nMissions;
    this.result = result;
  }

  public string Id { get; set; }

  public DateTime StartDateTime
  {
    get
    {
      return startDateTime;
    }
  }

  public DateTime EndDateTime
  {
    get
    {
      return endDateTime;
    }
  }

  public int NTurns
  {
    get
    {
      return nTurns;
    }
  }

  public int NTiles
  {
    get
    {
      return nTiles;
    }
  }

  public int NMissions
  {
    get
    {
      return nMissions;
    }
  }

  public EndGameUIState Result
  {
    get
    {
      return result;
    }
  }

  public override string ToString()
  {
    return "GameType: " + id + " " + startDateTime + " " + endDateTime + " " + nTurns + " " + nTiles + " " + nMissions + " " + result;
  }
}