using UnityEngine;
using System.Collections.Generic;

public class DestroyedTileManager : MonoBehaviour
{

		public static DestroyedTileManager thisM;
		private GameObject destroyedWallFX;
	
		[HideInInspector]
		public Pooler
				destroyedWallPooler = null;
	
		public float wallReset;
		public int maxWalls;
	
	
		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}
	
		void Start ()
		{
				destroyedWallFX = tileDictionary.thisM.destroyedWallTile;	
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
