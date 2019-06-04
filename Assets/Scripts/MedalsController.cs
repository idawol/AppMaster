using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalsController : MonoBehaviour {
    public RawImage StoryImage;
    public Text StoryText;
    public Image MedalPanel, Medal;
    public SQLiteController SqlCtrl;

    public void Start()
    {
        List<string> names = new List<string>();
        List<string> images = new List<string>();

        Debug.Log("mastergame start to get medals");
        bool userHasMedals = SqlCtrl.getMedals(CurrentUser.ID, out names, out images);
        Debug.Log("mastergame " + userHasMedals);
        if (userHasMedals)
        {
            Debug.Log("mastergame " + names + images);
            for (int i=0; i>names.Count; i++)
            {
                StoryImage.texture = LoadPicture(images[i]);
                StoryText.text = names[i];
            }
        }
        else
        {
            StoryImage.enabled = false;
            StoryText.enabled = false;
            Medal.enabled = false;
            MedalPanel.enabled = false;
        }
    }

    public Texture LoadPicture(string absoluteImagePath)
    {
        string finalPath;
        WWW localFile;

        finalPath = "jar:file://" + Application.dataPath + "!/assets/" + absoluteImagePath;
        localFile = new WWW(finalPath);
        while (!localFile.isDone) { }

        return localFile.texture;
    }
}
