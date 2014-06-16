using UnityEngine;
using System.Collections.Generic;

public class NetworkManeger : MonoBehaviour
{

		public GameObject crossHair;
		public List<GameObject> players;
		public int dethHeight;
		public bool inGame;


		public bool dead = false;
		public bool firstPlayer = false;
		public string currMenu = "null";
		public bool voted = false;
		// public

		private string version;
		private bool offline = false;
		private GameObject myPlayer;
		private SpawnSpot[] SS;

		private bool connecting = false;
		List <string> chatMessages;
		
		int MaxChatMess = 5;
		private string code = "";
		private string status = "[newbie]";
		private string gameMode = "  ";
		private bool spec = false;
		GameObject specPlayer;

		int randomVote = 0;


		//UnityStuff
		void Start ()
		{
				version = "NetTest 0.2.2";
				
				chatMessages = new List<string> ();
			
			
				changeLvl ("startMenu");
				MaxChatMess = 5;
				Screen.showCursor = true;
				inGame = false;
			

		}

		void Update ()
		{
	
				
				if (inGame) {

						Screen.lockCursor = true;
						Screen.showCursor = false;
						Time.timeScale = 1;
						currMenu = "null";
						
				} else {
						if (offline) {
								Time.timeScale = 0;
						}
						if (currMenu == "null") {
								currMenu = "escape";
						}

						Screen.lockCursor = false;
						Screen.showCursor = true;
						


				}
				if (Input.GetKeyDown (KeyCode.Escape))
						inGame = !inGame;
				

		}

		void OnGUI ()
		{
				if (inGame) {
						if (offline) {
								GUILayout.Label (version + "  " + "OfflineMode" + "   " + gameMode);
						} else {
			
								GUILayout.Label (version + "  " + PhotonNetwork.connectionStateDetailed.ToString () + "   " + gameMode);
						}
				}

				if ((!inGame) && PhotonNetwork.connected && (!connecting) && !dead) {
						if (currMenu == "escape") {
								if (GUILayout.Button (status + " Status Powers")) {
										currMenu = "statusPower";
								}
						}
						if (currMenu == "statusPower") {
								if (status == "[newbie]") {



								}
								if (status == "[admin]") {

					if (GUILayout.Button ("Random level")) {
						int rLevel = Random.Range (0, levels.Length);
						changeLvl (rLevel);
						
					}
					foreach (level l in levels) {
						if (l.spawnable) {
							if (GUILayout.Button (l.name)) {
								voted = true;
								changeLvl (l.name);
								//addChatMassage(myPlayer.name + " has voted to change the level into " + l.name);
								//thisView.RPC ("vote", PhotonTargets.All, l.name);
								
							}
							
						}
					}


								}
								if (status == "[Dev]") {

									
										if (GUILayout.Button ("Random level")) {
												int rLevel = Random.Range (0, levels.Length);
												changeLvl (rLevel);
										
										}
										foreach (level l in levels) {
												if (l.spawnable) {
														if (GUILayout.Button (l.name)) {
																voted = true;
																changeLvl (l.name);
																//addChatMassage(myPlayer.name + " has voted to change the level into " + l.name);
																//thisView.RPC ("vote", PhotonTargets.All, l.name);

														}

												}
										}

								}
						}

				}

				if (dead) {
						inGame = false;
						GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
						GUILayout.BeginVertical ();
						GUILayout.FlexibleSpace ();

						GUILayout.BeginHorizontal ();

						if (GUILayout.Button ("Spawn")) {
								dead = false;
								CreatePlayer ();
								if (spec && specPlayer != null)
										specPlayer.transform.FindChild ("soldier").FindChild ("Hips")
						.FindChild ("Camera").gameObject.SetActive (false);
								currentLevel.standby.SetActive (false);
								
						}
						
						

						if (currMenu == "Spectate") {
								foreach (GameObject player in players) {
										if (GUILayout.Button (player.name)) {
												currentLevel.standby.SetActive (false);
												if (spec && specPlayer != null)
														specPlayer.transform.FindChild ("soldier").FindChild ("Hips")
							.FindChild ("Camera").gameObject.SetActive (false);
												player.transform.FindChild ("soldier").FindChild ("Hips").
							FindChild ("Camera").gameObject.SetActive (true);
												specPlayer = player;
						spec = true;
										}
								}
								if (GUILayout.Button ("Standby")) {
										if (spec && specPlayer != null)
												specPlayer.transform.FindChild ("soldier").FindChild ("Hips")
							.FindChild ("Camera").gameObject.SetActive (false);
										spec = false;
										currentLevel.standby.SetActive (true);
										specPlayer = null;

								}

						} else if (players.Count != 0) {
								if (GUILayout.Button ("Spectate")) {
										currMenu = "Spectate";
								}

						}
					
						
						GUILayout.FlexibleSpace ();
						GUILayout.EndVertical ();
						GUILayout.EndArea ();


				}
				

		
				if (PhotonNetwork.connected == false && connecting == false) {
						inGame = false;

						GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
						GUILayout.BeginHorizontal ();
						GUILayout.FlexibleSpace ();
						GUILayout.BeginVertical ();
						GUILayout.FlexibleSpace ();
						
						GUILayout.BeginHorizontal ();
						GUILayout.Label ("UserName: ");
						PhotonNetwork.player.name = GUILayout.TextField (PhotonNetwork.player.name);
			
						GUILayout.EndHorizontal ();
			
						GUILayout.BeginHorizontal ();
						GUILayout.Label ("Code: ");
						code = GUILayout.PasswordField (code, "*" [0], 20);
			
			
						GUILayout.EndHorizontal ();
			
			
						if (GUILayout.Button ("SinglePlayer")) {
								connecting = true;
								status = "[admin]";
								PhotonNetwork.offlineMode = true;
								offline = true;
								OnJoinedLobby ();
								gameMode = "freeRoam";
								changeLvl (Random.Range (0, levels.Length));
								inGame = true;

						}
			
			
						if (GUILayout.Button ("MultiPlayer")) {
								connecting = true;
								if (code == "ms10") {
										status = "[admin]";
								}
								if (code == "supermsp10") {
										status = "[Dev]";
								}
								Connect ();

								gameMode = "freeRoam";
								inGame = true;

						}
						GUILayout.FlexibleSpace ();
						GUILayout.EndVertical ();
						GUILayout.FlexibleSpace ();
						GUILayout.EndHorizontal ();
						GUILayout.EndArea ();
				}
				if (PhotonNetwork.connected && connecting == false) {
						GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
						GUILayout.BeginVertical ();
						GUILayout.FlexibleSpace ();
						foreach (string mess in chatMessages) {
								GUILayout.Label (mess);
						}
			
						GUILayout.EndVertical ();
						GUILayout.EndArea ();
			
				}
				
		}

