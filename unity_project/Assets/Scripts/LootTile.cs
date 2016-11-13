using UnityEngine;
using System.Collections;

public class LootTile : MonoBehaviour
{
		public Transform position;
		protected Map thisMap;


		void Start ()
		{
				thisMap = Map.firstMap;
		}
		public virtual	void generateLoot ()
		{
				if (thisMap != null) {
						GameObject g = (GameObject)PhotonNetwork.InstantiateSceneObject (thisMap.loot [Random.Range (0, thisMap.loot.Length)], position.position, Quaternion.identity, 0, null);
						g.SetActive (true);
				} else {
						Debug.Log ("No Map, cannot generate loot");
				}

		}

    public virtual void NetworkInit()
    {

    }

}

