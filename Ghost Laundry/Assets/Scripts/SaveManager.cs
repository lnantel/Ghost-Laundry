﻿using System.Collections;
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
            int latestDay = 0;
            for (int i = 0; i < Data.Days.Count; i++) {
                if (Data.Days[i].CurrentDay > latestDay)
                    latestDay = Data.Days[i].CurrentDay;
            }
            LoadDay(latestDay);
        }
        else {
            CreateNewSave();
        }
    }

    public static void LoadDay(int dayToLoad) {
        PurgeDaysFollowing(dayToLoad);
        for(int i = 0; i < Data.Days.Count; i++) {
            if(Data.Days[i].CurrentDay == dayToLoad) {
                SaveData.DayData day = Data.Days[i];
                TimeManager.instance.CurrentDay = day.CurrentDay;
                DetergentManager.instance.CurrentAmount = day.Detergent;
                MoneyManager.instance.SetCurrentAmount(day.Money);
                ReputationManager.instance.HighScore = day.ReputationHighScore;
                EventManager.instance.ollieEventManager.SafetyPoints = day.OllieSafetyPoints;
                for(int j = 0; j < day.narrativeData.Count; j++) {
                    SaveData.EventData eventData = day.narrativeData[j];
                    EventManager.instance.EventTrees[eventData.TreeIndex].tree[eventData.Index].Completed = eventData.Completed;
                    EventManager.instance.EventTrees[eventData.TreeIndex].tree[eventData.Index].NextEventIndex = eventData.NextIndex;
                }
                break;
            }
        }
        if (LoadingComplete != null) LoadingComplete();
        Save();
    }

    public static void CreateNewSave() {
        GameManager.instance.ResetEventManager();
    }

    private void OnEventsReset() {
        Data = new SaveData();
        SaveData.DayData day = new SaveData.DayData();
        day.CurrentDay = 0;
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
    }
}
