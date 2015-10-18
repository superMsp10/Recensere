using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ErrorUI : MonoBehaviour, UIState
{
		public Text title;
		public Text desciption;

	
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
}
