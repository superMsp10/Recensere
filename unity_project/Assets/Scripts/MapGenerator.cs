using UnityEngine;
using System.Collections;

public class MapGenerator
{
		public static Transform startPos;
		Map thisMap;
		public MapGenerator (Map map)
		{
				thisMap = map;
		}

		public Tile[][] generateMap (Transform parent)
		{

				Tile[][] genTiles = null;
				for (int x = 0; x < thisMap.arenaSize; x++) {
						for (int y = 0; y < thisMap.arenaSize; y++) {
								Debug.Log ("hello");
						}
				}
				return genTiles;
		}
}
