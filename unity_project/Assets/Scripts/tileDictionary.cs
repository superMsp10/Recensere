using UnityEngine;
using System.Collections;

public  class tileDictionary : MonoBehaviour
{

		public static tileDictionary thisM;
		//----------------Tiles-------------------//
		public   Tile floorTile;
		public   Tile wallTile;
		public   Tile border;
		public SpawnSpot spawnSpot;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

}
