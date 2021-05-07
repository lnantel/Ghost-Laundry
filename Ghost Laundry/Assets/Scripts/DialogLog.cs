using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class DialogLog : MonoBehaviour
{
    public static DialogLog instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public string LogString { get => logString; }

    private string logString;

    private void Start() {
        logString = "";
    }

    private void OnEnable() {
        Fungus.BlockSignals.OnCommandExecute += LogText;
        Fungus.BlockSignals.OnBlockEnd += LogBlockEnd;
    }

    private void OnDisable() {
        Fungus.BlockSignals.OnCommandExecute -= LogText;
        Fungus.BlockSignals.OnBlockEnd -= LogBlockEnd;
    }

    private void LogText(Fungus.Block block, Fungus.Command command, int commandIndex, int maxCommandIndex) {
        if (block.BlockName.Equals("Toast")) {
            if(command is Fungus.Say sayCommand) {
                logString += "\n\n" + block.GetFlowchart().GetStringVariable("Line");
            }
        }
        else if (command is Fungus.Say sayCommand) {
            string standardText = sayCommand.GetStandardText();
            standardText = standardText.Trim();
            standardText = RemoveBetween(standardText, '{', '}');
            if (standardText.Length > 0) {
                string characterName = sayCommand._Character.NameText;
                if (characterName.Length > 0) characterName += ": ";
                string line = characterName + standardText;
                logString += "\n\n" + line;
            }
        }
    }

    private void LogBlockEnd(Fungus.Block block) {
        logString += "\n\n" + "* * *";
    }

    private string RemoveBetween(string s, char begin, char end) {
        Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
        return new Regex(" +").Replace(regex.Replace(s, string.Empty), " ");
    }
}
