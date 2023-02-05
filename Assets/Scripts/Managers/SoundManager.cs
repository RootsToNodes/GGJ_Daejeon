using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public enum AudioEnum : int
{
    Hited,
    TurretAttack,
    EnemyAttack,
    EnemyHited,
    EnemyDie,
    Defeat,
    StartSound,
    Money,
    CountDown,
    Build,
    Cut,
    Win
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
