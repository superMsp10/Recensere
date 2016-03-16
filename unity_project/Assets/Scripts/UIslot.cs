using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIslot : MonoBehaviour
{
		protected	GameManager thisM;

		//Holding
		public Holdable holding;
		int _amount;
		public bool selected = false;
	
		//Visuals
		public Image slot;
		public Sprite defaultSlotImage;
		public Image outline;
		public Text amountText;
		public AudioClip clickSound;

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
//								Debug.Log ("UI slot change amout to: " + value);
								_amount = value;
								amountText.text = amount.ToString ();
								amountText.transform.parent.gameObject.SetActive (true);
						}
				}
		}

		

		public void changeHolding (Holdable h, int amounts = 0)
		{
//				Debug.Log ("UI slot change holding. Amount: " + amounts);
				if (holding != null) {
//						Debug.Log ("UI slot change holding,\n old holding drop");
						holding.onDrop ();
				}

				if (h == null || amounts <= 0) {
//						Debug.Log ("UI slot change holding,\n parameter holdable is null");
						clearHolding ();
					
				} else { 

						holding = h;
						amount = amounts;
						slot.sprite = holding.holdUI;
//						Debug.Log ("UI slot change holding,\n changed to new holding");
						holding.onPickup ();
						if (selected)
								holding.onSelect ();
				}
		}

		public void clearHolding ()
		{
//				Debug.Log ("UI slot clear holding");
				slot.sprite = defaultSlotImage;
				holding = null;
				amount = 0;
		}
	

		public void buttonDown ()
		{
//				Debug.Log ("Used");
				if (holding.buttonDown ()) {
						amount = holding.amount;
						if (amount <= 0) {
								clearHolding ();
						}
				}
		}

		public virtual void onClick ()
		{
//				Debug.Log ("On click to drop");

				if (holding != null) {
						holding.onDrop ();
				}
				clearHolding ();
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
}

