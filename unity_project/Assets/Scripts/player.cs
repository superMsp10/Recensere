using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class player : MonoBehaviour,Health
{
		GameManager thisM = GameManager.thisM;
		//Health
		public	float health;
		public float originalHealth;
		public	string lastAttacker;
		public	bool takeDmg = true;
		public float Sturdy = 10f;
		public LayerMask damagedBy;

		//healthUI
		private Text HPText;
	
		//Network
		public MonoBehaviour[] networkSet;
		public int playerID;
		public PhotonView thisView;

		public GameObject thisCam;
		public MouseLook look;
		public MouseLook look2;

		//Items
		public Transform left_hand;
		public Transform right_hand;
		public string handLayer;

		//Animation
		public Animator anim;
		public Rigidbody r;
		public GameObject animModel;


		void FixedUpdate ()
		{
				anim.SetFloat ("YVelo", r.velocity.y);
		}


		[PunRPC]
		void hello ()
		{
				Debug.Log ("hello from: " + gameObject.name);
		}
		public	virtual bool takeDamage (float damage, string attacker)
		{
				//				Debug.Log ("Take Damage From Tile");
				Debug.Log ("hello to: " + gameObject.name);
				thisView.RPC ("hello", PhotonTargets.All, null);
				if (health <= 0)
						return false;
				HP -= damage;
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
//				Destroy (gameObject);
				thisM.NetworkDisable ();
				StartCoroutine (tileDictionary.thisM.pauseUI.GetComponent<pauseUI> ().Respawn (5f));
		
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
						HPText.text = "Health: " + value;
				}
		}
	
		public float Sturdyness {
				get {
						return Sturdy; 
				}
		
		}

		public void networkInit ()
		{
				thisM.ChangeCam (thisCam);
//				thisCam.gameObject.SetActive (true);
				foreach (MonoBehaviour m in networkSet) {
						m.enabled = true;
				}
				GetComponent<Rigidbody> ().velocity = new Vector3 (0, 0, 0);
				GetComponent<Rigidbody> ().useGravity = true;

				playerMove pm = GetComponent<playerMove> ();
				pm.Start ();

				HPText = tileDictionary.thisM.HPText;
				HP = originalHealth;
		}

		public void networkDisable ()
		{
//				thisCam.gameObject.SetActive (false);
				foreach (MonoBehaviour m in networkSet) {
						m.enabled = false;
				}
		
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

		void OnCollisionEnter (Collision collision)
		{
				if (damagedBy == (damagedBy | (1 << collision.gameObject.layer))) {

						float sdm = GameManager.speedToDamageMultiplier;
						//				Debug.Log ("Collision Enter at Tile");
						float dmg = Mathf.Pow (collision.relativeVelocity.magnitude, sdm);
						if (takeDmg && collision.relativeVelocity.magnitude > Sturdy) {
			
								if (takeDamage (dmg, collision.collider.name)) {
										if (collision.collider.attachedRigidbody != null) {
												collision.collider.attachedRigidbody.velocity *= sdm;
										}
								} 
			
			
						}
				}
		}
}
