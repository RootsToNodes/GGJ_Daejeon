using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    AudioClip[] audioClip;
    SoundStorage storage;
    public SoundManager()
    {
        Debug.Log(this.GetType().Name + ": �ʱ�ȭ �Ϸ�");
        //audioSource = new AudioSource[(System.Enum.GetValues(typeof(AudioList)).Length)];
        storage = Camera.main.GetComponent<SoundStorage>();
        audioSource = storage.audioSources;
        audioClip = storage.audioClips;
        // ��ü ��� ��������?
    }

    public static void PlaySound(AudioList audio)
    {
        for (int i = 0; i < GetInstance().audioSource.Length; i++)
        {
            if (GetInstance().audioSource[i].isPlaying)
            {
                return;
            }
            else
            {
                GetInstance().audioSource[i].clip = GetInstance().audioClip[(int)audio];
                GetInstance().audioSource[i].Play();
                return;
            }
        }
        
    }
    
}
