using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using TicTacToe.RecvCommands;
using System.Text.Json;

namespace TicTacToe
{
	class ServerObject
	{
		private int port;
		private int priority;
		private bool status;

		public int Port => port;
		public int Priority => priority;

		public ServerObject(int port, int priority, bool status)
		{
			this.port = port;
			this.priority = priority;
			this.status = status;
		}

		public void SetStatus(bool status)
		{
			this.status = status;
		}

		public bool GetStatus()
		{
			return status;
		}
	}
}
