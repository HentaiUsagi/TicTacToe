using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;
using TicTacToe.SyncCommands;
using System.Text.Json;

namespace TicTacToe
{
	class ServerObject
	{
		private int port;
		private int serverPriority;     // current server priority
		private int mainServerPriority; // 

		private int[] ports;            // All servers ports
		private int[] priorities;       // All servers priorities

		protected internal List<ClientObject> clients = new List<ClientObject>();
		protected internal List<ClientObject> reserveServers = new List<ClientObject>();
		protected internal List<ClientInfo> rsClientsInfo = new List<ClientInfo>();  // info about clients for reserve server
		protected internal GameInfo rsGameInfo = new GameInfo();

		private TcpListener listener;
		private TcpClient reserveServer = null; // 
		private NetworkStream stream = null;    // reserve server stream
		
		public ServerObject() {}

		public ServerObject(int port, int priority, int[] ports, int[] priorities)
		{
			this.port = port;
			this.serverPriority = priority;
			this.ports = ports;
			this.priorities = priorities;
			mainServerPriority = priorities[0];
		}

		protected internal void RemoveConnection(string id)
		{
			ClientObject client = clients.FirstOrDefault(c => c.Id == id); // arg is lambda

			if (client != null) clients.Remove(client);
		}

		protected internal int GetClientIndex(ClientObject client)
		{
			if (clients[0] == client) return 0;
			else if (clients[1] == client) return 1;
			else return -1;
		}

		public void ListenClient()
		{
			try
			{
				listener = new TcpListener(IPAddress.Any, port);
				listener.Start();

				if (serverPriority != mainServerPriority) ConnectMainServer(mainServerPriority);
				else 
				{
					Console.WriteLine("Waiting for connections...\n");
					Console.WriteLine("port: {0}\n" +
									  "server prioriry: {1}\n" +
									  "main server priority: {2}\n", port, serverPriority, mainServerPriority);

					Console.WriteLine("main server\n"); 
				}

				while (true) 
				{
					if (clients.Count < 2)
					{
						TcpClient tcpClient = listener.AcceptTcpClient();
						ClientObject clientObject = new ClientObject(tcpClient, this);
						Thread thread = new Thread(clientObject.Process);
						thread.Start();
					}
					else { Console.WriteLine("Room are full"); }
				}
			}
			catch 
			{ 
				Disconnect(); 
			}
		}

		private void ConnectMainServer(int begin)
		{
			mainServerPriority = begin;
			bool isMain = false;

			for (int i = begin; i < priorities.Length; i++)
			{
				if (i == serverPriority) break;
				else
				{
					try
					{
						reserveServer = new TcpClient("127.0.0.1", ports[i]);
						stream = reserveServer.GetStream();
						isMain = true;
						break;
					}
					catch
					{
						Console.WriteLine("server {0} disabled", ports[i]);
						isMain = false;
					}
				}
			}

			Console.WriteLine("Waiting for connections...\n");
			Console.WriteLine("port: {0}\n" +
							  "server prioriry: {1}\n" +
							  "main server priority: {2}\n", port, serverPriority, mainServerPriority);

			if (isMain)
			{
				Console.WriteLine("reserve server");

				ServerConnectCommand command = new ServerConnectCommand(port, serverPriority);             //
				string jsonCommand = JsonSerializer.Serialize<ServerConnectCommand>(command);              // send message to main server
				SendCommand sendCommand = new SendCommand((int)CommandTypeId.SERVER_CONNECT, jsonCommand); // to connect with reserve server
				string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);               //

				byte[] data = Encoding.Unicode.GetBytes(jsonSendCommand);
				stream.Write(data, 0, data.Length);
				
				Thread thread = new Thread(ListenMainServer);
				thread.Start();
			}
			else { Console.WriteLine("main server\n"); }
		}

		private void ListenMainServer()
		{
			string message;

			while (true)
			{
				if (stream == null && !stream.DataAvailable) continue;

				try 
				{
					int bytes;
					byte[] data = new byte[512];
					StringBuilder builder = new StringBuilder();

					do
					{
						bytes = stream.Read(data, 0, data.Length);
						builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
					}
					while (stream.DataAvailable);

					message = builder.ToString();	
				}
				catch
				{
					ConnectMainServer(mainServerPriority + 1);
					break;
				}

				if (string.IsNullOrEmpty(message)) continue;

				try
				{
					RecvCommand recvCommand = JsonSerializer.Deserialize<RecvCommand>(message);
					string str = SyncCommandHandler.ProcessCommand(this, recvCommand);

					if (str == "Sync StartGame")
					{
						for (int i = 0; i < clients.Count; i++)
						{
							if (clients[i].game == null) Console.WriteLine("game is null");
						}
					}

					Console.WriteLine(str);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: {0}", ex.Message);
				}				
			}

			//foreach (RSInfoClient i in reserveServerClientInfo) { Console.WriteLine(i.Id); }
		}

		protected internal void BroadcastMessage(string message) // Send message to all clients
		{
			byte[] data = Encoding.Unicode.GetBytes(message);

			for (int i = 0; i < clients.Count; i++)
			{
				clients[i].Stream.Write(data, 0, data.Length);
			}
		}

		protected internal void BroadcastMessage(string message, string id) // Send message to all clients
		{
			byte[] data = Encoding.Unicode.GetBytes(message);

			for (int i = 0; i < clients.Count; i++)
			{
				if (clients[i].Id != id)
				{
					clients[i].Stream.Write(data, 0, data.Length);
				}
			}
		}

		protected internal void SyncServers(string message)
		{
			byte[] data = Encoding.Unicode.GetBytes(message);

			for (int i = 0; i < reserveServers.Count; i++)
			{
				try { reserveServers[i].Stream.Write(data, 0, data.Length); }
				catch { Console.WriteLine("Synchronization Error"); }
			}
		}

		protected internal void Disconnect()
		{
			listener.Stop();

			foreach (ClientObject i in clients) { i.Disconnect(); }

			Environment.Exit(0);
		}
	}
}
