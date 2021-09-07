using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe;
using TicTacToe.SendCommands;
using TicTacToe.SyncCommands;
using System.Text.Json;

namespace TicTacToe.RecvCommands
{
	class RecvCommandHandler
	{
		static public (int, string) ProcessCommand(ClientObject client, RecvCommand command)
		{
			CommandTypeId id = (CommandTypeId)command.CommandType;

			switch (id)
			{
				case CommandTypeId.REGISTER: return Register(client, command);
				case CommandTypeId.SERVER_CONNECT: return ServerConnect(client, command);
				case CommandTypeId.GAMER_READY: return GamerReady(client, command);
				case CommandTypeId.GAMER_STEP: return Step(client, command);
				case CommandTypeId.RECOVERY_CONNECT: return RecoveryConnect(client, command);
			}

			return (-1, "");
		}

		private static (int, string) Register(ClientObject client, RecvCommand command)
		{
			if (client.Server.clients.Count >= 2) throw new Exception("Too many clients");

			RecvRegisterCommand clientMessage = JsonSerializer.Deserialize<RecvRegisterCommand>(command.JsonInfo); //
			client.Id = clientMessage.Id;                                                                          // read info from client
			client.Server.clients.Add(client);

			SyncRegisterCommand syncMessage = new SyncRegisterCommand(client.Id);                                  //
			string jsonSyncMessage = JsonSerializer.Serialize<SyncRegisterCommand>(syncMessage);                   //
			SendCommand sendSyncMessage = new SendCommand((int)CommandTypeId.REGISTER, jsonSyncMessage);           // synchronization servers
			string jsonSendSyncMessage = JsonSerializer.Serialize<SendCommand>(sendSyncMessage);                   //
			client.Server.SyncServers(jsonSendSyncMessage);                                                        //

			SendRegisterCommand serverMessage = new SendRegisterCommand(client.Id);                                //
			string jsonServerMessage = JsonSerializer.Serialize<SendRegisterCommand>(serverMessage);               // answer to client
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.REGISTER, jsonServerMessage);             //
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);                           //

			Console.WriteLine("Connect: {0}", client.Id);

