using UnityEngine;
using System.Collections;

public class player : MonoBehaviour,Health
{
		//Health
		public	float health;
		public float originalHealth;
		public	string lastAttacker;
		public	bool takeDmg = true;
		public float Sturdy = 10f;
	
		//Network
		public MonoBehaviour[] networkSet;
		public int playerID;

		public Camera thisCam;

		void Start ()
		{
		}

		public	virtual bool takeDamage (float damage, string attacker)
		{
				//				Debug.Log ("Take Damage From Tile");
		
				if (health <= 0)
						return false;
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
						return true;
				}
				return false;
		
		}
	
		public	virtual void syncDamage (float damage, string attacker)
		{
				//				Debug.Log ("Sync Damage From Tile");
		
				health -= damage;
				lastAttacker = attacker;
				if (health <= 0) {
						Destroy ();
				}
		
		}
	
	
		public virtual	void Destroy ()
		{
				Destroy (gameObject);
		
		}
		public	 string lastDamageBy ()
		{
				return lastAttacker;
		}
	
		public float HP {
				get {
						return health; 
				}
				set {
						health = value; 
				}
		}
	
		public float Sturdyness {
				get {
						return Sturdy; 
				}
		
		}

		public void networkInit ()
		{
				thisCam.gameObject.SetActive (true);
				foreach (MonoBehaviour m in networkSet) {
						m.enabled = true;
				}
				GetComponent<Rigidbody> ().useGravity = true;
				health = originalHealth;
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
