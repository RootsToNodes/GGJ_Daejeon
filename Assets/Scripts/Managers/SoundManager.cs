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
        Debug.Log(this.GetType().Name + ": �ʱ�ȭ �Ϸ�");
        audioSource = new AudioSource[(System.Enum.GetValues(typeof(AudioList)).Length)];
        // ��ü ��� ��������?
    }

    public static void PlaySound(AudioList audio)
    {
        GetInstance().audioSource[(int)audio].Play();
    }
    
}
