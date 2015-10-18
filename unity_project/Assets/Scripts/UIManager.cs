﻿using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
		public static UIManager thisM;
		public GameObject connectError;
		public GameObject startUI;

		public UIState currentUI;

		void Awake ()
		{
				if (thisM == null)
						thisM = this;
		
		}

		public void changeUI (GameObject ui)
		{
				if (currentUI != null) {
						currentUI.EndUI ();
				} 

				currentUI = ui.GetComponent<UIState> ();
				if (currentUI != null)
						currentUI.StartUI ();
				else
						Debug.LogError ("Gameobject: " + ui.name + "is not a UI state");
		}
}