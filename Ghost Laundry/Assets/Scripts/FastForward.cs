using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    //private Fungus.DialogInput[] dialogInputs;

    //private void Start() {
    //    dialogInputs = FindObjectsOfType<Fungus.DialogInput>();
    //}

    public Fungus.DialogInput dialogInput;
    public Fungus.DialogInput dialogInput2;

    private void OnEnable() {
        Fungus.BlockSignals.OnBlockEnd += ResetFastForwardingFlag;
    }

    private void OnDisable() {
        Fungus.BlockSignals.OnBlockEnd -= ResetFastForwardingFlag;
    }

    private void ResetFastForwardingFlag(Fungus.Block block) {
        dialogInput.ResetFastForwardingFlag();
        dialogInput2.ResetFastForwardingFlag();
    }
}
