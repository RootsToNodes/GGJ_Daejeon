using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioList : int
{
    Hited,
    Attack,
    EnemyAttack,
    Defeat,
    StartSound
}
public class SoundManager : Manager<SoundManager>
{
    [SerializeField]
    AudioSource[] audioSource;

    public SoundManager()
    {
        GetInstance();
    }

    public void PlaySound(AudioList audio)
    {
        audioSource[(int)audio].Play();
    }
    
}
