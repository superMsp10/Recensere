using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIslot : MonoBehaviour
{
		public Holdable holding;
		public Image slot;
		public Sprite empty;
		public Image outline;
		int _amount;
		public Text amountText;
		public bool selected = false;
		public AudioClip clickSound;
		protected	GameManager thisM;
		Vector2 playerPos;
		protected pickups tmp;

		void Start ()
		{

				thisM = GameManager.thisM;
				selected = false;
		}

		public int amount {
				get {
						return _amount; 
				}
				set {
						if (value <= 0) {
								amountText.transform.parent.gameObject.SetActive (false);
								amountText.text = "";
								_amount = 0; 
								
						} else {
								amountText.text = amount.ToString ();
								amountText.transform.parent.gameObject.SetActive (true);
								_amount = value;
						}
				}
		}

		

		public void changeHolding (Holdable h)
		{
				if (h == null) {

						if (holding != null)
								holding.onDrop ();

						slot.sprite = empty;
						holding = null;
						amount = 0;
				} else {
						
						holding = h;
						holding.onPickup ();
						slot.sprite = h.holdUI;
						amount = 1;
						amountText.text = "1";
						if (selected)
								holding.onSelect ();
				}
		}

		public void changeHolding (Holdable h, int amounts)
		{

				if (h == null || amounts <= 0) {
						if (holding != null)
								holding.onDrop ();
						slot.sprite = empty;
						holding = null;
						amount = 0;
				} else { 
						if (holding != null)
								holding.onDrop ();

						amount = amounts;
						slot.sprite = h.holdUI;

						if (selected)
								holding.onSelect ();
				}
		}
	

		public void Use ()
		{
//				Debug.Log ("Used");
				if (holding.onUse ()) {
						amount -= 1;
						if (amount <= 0) {
								changeHolding (null);
						}
				}
		}

		public virtual void onClick ()
		{
//				Debug.Log ("On click to drop");

				if (holding != null) {
						holding.onDrop ();
						if (thisM.myPlayer != null) {
								playerPos = thisM.myPlayer.transform.position;
								
								GameObject p = (GameObject)Instantiate (holding.phisical.gameObject, playerPos, Quaternion.identity);
								tmp = p.GetComponent<pickups> ();
//								tmp.thisLevel = thisM.currentLevel;
								tmp.pickable = false;
								tmp.amount = amount;
								Invoke ("resetPickable", tmp.resetPickup);
								changeHolding (null);
						}
				}
		}

		public void onSelect ()
		{
//				Debug.Log ("On select");

				if (holding != null)
						holding.onSelect ();
				selected = true;



		}

		public void onDeselect ()
		{
				if (holding != null)
						holding.onDeselect ();
				selected = false;

		}
		void resetPickable ()
		{
				tmp.pickable = true;
		
		}
}

