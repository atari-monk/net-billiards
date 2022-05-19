using Sim.Core;

namespace Pool.Logic;

public class BlackScoredLast
    : FaulState
{
    private const int SpecialBallsCount = 2;

    public BlackScoredLast(
        IFaulState state)
            : this(state?.Game)
    {

    }

    public BlackScoredLast(
        IBilliardGameMasterContext? game) => Game = game;

    public override void FixFaul() => Game!.SetWonState(Game.State);

    public override bool IsFaulConditionMeet() => IsBlackBallScored() &&
        IsAllPlayersBallsScored();

    private bool IsAllPlayersBallsScored() =>
        Game!.ScoredBilliardBalls.Count(
            scoredBall => 
                scoredBall.TextFlag == 
                    Game!.PlayerMenager?.PlayerInCurrentRound?.Color) 
                        == SpecialBallsCount;
}