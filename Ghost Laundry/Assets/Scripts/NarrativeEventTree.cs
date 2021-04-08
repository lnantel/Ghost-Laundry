using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeEventTree
{
    public NarrativeEvent[] tree;

    public NarrativeEvent GetNextEvent() {
        for(int i = 0; i < tree.Length;) {
            if (tree[i].Completed && i < tree.Length - 1) {
                i = tree[i].NextEventIndex;
            }
            else if(!tree[i].Completed)
                return tree[i];
        }
        return null;
    }

    public bool HasEventOnDay(int day) {
        NarrativeEvent e = GetNextEvent();
        if (e.Day == day) return true;
        else return false;
    }

    public bool IsCompleted() {
        return tree[tree.Length - 1].Completed;
    }

    public int EndingObtained() {
        return tree[tree.Length - 1].NextEventIndex;
    }
}
