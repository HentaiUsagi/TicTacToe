using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe.RecvCommands
{
	class ServerConnectCommand
	{
		public int Port { get; set; }
		public int ServerPriority { get; set; }

		public ServerConnectCommand() {}

		public ServerConnectCommand(int port, int priority)
		{
			Port = port;
			ServerPriority = priority;
		}
	}
}
