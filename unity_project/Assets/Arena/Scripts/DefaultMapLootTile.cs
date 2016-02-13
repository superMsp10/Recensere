using UnityEngine;
using System.Collections;

public class DefaultMapLootTile : LootTile
{
		void Start ()
		{
				thisMap = Map.firstMap;
				Debug.Log ("hello");
		}

}

