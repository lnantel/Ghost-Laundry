using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NarrativeEventTree
{
    public NarrativeEvent[] tree;

    public NarrativeEvent GetNextEvent() {
        for(int i = 0; i < tree.Length;) {
            if (tree[i].Completed) {
                i = tree[i].NextEventIndex;
            }
            else
                return tree[i];
        }
        return null;
    }

    public bool HasEventOnDay(int day) {
        for(int i = 0; i < tree.Length;) {
            if (tree[i].Day == day) return true;
            if (tree[i].Completed) {
                i = tree[i].NextEventIndex;
            }
            else
                return false;
        }
        return false;
    }
}
