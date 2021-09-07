using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using TicTacToe;

namespace TicTacToe
{
	class Program
	{
        private static ServerObject server;
		private static Thread listenThread;

        static void Main(string[] args)
        {
            Console.Clear();

            int port = Convert.ToInt32(args[0]);
            int priority = Convert.ToInt32(args[1]);

            int[] ports = ConvertArgs(args[2]);
            int[] priorities = ConvertArgs(args[3]);
            
            try
            {
                server = new ServerObject(port, priority, ports, priorities);
                listenThread = new Thread(new ThreadStart(server.ListenClient));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                server.Disconnect();
            }
        }

        private static int[] ConvertArgs(string arg)
        {
            string[] argArrayStr = arg.Split('-');
            int[] argArrayNum = new int[argArrayStr.Length];

            for (uint i = 0; i < argArrayStr.Length; i++)
            {
                argArrayNum[i] = Convert.ToInt32(argArrayStr[i]);
            }

            return argArrayNum;
        }
	}
}
