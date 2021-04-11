using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogLog : MonoBehaviour
{
    public GameObject dialogLog;
    public Scrollbar scrollbar;
    public Text logText;
    public Fungus.DialogInput dialogInput;
    public Fungus.DialogInput dialogInput2;

    private bool LogVisible;

    private string logString;

    private void Start() {
        logString = "";
    }

    private void OnEnable() {
        Fungus.BlockSignals.OnCommandExecute += LogText;
    }

    private void OnDisable() {
        Fungus.BlockSignals.OnCommandExecute -= LogText;
    }

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

    private void LogText(Fungus.Block block, Fungus.Command command, int commandIndex, int maxCommandIndex) {
        if (command is Fungus.Say sayCommand) {
            string standardText = sayCommand.GetStandardText();
            standardText = standardText.Trim();
            standardText = RemoveBetween(standardText, '{', '}');
            if (standardText.Length > 0) {
                string line = sayCommand._Character.NameText + ": " + standardText;
                logString += "\n\n" + line;
                logText.text = logString;
            }
        }
    }

    private string RemoveBetween(string s, char begin, char end) {
        Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
        return new Regex(" +").Replace(regex.Replace(s, string.Empty), " ");
    }
}
