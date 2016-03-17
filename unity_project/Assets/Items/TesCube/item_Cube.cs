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
		public PhotonView thisView;
		int playerID;
		bool selected = false;
		bool startedHold = false;
		Renderer ren;

		void Start ()
		{
				projectilePooler = new NetworkPooler (maxItems, projectile);
				ren = GetComponent<Renderer> ();

		}

		//Item------------------------------------------//
		public	void Update ()
		{
				if (selected) {
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
		}

		public	void detach (GameObject g)
		{
				projectilePooler.disposeObject (g.GetComponent<Poolable> ());
		
		}

		void OnCollisionEnter (Collision collision)
		{
				if (pickable) {
						if (collision.collider.gameObject.layer == LayerMask.NameToLayer (GameManager.thisM.PlayerLayer)) {
								Debug.Log ("OnCollisionEnter, Cube and Player Amount: " + _amount);
//								p = collision.collider.gameObject.GetComponent<player> ();
								playerID = collision.collider.gameObject.GetComponent<PhotonView> ().viewID;
								thisView.RPC ("pickedUpBy", PhotonTargets.All, playerID);

								invManager.thisInv.addHoldable (this, _amount);
								_pickable = false;
						}
				}
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
				thisView.RPC ("buttonDownBy", PhotonTargets.All, null);
	
				return false;
		}

		[PunRPC]
		void  buttonDownBy ()
		{
				//				Debug.Log ("buttonDown by Cube");
				timeStarted = Time.time;
				startedHold = true;
		}

		public	void  buttonUP ()
		{
				thisView.RPC ("buttonUpBy", PhotonTargets.All, null);

				//Projectile Stuff
				GameObject g = projectilePooler.getObject ();
				//Set Transform to this and reset Timer
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
		}

		[PunRPC]
		void  buttonUpBy ()
		{
				timeEnded = Time.time;
				startedHold = false;
				//				Debug.Log ("buttonUP by Cube");
		}

		public	void  onSelect ()
		{
				thisView.RPC ("selectedBy", PhotonTargets.All, null);
		}

		[PunRPC]
		void selectedBy ()
		{
				gameObject.SetActive (true);
				selected = true;
		}

		public void  onDeselect ()
		{
				thisView.RPC ("deselectedBy", PhotonTargets.All, null);
		}

		[PunRPC]
		void deselectedBy ()
		{
				//				Debug.Log ("onDeselect by Cube");
				gameObject.SetActive (false);
				selected = false;

		}
		public	void  onPickup ()
		{
//				Debug.Log ("onPickup by Cube");
				thisView.TransferOwnership (PhotonNetwork.player.ID);
		}

		[PunRPC]
		void pickedUpBy (int viewID)
		{
				GetComponent<Renderer> ().material.color = normal;
				p = GameManager.thisM.getPlayerByViewID (viewID);
				_pickable = false;
				r.isKinematic = true;
				gameObject.layer = LayerMask.NameToLayer (p.handLayer);
				transform.parent = p.right_hand;
				transform.position = p.right_hand.position;
				transform.rotation = p.right_hand.rotation;
				gameObject.SetActive (false);
		}

		public	void  onDrop ()
		{
				thisView.RPC ("droppedBy", PhotonTargets.All, null);

		}

		[PunRPC]
		void  droppedBy ()
		{
				//				Debug.Log ("onDrop by Cube");
				GetComponent<Renderer> ().material.color = normal;
				r.velocity = Vector3.zero;
				r.isKinematic = false;
				gameObject.layer = LayerMask.NameToLayer (itemLayer);
				transform.parent = GameManager.thisM.currLevel.items;

				gameObject.SetActive (true);
				//				transform.position = p.transform.position;
				Invoke ("resetPick", 5.0f);
		}

		[PunRPC]
		void resetPickBy ()
		{
//				Debug.Log ("ResetPick by Cube");
				_pickable = true;

		}

		public	void resetPick ()
		{
				thisView.RPC ("resetPickBy", PhotonTargets.All, null);
		}
		

		

}
