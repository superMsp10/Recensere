using UnityEngine;
using System.Collections;

public class Persistent : MonoBehaviour
{

    public static Persistent thisPersist;
    public string Username;
    public string Token;


    // Use this for initialization
    void Awake()
    {
        if (thisPersist == null)
        {
            thisPersist = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
        DatabaseConnect.thisM.logout();
    }
}
