using UnityEngine;
using System.Collections;

public  class tileDictionary : MonoBehaviour
{

		public static tileDictionary thisM;
		//----------------Tiles-------------------//
		public   DefaultTile floorTile;
		public   DefaultTile wallTile;
		public   DefaultTile border;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

}
