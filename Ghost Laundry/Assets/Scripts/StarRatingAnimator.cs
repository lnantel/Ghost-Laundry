using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRatingAnimator : MonoBehaviour
{
    public void PlaySound() {
        AudioManager.instance.PlaySound(SoundName.ReputationGain);
    }
}
