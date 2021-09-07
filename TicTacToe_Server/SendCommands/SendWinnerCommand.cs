using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendWinnerCommand
	{
		public string Id { get; set; }

		public SendWinnerCommand() {}

		public SendWinnerCommand(string id) => Id = id;
	}
}
