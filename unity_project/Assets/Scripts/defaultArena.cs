using UnityEngine;
using System.Collections;

public class defaultArena : Map
{
		public defaultArena (int size):base(size)
		{
				arenaSize = size;
				tileTypes.Add (tileDictionary.thisM.floorTile);
				
		}

		public override Tile getTile (int index)
		{
				
				return tileTypes [0];
		}

}
