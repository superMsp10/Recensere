using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pauseUI : MonoBehaviour,UIState
{
		public GameObject inGame;
		public GameObject back;
		public GameObject respawnButton;
		public Text console;

		public string message = "";


		public void StartUI ()
		{
				gameObject.SetActive (true);
				GameManager.thisM.paused = true;


				if (GameManager.thisM.dead) {
						respawnButton.SetActive (false);
						back.SetActive (false);
						console.text = message;

				} else {
						back.SetActive (true);
						respawnButton.SetActive (true);
						console.text = "";

				}

		
		}
	
		public void EndUI ()
		{
				gameObject.SetActive (false);
		
		}
	
		public	void UpdateUI ()
		{
				if (!GameManager.thisM.dead)
						UIManager.thisM.changeUI (inGame);
		}

		public	void Respawn ()
		{
				message = "<b><color=red>Death</color> by sucide</b>, Respawing in <i><color=blue>5</color> seconds</i>";
				GameManager.thisM.NetworkDisable ();
				Invoke ("Spawn", 5.0f);

		}

		void Spawn ()
		{
				GameManager.thisM.NetworkEnable ();
		}
}
