using UnityEngine;
using System.Collections;

public abstract class Level : MonoBehaviour
{
		public Transform levelStart;
		public Transform items;

		private SpawnSpot[] sS;
		private SpawnSpot mySS;

		public LootTile[] lootTiles;
		public float lootTime;
		public float fuelRate = 2f;

		public Tile[,] liveTiles;


		public GameObject cam;
		


		// Use this for initialization
		void Start ()
		{
//				cam = FindObjectOfType<Camera> ().gameObject;
				GameManager.thisM.currLevel = this;
				GameManager.thisM.ChangeCam (cam);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public virtual void  generateArena (MapGenerator m)
		{
//				liveTiles = gen.generateMap (levelStart);
				liveTiles = m.findTiles (levelStart);
				lootTiles = m.findLootTiles ();
				if (PhotonNetwork.isMasterClient)
						InvokeRepeating ("generateLoot", 0, lootTime);
		}
		public virtual void  generateArena ()
		{
				MapGenerator gen = new MapGenerator (Map.firstMap);
				//				liveTiles = gen.generateMap (levelStart);
				liveTiles = gen.findTiles (levelStart);
				lootTiles = gen.findLootTiles ();
				if (PhotonNetwork.isMasterClient)
						InvokeRepeating ("generateLoot", 0, lootTime);
		}

		public void generateLoot ()
		{
				Debug.Log ("Generated loot");
				foreach (LootTile t in lootTiles) {
						t.generateLoot ();
				}
		}
		public void OnConnected ()
		{
				//Debug.Log ("Connected in GameManager");
				generateArena ();
				UIManager.thisM.currentUI = null;
				GameManager.thisM.instantiatePlayer ();
		}
	
}

