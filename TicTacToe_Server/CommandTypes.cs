using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	public enum CommandTypeId
	{
		REGISTER = 0,
		SERVER_CONNECT = 1,
		GAME_START = 2,
		GAMER_READY = 3,
		GAMER_STEP = 4,
		GAMER_WINNER = 5,
		RECOVERY_CONNECT = 6
	}
}
