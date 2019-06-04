using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ShowPictureController : MonoBehaviour {

    public RawImage background;
    public AspectRatioFitter fit;
    public SQLiteController sqlCtrl;

    private void Start()
    {
        Debug.Log("Start loading picture");

        Texture2D tex = new Texture2D(2, 2);
        //WWW localfile = new WWW("URI=file:" + Application.persistentDataPath + fileName);
        //WWW localfile = new WWW("URI=file:" + Application.persistentDataPath + CurrentUser.CurrentPicture);
        //while (!localfile.isDone) { }

        byte[] localfile = File.ReadAllBytes(Application.persistentDataPath + CurrentUser.CurrentPicture);
        Debug.Log("picture ");


        tex.LoadImage(localfile);
        Debug.Log("Picture Loaded");

        background.texture = tex;

        CurrentUser.CanNext = true;
    }
}
