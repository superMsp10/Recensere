using UnityEngine;
using System.Collections.Generic;

public class DestroyedTileManager : MonoBehaviour
{

		public static DestroyedTileManager thisWall;
		public static DestroyedTileManager thisFloor;

		private GameObject destroyedWallFX;
	
		[HideInInspector]
		public Pooler
				destroyedWallPooler = null;
	
		public float wallReset;
		public int maxWalls;
		public bool floor;
	
	
		void Awake ()
		{
				if (!floor) {
						if (thisWall == null)
								thisWall = this;
				} else {
						if (thisFloor == null)
								thisFloor = this;
				}
		
		}
	
		void Start ()
		{
				if (!floor) {
						destroyedWallFX = tileDictionary.thisM.destroyedWallTile;	

				} else {
						destroyedWallFX = tileDictionary.thisM.destroyedFloorTile;	

				}
				destroyedWallPooler = new Pooler (maxWalls, destroyedWallFX);
		
		}
	
	
		public void addDestroyedWall (Vector3 pos, Quaternion rotation)
		{
		
				if (destroyedWallFX == null) {
						Debug.Log ("No destoyedWallFX");
						return;
				}
		
			
				GameObject g = destroyedWallPooler.getObject ();
				g.transform.parent = transform;
		
				g.transform.position = pos;
				g.transform.rotation = rotation;
				g.GetComponent<Timer> ().StartTimer (wallReset);
		
		}
}
