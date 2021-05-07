using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogButton : MonoBehaviour
{
    public GameObject dialogLog;
    public Scrollbar scrollbar;
    public Fungus.DialogInput dialogInput;
    public Fungus.DialogInput dialogInput2;

    private bool LogVisible;

    public void ToggleLog() {
        if (Input.GetMouseButtonDown(0)) {
            LogVisible = !LogVisible;
            dialogLog.SetActive(LogVisible);
            if (LogVisible) {
                scrollbar.value = 0;
                dialogInput.SetIgnoreClicksFlag();
                dialogInput2.SetIgnoreClicksFlag();
            }
            else {
                dialogInput.ResetIgnoreClicksFlag();
                dialogInput2.ResetIgnoreClicksFlag();
            }
        }
    }
}
