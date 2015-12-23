using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
		public static GameManager thisM;
		public static byte Version = 2;
		public LayerMask def;
		public bool paused = false;

		//PlayerStuff------------------------------------------//

		public GameObject playerInstantiate;
		public player myPlayer;
		public bool dead = true;

		//LevelStuf------------------------------------------//
		public Level currLevel;
		public	PhotonView p;
		public	static float speedToDamageMultiplier = 1f;

	
	
		void Awake ()
		{
				if (thisM == null) {
						thisM = this;
						DontDestroyOnLoad (gameObject);

				} else {
						Debug.Log ("GameManager Exists");
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
				if (currLevel.liveTiles [x, y] != null) {
						currLevel.liveTiles [x, y].syncDamage (damage, attacker);
				} else {
						Debug.Log ("X :" + x + ", Y :" + y + " damaged by :" + attacker + " after being destroyed");
				}

		}

		[PunRPC]
		public void syncWallTileDamage (float damage, string attacker, int x, int y, bool yWall)
		{
//				Debug.Log ("sync wall");
				floorTile t = (floorTile)currLevel.liveTiles [x, y];
				if (t .yTile != null || t.xTile != null) {
						if (yWall) {
								t.yTile.syncDamage (damage, attacker);
						} else {
								t.xTile.syncDamage (damage, attacker);
				
						}
				} else {
						Debug.Log ("X :" + x + ", Y :" + y + " damaged by :" + attacker + " after being destroyed");
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

				NetworkEnable ();
		}

		public void NetworkEnable ()
		{
				int playerID = PhotonNetwork.countOfPlayers;
				myPlayer.transform.position = FindObjectsOfType<SpawnSpot> () [playerID % 4].transform.position;
				myPlayer.transform.rotation = Quaternion.identity;
				currLevel.cam.SetActive (false);
				myPlayer.networkInit ();
				dead = false;
				UIManager.thisM.changeUI (tileDictionary.thisM.inGameUI);

		}
	
		public void NetworkDisable ()
		{
				currLevel.cam.SetActive (true);
				myPlayer.networkDisable ();
				dead = true;
				UIManager.thisM.changeUI (tileDictionary.thisM.pauseUI);
		}



}
