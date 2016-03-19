using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
		public static UIManager thisM;
		public GameObject connectError;
		public GameObject startUI;

		public UIState currentUI;

		void Awake ()
		{
				thisM = this;

		}

		void Update ()
		{
				if (currentUI != null && Input.GetKeyDown (KeyCode.Escape)) {
						currentUI.UpdateUI ();
				}
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
