using UnityEngine;
using System.Collections;

public class LootTile : MonoBehaviour
{
		public GameObject generate;
		public Transform position;

		public	void generateLoot ()
		{
				GameObject g = (GameObject)Instantiate (generate, position.position, Quaternion.identity);
				g.SetActive (true);

		}
	
}

