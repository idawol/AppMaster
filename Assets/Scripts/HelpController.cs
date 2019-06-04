using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpController : MonoBehaviour {
    int number = 41215170;
    string message;

    public void CallCaregiver()
    {
        Application.OpenURL("tel://" + number);
    }

    public void textCaregiver()
    {

        message = CurrentUser.Username + " trenger hjelp på latitude " + Input.location.lastData.latitude + " longitude " + Input.location.
            lastData.longitude;

        string URL = string.Format("sms:{0}?body={1}", number, message);

        Application.OpenURL(URL);
    }

    public void changeToHelpScreen()
    {
        SceneManager.LoadScene("HelpScreen", LoadSceneMode.Single);
    }
}
