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

		public Tile[,] generateMap (Transform levStart)
		{

				Tile[,] genTiles = new Tile[thisMap.arenaSize, thisMap.arenaSize];
				Tile t;
				Vector3 pos;
				GameObject g;
				for (int x = 0; x < thisMap.arenaSize; x++) {
						for (int y = 0; y < thisMap.arenaSize; y++) {
								t = thisMap.getTile (0);
								pos = new Vector3 (x * t.tileSize, 0, y * t.tileSize);
								g = (GameObject)GameObject.Instantiate (t.gameObject, pos, t.transform.rotation);
//								t = g.GetComponent<Tile> ();
								genTiles [x, y] = g.GetComponent<Tile> ();
								g.name = (x + (y * thisMap.arenaSize)).ToString ();
								g.transform.SetParent (levStart, false);


						}
				}
				return genTiles;
		}
}
