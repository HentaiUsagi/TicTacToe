using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendStepCommand
	{
		public string Id { get; set; }
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }

		public SendStepCommand() {}

		public SendStepCommand(GameInfo info)
		{
			Grid = info.Grid;
			IndexGamerStepping = info.IndexGamerStepping;
		}
	}
}
