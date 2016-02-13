using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class invManager : slotCollection,UIState
{
		
		private GameManager thismanage;
		public static invManager thisInv;

		public UIslot SelectedSlot;
		public int SelectedInt;

		public Color highlighted;
		public Color normal;
//		public Holdable giveOnStart;
		public GameObject paused;

		//Mono stuff
		void Awake ()
		{

				if (thisInv == null)
						thisInv = this;

		}

		void Start ()
		{
				thismanage = GameManager.thisM;
//				if (giveOnStart != null) {
//
//				}
				selectSlot (0);
		}
	
		void Update ()
		{
				if (!thismanage.paused) {
						if (Input.GetAxisRaw ("slotChangeWheel") > 0 || Input.GetKeyDown (KeyCode.E)) {
								selectSlot (SelectedInt + 1);
						} else if (Input.GetAxisRaw ("slotChangeWheel") < 0 || Input.GetKeyDown (KeyCode.Q)) {
								selectSlot (SelectedInt - 1);
						}
						if (SelectedSlot.holding != null) {
								if (Input.GetButtonDown ("InvSelected")) {
										SelectedSlot.buttonDown ();
								}
								if (Input.GetButtonUp ("InvSelected")) {
										SelectedSlot.holding.buttonUP ();
								}
								if (SelectedSlot.holding != null)
										SelectedSlot.holding.updateItem ();
						}
				}
				
		}

		//UIState stuff
		public void StartUI ()
		{
				gameObject.SetActive (true);
				GameManager.thisM.paused = false;
		
		}
	
		public void EndUI ()
		{
				gameObject.SetActive (false);
		
		}
	
		public	void UpdateUI ()
		{
				UIManager.thisM.changeUI (paused);
		
		}

		//slot collection stuff
		public void selectSlot (int i)
		{
				if (i >= 0 && i < slots.Capacity) {
						SelectedInt = i;
						if (SelectedSlot != null) {
								SelectedSlot.outline.color = normal;
								SelectedSlot.onDeselect ();
						}
						SelectedSlot = slots [i];
						SelectedSlot.outline.color = highlighted;
						SelectedSlot.onSelect ();
						
				}
		}

		public void clearInv ()
		{
				foreach (UIslot s in slots) {
						s.onClick ();

				}

		}

		public void dc_giveDefaultItem ()
		{
		
				for (int i = 0; i <  slots.Count; i ++) {
			
						if (slots [i].holding == null) {
//								slots [i].changeHolding (giveOnStart, 500);
								return;
						}
				}
		}

}
