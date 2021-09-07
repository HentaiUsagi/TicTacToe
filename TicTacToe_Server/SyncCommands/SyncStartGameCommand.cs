using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SyncCommands
{
	class SyncStartGameCommand
	{
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }
		public string[] Id { get; set; }
		public bool[] Sign { get; set; }

		public SyncStartGameCommand() {}

		public SyncStartGameCommand(GameInfo info)
		{
			Id = info.Id;
			Grid = info.Grid;
			Sign = info.Sign;
			IndexGamerStepping = info.IndexGamerStepping;
		}
	}
}
