using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.RecvCommands
{
	class RecvCommand
	{
		public int CommandType { get; set; }
		public string JsonInfo { get; set; }

		public RecvCommand() {}
	}
}
