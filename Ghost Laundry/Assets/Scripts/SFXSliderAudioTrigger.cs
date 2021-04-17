using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SFXSliderAudioTrigger : EventTrigger
{
    public override void OnEndDrag(PointerEventData eventData) {
        AudioManager.instance.PlaySound(SoundName.CustomerArrives);
    }
}
