using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundController : MonoBehaviour {
    public AudioSource sound;

    public void PlaySound()
    {
        Debug.Log("masterGame playing sound");
        sound.Play();
    } 
}
