using UnityEngine;
using System.Collections;

public class Connect : Photon.MonoBehaviour
{



    private byte Version = GameManager.Version;
    bool showConnectionState = true;
    string prevConnectionState;
    public int maxPlayerNum = 20;

    // Use this for initialization
    void Start()
    {
        if (Persistent.thisPersist.offline)
        {
            PhotonNetwork.Disconnect();
            Debug.Log("Offline Mode!");
            PhotonNetwork.offlineMode = true;
        }
        else if (!PhotonNetwork.connected)
        {
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings(Version.ToString());
        }
    }

    public void FindRooms()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void Create()
    {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = (byte)maxPlayerNum }, null);
        PhotonNetwork.LoadLevel(GameManager.thisM.startGameScene);
    }

    void Update()
    {
        if (showConnectionState)
        {
            if (PhotonNetwork.connectionStateDetailed.ToString() != prevConnectionState)
            {
                Debug.Log(PhotonNetwork.connectionStateDetailed);
                prevConnectionState = PhotonNetwork.connectionStateDetailed.ToString();
            }
        }

    }

    public virtual void OnConnectedToMaster()
    {
        if (PhotonNetwork.networkingPeer.AvailableRegions != null)
            Debug.LogWarning("List of available regions counts " + PhotonNetwork.networkingPeer.AvailableRegions.Count + ". First: " + PhotonNetwork.networkingPeer.AvailableRegions[0] + " \t Current Region: " + PhotonNetwork.networkingPeer.CloudRegion);
        Debug.Log("OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room. Calling: PhotonNetwork.JoinRandomRoom();");
        if (Persistent.thisPersist.autoJoin)
            PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        if (UIManager.thisM.currentUI != null)
            UIManager.thisM.currentUI.UpdateUI();
        if (Persistent.thisPersist.autoJoin)
        {
            PhotonNetwork.automaticallySyncScene = true;
            PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = (byte)maxPlayerNum }, null);
        }

    }

    // the following methods are implemented to give you some context. re-implement them as needed.

    public virtual void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        Debug.LogError("Cause: " + cause);
    }

    public void OnJoinedRoom()
    {
        showConnectionState = false;
        GameManager.thisM.OnConnected();
    }



    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). Use a GUI to show existing rooms available in PhotonNetwork.GetRoomList().");
    }

}
