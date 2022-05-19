using Sim.Core;

namespace Pool.Logic;

public class BilliardPlayerManager
    : TwoPlayerManager
        , IBilliardPlayerManager
{
    private const string FullColorBall = "full";

    private const string StripeColorBall = "stripe";

    public BilliardPlayerManager(
        IPlayer[] players)
    {
        PlayerOne = players[0];
        PlayerTwo = players[1];
        PlayerInCurrentRound = PlayerOne;
    }

    public IPlayer GetOther()
    {
        ArgumentNullException.ThrowIfNull(PlayerOne);
        ArgumentNullException.ThrowIfNull(PlayerTwo);
        if (PlayerInCurrentRound == PlayerOne)
            return PlayerTwo;
        else return PlayerOne;
    }

    public void OpenTable(string flag)
    {
        ArgumentNullException.ThrowIfNull(PlayerInCurrentRound);
        switch (flag)
        {
            case FullColorBall:
                PlayerInCurrentRound.Color = FullColorBall;
                GetOther().Color = StripeColorBall;
                break;
            case StripeColorBall:
                PlayerInCurrentRound.Color = StripeColorBall;
                GetOther().Color = FullColorBall;
                break;
            default:
                break;
        }
    }
}