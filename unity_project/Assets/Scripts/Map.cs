using UnityEngine;
using System.Collections.Generic;

public abstract class Map
{
		public static defaultArena firstMap = new defaultArena (17, new string[]{ "Grenade", "Cube (1)", "PlaceableTileItem" });
		public  int arenaSize;
		public List<Tile> tileTypes = new List<Tile> ();
		public List<Tile> wallTypes = new List<Tile> ();
		public string[] loot;


		public Map (int size)
		{
				arenaSize = size;
		}
		public Map (int size, string[] loots)
		{
				arenaSize = size;
				loot = loots;
		}

		public  virtual Tile getTile (int index)
		{
				if (index > 0 && index < tileTypes.Count) {
						return tileTypes [index];

				}
				return null;
		}
		public  virtual Tile getWall (int index)
		{
				if (index > 0 && index < wallTypes.Count) {
						return wallTypes [index];
			
				}
				return null;
		}



		
}
