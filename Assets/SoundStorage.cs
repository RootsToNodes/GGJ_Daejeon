using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundStorage : MonoBehaviour
{
    public AudioSource audioSources;
    public AudioClip[] audioClips = new AudioClip[System.Enum.GetValues(typeof(AudioEnum)).Length];

    public AudioEnum soundType;
}
