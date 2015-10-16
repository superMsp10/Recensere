using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;



public class DatabaseConnect : MonoBehaviour
{
		public InputField getId;
	
		public InputField postDataName;
		public InputField postData;
		public Text console;
	
		private string url = "http://bbman-supermsp10.rhcloud.com/Utilities";
	
		IEnumerator Start ()
		{
				WWW www;
				www = new WWW (url + "/560f2a96e4b09f9838bbf46a");
				yield return www;
				if (www.error == null) {
						JSONObject j = JSONObject.Parse (www.text);
						int gameVersion = GameManeger.Version;		
						int databaseVersion = (int)j.GetNumber ("Version");
			
						if (gameVersion == databaseVersion) {
								GetComponent<Connect> ().TryConnectToServer ();
						} else {
								Debug.LogError ("Database ERROR: Server and Client are on different versions. Please uypdate to the newest version of the game.");

						}
			
			
				} else {
						Debug.LogError ("ERROR: " + www.error);
				}        
				www.Dispose ();
		}
	
	
		IEnumerator getHttp ()
		{
		
				WWW www;
				www = new WWW (url);
				yield return www;
				if (www.error == null) {
						console.text = www.text;
				} else {
						Debug.Log ("ERROR: " + www.error);
				}        
				www.Dispose ();
		}
		IEnumerator postHttp ()
		{
		
				WWWForm wForm;
		
				wForm = new WWWForm ();
				wForm.AddField (postDataName.text, postData.text);
		
		
				yield return new WWW ("https://bbman-supermsp10.rhcloud.com/Utilities", wForm);
				console.text = "post";
				Debug.Log ("hello");
		}
	
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		public void post ()
		{
				Debug.Log ("Post: " + postDataName.text + ": " + postData.text);
				StartCoroutine (postHttp ());
		
		}
	
		public void get ()
		{
				Debug.Log ("Get: " + getId.text);
				StartCoroutine (getHttp ());
		
				//	getHttp ();
		}
}
