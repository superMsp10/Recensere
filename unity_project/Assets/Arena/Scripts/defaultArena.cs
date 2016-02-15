﻿using UnityEngine;
using System.Collections;

public class defaultArena : Map
{
		public defaultArena (int size, string[] loot):base(size,loot)
		{
				tileTypes.Add (tileDictionary.thisM.floorTile);
				wallTypes.Add (tileDictionary.thisM.wallTile);

				
		}

		public override Tile getTile (int index)
		{
				
				return tileTypes [0];
		}
		public override Tile getWall (int index)
		{
		
				return wallTypes [0];
		}


}