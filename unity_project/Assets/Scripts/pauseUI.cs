using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class pauseUI : MonoBehaviour,UIState
{
		public GameObject inGame;
		public GameObject back;
		public GameObject respawnButton;
		public Text console;
		public string deathMessage;
		public Color originalColor;
		public Color respawnColor;


		public void StartUI ()
		{
				gameObject.SetActive (true);
				GameManager.thisM.paused = true;


				if (GameManager.thisM.dead) {
						respawnButton.SetActive (false);
						back.SetActive (false);
//						console.text = message;

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

		public	void Disconnect ()
		{
				GameManager.thisM.NetworkDisconnect ();
				Application.LoadLevel ("StartUIScene");
		}

		public void button_respawn ()
		{
				deathMessage = "<b><color=red>Death</color> by suicide</b>;";
				GameManager.thisM.NetworkDisable ();
				StartCoroutine (Respawn (5f));
		}

		public IEnumerator Respawn (float seconds)
		{
				float orgSecs = seconds;
				while (seconds>0) {

						console.text = deathMessage +
								" Respawing in <i><color=#" +
								Color.Lerp (originalColor, respawnColor, seconds / orgSecs).GetHashCode () +
								">" + seconds.ToString () + "</color> seconds</i>";

						yield return  new WaitForSeconds (0.5f);
						seconds -= 0.5f;
				}
				GameManager.thisM.NetworkEnable ();

		}

		
}
