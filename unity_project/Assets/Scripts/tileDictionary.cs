using UnityEngine;
using System.Collections;

public  class tileDictionary : MonoBehaviour
{

		public static tileDictionary thisM;
		//----------------Tiles-------------------//
		public   DefaultTile floorTile;
		public   DefaultTile wallTile;
		public   DefaultTile border;
		public SpawnSpot spawnSpot;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

}
