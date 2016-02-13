using UnityEngine;
using System.Collections;

public class item_Cube : MonoBehaviour,Holdable
{

		//Holdable Stuff
		public Sprite _holdUI;
		public bool _pickable;
		public int _amount;
		
		//Pooler
		[HideInInspector]
		public NetworkPooler
				projectilePooler = null;
		public float itemReset;
		public int maxItems;
		public GameObject projectile;
		public float throwMultiplier;

		//Item Stuff
		public Rigidbody r;
		player p;
		public string itemLayer;
		public Color normal;
		public Color highlighted;
		float timeStarted;
		float timeEnded;
		public float wantedTime;

		bool startedHold = false;

		void Start ()
		{
				projectilePooler = new NetworkPooler (maxItems, projectile);
		}

		//Item------------------------------------------//
		public	void updateItem ()
		{
				Renderer ren = GetComponent<Renderer> ();

				if (ren == null) {
						Debug.Log ("No renderer on this object");
						return;
				}
				if (startedHold) {
						ren.material.color = Color.Lerp (normal, highlighted, (Time.time - timeStarted) / wantedTime);
				} else {
						ren.material.color = normal;
				}
		
		}
		void OnCollisionEnter (Collision collision)
		{
				if (pickable) {
						if (collision.collider.gameObject.layer == LayerMask.NameToLayer (GameManager.thisM.PlayerLayer)) {
								Debug.Log ("OnCollisionEnter, Cube and Player Amount: " + _amount);
								p = GameManager.thisM.myPlayer;
								invManager.thisInv.addHoldable (this, _amount);
								_pickable = false;
						}
				}
		}

		public	void detach (GameObject g)
		{
				projectilePooler.disposeObject (g.GetComponent<Poolable> ());
		
		}
		
		//Holdable------------------------------------------//
		public Sprite holdUI {
				get {
						return _holdUI; 
				}
		
		}
		public int stackSize {
				get {
						return 16; 
				}
		
		}
		public string description {
				get {
						return "<b>Hello! <color=red>My name is, </color> Cube, </b>I will be <i><color=blue>helping you</color> test!</i>"; 
				}
		
		}
		public bool pickable {
				get {
						return _pickable; 
				}
		
		}
		public int amount {
				get {
						return _amount; 
				}
				set {
						_amount = value; 
				}
		}
		public	bool  buttonDown ()
		{
				Debug.Log ("buttonDown by Cube");
				timeStarted = Time.time;
				startedHold = true;
				return false;
		}
		public	void  buttonUP ()
		{
				Debug.Log ("buttonUP by Cube");
				timeEnded = Time.time;

				//Projectile Stuff
				GameObject g = projectilePooler.getObject ();
				//Set Transform to this and reset Timer
				g.transform.SetParent (tileDictionary.thisM.projectiles, true);
				g.transform.position = transform.position;
				g.transform.rotation = transform.rotation;
				g.GetComponent<Timer> ().StartTimer (itemReset);
				g.GetComponent<cube_Projectile> ().thisPooler = this;
				//Apply Force
				float force = 0f;
				float heldTime = Time.time - timeStarted;
				if (heldTime > wantedTime)
						force = throwMultiplier;
				else
						force = (heldTime / wantedTime) * throwMultiplier;
				g.GetComponent<Rigidbody> ().AddForce (p.left_hand.forward * force);

				startedHold = false;
		}
		public	void  onSelect ()
		{
				Debug.Log ("onSelect by Cube");
				gameObject.SetActive (true);

		}
		public void  onDeselect ()
		{
				Debug.Log ("onDeselect by Cube");
				gameObject.SetActive (false);

		}
		public	void  onPickup ()
		{
				Debug.Log ("onPickup by Cube");
				GetComponent<Renderer> ().material.color = normal;

				_pickable = false;
				r.isKinematic = true;
				gameObject.layer = LayerMask.NameToLayer (p.handLayer);
				transform.parent = p.left_hand;
				transform.position = p.left_hand.position;
				gameObject.SetActive (false);


		}
		public	void  onDrop ()
		{
				Debug.Log ("onDrop by Cube");
				GetComponent<Renderer> ().material.color = normal;
				r.velocity = Vector3.zero;
				r.isKinematic = false;
				gameObject.layer = LayerMask.NameToLayer (itemLayer);
				transform.parent = GameManager.thisM.currLevel.items;
				gameObject.SetActive (true);
//				transform.position = p.transform.position;
				Invoke ("resetPick", 5.0f);
		}
	
		public	void resetPick ()
		{
				Debug.Log ("ResetPick by Cube");
				_pickable = true;

		}
		
		

}
