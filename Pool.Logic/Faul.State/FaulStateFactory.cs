using Sim.Core;

namespace Pool.Logic;

public class FaulStateFactory
    : IFaulStateFactory
{
    public IFaulState GetBlackScoredInBreak(IFaulState faulState) =>
        new BlackScoredInBreak(faulState);

    public IFaulState GetBlackScoredLast(IFaulState faulState) =>
        new BlackScoredLast(faulState);

    public IFaulState GetBlackScoredNotLast(IFaulState faulState) =>
        new BlackScoredNotLast(faulState);

    public IFaulState GetFixedFaul(IFaulState faulState) =>
        new FixedFaul(faulState);

    public IFaulState GetFixedFaul(IBilliardGameMasterContext context) =>
        new FixedFaul(context);

    public IFaulState GetNoFaul(IFaulState faulState) =>
        new NoFaul(faulState);

    public IFaulState GetNoFaul(IBilliardGameMasterContext context) =>
        new NoFaul(context);

    public IFaulState GetWhiteScored(IFaulState faulState) =>
        new WhiteScored(faulState);
}