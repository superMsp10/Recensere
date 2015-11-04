using UnityEngine;
using System.Collections;

public  class tileDictionary : MonoBehaviour
{

		public static tileDictionary thisM;
		//----------------Tiles-------------------//
		public   Tile floorTile;
		public   Tile wallTile;
		public   Tile border;
		public   GameObject destroyedFloorTile;
		public   GameObject destroyedWallTile;

		//----------------Other Stuff-------------------//
		public SpawnSpot spawnSpot;
		public GameObject hitDecal;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

}
