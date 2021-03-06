﻿using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using UnityEngine.SceneManagement;

public class DatabaseConnect : MonoBehaviour
{
    public static DatabaseConnect thisM;

    public UIManager thisUI;
    ErrorUI error;

    string username, password;
    //TODO: Switch back to openshift server
    private string url = "https://aqueous-tundra-86592.herokuapp.com/api";
    //private string url = "http://localhost:8080/api";

    public GameObject myPlayMenu;

    void Awake()
    {
        if (thisM == null)
            thisM = this;
        else
            Destroy(gameObject);


    }

    IEnumerator Start()
    {
        error = Persistent.thisPersist.connectError.GetComponent<ErrorUI>();

        WWW www;
        www = new WWW(url + "/Utilities/version");
        Debug.Log("Sending request to database");
        yield return www;
        if (www.error == null)
        {
            JSONObject j = JSONObject.Parse(www.text);
            int gameVersion = GameManager.Version;
            int databaseVersion = (int)j.GetNumber("Version");
            Debug.Log("Connected to the database");
            if (gameVersion == databaseVersion)
            {
                Debug.Log("Server and client versions match, starting program");
                thisUI.changeUI(thisUI.startUI);
            }
            else
            {
                Debug.LogError("Database ERROR: Server and Client are on different versions. Please update to the newest version of the game.");
                ConnectionError("Outdated Version");
            }
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }
        www.Dispose();
    }

    IEnumerator iLogin()
    {

        WWW www;

        WWWForm wForm;

        www = new WWW(url + "/checkUser/" + username);
        yield return www;
        if (www.error == null)
        {
            bool exists = bool.Parse(www.text);
            if (exists)
            {
                wForm = new WWWForm();
                wForm.AddField("username", username);
                wForm.AddField("password", password);
                www = new WWW(url + "/login", wForm);
                yield return www;
                if (www.error == null)
                {
                    string level;
                    www.responseHeaders.TryGetValue("token", out Persistent.thisPersist.Token);
                    www.responseHeaders.TryGetValue("level", out level);
                    Persistent.thisPersist.Level = int.Parse(level);
                    Persistent.thisPersist.Username = username;
                    
                    bool logged = bool.Parse(www.text);

                    if (logged)
                    {
                        changeToPlayMenu();
                        Debug.Log("Logged In");
                    }
                    else
                    {
                        StartingUI error = thisUI.startUI.GetComponent<StartingUI>();
                        error.error.text = "Wrong Password";
                    }

                }
                else
                {
                    Debug.LogError("ERROR: " + www.error);
                    ConnectionError(www.error);
                }
            }
            else
            {
                StartingUI error = thisUI.startUI.GetComponent<StartingUI>();
                error.error.text = "An account with the username " + username + " does not exist";
            }
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }


    }

    IEnumerator iCheckAccess()
    {

        WWW www;
        WWWForm wForm;

        wForm = new WWWForm();
        wForm.AddField("username", username);
        wForm.AddField("token", Persistent.thisPersist.Token);
        www = new WWW(url + "/getStatus/", wForm);
        yield return www;
        if (www.error == null)
        {
            if (www.text != null && www.text.Equals("Developer"))
            {
                Persistent.thisPersist.Dev = true;
            }
            else
            {
                Persistent.thisPersist.Dev = false;
            }
            thisUI.changeUI(myPlayMenu);

        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }

    }


    IEnumerator iLogout()
    {

        WWW www;

        WWWForm wForm;

        wForm = new WWWForm();
        wForm.AddField("username", username);
        wForm.AddField("token", Persistent.thisPersist.Token);
        www = new WWW(url + "/logout", wForm);
        yield return www;
        if (www.error == null)
        {
            Debug.Log("Logged Out");
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }

    }

    IEnumerator iCheckAccount()
    {

        WWW www;

        www = new WWW(url + "/checkUser/" + username);
        yield return www;
        if (www.error == null)
        {
            //bool exists = bool.Parse(www.text);
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }
        www.Dispose();

    }



    IEnumerator iCreateAccount()
    {
        WWW www;

        WWWForm wForm;

        www = new WWW(url + "/checkUser/" + username);
        yield return www;
        if (www.error == null)
        {
            bool exists = bool.Parse(www.text);
            if (!exists)
            {
                wForm = new WWWForm();
                wForm.AddField("username", username);
                wForm.AddField("password", password);

                changeToPlayMenu();
                yield return new WWW(url + "/createAccount", wForm);
            }
            else
            {
                StartingUI error = thisUI.startUI.GetComponent<StartingUI>();
                error.error.text = "An account with the username " + username + " exists, please choose a different username to continue";
            }
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }


    }

    IEnumerator iSetLevel(int level)
    {
        WWW www;
        WWWForm wForm;

        wForm = new WWWForm();
        wForm.AddField("username", Persistent.thisPersist.Username);
        wForm.AddField("token", Persistent.thisPersist.Token);
        wForm.AddField("level", level);

        www = new WWW(url + "/setLevel/", wForm);
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.LogError("ERROR: " + www.error);
            ConnectionError(www.error);
        }

    }

    public void setLevel(int lev)
    {
        if (Persistent.thisPersist.Username != "")
            StartCoroutine("iSetLevel",lev);
    }

    public void checkAccess()
    {
        if (Persistent.thisPersist.Username != "")
            StartCoroutine("iCheckAccess");
    }

    public void createAccount(string username, string password)
    {
        this.username = username;
        this.password = password;

        StartCoroutine("iCreateAccount");
    }

    public void login(string username, string password)
    {
        this.username = username;
        this.password = password;

        StartCoroutine("iLogin");

    }

    public void logout()
    {
        if (Persistent.thisPersist.Username != "")
            StartCoroutine("iLogout");
    }

    public void checkAccount(string username)
    {
        this.username = username;
        StartCoroutine("iCheckAccount");
    }

    void changeToPlayMenu()
    {
        checkAccess();
    }


    //---------------------Connection Errors-------------------------------------//
    public void ConnectionError(string Error)
    {
        if (Error.Contains("Could not resolve host"))
        {
            ConnectionError_Disconnected();
        }
        else if (Error.Contains("Timed out"))
        {
            ConnectionError_TimedOut();
        }
        else if (Error.Contains("Service Temporarily Unavailable"))
        {
            ConnectionError_ServiceUnavailable();
        }
        else if (Error.Contains("Outdated Version"))
        {
            ConnectionError_OutdatedVersion();
        }
    }

    void ConnectionError_ServiceUnavailable()
    {
        error.desciption.text = "The Server is temporarily down for maintanaince. Sorry for the inconvenience.";
        error.title.text = "Server unavailable";

        thisUI.changeUI(thisUI.connectError);
    }

    void ConnectionError_OutdatedVersion()
    {
        error.desciption.text = "Server and Client are on different versions. Please download the latest version of the game from the Game Page.";
        error.title.text = "Outdated version";

        thisUI.changeUI(thisUI.connectError);
    }

    void ConnectionError_TimedOut()
    {
        error.desciption.text = "The server took too long to respond, this might be beacuse of technical problems. Sorry for the inconvenience.";
        error.title.text = "Timed out";

        thisUI.changeUI(thisUI.connectError);
    }

    void ConnectionError_Disconnected()
    {
        error.desciption.text = "Device is not connected to the internet. Please connect to continue.";
        error.title.text = "No internet connection";

        thisUI.changeUI(thisUI.connectError);
    }

    public void GameMessage_HostExited()
    {
        error.desciption.text = "This game is over beacuse the host has exited";
        error.title.text = "Game Over :(";

        thisUI.changeUI(thisUI.connectError);
    }




}
