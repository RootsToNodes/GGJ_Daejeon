using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class test : MonoBehaviour
{
    AudioEnum myAudio = AudioEnum.Attack;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.PlaySound(myAudio);
        SoundManager.PlaySound(myAudio = AudioEnum.StartSound);

    }
}
