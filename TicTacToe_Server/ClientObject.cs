using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;
using System.Text.Json;

namespace TicTacToe
{
	class ClientObject
	{
		public string Id { get; set; }
		public bool IsCanStep { get; set; }
		public bool IsReady { get; set; }
		public bool Sign { get; set; }
		public GameProcess game = new GameProcess();

		protected internal ServerObject Server { get; private set; }
		protected internal TcpClient client;
		protected internal NetworkStream Stream { get; private set; }
		//protected internal List<RecvCommand> recvQueue = new List<RecvCommand>();

		public ClientObject(TcpClient client, ServerObject server)
		{ 
			this.client = client;
			Server = server;
			IsCanStep = false;
			Sign = false;
			//game = new GameProcess();
		}

		public void Process()
		{
			string message;

			try
			{
				Stream = client.GetStream();

				while (true)
				{
					try
					{
						message = GetMessage();
						//Console.WriteLine("message {0}: {1}\n", Id, message);
						RecvCommand recvCommand = JsonSerializer.Deserialize<RecvCommand>(message);
						(int, string) sendCommand = RecvCommandHandler.ProcessCommand(this, recvCommand);
						SendMessage(sendCommand);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						Disconnect();
						break;
					}
				}
			}
			catch
			{
				Disconnect();
			}

			Server.RemoveConnection(Id);
		}

		private string GetMessage()
		{
			int bytes;
			byte[] data = new byte[512];
			StringBuilder builder = new StringBuilder();

			do
			{
				bytes = Stream.Read(data, 0, data.Length);
				builder.Append(Encoding.Unicode.GetString(data, 0, bytes));

				if (builder.Length > 512) { throw new Exception($"{Id} send large packet"); }
			}
			while (Stream.DataAvailable);

			return builder.ToString();
		}

		public void SendMessage((int, string) message)
		{
			try
			{
				if (message.Item1 != -1)
				{
					switch (message.Item1)
					{
						case 0:
							byte[] data = Encoding.Unicode.GetBytes(message.Item2);
							Stream.Write(data, 0, data.Length);
							break;
						case 1:
							Server.BroadcastMessage(message.Item2);
							break;
						case 2:
							Server.BroadcastMessage(message.Item2, Id);
							break;
					}
				}
			}
			catch 
			{
				Console.WriteLine("Error: SendMessage");
			}
		}

		protected internal void Disconnect()
		{
			Console.WriteLine("Disconnect: " + Id);

			if (Stream != null) { Stream.Close(); }
			if (client != null) { Server.RemoveConnection(Id); }
		}
	}
}
