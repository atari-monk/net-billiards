using Sim.Core;

namespace Pool.Logic;

public class BlackScoredNotLast
    : FaulState
{
    private const int SpecialBallsCount = 2;

    public BlackScoredNotLast(
        IFaulState state) : this(state?.Game) { }

    public BlackScoredNotLast(
        IBilliardGameMasterContext? game) => Game = game;

    public override void FixFaul()
    {
        ArgumentNullException.ThrowIfNull(Game);
        Game.PlayerMenager.SwitchCurrentRoundPlayer();
        Game.SetWonState(Game.State);
    }

    public override bool IsFaulConditionMeet() => IsBlackBallScored() &&
        IsNotAllPlayersBallsScored();

    private bool IsNotAllPlayersBallsScored() =>
        Game?.ScoredBilliardBalls.Count(
            scoredBall => scoredBall.TextFlag == Game?.PlayerMenager?.PlayerInCurrentRound?.Color) < SpecialBallsCount;
}