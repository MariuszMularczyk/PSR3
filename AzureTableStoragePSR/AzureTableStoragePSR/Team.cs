using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzureTableStoragePSR
{
	class Team : TableEntity
	{

		public string Name { get; set; }
		public int Wins { get; set; }
		public int Looses { get; set; }
		public int Draws { get; set; }




		public Team()
		{
			PartitionKey = "Teams";
			RowKey = Guid.NewGuid().ToString();
		}

		override public string ToString()
		{
			return "nazwa " + Name + ", wygrane " + Wins + ", remisy: " + Draws + ", przegrane: " + Looses;
		}
	}
}
