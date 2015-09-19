using UnityEngine;
using System.Collections;

public class GameManeger : MonoBehaviour
{
		public static GameManeger thisM;
		public GameObject menuCam;
		public GameObject playerInstantiate;
		public Transform levelStart;
		private SpawnSpot[] SS;
		private SpawnSpot MySS;
		public Tile[,] liveTiles;

		void Awake ()
		{
				if (thisM == null)
						thisM = this;

		}
		// Use this for initialization
		void Start ()
		{
				
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void generateArena ()
		{

				MapGenerator gen = new MapGenerator (Map.firstMap);
				liveTiles = gen.generateMap (levelStart);


		}

		public void OnConnected ()
		{
				//Debug.Log ("Connected in GameManager");
				instantiatePlayer ();
				generateArena ();
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
