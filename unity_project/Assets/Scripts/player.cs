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

		

		public static Color getPlayerColour (int playerID)
		{
				int team = playerID % 4;
				switch (team) {
				case 0:
						return Color.blue;

				case 1:
						return Color.red;

				case 2:
						return Color.green;

				case 3:
						return Color.magenta;

			
				default:
						break;
				}

				return Color.blue;

		}
}
