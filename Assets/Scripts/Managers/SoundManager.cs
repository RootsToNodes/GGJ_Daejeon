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
        Debug.Log(this.GetType().Name + ": 초기화 완료");
        audioSource = new AudioSource[(System.Enum.GetValues(typeof(AudioList)).Length)];
        // 개체 어떻게 가져올지?
    }

    public static void PlaySound(AudioList audio)
    {
        GetInstance().audioSource[(int)audio].Play();
    }
    
}
