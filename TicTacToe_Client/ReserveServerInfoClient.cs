using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	class ReserveServerInfoClient
	{
		public string Id { get; private set; }
		public int RoomId { get; private set; }
		public bool PermitedToMove { get; private set; }

		public ReserveServerInfoClient(string id, int roomId = -1, bool permitedToMove = false)
		{
			Id = id;
			RoomId = roomId;
			PermitedToMove = permitedToMove;
		}
	}
}
