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

		public Tile[,] generateWall (Transform levStart)
		{
		
				Tile[,] genTiles = new Tile[thisMap.arenaSize + 2, thisMap.arenaSize + 2];
				Tile t;
				Vector3 pos;
				GameObject g;
				for (int x = 0; x < genTiles.GetLength(0); x++) {
						for (int y = 0; y < genTiles.GetLength(1); y++) {
//								t = thisMap.getWall (0);
//								pos = new Vector3 (x * t.tileSize, t.tileSize, y * t.tileSize);
//								g = (GameObject)GameObject.Instantiate (t.gameObject, pos, t.transform.rotation);
//								//								t = g.GetComponent<Tile> ();
//								genTiles [x, y] = g.GetComponent<Tile> ();
//								g.name = "wall" + (x + (y * thisMap.arenaSize)).ToString ();
//								g.transform.SetParent (levStart, false);
//								rotateWall (g);
//				
								if (y == 0) {
										t = thisMap.getWall (0);
										pos = new Vector3 (x * t.tileSize, t.tileSize, y * t.tileSize);
										g = (GameObject)GameObject.Instantiate (t.gameObject, pos, t.transform.rotation);
										//								t = g.GetComponent<Tile> ();
										genTiles [x, y] = g.GetComponent<Tile> ();
										g.name = "wall" + (x + (y * thisMap.arenaSize)).ToString ();
										g.transform.SetParent (levStart, false);
										//rotateWall (g);
								} else if (x == 0) {
										t = thisMap.getWall (0);
										pos = new Vector3 (x * t.tileSize, t.tileSize, y * t.tileSize);
										g = (GameObject)GameObject.Instantiate (t.gameObject, pos, new Quaternion (0, 90, 0, 90));
										//								t = g.GetComponent<Tile> ();
										genTiles [x, y] = g.GetComponent<Tile> ();
										g.name = "wall" + (x + (y * thisMap.arenaSize)).ToString ();
										g.transform.SetParent (levStart, false);
										//rotateWall (g);
								} else if (y == genTiles.GetLength (1) - 2) {
										t = thisMap.getWall (0);
										pos = new Vector3 (x * t.tileSize, t.tileSize, y * t.tileSize);
										g = (GameObject)GameObject.Instantiate (t.gameObject, pos, new Quaternion (0, 0, 0, 0));
										//								t = g.GetComponent<Tile> ();
										genTiles [x, y] = g.GetComponent<Tile> ();
										g.name = "wall" + (x + (y * thisMap.arenaSize)).ToString ();
										g.transform.SetParent (levStart, false);
										//rotateWall (g);
								} else if (x == genTiles.GetLength (0) - 2) {
										t = thisMap.getWall (0);
										pos = new Vector3 (x * t.tileSize, t.tileSize, y * t.tileSize);
										g = (GameObject)GameObject.Instantiate (t.gameObject, pos, new Quaternion (0, 90, 0, 90));
										//								t = g.GetComponent<Tile> ();
										genTiles [x, y] = g.GetComponent<Tile> ();
										g.name = "wall" + (x + (y * thisMap.arenaSize)).ToString ();
										g.transform.SetParent (levStart, false);
										//rotateWall (g);
								}
						}
				}
				return genTiles;
		}



		void rotateWall (GameObject g)
		{
				int i = Random.Range (0, 3);
				int rotation = 90;
				
				switch (i) {
	
				case 0:
						rotation = 0;
						break;
				case 1:
						rotation = 180;
						break;
				case 2:
						rotation = 270;
						break;

		
						rotation = 0;
				}
				g.transform.rotation = new Quaternion (0, -rotation, 0, rotation);

		}

}
