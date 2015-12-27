using UnityEngine;
using System.Collections;

public class item_Cube : MonoBehaviour,Holdable
{

		//Holdable Stuff
		public Sprite _holdUI;
		public bool _pickable;
		public int _amount;
		
		//Item Stuff
		public Rigidbody r;
		player p;
		public string itemLayer;
		public Color normal;
		public Color highlighted;
	
		//Item------------------------------------------//
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
				
				return false;
		}
		public	void  buttonUP ()
		{
				Debug.Log ("buttonUP by Cube");
				
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
				r.isKinematic = false;
				gameObject.layer = LayerMask.NameToLayer (itemLayer);
				transform.parent = GameManager.thisM.currLevel.items;
				transform.position = p.transform.position;
				gameObject.SetActive (true);
				Invoke ("resetPick", 5.0f);
		}
	
		public	void resetPick ()
		{
				Debug.Log ("ResetPick by Cube");
				_pickable = true;

		}
		
		

}
