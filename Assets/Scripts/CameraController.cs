using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
    private bool cameraAvailable;
    private WebCamTexture webCamTexture;
    private bool frontFacing;
    private WebCamDevice[] devices;
    //private Texture backgroundTexture;

    public RawImage background;
    public AspectRatioFitter fit;
    public SQLiteController SqlCtrl;
    public ChangeSceneController changeSceneController;

    IEnumerator StartNew()
    {
        webCamTexture = new WebCamTexture();

        devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                frontFacing = true;
            }
        }
        background.texture = webCamTexture;
        webCamTexture.Play();
        yield return new WaitForEndOfFrame();
    }

    void Start()
    {
        //backgroundTexture = background.texture;
        /*devices= WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No camera detected");
            cameraAvailable = false;
            return;
        }

        for (int i =0; i<devices.Length; i++)
        {
            if (devices[i].isFrontFacing)
            {
                webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                frontFacing = true;
            }
        }
        if ((!frontFacing)&&(devices.Length >= 1))
        {
            webCamTexture = new WebCamTexture(devices[1].name, Screen.width, Screen.height);
            frontFacing = devices[1].isFrontFacing;
        }*/
        webCamTexture = new WebCamTexture();

        if (webCamTexture == null)
        {
            Debug.Log("unable to find front facing camera");
            return;
        }
        webCamTexture.Play();
        background.texture = webCamTexture;

        cameraAvailable = true;
    }

    void Update()
    {
        if (!cameraAvailable)
        {
            return;
        }
        float ratio = (float)webCamTexture.width / (float)webCamTexture.height;
        fit.aspectRatio = ratio;

        float scaleY = webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

        int orient = -webCamTexture.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0, 0, orient);
    }

    public void TakePicture()
    {

        // NOTE - you almost certainly have to do this here:

        //yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        Color[] pix = photo.GetPixels(0, 0, 256, 256);
        System.Array.Reverse(pix, 0, pix.Length);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //background.texture = photo;
        //new WaitForSeconds(15);
        //background.texture = webCamTexture;

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible

        int id = SqlCtrl.NextID("pictures");
        CurrentUser.CurrentPicture = "photo" + id + ".png";
        Debug.Log("picture about to be writte");
        File.WriteAllBytes(Application.persistentDataPath + "photo"+id+".png", bytes);
        Debug.Log("picture sql about to written");
        SqlCtrl.SavePicture(id, CurrentUser.ID, 1, ("photo" + id + ".png"));
        Debug.Log("Picture Written");

        webCamTexture.Stop();
        
        changeSceneController.ChangeToShowPictureScene();
    }

    public void TakePictureOld()
    {
        TakePhoto();
    }

    public IEnumerator TakePhoto()
    {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 

        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
        photo.SetPixels(webCamTexture.GetPixels());
        photo.Apply();

        //Encode to a PNG
        byte[] bytes = photo.EncodeToPNG();
        //Write out the PNG. Of course you have to substitute your_path for something sensible
        int id = SqlCtrl.NextID("pictures");
        CurrentUser.CurrentPicture = "photo" + id + ".png";
        Debug.Log("picture about to be writte");
        File.WriteAllBytes(Application.persistentDataPath + "photo" + id + ".png", bytes);

        webCamTexture.Stop();

        changeSceneController.ChangeToShowPictureScene();
    }

    public void TurnCamera()
    {
        if (frontFacing)
        {
            for (int i = 0; i < devices.Length; i++)
            {
                if (!devices[i].isFrontFacing)
                {
                    webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                    frontFacing = false;
                }
            }
        }
        else if (!frontFacing)
        {
            for (int i = 0; i < devices.Length; i++)
            {
                if (devices[i].isFrontFacing)
                {
                    webCamTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                    frontFacing = true;
                }
            }
        }
        webCamTexture.Play();
        background.texture = webCamTexture;
    }
}
