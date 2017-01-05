using UnityEngine;
using System.Collections;

public class Persistent : MonoBehaviour
{

    public static Persistent thisPersist;
    public string Username = "Default123";
    public string Token;
    public bool Dev = false;
    public GameObject connectError;



    // Use this for initialization
    void Awake()
    {
        if (thisPersist == null)
        {
            thisPersist = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        connectError.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnApplicationQuit()
    {
        //DatabaseConnect.thisM.logout();
    }

    public void Reset()
    {
        Username = "";
        Token = "";
        connectError.SetActive(false);
    }
}
