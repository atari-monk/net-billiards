using System.Collections.Concurrent;
using Sim.Core;
using Vector.Lib;

namespace Pool.Engine;

public class ShapeGameLoop
	: GameLoop
{
	private readonly IGameData gameData;
	private readonly ICanvasSerializaton<IShape> canvas;
	private readonly IPhysics physics;
	private readonly ILogics logics;
	private readonly ILoggerToMemory logger;

	public ShapeGameLoop(
		IGameData gameData
		, ICanvasSerializaton<IShape> canvas
		, IBilliardGameContext billiardGameContext
		, IPhysics physic
		, ILogics logic
		, ITimer timer
		, ILoggerToMemory logger)
            : base(billiardGameContext, timer)
	{
		this.gameData = gameData;
		this.canvas = canvas;
		physics = physic;
		logics = logic;
		this.logger = logger;
		InitializeBalls();
	}

	private void InitializeBalls()
	{
		foreach (var billiardBall in gameData.Circles)
		{
			billiardBall.Mass = 10;
			billiardBall.Velocity = new Vector2(0, 0);
		}
	}

	public override void ProcessInput() { }

	public override void Update(double dt)
	{
		gameData.VectorAnalitics = new BlockingCollection<IShape>();
		var physicsTask = Task.Factory.StartNew(() =>
		{
			//Console.Write("producer starts\n");
			physics.RunPhysics(dt);
			//Console.Write("producer ends\n");
		});
		physicsTask.Wait();
		var logicTask = Task.Factory.StartNew(() =>
		{
			//Console.Write("producer starts\n");
			logics.RunLogics(dt);
			//Console.Write("producer ends\n");
		});
		logicTask.Wait();
	}

	public override void Render()
	{
		var renderTask = Task.Factory.StartNew(() =>
		{
			//Console.Write("consumer starts\n");
			canvas.RunDispatcher(Draw);
			//Console.Write("consumed sum = {0}");
			//Console.Write("\nconsumer ends\n");
		});
		renderTask.Wait();
	}

	private void Draw()
	{
		var renderable = from shape in gameData.Shapes
						 where (shape is ICircle && shape.Context == Context.Physic) ||
						 shape is IRectangle
						 select shape;
		var white = renderable.FirstOrDefault(o => o.TextFlag == "white");
		if (white != null)
		{
			//_logger.LogContent.Clear();
			//_logger.LogContent.Add($"{white.MassCenter}");
			//_logger.LogContent.Add($"{white.Velocity}");
		}
		//_canvas.Render(renderable.ToList());
		canvas.UpdateShapes();
		//_canvas.Render(_gameData.VectorAnalitics);
	}

	public override void Log() { }
}