using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;



public class DatabaseConnect : MonoBehaviour
{
		public InputField getId;
		public static DatabaseConnect thisM;
		public InputField postDataName;
		public InputField postData;
		public Text console;

		public	string username, password;
	
		private string url = "http://bbman-supermsp10.rhcloud.com/";

		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

		IEnumerator Start ()
		{
				WWW www;
				www = new WWW (url + "Utilities/560f2a96e4b09f9838bbf46a");
				yield return www;
				if (www.error == null) {
						JSONObject j = JSONObject.Parse (www.text);
						int gameVersion = GameManeger.Version;		
						int databaseVersion = (int)j.GetNumber ("Version");
						Debug.Log ("Connected to the database");
						if (gameVersion == databaseVersion) {
								Debug.Log ("Server and client versions match, starting program");
								UIManager.thisM.changeUI (UIManager.thisM.startUI);
//								GetComponent<Connect> ().TryConnectToServer ();
						} else {
								Debug.LogError ("Database ERROR: Server and Client are on different versions. Please update to the newest version of the game.");
								ErrorUI error = UIManager.thisM.connectError.GetComponent<ErrorUI> ();
								error.desciption.text = "Server and Client are on different versions. Please update to the latest version of the game.";
								error.title.text = "Outdated Version";

								UIManager.thisM.changeUI (UIManager.thisM.connectError);

						}
			
			
				} else {
						Debug.LogError ("ERROR: " + www.error);
						displayConnectionError ();

				}        
				www.Dispose ();
		}

		IEnumerator iLogin ()
		{

				WWW www;
		
				WWWForm wForm;
		
				www = new WWW (url + "checkUser/" + username);
				yield return www;
				if (www.error == null) {
						bool exists = bool.Parse (www.text);
						if (exists) {
								wForm = new WWWForm ();
								wForm.AddField ("username", username);
								wForm.AddField ("password", password);
								www = new WWW (url + "/login", wForm);
								yield return  www;
								if (www.error == null) {
										bool logged = bool.Parse (www.text);

										if (logged) {

										} else {
												StartingUI error = UIManager.thisM.startUI.GetComponent<StartingUI> ();
												error.error.text = "Wrong Password";
										}

								} else {
										Debug.LogError ("ERROR: " + www.error);
										displayConnectionError ();
								}
						} else {
								StartingUI error = UIManager.thisM.startUI.GetComponent<StartingUI> ();
								error.error.text = "An account with the username " + username + " does not exist";
						}
				} else {
						Debug.LogError ("ERROR: " + www.error);
						displayConnectionError ();
				}

		
		}
	
		IEnumerator iCheckAccount ()
		{
		
				WWW www;
		
				www = new WWW (url + "checkUser/" + username);
				yield return www;
				if (www.error == null) {
						bool exists = bool.Parse (www.text);
				} else {
						Debug.LogError ("ERROR: " + www.error);
						displayConnectionError ();
				}
				www.Dispose ();

		}



		IEnumerator iCreateAccount ()
		{
				WWW www;

				WWWForm wForm;

				www = new WWW (url + "checkUser/" + username);
				yield return www;
				if (www.error == null) {
						bool exists = bool.Parse (www.text);
						if (!exists) {
								wForm = new WWWForm ();
								wForm.AddField ("username", username);
								wForm.AddField ("password", password);
				
				
								yield return new WWW (url + "/createAccount", wForm);
						} else {
								StartingUI error = UIManager.thisM.startUI.GetComponent<StartingUI> ();
								error.error.text = "An account with the username " + username + " exists, please consider a different username to continue";
						}
				} else {
						Debug.LogError ("ERROR: " + www.error);
						displayConnectionError ();
				}
		
			
		}

		public void createAccount (string username, string password)
		{
				this.username = username;
				this.password = password;

				StartCoroutine ("iCreateAccount");
		}

		public void login (string username, string password)
		{
				this.username = username;
				this.password = password;
		
				StartCoroutine ("iLogin");
		}

		public void checkAccount (string username)
		{
				this.username = username;
				StartCoroutine ("iCheckAccount");
		}

		void displayConnectionError ()
		{
				ErrorUI error = UIManager.thisM.connectError.GetComponent<ErrorUI> ();
				error.desciption.text = "Your device is not connected to the internet. Please connect to the Internet to continue.";
				error.title.text = "No Connection";
		
				UIManager.thisM.changeUI (UIManager.thisM.connectError);
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		
}
