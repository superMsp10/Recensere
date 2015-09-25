using UnityEngine;
using System.Collections;

public class player : MonoBehaviour
{
		public Camera thisCam;
		public MonoBehaviour[] networkSet;
		public int playerID;
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

		public void networkDisable ()
		{
				thisCam.gameObject.SetActive (false);
				foreach (MonoBehaviour m in networkSet) {
						m.enabled = false;
				}
		
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnControllerColliderHit (ControllerColliderHit hit)
		{
				Debug.Log ("hello");
		}
		public static Color getPlayerColour (int playerID)
		{
				int team = playerID % 4;
				switch (team) {
				case 0:
						return Color.blue;
						break;

				case 1:
						return Color.red;
						break;

				case 2:
						return Color.green;
						break;

				case 3:
						return Color.magenta;
						break;

			
				default:
						break;
				}

				return Color.blue;

		}
}
