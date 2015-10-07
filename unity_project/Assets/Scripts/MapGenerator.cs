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

		enum Direction
		{
				North = 0,
				East = 90,
				South = 180,
				West = 270}
		;
		

		public Tile[,] generateMap (Transform levStart)
		{
				generateSpawnSpots (levStart, thisMap.getTile (0).tileSize);
				Tile[,] genTiles = new Tile[thisMap.arenaSize, thisMap.arenaSize];
				Tile t;
				Vector3 pos;
				GameObject g;
				GameObject trans = new GameObject ();

				Tile wall;
				wallTile currentWall;
				floorTile currentTile;
				GameObject trans2 = new GameObject ();
				trans2.name = "Walls";

				trans.transform.SetParent (levStart, false);
				generateBorders (levStart);
				wall = thisMap.getWall (0);
				t = thisMap.getTile (0);


				trans.name = "Tiles";
				trans.transform.SetParent (levStart, false);
				trans2.transform.SetParent (levStart, false);

				for (int x = 0; x < thisMap.arenaSize; x++) {
						for (int y = 0; y < thisMap.arenaSize; y++) {
								pos = new Vector3 (x * t.tileSize, 0, y * t.tileSize);
								g = (GameObject)GameObject.Instantiate (t.gameObject, pos, t.transform.rotation);
								currentTile = g.GetComponent<floorTile> ();
								currentTile.xPos = x;
								currentTile.yPos = y;

								genTiles [x, y] = currentTile;
								g.name = "X" + (x).ToString () + "Y" + (y).ToString ();
								g.transform.SetParent (trans.transform, false);

								if (y != 0) {
										g = (GameObject)GameObject.Instantiate (wall.gameObject, new Vector3 ((x) * wall.tileSize + 0.01f, wall.tileSize, y * wall.tileSize), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
										currentWall = currentTile.yTile = g.GetComponent<wallTile> ();
										currentWall.xPos = x;
										currentWall.yPos = y;
										currentWall.yWall = true;
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans2.transform, false);
								}
								if (x != 0) {
										g = (GameObject)GameObject.Instantiate (wall.gameObject, new Vector3 (x * wall.tileSize, wall.tileSize, (y) * wall.tileSize), new Quaternion (0, (float)Direction.East, 0, -(float)Direction.East));
										currentWall = currentTile.xTile = g.GetComponent<wallTile> ();
										currentWall.xPos = x;
										currentWall.yPos = y;
										currentWall.yWall = false;
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans2.transform, false);
								}

								
								
						}
				}
				return genTiles;
		}

		public void generateWall (Transform levStart)
		{
				Tile t;
				GameObject g;
				GameObject trans = new GameObject ();
				trans.name = "Walls";
				trans.transform.SetParent (levStart, false);
				generateBorders (levStart);
				t = thisMap.getWall (0);

				for (int x = 0; x < thisMap.arenaSize; x++) {
						for (int y = 0; y < thisMap.arenaSize; y++) {
								if (y != 0) {
										g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 ((x) * t.tileSize + 0.01f, t.tileSize, y * t.tileSize), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
										//	g.transform.localScale *= -1;
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans.transform, false);
								}
								if (x != 0) {
										g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 (x * t.tileSize, t.tileSize, (y) * t.tileSize), new Quaternion (0, (float)Direction.East, 0, -(float)Direction.East));
										//	g.transform.localScale *= -1;
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans.transform, false);
								}
						}
				}


		}

		void generateSpawnSpots (Transform levStart, int tileSize)
		{

				GameObject g;
				GameObject trans = new GameObject ();
				GameObject spawn = tileDictionary.thisM.spawnSpot.gameObject;
				trans.name = "_SpawnSpots";
				trans.transform.SetParent (levStart, false);
				g = (GameObject)GameObject.Instantiate (spawn, new Vector3 (0.5f, 5f, 0.5f), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
				//	g.transform.localScale *= -1;
				g.name = "SpawnSpot + " + 1;
				g.transform.SetParent (trans.transform, false);

				g = (GameObject)GameObject.Instantiate (spawn, new Vector3 ((thisMap.arenaSize * tileSize) - 0.5f, 5f, 0.5f), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
				//	g.transform.localScale *= -1;
				g.name = "SpawnSpot + " + 2;
				g.transform.SetParent (trans.transform, false);

				g = (GameObject)GameObject.Instantiate (spawn, new Vector3 ((thisMap.arenaSize * tileSize) - 0.5f, 5f, (thisMap.arenaSize * tileSize) - 0.5f), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
				//	g.transform.localScale *= -1;
				g.name = "SpawnSpot + " + 3;
				g.transform.SetParent (trans.transform, false);

				g = (GameObject)GameObject.Instantiate (spawn, new Vector3 (0.5f, 5f, (thisMap.arenaSize * tileSize) - 0.5f), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
				//	g.transform.localScale *= -1;
				g.name = "SpawnSpot + " + 4;
				g.transform.SetParent (trans.transform, false);


		}

		void generateBorders (Transform levStart)
		{
				GameObject g;
				GameObject trans = new GameObject ();
				trans.name = "Borders";
				trans.transform.SetParent (levStart, false);

				Tile t = tileDictionary.thisM.border;
				for (int x =0; x < thisMap.arenaSize; x++) {
						g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 (x * t.tileSize, t.tileSize, 0), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
						//	g.transform.localScale *= -1;
						g.name = "BorderX + " + x;
						g.transform.SetParent (trans.transform, false);

						g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 ((x) * t.tileSize, t.tileSize, thisMap.arenaSize * t.tileSize), new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
						//g.transform.localScale *= -1;
						g.name = "BorderX2 + " + x;
						g.transform.SetParent (trans.transform, false);

						g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 (0, t.tileSize, (x) * t.tileSize), new Quaternion (0, (float)Direction.East, 0, -(float)Direction.East));
						//		g.transform.localScale *= -1;
						g.name = "BorderY + " + x;
						g.transform.SetParent (trans.transform, false);

						g = (GameObject)GameObject.Instantiate (t.gameObject, new Vector3 (thisMap.arenaSize * t.tileSize, t.tileSize, (x) * t.tileSize), new Quaternion (0, (float)Direction.East, 0, -(float)Direction.East));
						//g.transform.localScale *= -1;
						g.name = "BorderY2 + " + x;
						g.transform.SetParent (trans.transform, false);

				}


		}



//		void rotateWall (GameObject g)
//		{
//				int i = Random.Range (0, 3);
//				int rotation = 90;
//				
//				switch (i) {
//	
//				case 0:
//						rotation = 0;
//						break;
//				case 1:
//						rotation = 180;
//						break;
//				case 2:
//						rotation = 270;
//						break;
//
//		
//						rotation = 0;
//				}
//				g.transform.rotation = new Quaternion (0, -rotation, 0, rotation);
//
//		}

}
