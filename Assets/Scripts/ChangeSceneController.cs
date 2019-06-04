using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneController : MonoBehaviour {

    public void ChangeToMedalsScene()
    {
        Debug.Log("change to user scene");
        SceneManager.LoadScene("Medals", LoadSceneMode.Single);
    }

    public void ExitScene()
    {
        Debug.Log("exit button");
        //Input.location.Stop();
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void ChangeToUserScene()
    {
        Debug.Log("change to user scene");
        SceneManager.LoadScene("User", LoadSceneMode.Single);
    }

    public void ChangeToCreateUserScene()
    {
        Debug.Log("change to create user scene");
        SceneManager.LoadScene("CreateUser", LoadSceneMode.Single);
    }

    public void ChangeToShowPictureScene()
    {
        Debug.Log("change to showPicture scene");
        SceneManager.LoadScene("ShowPicture", LoadSceneMode.Single);
    }

    public void ChangeToContinueScene()
    {
        Debug.Log("change to continue scene");
        SceneManager.LoadScene("ContinueGame", LoadSceneMode.Single);
    }

    public void ChangeToCameraScene()
    {
        Debug.Log("change to camera scene");
        SceneManager.LoadScene("Camera", LoadSceneMode.Single);
    }

    public void LogOut()
    {
        //Log out
        Debug.Log("Logged out");
        SceneManager.LoadScene("LogIn", LoadSceneMode.Single);
    }
}
