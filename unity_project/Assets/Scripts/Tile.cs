using UnityEngine;
using System.Collections;

public abstract class Tile:MonoBehaviour
{
		public int tileSize = 4;


		public Tile (int size)
		{
				tileSize = size;
		}
	
}
