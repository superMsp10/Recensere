using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class DatabaseConnect : MonoBehaviour
{
		public InputField getId;

		public InputField postDataName;
		public InputField postData;

		public Text console;

		public string url = "http://bbman-supermsp10.rhcloud.com/Utilities";

		IEnumerator getHttp ()
		{

				WWW www;
				www = new WWW ("https://bbman-supermsp10.rhcloud.com/Utilities");
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

		// Use this for initialization
		void Start ()
		{

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
