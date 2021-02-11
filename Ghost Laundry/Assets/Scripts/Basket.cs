using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket
{
    public List<Garment> contents;
    public int capacity;
    public int currentLoad;

    public Basket(){
        this.contents = new List<Garment>();
        this.capacity = 10;
        this.currentLoad = 0;
    }

    public bool AddGarment(Garment garment) {
        if (HasSpaceFor(garment.size)) {
            contents.Add(garment);
            currentLoad += garment.size;
            return true;
        }
        return false;
    }

    public Garment RemoveTopGarment() {
        if (contents.Count > 0) {
            Garment garment = contents[contents.Count - 1];
            contents.RemoveAt(contents.Count - 1);
            currentLoad -= garment.size;
            return garment;
        }
        return null;
    }

    public List<Garment> RemoveAll() {
        List<Garment> temp = contents;
        contents = new List<Garment>();
        currentLoad = 0;
        return temp;
    }

    public bool HasSpaceFor(int load) {
        return currentLoad + load <= capacity;
    }
}
