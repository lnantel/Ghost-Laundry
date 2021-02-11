using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket
{
    public List<Garment> contents;
    public List<Vector3> positions;
    public int capacity;
    public int currentLoad;
    public int tag;

    public Basket(){
        this.contents = new List<Garment>();
        this.positions = new List<Vector3>();
        this.capacity = 10;
        this.currentLoad = 0;
        this.tag = 0;
    }

    //Adds a Garment to the basket, with a random BasketView position
    public bool AddGarment(Garment garment) {
        if (HasSpaceFor(garment.size)) {
            contents.Add(garment);
            positions.Add(GetRandomPosition());
            currentLoad += garment.size;
            return true;
        }
        return false;
    }

    //Adds a Garment to the basket, with a predefined BasketView position
    public bool AddGarment(Garment garment, Vector2 position) {
        if (HasSpaceFor(garment.size)) {
            contents.Add(garment);
            positions.Add(position);
            currentLoad += garment.size;
            return true;
        }
        return false;
    }

    public bool RemoveGarment(Garment garment) {
        if (contents.Contains(garment)) {
            int index = contents.IndexOf(garment);
            contents.RemoveAt(index);
            positions.RemoveAt(index);
            currentLoad -= garment.size;
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

    private Vector3 GetRandomPosition() {
        return new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0.0f);
    }
}