		void OnDestroy ()
		{
				PlayerPrefs.SetString ("UserName", PhotonNetwork.player.name);
		}

		//PhotonStuff

		void Connect ()
		{
				PhotonNetwork.ConnectUsingSettings (version);
		}

		void OnJoinedLobby ()
		{
				PhotonNetwork.JoinRandomRoom ();
		}

		void OnJoinedRoom ()
		{
				connecting = false;
				if (firstPlayer) {
						int rLevel = Random.Range (0, levels.Length);
						changeLvl (rLevel);
						thisView.RPC ("changeLvl", PhotonTargets.OthersBuffered, rLevel);
				}
		}

		void OnPhotonRandomJoinFailed ()
		{
				firstPlayer = true;
				PhotonNetwork.CreateRoom ("randomCreated");
				
				
		}

		//MyStuff
	
		[RPC]	
		public void changeLvl (int i)
		{
				levelex (levels [i]);
		
		}
	
		[RPC]
		public void changeLvl (string i)
		{
				foreach (level l in levels) {
						if (l.name == i) {
								levelex (l);
						}
				}
		}

		[RPC]
		void addChatMassage_RPC (string message)
		{
				if (chatMessages.Count >= MaxChatMess) {
						chatMessages.RemoveAt (0);
				}
				chatMessages.Add (message);
		}

		[RPC]
		void playerJoin (string name, int playerId)
		{

				GameObject[] t = GameObject.FindGameObjectsWithTag ("Player");

				foreach (GameObject i  in t) {
						if (i.GetComponent<PhotonView> ().ownerId == playerId) {
								i.gameObject.name = name;
								players.Add (i);
						} 
				}
		}

		[RPC]
		public void vote (string lev)
		{
				foreach (level l in levels) {
						if (l.name == lev) {
								l.votes++;
						}
				}
		
		}

		private void levelex (level lev)
		{
				voted = false;
				if (currentLevel != null) {
						currentLevel.standby.SetActive (false);
						currentLevel.gameObject.SetActive (false);
						if (myPlayer != null) {
								myHp.Die ();
						}
								
				}
				currentLevel = lev;
				RenderSettings.skybox = currentLevel.sky;
				currentLevel.gameObject.SetActive (true);
				currentLevel.standby.SetActive (true);
				if (currentLevel.spawnable) {
						dead = true;
						SS = FindObjectsOfType<SpawnSpot> ();
						if (PhotonNetwork.connected) {
								foreach (level l in levels) {
										l.votes = 0;

								}

						}

				} else if (PhotonNetwork.connected)
						changeLvl ("cottage");		
		}
	
		public void addChatMassage (string message)
		{
				thisView.RPC ("addChatMassage_RPC", PhotonTargets.All, status + " " + message);
		}

		void CreatePlayer ()
		{

				MySS = SS [Random.Range (0, SS.Length)];
				myPlayer = (GameObject)PhotonNetwork.Instantiate ("plYER", MySS.transform.position, MySS.transform.rotation, 0);
				myPlayer.name = PhotonNetwork.player.name;
				myHp = myPlayer.GetComponent <health> ();
				myPlayer.GetComponent<MouseLook> ().enabled = true;
				crossHair.SetActive (true);
				
				((MonoBehaviour)myPlayer.GetComponent ("FPSInputController")).enabled = true;
				((MonoBehaviour)myPlayer.GetComponent ("CharacterMotor")).enabled = true;
				((MonoBehaviour)myPlayer.GetComponent ("MouseLook")).enabled = true;
				((MonoBehaviour)myPlayer.GetComponent ("playerShooter")).enabled = true;
				myPlayer.transform.FindChild ("soldier").FindChild ("Hips").FindChild ("Camera").gameObject.SetActive (true); 
				thisUI.transform.FindChild ("crossHair_0,2").gameObject.SetActive (true);
				addChatMassage (PhotonNetwork.player.name + " has joined!");
				if (status == "[Dev]") {
						addChatMassage ("Hail the mighty developer " + PhotonNetwork.player.name);
				}
				players.Add (myPlayer);
				GetComponent <PhotonView> ().RPC ("playerJoin", PhotonTargets.OthersBuffered, PhotonNetwork.player.name, PhotonNetwork.player.ID);
		}

}
