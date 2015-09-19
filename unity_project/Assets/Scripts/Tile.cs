using UnityEngine;
using System.Collections;

public abstract class Tile
{
		public static int tileSize = 4;
		//----------------Tiles-------------------//
		public static DefaultTile floorTile;
		public static DefaultTile wallTile;

		public Tile (int size)
		{
				tileSize = size;
		}
	
}
