using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;
using System.Text.Json;
using System.Threading;

namespace TicTacToe
{
	class ClientObject
	{
		private const string address = "127.0.0.1";
		private int[] ports;
		private int currServer;
		private bool isTriedRecover;

		public string Id { get; set; }
		public bool Sign { get; set; }
		public GameProcess Game { get; set; }

		private NetworkStream stream;
		private ServerObject mainServer;
		private List<ServerObject> servers = new List<ServerObject>();
		private TcpClient client;

		public ClientObject(int[] ports, int[] priorities)
		{ 
			this.ports = ports;
			currServer = 0;
			isTriedRecover = false;

			for (int i = 0; i < ports.Length; i++)
			{
				ServerObject server = new ServerObject(ports[i], priorities[i], true);
				servers.Add(server);
			}

			mainServer = servers[0];
		}

		public void Process()
		{
			try 
			{
				string message;
				client = new TcpClient(address, ports[currServer]);
				stream = client.GetStream();

				Console.Write("Enter name: ");
				string name = Console.ReadLine();

				message = SendCommandHandler.SendRegister(name);
				byte[] data = Encoding.Unicode.GetBytes(message);
				stream.Write(data, 0, data.Length);

				//Thread thread = new Thread(ListenServer);
				//thread.Start();

				ListenServer();
			}
			catch
			{
				if (!isTriedRecover)
				{
					mainServer.SetStatus(false);
					Reconnect();
				}
				else
				{
					Console.WriteLine("Error Process");
					Disconnect();
				}
			}
		}

		private void ListenServer()
		{
			string message;

			while (true)
			{
				try
				{
					message = GetMessage();
					RecvCommand recvCommand = JsonSerializer.Deserialize<RecvCommand>(message);
					(bool, string) sendCommand = RecvCommandHandler.ProcessCommand(this, recvCommand);

					if (sendCommand.Item1)
					{
						SendMessage(sendCommand.Item2);
					}
				}
				catch (Exception ex)
				{
					if (!isTriedRecover)
					{
						mainServer.SetStatus(false);
						Reconnect();
					}
					else
					{
						Console.WriteLine(ex.Message);
						Disconnect();
						break;
					}
				}
			}
		}

		protected internal string GetMessage()
		{
			try
			{
				byte[] data = new byte[512];
				StringBuilder builder = new StringBuilder();
				int bytes = 0;

				do
				{
					bytes = stream.Read(data, 0, data.Length);
					builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
				}
				while (stream.DataAvailable);

				return builder.ToString();
			}
			catch
			{
				if (!isTriedRecover)
				{
					mainServer.SetStatus(false);
					Reconnect();
				}

				return "";
			}
		}

		private void Reconnect()
		{
			isTriedRecover = true;
			Console.WriteLine("Try reconnect");
			try 
			{
				int index = servers.FindIndex(find => find.GetStatus() == true);

				if (index != -1)
				{
					mainServer = servers[index];

					client = new TcpClient();
					IAsyncResult result = client.BeginConnect(address, mainServer.Port, null, null);
					bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(500));

					if (!success)
					{
						throw new Exception("Failed to connect.");
					}

					client.EndConnect(result);
					stream = client.GetStream();

					string message = SendCommandHandler.SendRecoveryConnect(Id);
					byte[] data = Encoding.Unicode.GetBytes(message);
					stream.Write(data, 0, data.Length);

					isTriedRecover = false;
				}
				else
				{
					Console.WriteLine("No connection");
					Disconnect();
				}
			}
			catch { }
		}

		public bool CheckConnection()
		{
			return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
		}

		protected internal void SendMessage(string message)
		{
			byte[] data = Encoding.Unicode.GetBytes(message);
			stream.Write(data, 0, data.Length);
		}

		private void Disconnect()
		{
			if (stream != null) { stream.Close(); }
			if (client != null) { client.Close(); }
		}
	}
}