			return (0, jsonSendCommand);
		}

		private static (int, string) ServerConnect(ClientObject client, RecvCommand command)
		{
			ServerConnectCommand clientMessage = JsonSerializer.Deserialize<ServerConnectCommand>(command.JsonInfo);
			client.Server.reserveServers.Add(client);

			SendCommand sendCommand = new SendCommand((int)CommandTypeId.SERVER_CONNECT);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			Console.WriteLine("Connect with: {0}, {1}", clientMessage.ServerPriority, clientMessage.Port);

			return (0, jsonSendCommand);
		}	

		private static (int, string) GamerReady(ClientObject client, RecvCommand command)
		{
			RecvReadyCommand clientMessage = JsonSerializer.Deserialize<RecvReadyCommand>(command.JsonInfo);

			if (client.Server.clients.Count == 1)
			{
				client.IsReady = clientMessage.IsReady;
				client.IsCanStep = true;
				client.Sign = true;
			}
			else if (client.Server.clients.Count == 2)
			{
				client.IsReady = clientMessage.IsReady;

				if (client.Server.clients[0].IsReady && client.Server.clients[1].IsReady) // StartGame
				{
					Console.WriteLine("Start Game");

					client.game.SetParams(client.Server.clients);

					SendStartGameCommand serverMessage = new SendStartGameCommand(client.game.GetGameInfo());
					string jsonServerMassage = JsonSerializer.Serialize<SendStartGameCommand>(serverMessage);
					SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAME_START, jsonServerMassage);
					string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

					SyncStartGameCommand syncMessage = new SyncStartGameCommand(client.game.GetGameInfo());         //
					string jsonSyncMessage = JsonSerializer.Serialize<SyncStartGameCommand>(syncMessage);           //
					SendCommand sendSyncMessage = new SendCommand((int)CommandTypeId.GAME_START, jsonSyncMessage);  // synchronization servers
					string jsonSendSyncMessage = JsonSerializer.Serialize<SendCommand>(sendSyncMessage);            //
					client.Server.SyncServers(jsonSendSyncMessage);

					return (1, jsonSendCommand);				
				}
			}
			else throw new Exception("GamerReady too many clients");

			return (0, "");
		}

		private static (int, string) Step(ClientObject client, RecvCommand command)
		{
			RecvStepCommand clientMessage = JsonSerializer.Deserialize<RecvStepCommand>(command.JsonInfo);
			client.game.StepUpdate(clientMessage.Id, clientMessage.Grid, clientMessage.IndexGamerStepping);

			if (client.game.CheckIsGameEnded())
			{
				{
					SendStepCommand serverMessage = new SendStepCommand(client.game.GetGameInfo());
					string jsonServerMassage = JsonSerializer.Serialize<SendStepCommand>(serverMessage);
					SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAMER_STEP, jsonServerMassage);
					string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

					SyncStepCommand syncMessage = new SyncStepCommand(client.game.GetGameInfo());                  //
					string jsonSyncMessage = JsonSerializer.Serialize<SyncStepCommand>(syncMessage);               //
					SendCommand sendSyncMessage = new SendCommand((int)CommandTypeId.GAMER_STEP, jsonSyncMessage); // synchronization servers
					string jsonSendSyncMessage = JsonSerializer.Serialize<SendCommand>(sendSyncMessage);           //
					client.Server.SyncServers(jsonSendSyncMessage);

					client.SendMessage((1, jsonSendCommand));
				}
				{
					SendWinnerCommand serverMessage = new SendWinnerCommand(clientMessage.Id);
					string jsonServerMassage = JsonSerializer.Serialize<SendWinnerCommand>(serverMessage);
					SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAMER_WINNER, jsonServerMassage);
					string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

					SyncWinnerCommand syncMessage = new SyncWinnerCommand(clientMessage.Id);                         //
					string jsonSyncMessage = JsonSerializer.Serialize<SyncWinnerCommand>(syncMessage);               //
					SendCommand sendSyncMessage = new SendCommand((int)CommandTypeId.GAMER_WINNER, jsonSyncMessage); // synchronization servers
					string jsonSendSyncMessage = JsonSerializer.Serialize<SendCommand>(sendSyncMessage);             //
					client.Server.SyncServers(jsonSendSyncMessage);

					client.SendMessage((1, jsonSendCommand));
					
					return (-1, "");
				}				
			}
			else
			{
				SendStepCommand serverMessage = new SendStepCommand(client.game.GetGameInfo());
				string jsonServerMassage = JsonSerializer.Serialize<SendStepCommand>(serverMessage);
				SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAMER_STEP, jsonServerMassage);
				string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

				SyncStepCommand syncMessage = new SyncStepCommand(client.game.GetGameInfo());                  //
				string jsonSyncMessage = JsonSerializer.Serialize<SyncStepCommand>(syncMessage);               //
				SendCommand sendSyncMessage = new SendCommand((int)CommandTypeId.GAMER_STEP, jsonSyncMessage); // synchronization servers
				string jsonSendSyncMessage = JsonSerializer.Serialize<SendCommand>(sendSyncMessage);           //
				client.Server.SyncServers(jsonSendSyncMessage);

				return (1, jsonSendCommand);
			}
		}

		private static (int, string) RecoveryConnect(ClientObject client, RecvCommand command)
		{
			RecvRecoveryConnect clientMessage = JsonSerializer.Deserialize<RecvRecoveryConnect>(command.JsonInfo);
			int index = client.Server.rsClientsInfo.FindIndex(find => find.Id == clientMessage.Id);

			Console.WriteLine("Recovery connect");

			if (index != -1)
			{
				ClientInfo info = client.Server.rsClientsInfo[index];

				client.Id = info.Id;
				client.Sign = info.Sign;
				client.IsCanStep = info.IsCanStep;

				client.Server.rsClientsInfo.Remove(info);
			}

			client.Server.clients.Add(client);
			client.game.RecoveryInfo(client.Server.rsGameInfo);

			SendConfRecoveryConnect serverMessage = new SendConfRecoveryConnect(); // client.Id
			string jsonServerMassage = JsonSerializer.Serialize<SendConfRecoveryConnect>(serverMessage);
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.RECOVERY_CONNECT, jsonServerMassage);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			return (0, jsonSendCommand);
		}
	}
}
