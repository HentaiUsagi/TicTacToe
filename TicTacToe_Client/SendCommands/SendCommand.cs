using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendCommand
	{
		public int CommandType { get; set; }
		public string JsonInfo { get; set; }

		public SendCommand(int commandType, string jsonInfo = "")
		{
			CommandType = commandType;
			JsonInfo = jsonInfo;
		}
	}
}
