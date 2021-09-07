using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TicTacToe;
using TicTacToe.SendCommands;
using System.Threading;

namespace TicTacToe.RecvCommands
{
	class RecvCommandHandler
	{
		static public (bool, string) ProcessCommand(ClientObject client, RecvCommand command)
		{
			CommandTypeId id = (CommandTypeId)command.CommandType;

			switch (id)
			{
				case CommandTypeId.REGISTER: return Register(client, command);
				case CommandTypeId.GAME_START: return StartGame(client, command);
				case CommandTypeId.GAMER_STEP: return Step(client, command);
				case CommandTypeId.GAMER_WINNER: return Winner(client, command);
				//case CommandTypeId.RECOVERY_CONNECT: return RecoveryConnect(client, command);
			}

			return (false, "");
		}

		private static (bool, string) Register(ClientObject client, RecvCommand command)
		{
			RecvRegisterCommand clientMessage = JsonSerializer.Deserialize<RecvRegisterCommand>(command.JsonInfo);                                                                         // read info from client
			client.Id = clientMessage.Id;

			Console.WriteLine("Connect: {0}", client.Id);

			return (true, SendCommandHandler.SendGamerReady(true));
		}

		private static (bool, string) StartGame(ClientObject client, RecvCommand command)
		{
			RecvStartGameCommand serverMessage = JsonSerializer.Deserialize<RecvStartGameCommand>(command.JsonInfo);
			Console.WriteLine("Start Game");

			client.Game = new GameProcess(serverMessage.Sign, serverMessage.Id, 
										  serverMessage.Grid, serverMessage.IndexGamerStepping, client);

			//client.Game.Start();
			Thread thread = new Thread(client.Game.Start);
			thread.Start();

			return (false, "");
		}

		private static (bool, string) Step(ClientObject client, RecvCommand command)
		{
			RecvStepCommand serverMessage = JsonSerializer.Deserialize<RecvStepCommand>(command.JsonInfo);
			client.Game.StepUpdate(serverMessage.Grid, serverMessage.IndexGamerStepping);
			
			return (false, "");
		}

		private static (bool, string) Winner(ClientObject client, RecvCommand command)
		{
			RecvWinnerCommand serverMessage = JsonSerializer.Deserialize<RecvWinnerCommand>(command.JsonInfo);
			client.Game.PrintWinner(serverMessage.Id);

			return (false, "");
		}
	}
}
