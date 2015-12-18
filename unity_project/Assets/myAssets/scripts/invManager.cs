using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class invManager : slotCollection,UIState
{
		
		private GameManager thismanage;
		public static invManager thisInv;
		public UIslot selected;
		public Color highlighted;
		public Color normal;
		public int selectedId;
		public Holdable give;

		void Awake ()
		{

				if (thisInv == null)
						thisInv = this;

		}
		void Start ()
		{
				thismanage = GameManager.thisM;

				selectSlot (0);
		}

		void Update ()
		{
				if (!thismanage.paused) {
						if (Input.GetAxisRaw ("slotChangeWheel") > 0 || Input.GetKeyDown (KeyCode.E)) {
								selectSlot (selectedId + 1);
						} else if (Input.GetAxisRaw ("slotChangeWheel") < 0 || Input.GetKeyDown (KeyCode.Q)) {
								selectSlot (selectedId - 1);
						}
						if (Input.GetButtonDown ("InvSelected") && selected.holding != null) {
								selected.Use ();
						}
				}
				
		}

		public void StartUI ()
		{
				gameObject.SetActive (true);
		
		}
	
		public void EndUI ()
		{
				gameObject.SetActive (false);
		
		}
	
		public	void UpdateUI ()
		{
		
		}

		public void selectSlot (int i)
		{
				if (i >= 0 && i < slots.Capacity) {
						selectedId = i;
						if (selected != null) {
								selected.outline.color = normal;
								selected.onDeselect ();
						}
						selected = slots [i];
						selected.outline.color = highlighted;
						selected.onSelect ();
						
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
								slots [i].changeHolding (give, 500);
								if (give.weapon) {
										slots [i].onClick ();
								}
								return;
						}
				}
		}

}
