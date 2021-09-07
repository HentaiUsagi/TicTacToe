using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.Json;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] ports = ConvertArgs(args[0]);
            int[] priorities = ConvertArgs(args[1]);

            ClientObject client = new ClientObject(ports, priorities);
            client.Process();
            Console.WriteLine("- main");
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

/*
SendRegisterCommand sendRegCommand = new SendRegisterCommand("Oleg");
string jsonSendRegCommand = JsonSerializer.Serialize<SendRegisterCommand>(sendRegCommand);
SendCommand sendCommand = new SendCommand(0, jsonSendRegCommand);
string jsonSendCommand = JsonSerializer.Serialize<SendCommand>(sendCommand);
 */
