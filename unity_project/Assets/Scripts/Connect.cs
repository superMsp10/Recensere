using UnityEngine;
using System.Collections;

public class Connect :  Photon.MonoBehaviour
{


		public byte Version = 1;
		public bool offline = false;

		// Use this for initialization
		void Start ()
		{
				PhotonNetwork.autoJoinLobby = false;    // we join randomly. always. no need to join a lobby to get the list of rooms.
		

				if (offline) 
						PhotonNetwork.offlineMode = true;
				else
						PhotonNetwork.ConnectUsingSettings (Version.ToString ());

		}

		public virtual void OnConnectedToMaster ()
		{
				if (PhotonNetwork.networkingPeer.AvailableRegions != null)
						Debug.LogWarning ("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions [0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
				Debug.Log ("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
				PhotonNetwork.JoinRandomRoom ();
		}
	
		public virtual void OnPhotonRandomJoinFailed ()
		{
				Debug.Log ("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");
				PhotonNetwork.CreateRoom (null, new RoomOptions () { maxPlayers = 4 }, null);
		}
	
		// the following methods are implemented to give you some context. re-implement them as needed.
	
		public virtual void OnFailedToConnectToPhoton (DisconnectCause cause)
		{
				Debug.LogError ("Cause: " + cause);
		}
	
		public void OnJoinedRoom ()
		{
				GameManeger.thisM.OnConnected ();
		}
	
		public void OnJoinedLobby ()
		{
				Debug.Log ("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
		}

}
