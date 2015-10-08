using UnityEngine;
using System.Collections;

public class GameManeger : MonoBehaviour
{
		public static GameManeger thisM;
		public GameObject menuCam;
		public	static float speedToDamageMultiplier = 0.25f;


		//PlayerStuff------------------------------------------//

		public GameObject playerInstantiate;
		public player myPlayer;

		//LevelStuf------------------------------------------//

		public Transform levelStart;
		private SpawnSpot[] SS;
		private SpawnSpot MySS;
		public Tile[,] liveTiles;
		public PhotonView p;


		void Awake ()
		{
				if (thisM == null)
						thisM = this;

		}
		

	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetMouseButton (0)) {
						//	p.RPC ("message", PhotonTargets.All);
				}
	
		}


		public void sendFloorTileDamage (float damage, string attacker, int x, int y)
		{
				Debug.Log ("send floor dmg");
				p.RPC ("syncFloorTileDamage", PhotonTargets.OthersBuffered, damage, attacker, x, y);
		
		}

		public void sendWallTileDamage (float damage, string attacker, int x, int y, bool yWall)
		{
				Debug.Log ("send wall dmg");
				p.RPC ("syncWallTileDamage", PhotonTargets.OthersBuffered, damage, attacker, x, y, yWall);

		
		}


		[PunRPC]
		public void syncFloorTileDamage (float damage, string attacker, int x, int y)
		{
				Debug.Log ("sync floor");
				liveTiles [x, y].syncDamage (damage, attacker);

		}

		[PunRPC]
		public void syncWallTileDamage (float damage, string attacker, int x, int y, bool yWall)
		{
				Debug.Log ("sync wall");
				floorTile t = (floorTile)liveTiles [x, y];
				if (yWall) {
						t.yTile.syncDamage (damage, attacker);
				} else {
						t.xTile.syncDamage (damage, attacker);

				}


		
		}

		void generateArena ()
		{

				MapGenerator gen = new MapGenerator (Map.firstMap);
				liveTiles = gen.generateMap (levelStart);
				//	gen.generateWall (levelStart);


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
