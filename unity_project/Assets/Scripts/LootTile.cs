using UnityEngine;
using System.Collections;

public class LootTile : MonoBehaviour
{
		public Transform position;
		protected Map thisMap;

		public	void generateLoot ()
		{
				if (thisMap != null) {
						GameObject g = (GameObject)PhotonNetwork.Instantiate (thisMap.loot [0], position.position, Quaternion.identity, 0);
						g.SetActive (true);
				} else {
						Debug.Log ("No Map, cannot generate loot");
				}

		}
	
}
