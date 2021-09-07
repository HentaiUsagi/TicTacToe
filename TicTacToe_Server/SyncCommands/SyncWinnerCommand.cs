using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SyncCommands
{
	class SyncWinnerCommand
	{
		public string Id { get; set; }

		public SyncWinnerCommand() {}

		public SyncWinnerCommand(string id) => Id = id;
	}
}
