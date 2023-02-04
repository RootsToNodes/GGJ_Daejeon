using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum AudioEnum : int
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
    AudioSource audioSource;
    AudioClip[] audioClip;
    SoundStorage storage;
    public SoundManager()
    {
        Debug.Log(this.GetType().Name + ": 초기화 완료");
        //audioSource = new AudioSource[(System.Enum.GetValues(typeof(AudioList)).Length)];
        storage = Camera.main.GetComponent<SoundStorage>();
        audioSource = storage.audioSources;
        audioClip = storage.audioClips;
    }

    public static void PlaySound(AudioEnum audio)
    {
        GetInstance().audioSource.clip = GetInstance().audioClip[(int)audio];
        GetInstance().audioSource.PlayOneShot(GetInstance().audioSource.clip);
    }
}
