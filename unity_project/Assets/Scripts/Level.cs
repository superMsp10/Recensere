using UnityEngine;
using System.Collections;

public abstract class Level : MonoBehaviour
{
		public Transform levelStart;
		private SpawnSpot[] sS;
		private SpawnSpot mySS;
		public Tile[,] liveTiles;
		public GameObject cam;
		


		// Use this for initialization
		void Start ()
		{
//				cam = FindObjectOfType<Camera> ().gameObject;
				GameManager.thisM.currLevel = this;
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public virtual void  generateArena ()
		{
				MapGenerator gen = new MapGenerator (Map.firstMap);
				liveTiles = gen.generateMap (levelStart);
		}



		public void OnConnected ()
		{
				//Debug.Log ("Connected in GameManager");
				generateArena ();
				UIManager.thisM.currentUI = null;
				GameManager.thisM.instantiatePlayer ();
		}
	
}

