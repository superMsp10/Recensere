using UnityEngine;
using System.Collections.Generic;

public abstract class Map
{
		public static defaultArena firstMap = new defaultArena (17);
		public  int arenaSize;
		public List<Tile> tileTypes = new List<Tile> ();

		public Map (int size)
		{
				arenaSize = size;
		}

		public  virtual Tile getTile (int index)
		{
				if (index > 0 && index < tileTypes.Count) {
						return tileTypes [index];

				}
				return null;
		}


		
}
