using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendReadyCommand
	{
		public bool IsReady { get; set; }

		public SendReadyCommand() {}
		
		public SendReadyCommand(bool isReady) => IsReady = isReady; 
	}
}
