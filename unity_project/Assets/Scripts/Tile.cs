using UnityEngine;
using System.Collections;

public abstract class Tile:MonoBehaviour
{
		public static int tileSize = 4;


		public Tile (int size)
		{
				tileSize = size;
		}
	
}
