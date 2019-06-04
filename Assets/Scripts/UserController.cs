using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour {
    private int usersGender;

    public ChangeSceneController sceneCtrl;
    public SQLiteController sqlCtrl;

    public InputField inputUsername;
    public Text errorMessage;
    public RawImage MalePicture;
    public RawImage FemalePicture;
    public Image UserBackground;
    public Text username;

    public void Start()
    {
        if (inputUsername != null)
        {
            inputUsername.text = CurrentUser.Username;
        }
        else if (username != null)
        {
            username.text = CurrentUser.Username;
        }

        if (CurrentUser.Gender == 1)
        {
            FemalePicture.enabled = false;
            MalePicture.enabled = true;
        }
        else
        {
            usersGender = 2;
            MalePicture.enabled = false;
            FemalePicture.enabled = true;
            Color myColor = new Color32(0xFF, 0xB6, 0xE0, 0xFF);
            UserBackground.color = myColor;
        }
    }

    public void Logout()
    {
        CurrentUser.Username = null;
        CurrentUser.Gender = 0;
        CurrentUser.ID = 0;

        sceneCtrl.LogOut();
    }

    public void LoginUser()
    {
        string username = inputUsername.text;
        int gender;
        int id;

        bool userExist = sqlCtrl.GetUser(username, out gender, out id);
        if (!userExist) 
        {
            errorMessage.text = "Brukernavn finnes ikke";
            return;
        }
        CurrentUser.Gender = gender;
        CurrentUser.ID = id;
        CurrentUser.Username = username;
        sceneCtrl.ChangeToMainScene();
    }

    public void ChangeToFemale()
    {
        usersGender = 2;
        FemalePicture.enabled = true;
        MalePicture.enabled = false;
        //FF B6 E0 FF
        Color myColor = new Color32(0xFF, 0xB6, 0xE0, 0xFF);
        UserBackground.color = myColor ;
    }
    
    public void ChangeToMale()
    {
        usersGender = 1;
        FemalePicture.enabled = false;
        MalePicture.enabled = true;
    }

    public void Save()
    {
        string newUsername = inputUsername.text;
        int ID;
        Debug.Log("mastergame Id " + CurrentUser.ID);
        if (CurrentUser.ID == 0)
        {
            if (!sqlCtrl.isAvailableUsername(newUsername))
            {
                errorMessage.text = "Brukernavn er opptatt";
                return;
            }
            ID = sqlCtrl.NextID("users");
        }
        else
        {
            ID = CurrentUser.ID;
        }

        if (sqlCtrl.SaveUser(ID, newUsername, usersGender))
        {
            CurrentUser.ID = ID;
            CurrentUser.Username = newUsername;
            CurrentUser.Gender = usersGender;
            sceneCtrl.ChangeToMainScene();
        }
        else
        {
            errorMessage.text = "Ble ikke lagret";
        }
    }

    public void Create() //Not used
    {
        string newUsername = inputUsername.text;
        int ID;
        Debug.Log("mastergame Id " + CurrentUser.ID);
        if (CurrentUser.ID==0)
        {
            if (!sqlCtrl.isAvailableUsername(newUsername))
            {
                errorMessage.text = "Brukernavn er opptatt";
                return;
            }
            ID = sqlCtrl.NextID("users");
        }
        else
        {
            ID = CurrentUser.ID;
        }
        
        if (sqlCtrl.CreateUser(ID, newUsername, usersGender))
        {
            CurrentUser.ID = ID;
            CurrentUser.Username = newUsername;
            CurrentUser.Gender = usersGender;
            sceneCtrl.ChangeToMainScene();
        }
        else
        {
            errorMessage.text = "Ble ikke lagret";
        }
    }
}
