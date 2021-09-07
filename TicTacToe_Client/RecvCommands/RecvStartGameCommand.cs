using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class RecvStartGameCommand
	{
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }
		public string[] Id { get; set; }
		public bool[] Sign { get; set; }
	}
}
