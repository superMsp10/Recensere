using UnityEngine;
using System.Collections;

public class HeightMapGenerator : MapGenerator
{
		float slope;
		public HeightMapGenerator (Map map, float slope):base(map)
		{
				this.slope = slope;
		}

		public virtual Tile[,] generateMap (Transform levStart)
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

								float xPoint = x;
								float yPoint = y;
								float xPow = Mathf.Pow (x - thisMap.arenaSize / 2, 2);
								float yPow = Mathf.Pow (y - thisMap.arenaSize / 2, 2);
								float hPoint = (slope * (xPow + yPow)) + ((-slope) 
										* (Mathf.Pow (thisMap.arenaSize / 2, 2) 
										+ Mathf.Pow (thisMap.arenaSize / 2, 2)));
//								Debug.Log ((-1f / 2f));
								pos = new Vector3 (xPoint * t.tileSize, hPoint, yPoint * t.tileSize);

								g = (GameObject)GameObject.Instantiate (t.gameObject, pos, t.transform.rotation);
								currentTile = g.GetComponent<floorTile> ();

				
								genTiles [x, y] = currentTile;
								g.name = "X" + (x).ToString () + "Y" + (y).ToString ();
								g.transform.SetParent (trans.transform, false);
								
								if (y != 0) {
						

										pos = new Vector3 ((xPoint) * wall.tileSize + 0.01f, wall.tileSize + hPoint, yPoint * wall.tileSize);

										g = (GameObject)GameObject.Instantiate (wall.gameObject, pos, new Quaternion (0, (float)Direction.North, 0, (float)Direction.North));
									
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans2.transform, false);
								}
								if (x != 0) {

										pos = new Vector3 (xPoint * wall.tileSize, wall.tileSize + hPoint, (yPoint) * wall.tileSize);

										g = (GameObject)GameObject.Instantiate (wall.gameObject, pos, new Quaternion (0, (float)Direction.East, 0, -(float)Direction.East));
									
										g.name = "walX + " + x + " walY + " + y;
										g.transform.SetParent (trans2.transform, false);
								}
				
				
				
						}
				}
				return genTiles;
		}
}

