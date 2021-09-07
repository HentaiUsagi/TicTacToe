using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.RecvCommands
{
	class RecvStepCommand
	{
		public string Id { get; set; }
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }
	}
}
