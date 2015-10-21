using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StartingUI : MonoBehaviour,UIState
{
		public Text userName;
		public Text password;
		public Text error;

		public void StartUI ()
		{
				gameObject.SetActive (true);
				error.text = "";
		}
	
		public void EndUI ()
		{

				gameObject.SetActive (false);

		}
	
		public	void UpdateUI ()
		{
		
		}


}
