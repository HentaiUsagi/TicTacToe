using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendRecoveryConnect
	{
		string Id { get; set; }

		public SendRecoveryConnect() {}

		public SendRecoveryConnect(string id) => Id = id;
	}
}
