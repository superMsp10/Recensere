using UnityEngine;
using System.Collections;

public class GameManeger : MonoBehaviour
{
		public static GameManeger thisM;
		public GameObject menuCam;
		public GameObject playerInstantiate;
		private SpawnSpot[] SS;
		private SpawnSpot MySS;
		// Use this for initialization
		void Start ()
		{
				if (thisM == null)
						thisM = this;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void OnConnected ()
		{
				Debug.Log ("Connected in GameManager");
				instantiatePlayer ();
		}

		void instantiatePlayer ()
		{
				GameObject p;
				SS = FindObjectsOfType<SpawnSpot> ();
				MySS = SS [Random.Range (0, SS.Length)];

				menuCam.SetActive (false);
				p = PhotonNetwork.Instantiate (playerInstantiate.name, MySS.transform.position, Quaternion.identity, 0, null);
				player myPlayer = p.GetComponent<player> ();
				myPlayer.networkInit ();

		
		}
}
