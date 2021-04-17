using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAudioTrigger : EventTrigger
{
    private AudioSourceController loop;

    //Called when the user clicks a button
    public override void OnSelect(BaseEventData data) {
        AudioManager.instance.PlaySound(SoundName.MenuItemConfirmed);
    }

    //Called when the user highlights a buttton
    public override void OnPointerEnter(PointerEventData data) {
        AudioManager.instance.PlaySound(SoundName.MenuItemSelected);
        loop = AudioManager.instance.PlaySoundLoop(SoundName.MenuItemHighlighted);
    }

    public override void OnPointerExit(PointerEventData eventData) {
        AudioManager.instance.PlaySound(SoundName.MenuItemDeselected);
        if (loop != null) loop.Stop();
    }

    private void OnDisable() {
        if (loop != null) loop.Stop();
    }
}
