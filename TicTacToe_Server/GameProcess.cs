using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	class GameProcess
	{
		private bool[] sign = new bool[2];
		private string[] id = new string[2];
		private string winnerId;
		private int[] grid = new int[9];
		private int indexGamerStepping;

		public GameProcess() {}

		public void SetParams(List<ClientObject> clients)
		{
			id[0] = clients[0].Id;
			id[1] = clients[1].Id;
			sign[0] = clients[0].Sign;
			sign[1] = clients[1].Sign;
			indexGamerStepping = 0;
			winnerId = "";

			for (int i = 0; i < 9; i++) grid[i] = -1;
		}

		public void RecoveryInfo(GameInfo info)
		{
			id[0] = info.Id[0];
			id[1] = info.Id[1];
			sign[0] = info.Sign[0];
			sign[1] = info.Sign[1];
			indexGamerStepping = info.IndexGamerStepping;
			winnerId = info.winnerId;

			for (int i = 0; i < 9; i++) grid[i] = -1;
		}

		public GameInfo GetGameInfo()
		{
			return new GameInfo(id, grid, sign, indexGamerStepping);
		}

		public void StepUpdate(string id, int[] grid, int indexGamerStepping)
		{
			this.grid = grid;

			if (CheckIsGameEnded())
			{
				winnerId = id;
				Console.WriteLine("{0} is winner", id);
			}
			else
			{
				if (indexGamerStepping == 0) this.indexGamerStepping = 1;
				else if (indexGamerStepping == 1) this.indexGamerStepping = 0;
			}
		}

		public bool CheckIsGameEnded()
		{
			if (grid[0] == grid[1] && grid[1] == grid[2] && grid[0] != -1) return true;
			else if (grid[3] == grid[4] && grid[4] == grid[5] && grid[3] != -1) return true;
			else if (grid[6] == grid[7] && grid[7] == grid[8] && grid[6] != -1) return true;
			else if (grid[0] == grid[3] && grid[3] == grid[6] && grid[0] != -1) return true;
			else if (grid[1] == grid[4] && grid[4] == grid[7] && grid[1] != -1) return true;
			else if (grid[2] == grid[5] && grid[5] == grid[8] && grid[2] != -1) return true;
			else if (grid[0] == grid[4] && grid[4] == grid[8] && grid[0] != -1) return true;
			else if (grid[2] == grid[4] && grid[4] == grid[6] && grid[2] != -1) return true;
			else return false;
		}
	}
}
