using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.SendCommands
{
	class SendRegisterCommand
	{
		public string Id { get; set; }

		public SendRegisterCommand(string id) => Id = id; 
	}
}
