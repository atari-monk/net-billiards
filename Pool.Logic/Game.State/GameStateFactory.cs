using Sim.Core;

namespace Pool.Logic;

public class GameStateFactory : IGameStateFactory
	{
		public IGameState GetBreak(IGameState gameState) =>
			new Break(gameState);

		public IGameState GetBreak(IBilliardGameMasterContext gameState) =>
			new Break(gameState);

		public IGameState GetOpenTable(IGameState gameState) =>
			new OpenTable(gameState);

		public IGameState GetOpenTable(IBilliardGameMasterContext gameState) =>
			new OpenTable(gameState);

		public IGameState GetTurn(IGameState gameState) =>
			new Turn(gameState);

		public IGameState GetTurn(IBilliardGameMasterContext gameState) =>
			new Turn(gameState);

		public IGameState GetWonGame(IGameState gameState) =>
			new WonGame(gameState);

		public IGameState GetWonGame(IBilliardGameMasterContext gameState) =>
			new WonGame(gameState);
	}