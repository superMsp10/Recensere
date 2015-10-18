using UnityEngine;
using System.Collections;

public class StartingUI : MonoBehaviour,UIState
{
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
