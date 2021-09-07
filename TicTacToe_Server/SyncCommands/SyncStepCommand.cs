using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SyncCommands
{
	class SyncStepCommand
	{
		public string Id { get; set; }
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }

		public SyncStepCommand() {}

		public SyncStepCommand(GameInfo info)
		{
			Grid = info.Grid;
			IndexGamerStepping = info.IndexGamerStepping;
		}
	}
}
