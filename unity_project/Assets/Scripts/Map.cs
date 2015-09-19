using UnityEngine;
using System.Collections.Generic;

public abstract class Map
{
		public static defaultArena firstMap = new defaultArena (17);
		public  int arenaSize;
		public List<Tile> tileTypes;

		public Map (int size)
		{
				arenaSize = size;
		}

		public GameObject getTile (int index)
		{
				GameObject g = null;
				if (index > 0 && index < tileTypes.Count) {
						g = tileTypes [index];

				}
				return GameObject.Instantiate (g);
		}


		
}
