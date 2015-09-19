using UnityEngine;
using System.Collections;

public class defaultArena : Map
{
		public defaultArena (int size):base(size)
		{
				arenaSize = size;
				tileTypes.Add (DefaultTile.wallTile);
		}

}
