public class TurnType
{
  private int nTurn;

  private float turnDuration;

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