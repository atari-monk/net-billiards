using Sim.Core;

namespace Pool.Logic;

public class TwoPlayerManager
    : ITwoPlayerManager
{
    public IPlayer? PlayerInCurrentRound { get; set; }

    public IPlayer? PlayerOne { get; set; }

    public IPlayer? PlayerTwo { get; set; }

    public void SwitchCurrentRoundPlayer()
    {
        if (PlayerInCurrentRound == PlayerOne)
            PlayerInCurrentRound = PlayerTwo;
        else PlayerInCurrentRound = PlayerOne;
    }
}