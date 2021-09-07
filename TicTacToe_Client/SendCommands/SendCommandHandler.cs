using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TicTacToe.SendCommands;
using TicTacToe.RecvCommands;

namespace TicTacToe.SendCommands
{
	class SendCommandHandler
	{
		static public string SendRegister(string id)
		{
			SendRegisterCommand clientMessage = new SendRegisterCommand(id);
			string jsonClientMessage = JsonSerializer.Serialize<SendRegisterCommand>(clientMessage);
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.REGISTER, jsonClientMessage);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			return jsonSendCommand;
		}

		public static string SendGamerReady(bool isReady)
		{
			SendReadyCommand clientMessage = new SendReadyCommand(isReady);
			string jsonClientMessage = JsonSerializer.Serialize<SendReadyCommand>(clientMessage);
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAMER_READY, jsonClientMessage);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			return jsonSendCommand;
		}

		public static string SendStep(string id, int[] grid, int indexGamerStepping)
		{
			SendStepCommand clientMessage = new SendStepCommand(id, grid, indexGamerStepping);
			string jsonClientMessage = JsonSerializer.Serialize<SendStepCommand>(clientMessage);
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.GAMER_STEP, jsonClientMessage);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			return jsonSendCommand;
		}

		public static string SendRecoveryConnect(string id)
		{
			SendRecoveryConnect clientMessage = new SendRecoveryConnect(id);
			string jsonClientMessage = JsonSerializer.Serialize<SendRecoveryConnect>(clientMessage);
			SendCommand sendCommand = new SendCommand((int)CommandTypeId.RECOVERY_CONNECT, jsonClientMessage);
			string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);

			return jsonSendCommand;
		}
	}
}
