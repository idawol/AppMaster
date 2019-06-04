using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GPSController : MonoBehaviour {
    double targetLat;
    double targetLon;
    bool targetReached;

    double distanceBoundry= 0.013;

    public RawImage Image;
    public RawImage Arrow;
    public AudioSource Sound;
    public Text Direction;
    public SQLiteController SqlCtrl;
    public AudioSource Beep;

    void Start () {
        StartGPS();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Navigation"))
        {
            GetTarget();
            targetReached = false;
        }
    }

    public void StartGPS()
    {
        Input.location.Start();
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("gps off");
            return;
        }
    }
	
	void Update () {
        Debug.Log("mastergame update location");
        if (targetLat==0 || targetLon == 0 || targetReached)
        {
            return;
        }

        double lat = Input.location.lastData.latitude;
        double lon = Input.location.lastData.longitude;

        Debug.Log("mastergame gps " + lat + " " + lon);

        if (GetDistance(targetLat, targetLon, lat, lon)<=distanceBoundry)
        {
            Handheld.Vibrate();
            Beep.Play();
            targetReached= true;
            Input.location.Stop();
        }
    }

    double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371; // Radius of the earth in km
        var dLat = ToRadians(lat2 - lat1);  // deg2rad below
        var dLon = ToRadians(lon2 - lon1);
        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c; // Distance in km
        return d;
    }

    double ToRadians(double deg)
    {
        return deg * (Math.PI / 180);
    }

    public void GetTarget()
    {
        RotateController arrowCtrl = Arrow.GetComponent<RotateController>();
        string picture;
        double lat, lon;
        int direction = SqlCtrl.getDirection(CurrentUser.GetGameScreen(), out picture, out lat, out lon);
        targetLat = lat;
        targetLon = lon;
        Image.texture = LoadPicture(picture);
        if (direction == (int)Directions.diagonalLeft)
        {
            Direction.text = "Gå litt til venstre";
            arrowCtrl.TurnArrow(45);
            Sound.clip = LoadAudio("diagonalLeft.mp3"); 
        }
        else if (direction == (int)Directions.diagonalRight)
        {
            Direction.text = "Gå litt til høyre";
            arrowCtrl.TurnArrow(-45);
            Sound.clip = LoadAudio("diagonalRight.mp3"); 
        }
        else if (direction == (int)Directions.left)
        {
            Direction.text = "Gå til venstre";
            arrowCtrl.TurnArrow(90);
            Sound.clip = LoadAudio("Left.mp3");
        }
        else if (direction == (int)Directions.right)
        {
            Direction.text = "Gå til høyre";
            arrowCtrl.TurnArrow(-90);
            Sound.clip = LoadAudio("Right.mp3");
        }
        else if (direction == (int)Directions.straight)
        {
            Direction.text = "Gå rett fram";
            arrowCtrl.TurnArrow(0);
            Sound.clip = LoadAudio("Straight.mp3");
        }
        else if (direction == (int)Directions.turnaround)
        {
            Direction.text = "Snu";
            arrowCtrl.TurnArrow(180);
            Sound.clip = LoadAudio("TurnAround.mp3");
        }
        else if (direction == (int)Directions.stop)
        {
            Direction.text = "Stopp og vent";
            Debug.Log("mastergame stop direction");
            Arrow.texture = LoadPicture("stop.png");
            Sound.clip = LoadAudio("stop.mp3");
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

    public AudioClip LoadAudio(string absolutePath)
    {
        string finalPath;
        WWW localFile;

        finalPath = "jar:file://" + Application.dataPath + "!/assets/" + absolutePath;
        try
        {
            localFile = new WWW(finalPath);
            while (!localFile.isDone) { }

            return localFile.audioClip;
        }
        catch
        {
            Debug.Log("mastergame file not found");
        }
        return null;
    }
}
