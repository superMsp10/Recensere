using UnityEngine;
using System.Collections;

public class floorTile : Tile
{
		public wallTile yTile;
		public wallTile xTile;

		public floorTile (int size) : base (size)
		{
		}
}
