﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	class GameInfo
	{
		public int[] Grid { get; set; }
		public int IndexGamerStepping { get; set; }
		public string[] Id { get; set; }
		public bool[] Sign { get; set; }
		public string winnerId;

		public GameInfo() {}

		public GameInfo(string[] id, int[] grid, bool[] sign, int indexGamerStepping)
		{
			Id = id;
			Grid = grid;
			Sign = sign;
			IndexGamerStepping = indexGamerStepping;
			winnerId = "";
		}

		public void SetInfo(string[] id, int[] grid, bool[] sign, int indexGamerStepping)
		{
			Id = id;
			Grid = grid;
			Sign = sign;
			IndexGamerStepping = indexGamerStepping;
		}

		public void SetInfo(int[] grid, int indexGamerStepping)
		{
			Grid = grid;
			IndexGamerStepping = indexGamerStepping;
		}

		public void SetWinner(string winner) => winnerId = winner;
	}
}
