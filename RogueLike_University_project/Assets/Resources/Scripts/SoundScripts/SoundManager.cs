using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public void ChangeAudioSource(string ClipSound)
    {
        this.gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(ClipSound);
        this.gameObject.GetComponent<AudioSource>().Play();
    }
}
