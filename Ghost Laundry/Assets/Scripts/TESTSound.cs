using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTSound : MonoBehaviour
{
    private AudioSourceController controller;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            TimeManager.instance.StartDay();
            controller = AudioManager.instance.PlaySoundLoop(SoundName.Dash);
        }

        if (Input.GetKeyDown(KeyCode.S) && controller != null) controller.Stop();
    }
}
