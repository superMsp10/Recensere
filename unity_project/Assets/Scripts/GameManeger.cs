using UnityEngine;
using System.Collections;

public class GameManeger : MonoBehaviour
{
		public static GameManeger thisM;
		public GameObject menuCam;

		//PlayerStuff------------------------------------------//

		public GameObject playerInstantiate;
		public player myPlayer;

		//LevelStuf------------------------------------------//

		public Transform levelStart;
		private SpawnSpot[] SS;
		private SpawnSpot MySS;
		public Tile[,] liveTiles;
		public Tile[,] liveWalls;


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
				liveTiles = gen.generateWall (levelStart);


		}

		public void OnConnected ()
		{
				//Debug.Log ("Connected in GameManager");
				generateArena ();
				instantiatePlayer ();

		}

		void instantiatePlayer ()
		{
				GameObject p;
				SS = FindObjectsOfType<SpawnSpot> ();
				int playerID = PhotonNetwork.countOfPlayers;
				MySS = SS [playerID % 4];

				p = PhotonNetwork.Instantiate (playerInstantiate.name, MySS.transform.position, Quaternion.identity, 0, null);
				myPlayer = p.GetComponent<player> ();
				myPlayer.playerID = playerID;
				myPlayer.transform.FindChild ("Graphics").GetComponent<Renderer> ().material.color = player.getPlayerColour (playerID);

				Respwan ();

		
		}

		public void Respwan ()
		{
				menuCam.SetActive (false);
				myPlayer.networkInit ();

		}

		public void NetworkDisable ()
		{
				menuCam.SetActive (true);
				myPlayer.networkDisable ();
		}



}
