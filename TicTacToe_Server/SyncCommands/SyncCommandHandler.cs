using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;

namespace TicTacToe.SyncCommands
{
	class SyncCommandHandler
	{
		public static string ProcessCommand(ServerObject server, RecvCommand command)
		{
			CommandTypeId commandType = (CommandTypeId)command.CommandType;

			switch (commandType)
			{
				case CommandTypeId.REGISTER: return Register(server, command);
				case CommandTypeId.GAME_START: return StartGame(server, command);
				case CommandTypeId.GAMER_STEP: return Step(server, command);
				case CommandTypeId.GAMER_WINNER: return Winner(server, command);
			}

			return "";
		}

		private static string Register(ServerObject server, RecvCommand command)
		{
			SyncRegisterCommand message = JsonSerializer.Deserialize<SyncRegisterCommand>(command.JsonInfo); // message from main server
			int indexClient = server.rsClientsInfo.FindIndex(cl => cl.Id == message.Id);                           //

			if (indexClient == -1)
			{
				ClientInfo client = new ClientInfo(message.Id);
				server.rsClientsInfo.Add(client);

				if (server.rsClientsInfo.Count == 1)
				{
					server.rsClientsInfo[0].IsCanStep = true;
					server.rsClientsInfo[0].Sign= true;
				}

				return "Sync Register: " + message.Id;
			}

			return "Sync Register: fail";
		}

		private static string StartGame(ServerObject server, RecvCommand command)
		{
			SyncStartGameCommand message = JsonSerializer.Deserialize<SyncStartGameCommand>(command.JsonInfo);
			server.rsGameInfo.SetInfo(message.Id, message.Grid, message.Sign, message.IndexGamerStepping);

			return "Sync StartGame";
		}

		private static string Step(ServerObject server, RecvCommand command)
		{
			SyncStepCommand message = JsonSerializer.Deserialize<SyncStepCommand>(command.JsonInfo);
			server.rsGameInfo.SetInfo(message.Grid, message.IndexGamerStepping);

			return "Sync Step";
		}

		private static string Winner(ServerObject server, RecvCommand command)
		{
			SyncWinnerCommand message = JsonSerializer.Deserialize<SyncWinnerCommand>(command.JsonInfo);
			server.rsGameInfo.SetWinner(message.Id);

			return "Sync Winner: " + server.rsGameInfo.winnerId;
		}
	}
}
