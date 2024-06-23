public class TurnType
{
  private readonly int nTurn;

  private readonly float turnDuration;

  public TurnType(int nTurn, float turnDuration)
  {
    this.nTurn = nTurn;
    this.turnDuration = turnDuration;
  }

  public int NTurn
  {
    get
    {
      return nTurn;
    }
  }

  public float TurnDuration
  {
    get
    {
      return turnDuration;
    }
  }
}