using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class test : MonoBehaviour
{
    AudioList myAudio = AudioList.Attack;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySound(myAudio);
        SoundManager.PlaySound(myAudio = AudioList.StartSound);
        SoundManager.PlaySound(myAudio = AudioList.Defeat);
        SoundManager.PlaySound(myAudio = AudioList.Defeat);
        SoundManager.PlaySound(myAudio = AudioList.Defeat);
        SoundManager.PlaySound(myAudio = AudioList.Defeat);
        SoundManager.PlaySound(myAudio = AudioList.Defeat);



    }
}
