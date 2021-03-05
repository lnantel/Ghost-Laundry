using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    public Fungus.DialogInput dialogInput;

    private void OnEnable() {
        Fungus.BlockSignals.OnBlockEnd += ResetFastForwardingFlag;
    }

    private void OnDisable() {
        Fungus.BlockSignals.OnBlockEnd -= ResetFastForwardingFlag;
    }

    private void ResetFastForwardingFlag(Fungus.Block block) {
        dialogInput.ResetFastForwardingFlag();
    }
}
