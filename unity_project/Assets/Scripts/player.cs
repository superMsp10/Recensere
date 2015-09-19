using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
		public Camera thisCam;
		public MonoBehaviour[] networkSet;
		// Use this for initialization
		void Start ()
		{
		}

		public void networkInit ()
		{
				thisCam.gameObject.SetActive (true);
				foreach (MonoBehaviour m in networkSet) {
						m.enabled = true;
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
