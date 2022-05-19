using Sim.Core;

namespace Pool.Logic;

public interface ICircleInSink : IBinaryLogic
	{
		event Action<IShape> Scored;
	}
