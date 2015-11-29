using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIslot : MonoBehaviour
{
		public Holdable holding;
		public Image slot;
		public Sprite empty;
		public Image outline;
		public int amount;
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


		

		public void changeHolding (Holdable h)
		{
				if (h == null) {
						amountText.transform.parent.gameObject.SetActive (false);

						if (holding != null)
								holding.onDrop ();
						slot.sprite = empty;
						holding = null;
						amount = 0;
						amountText.text = "";
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

				if (h == null) {
						if (holding != null)
								holding.onDrop ();
						slot.sprite = empty;
						holding = null;
						amount = 0;
						amountText.text = "";
				} else { 
						if (holding != null)
								holding.onDrop ();
						if (holding != h) {
								holding = h;
								holding.onPickup ();
						}

						amount = amounts;
						slot.sprite = h.holdUI;
						amountText.text = amount.ToString ();
						amountText.transform.parent.gameObject.SetActive (true);

						if (selected)
								holding.onSelect ();
						else
								holding.onDeselect ();
				}
		}

		public void changeHolding (Holdable h, int amounts, bool update)
		{
		
				if (h == null) {
						if (holding != null)
								holding.onDrop ();
						slot.sprite = empty;
						holding = null;
						amount = 0;
						amountText.text = "";
				} else { 
						if (holding != h) {
								holding = h;
								holding.onPickup ();
						}
			
						amount = amounts;
						slot.sprite = h.holdUI;
						amountText.text = amount.ToString ();
						amountText.transform.parent.gameObject.SetActive (true);
						if (update) {
								if (selected)
										holding.onSelect ();
								else
										holding.onDeselect ();
						}
				}
		}

		public void Use ()
		{
				if (holding.onUse ()) {
						amount -= 1;
						amountText.text = amount.ToString ();
						AudioManager.thisAM.playerFX.PlayOneShot (clickSound);
						if (amount <= 0) {
								changeHolding (null);
								amountText.text = "";
						}
				}
		}

		public virtual void onClick ()
		{
				if (holding != null) {
						holding.onDrop ();
						if (thisM.myPlayer != null) {
								playerPos = thisM.myPlayer.transform.position;
								
								GameObject p = (GameObject)Instantiate (holding.phisical.gameObject, playerPos, Quaternion.identity);
								tmp = p.GetComponent<pickups> ();
								tmp.thisLevel = thisM.currentLevel;
								tmp.pickable = false;
								tmp.amount = amount;
								Invoke ("resetPickable", tmp.resetPickup);
								changeHolding (null);
						}
				}
		}

		public void onSelect ()
		{
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

