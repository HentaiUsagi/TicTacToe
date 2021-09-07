using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using TicTacToe.RecvCommands;
using TicTacToe.SendCommands;
using System.Threading;

namespace TicTacToe
{
	class GameProcess
	{
		private bool[] sign = new bool[2];
		private string[] id = new string[2];
		private int[] grid = new int[9];
		private int indexGamerStepping;
		private bool isCanStep;
		private bool isGameEnd;
		private string winner;
		ClientObject client;

		public GameProcess(bool[] sign, string[] id, int[] grid, int indexGamerStepping, ClientObject client)
		{
			this.id = id;
			this.sign = sign;
			this.grid = grid;
			this.indexGamerStepping = indexGamerStepping;
			this.client = client;
			isGameEnd = false;

			if (client.Id == id[indexGamerStepping]) isCanStep = true;
			else isCanStep = false;

			Render();
		}

		public void Start()
		{
			int i, j;
			string[] input;

			Render();

			while (!isGameEnd)
			{
				if (isCanStep)
				{
					Render();

					while (!isGameEnd)
					{

						Console.Write("Step > ");
						input = Console.ReadLine().Split(' ');

						if (input.Length == 1)
						{
							string command = input[0];

							if (command == "exit")
							{
								Console.WriteLine(command);
							}
							else
							{
								Console.WriteLine("Invalid input");
								continue;
							}
						}
						else if (input.Length == 2)
						{
							if (!CheckStepCorrect(input))
							{
								Console.WriteLine("Invalid input");
								continue;
							}

							i = Convert.ToInt32(input[0]) - 1;
							j = Convert.ToInt32(input[1]) - 1;

							if (CheckCellFree(i, j))
							{
								if (sign[indexGamerStepping]) grid[i * 3 + j] = 1;
								else grid[i * 3 + j] = 0;

								try
								{
									string jsonSendCommand = SendCommandHandler.SendStep(id[indexGamerStepping], grid, indexGamerStepping);
									client.SendMessage(jsonSendCommand);									
									break;
								}
								catch
								{
									Console.WriteLine("Error: GameProcess => Start");
								}
							}
							else
							{
								Console.WriteLine("Cell is already occupied");
								continue;
							}
						}
						else
						{
							Console.WriteLine("Invalid input");
							continue;
						}
					}

					isCanStep = false;
					Render();
				}
				else 
				{
					/*try
					{
						string message = client.GetMessage();
						RecvCommand recvCommand = JsonSerializer.Deserialize<RecvCommand>(message);
						(bool, string) sendCommand = RecvCommandHandler.ProcessCommand(client, recvCommand);

						if (sendCommand.Item1)
						{
							client.SendMessage(sendCommand.Item2);
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
						Console.ReadLine();
						break;
					}*/
				}
			}

			Render();
		}
		
		protected internal void StepUpdate(int[] grid, int indexGamerStepping)
		{
			this.grid = grid;
			this.indexGamerStepping = indexGamerStepping;

			if (client.Id == id[indexGamerStepping]) isCanStep = true;
			else isCanStep = false;
		}

		private bool CheckStepCorrect(string[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				for (int j = 0; j < input[i].Length; j++)
				{
					if ((input[i][j] < '1') || (input[i][j] > '3')) return false;
				}
			}

			return true;
		}

		private bool CheckCellFree(int i, int j)
		{
			if (grid[i * 3 + j] == -1) return true;
			else return false;
		}

		private void Render()
		{
			Console.Clear();

			if (!isGameEnd)
			{				
				Console.Write("Turn of: {0} - ", id[indexGamerStepping]);

				if (sign[indexGamerStepping]) Console.WriteLine("X");
				else Console.WriteLine("O");
			}
			else
			{
				Console.WriteLine("~ {0} is winner ~", winner);
			}

			Console.WriteLine();
			Console.WriteLine("    1   2   3 ");
			Console.WriteLine("   --- --- --- ");
			Console.WriteLine("1 | {0} | {1} | {2} |", RenderSign(grid[0]), RenderSign(grid[1]), RenderSign(grid[2]));
			Console.WriteLine("   --- --- --- ");
			Console.WriteLine("2 | {0} | {1} | {2} |", RenderSign(grid[3]), RenderSign(grid[4]), RenderSign(grid[5]));
			Console.WriteLine("   --- --- --- ");
			Console.WriteLine("3 | {0} | {1} | {2} |", RenderSign(grid[6]), RenderSign(grid[7]), RenderSign(grid[8]));
			Console.WriteLine("   --- --- --- ");
			Console.WriteLine();
		}

		private string RenderSign(int i)
		{
			switch (i)
			{
				case 0: return "O";
				case 1: return "X";
				default: return " ";
			}
		}

		public void PrintWinner(string id)
		{
			isGameEnd = true;
			winner = id;
		}
	}
}
