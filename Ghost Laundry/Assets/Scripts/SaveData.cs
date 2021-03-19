using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<DayData> Days;

    [System.Serializable]
    public struct EventData {
        public int TreeIndex;
        public int Index;
        public int NextIndex;
        public bool Completed;
    }

    [System.Serializable]
    public struct DayData {
        public int CurrentDay;
        public int Money;
        public int Detergent;
        public int Reputation;
        public List<EventData> narrativeData;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json) {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}
