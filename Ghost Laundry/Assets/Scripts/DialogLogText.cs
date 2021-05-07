using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogLogText : MonoBehaviour
{
    private Text text;

    private void Start() {
        text = GetComponent<Text>();
        if (text != null && DialogLog.instance != null) {
            text.text = DialogLog.instance.LogString;
        }
    }

    private void OnEnable() {
        if(text != null && DialogLog.instance != null) {
            text.text = DialogLog.instance.LogString;
        }
    }
}
