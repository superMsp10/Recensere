using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIslot : MonoBehaviour
{
		public Holdable holding;
		public Image slot;
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
								Debug.Log ("UI slot change amout to: " + value);
								_amount = value;
								amountText.text = amount.ToString ();
								amountText.transform.parent.gameObject.SetActive (true);
						}
				}
		}

		

		public void changeHolding (Holdable h)
		{
				if (h == null) {

						if (holding != null)
								holding.onDrop ();

						slot.sprite = null;
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
				Debug.Log ("UI slot change holding. Amount: " + amounts);
				if (holding != null) {
						Debug.Log ("UI slot change holding,\n old holding drop");
						holding.onDrop ();
				}

				if (h == null || amounts <= 0) {
						Debug.Log ("UI slot change holding,\n parameter holdable is null");

						slot.sprite = null;
						holding = null;
						amount = 0;
				} else { 

						holding = h;
						amount = amounts;
						slot.sprite = holding.holdUI;
						Debug.Log ("UI slot change holding,\n changed to new holding");

						if (selected)
								holding.onSelect ();
				}
		}
	

		public void Use ()
		{
//				Debug.Log ("Used");
				if (holding.onUse ()) {
						amount = holding.amount;
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

