using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	class ClientInfo
	{
		public string Id { get; set; }
		public bool IsCanStep { get; set; }
		public bool Sign { get; set; }

		public ClientInfo(string id, bool sign = false, bool isCanStep = false)
		{
			Id = id;
			Sign = sign;
			IsCanStep = isCanStep;
		}
	}
}
