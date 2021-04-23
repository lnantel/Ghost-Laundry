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
        public int ReputationHighScore;
        public List<EventData> narrativeData;
        public int OllieSafetyPoints;
    }

    public static DayData CloneDayData(DayData day) {
        DayData clone = new DayData();
        clone.CurrentDay = day.CurrentDay;
        clone.Money = day.Money;
        clone.Detergent = day.Detergent;
        clone.ReputationHighScore = day.ReputationHighScore;
        clone.narrativeData = new List<EventData>();
        clone.OllieSafetyPoints = day.OllieSafetyPoints;
        for(int i = 0; i < day.narrativeData.Count; i++) {
            EventData clonedEvent = new EventData();
            clonedEvent.TreeIndex = day.narrativeData[i].TreeIndex;
            clonedEvent.Index = day.narrativeData[i].Index;
            clonedEvent.NextIndex = day.narrativeData[i].NextIndex;
            clonedEvent.Completed = day.narrativeData[i].Completed;
        }
        return clone;
    }

    public string ToJson() {
        return JsonUtility.ToJson(this);
    }

    public void LoadFromJson(string a_Json) {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}
