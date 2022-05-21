using Sim.Core;
using System.Diagnostics;

namespace Pool.Container;

public class Timer
	: ITimer
{

	private readonly Stopwatch _stopWatch;

	public Timer() => _stopWatch = new Stopwatch();

	public void Start() =>
		_stopWatch.Start();

	public void Stop() =>
		_stopWatch.Stop();

	public void Reset() =>
		_stopWatch.Reset();

	public double ElapsedSeconds => _stopWatch.ElapsedMilliseconds / 1000.0;

}