using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe.SyncCommands;

namespace TicTacToe.SyncCommands
{
	class SyncRegisterCommand : SyncCommand
	{
		public string Id { get; set; }

		public SyncRegisterCommand() {}

		public SyncRegisterCommand(string id) => Id = id;
	}
}
