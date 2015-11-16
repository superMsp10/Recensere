using UnityEngine;
using System.Collections;

public class GameManeger : MonoBehaviour
{
		public static GameManeger thisM;
		public	static float speedToDamageMultiplier = 1f;
		public static byte Version = 2;
		public LayerMask def;


		//PlayerStuff------------------------------------------//

		public GameObject playerInstantiate;
		public player myPlayer;

		//LevelStuf------------------------------------------//
		public Level currLevel;
		public PhotonView p;


		void Awake ()
		{
				if (thisM == null) {
						thisM = this;
						DontDestroyOnLoad (gameObject);

				} else {
//						Destroy (gameObject);
				}

		}
		

	
		// Update is called once per frame
//		void Update ()
//		{
//				if (Input.GetMouseButton (0)) {
//						//	p.RPC ("message", PhotonTargets.All);
//				}
//	
//		}


		public void sendFloorTileDamage (float damage, string attacker, int x, int y)
		{
//				Debug.Log ("send floor dmg");
				p.RPC ("syncFloorTileDamage", PhotonTargets.OthersBuffered, damage, attacker, x, y);
		
		}

		public void sendWallTileDamage (float damage, string attacker, int x, int y, bool yWall)
		{
//				Debug.Log ("send wall dmg");
				p.RPC ("syncWallTileDamage", PhotonTargets.OthersBuffered, damage, attacker, x, y, yWall);

		
		}


		[PunRPC]
		public void syncFloorTileDamage (float damage, string attacker, int x, int y)
		{
//				Debug.Log ("sync floor");
				currLevel.liveTiles [x, y].syncDamage (damage, attacker);

		}

		[PunRPC]
		public void syncWallTileDamage (float damage, string attacker, int x, int y, bool yWall)
		{
//				Debug.Log ("sync wall");
				floorTile t = (floorTile)currLevel.liveTiles [x, y];
				if (yWall) {
						t.yTile.syncDamage (damage, attacker);
				} else {
						t.xTile.syncDamage (damage, attacker);

				}


		
		}

		public	void instantiatePlayer ()
		{
				GameObject p;
				int playerID = PhotonNetwork.countOfPlayers;

				p = PhotonNetwork.Instantiate (playerInstantiate.name, 
		                               FindObjectsOfType<SpawnSpot> () [playerID % 4].transform.position,
		                               Quaternion.identity, 0, null);

				myPlayer = p.GetComponent<player> ();
				myPlayer.playerID = playerID;
				myPlayer.transform.FindChild ("Graphics").GetComponent<Renderer> ().material.color = player.getPlayerColour (playerID);
				p.layer = def;

				Respwan ();
		}

		public void Respwan ()
		{
				currLevel.cam.SetActive (false);
				myPlayer.networkInit ();

		}

		public void NetworkDisable ()
		{
				currLevel.cam.SetActive (true);
				myPlayer.networkDisable ();
		}



}
