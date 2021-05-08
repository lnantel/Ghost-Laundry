using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveManager : MonoBehaviour
{
    public static Action LoadingComplete;

    public static SaveManager instance;

    public static SaveData Data;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else Destroy(gameObject);
    }

    private void OnEnable() {
        GameManager.EventManagerReset += OnEventsReset;
    }

    private void OnDisable() {
        GameManager.EventManagerReset -= OnEventsReset;
    }

    public static void Save() {
        if (TimeManager.instance.CurrentDay > 5) return;

        //Populate DayData
        SaveData.DayData day = new SaveData.DayData();
        day.CurrentDay = TimeManager.instance.CurrentDay;
        day.Detergent = DetergentManager.instance.CurrentAmount;
        day.Money = MoneyManager.instance.CurrentAmount;
        day.ReputationHighScore = ReputationManager.instance.HighScore;
        day.narrativeData = new List<SaveData.EventData>();
        day.OllieSafetyPoints = EventManager.instance.ollieEventManager.SafetyPoints;
        for(int i = 0; i < EventManager.instance.EventTrees.Length; i++) {
            for(int j = 0; j < EventManager.instance.EventTrees[i].tree.Length; j++) {
                SaveData.EventData eventData = new SaveData.EventData();
                eventData.Completed = EventManager.instance.EventTrees[i].tree[j].Completed;
                eventData.TreeIndex = i;
                eventData.Index = j;
                eventData.NextIndex = EventManager.instance.EventTrees[i].tree[j].NextEventIndex;
                day.narrativeData.Add(eventData);
            }
        }

        //Add new DayData to existing SaveData
        //Erase pre-existing DayData corresponding to current or future days, if applicable
        for (int i = 0; i < Data.Days.Count; i++) {
            if (Data.Days[i].CurrentDay >= day.CurrentDay)
                Data.Days.RemoveAt(i--);
        }

        //If past days are missing, create saves for them using current data
        for(int i = 0; i < day.CurrentDay; i++) {
            if (i >= Data.Days.Count) {
                SaveData.DayData pastDay = SaveData.CloneDayData(day);
                pastDay.CurrentDay = i;
                Data.Days.Insert(i, pastDay);
            }
        }

        Data.Days.Add(day);

        if(FileManager.WriteToFile("SaveData.dat", Data.ToJson())) {
            //Debug.Log("Save successful");
        }
        else {
            Debug.LogError("Failed to save!");
        }
    }

    public static void LoadSaveData() {
        //Load Data from file and load latest day
        string json;
        if(FileManager.LoadFromFile("SaveData.dat", out json)) {
            SaveData loadedData = new SaveData();
            loadedData.LoadFromJson(json);
            Data = loadedData;
            int latestDay = -1;
            for (int i = 0; i < Data.Days.Count; i++) {
                if (Data.Days[i].CurrentDay > latestDay)
                    latestDay = Data.Days[i].CurrentDay;
            }
            LoadDay(latestDay + 1);
        }
        else {
            CreateNewSave();
        }
    }

    public static void LoadDay(int dayToLoad) {
        PurgeDaysFollowing(dayToLoad - 1);
        SaveData.DayData dayData = Data.Days[0];
        //Find the latest DayData preceding the day to load
        for(int i = 1; i < Data.Days.Count; i++) {
            if(Data.Days[i].CurrentDay < dayToLoad && Data.Days[i].CurrentDay >= dayData.CurrentDay) {
                dayData = Data.Days[i];
            }
        }
        //for(int i = 0; i < Data.Days.Count; i++) {
        //    if(Data.Days[i].CurrentDay == dayToLoad) {
        //      SaveData.DayData day = Data.Days[i];
        TimeManager.instance.CurrentDay = dayToLoad;
        DetergentManager.instance.CurrentAmount = dayData.Detergent;
        MoneyManager.instance.SetCurrentAmount(dayData.Money);
        ReputationManager.instance.HighScore = dayData.ReputationHighScore;
        EventManager.instance.ollieEventManager.SafetyPoints = dayData.OllieSafetyPoints;
        for(int j = 0; j < dayData.narrativeData.Count; j++) {
            SaveData.EventData eventData = dayData.narrativeData[j];
            EventManager.instance.EventTrees[eventData.TreeIndex].tree[eventData.Index].Completed = eventData.Completed;
            EventManager.instance.EventTrees[eventData.TreeIndex].tree[eventData.Index].NextEventIndex = eventData.NextIndex;
        }
        //      break;
        //    }
        //}
        if (LoadingComplete != null) LoadingComplete();
        //Save();
    }

    public static SaveData.DayData GetDayData(int day) {
        SaveData.DayData dayData = Data.Days[0];
        //Find the DayData of the day to load, or the latest one before that
        for (int i = 1; i < Data.Days.Count; i++) {
            if (Data.Days[i].CurrentDay <= day && Data.Days[i].CurrentDay >= dayData.CurrentDay) {
                dayData = Data.Days[i];
            }
        }
        return dayData;
    }

    public static void CreateNewSave() {
        GameManager.instance.ResetEventManager();
    }

    private void OnEventsReset() {
        Data = new SaveData();
        SaveData.DayData day = new SaveData.DayData();
        day.CurrentDay = -1;
        day.Money = 0;
        day.ReputationHighScore = 0;
        day.Detergent = DetergentManager.instance.MaxAmount;
        day.narrativeData = new List<SaveData.EventData>();
        day.OllieSafetyPoints = 0;

        Data.Days = new List<SaveData.DayData>();
        Data.Days.Add(day);

        for (int i = 0; i < EventManager.instance.EventTrees.Length; i++) {
            for (int j = 0; j < EventManager.instance.EventTrees[i].tree.Length; j++) {
                SaveData.EventData eventData = new SaveData.EventData();
                eventData.Completed = false;
                eventData.TreeIndex = i;
                eventData.Index = j;
                eventData.NextIndex = EventManager.instance.EventTrees[i].tree[j].NextEventIndex;
                day.narrativeData.Add(eventData);
            }
        }

        if (FileManager.WriteToFile("SaveData.dat", Data.ToJson())) {
            //Debug.Log("New save file created successfully");
        }
        else {
            Debug.LogError("Failed to save!");
        }
    }

    public static void PurgeDaysFollowing(int day) {
        Data.Days.RemoveAll(d => d.CurrentDay > day);
        if (FileManager.WriteToFile("SaveData.dat", Data.ToJson())) {
            //Debug.Log("Save successful");
        }
        else {
            Debug.LogError("Failed to save!");
        }
    }
}
