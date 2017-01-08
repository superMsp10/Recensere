using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StartingUI : MonoBehaviour, UIState
{
    public Text userName;
    public InputField password;
    public Text error;

    public void StartUI()
    {

        gameObject.SetActive(true);
        error.text = "";
       
    }

    public void EndUI()
    {
        gameObject.SetActive(false);

    }

    public void CreateAccount()
    {
        if (validateUsername(userName.text) && validatePassword(password.text))
        {
            DatabaseConnect.thisM.createAccount(userName.text, password.text);
        }


    }

    public void login()
    {
        if (validateUsername(userName.text) && validatePassword(password.text))
        {
            DatabaseConnect.thisM.login(userName.text, password.text);
        }
    }

    bool validateUsername(string s)
    {

        if (s.Length < 3)
        {
            error.text = "Username has to be longer than 3 characters, please consider a longer username to continue";
            return false;
        }
        if (s.Length > 15)
        {
            error.text = "Username has to be shorter than 15 characters, please consider a shorter username to continue";
            return false;
        }
        if (s.Contains(" "))
        {
            error.text = "Username cannot include spaces, please remove all spaces to continue";
            return false;
        }
        error.text = "";
        return true;

    }
    bool validatePassword(string s)
    {
        if (s.Length < 5)
        {
            error.text = "Password has to be longer than 5 characters, please choose a longer password to continue";
            return false;
        }
        if (s.Length > 15)
        {
            error.text = "Password has to be shorter than 15 characters, please choose a shorter password to continue";
            return false;
        }
        if (s.Contains(" "))
        {
            error.text = "Password cannot include spaces, please remove all spaces to continue";
            return false;
        }
        error.text = "";
        return true;

    }

    public void UpdateUI()
    {

    }


}
