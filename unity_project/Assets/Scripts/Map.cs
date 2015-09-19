using UnityEngine;
using System.Collections.Generic;

public abstract class Map
{
		public static defaultArena firstMap = new defaultArena (17);
		public static int arenaSize;
		public List<Tile> tileTypes;

		public Map (int size)
		{
				arenaSize = size;
		}


		
}
