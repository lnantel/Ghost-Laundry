using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clothesline : WorkStation
{
    //Chaque emplacement dans le tableau hungGarments représente une pince à linge, et contient une référence vers le vêtement accroché
    //Si aucun vêtement n'est accroché, l'emplacement est null
    public Garment[] hungGarments;
    public int TimeToDry;
    public int Pegs;

    protected override void Start() {
        base.Start();
        hungGarments = new Garment[Pegs];
    }

    public bool HangGarment(int position, Garment garment) {
        //Check whether the position is free
        if(hungGarments[position] == null) {
            //If it is, check whether enough slots in the array are free on either side, depending on the garment's size
            //Algorithm:
            int r = position + 1;
            int l = position - 1;
            bool spaceRight = r < hungGarments.Length && r >= 0;
            bool spaceLeft = l < hungGarments.Length && l >= 0;
            List<int> indexes = new List<int>();
            indexes.Add(position);

            while (indexes.Count < garment.size) {
                if (spaceRight) {
                    if (hungGarments[r] == null)
                        indexes.Add(r++);
                    else
                        spaceRight = false;
                }         
                if (spaceLeft && indexes.Count < garment.size) {
                    if (hungGarments[l] == null)
                        indexes.Add(l--);
                    else
                        spaceRight = false;
                }

                if (r >= hungGarments.Length)
                    spaceRight = false;
                if (l < 0)
                    spaceLeft = false;

                if(!spaceLeft && !spaceRight)
                    break;
            }

            if(indexes.Count >= garment.size) {
                foreach(int i in indexes) {
                    hungGarments[i] = garment;
                }
                return true;
            }
            else {
                //no space!
                Debug.Log("Not enough space to hang garment here");
                return false;
            }
        }
        else {
            //position taken!
            Debug.Log("This pin is in use");
            return false;
        }
    }

    public Garment RemoveGarmentFromLine(int position) {
        //Check whether position contains a garment
        if(hungGarments[position] != null) {
            Garment garment = hungGarments[position];
            for(int i = 0; i < hungGarments.Length; i++) {
                if (hungGarments[i] != null && hungGarments[i].Equals(garment))
                    hungGarments[i] = null;
            }
            return garment;
        }
        return null;
    }

    private void Update() {
        //Each garment hung on the line dries over time
        List<Garment> uniqueGarments = new List<Garment>();
        for(int i = 0; i < hungGarments.Length; i++) {
            if (!uniqueGarments.Contains(hungGarments[i]))
                uniqueGarments.Add(hungGarments[i]);
        }

        foreach(Garment garment in uniqueGarments) {
            //Update humidity
            if(garment != null) {
                float deltaHumidity = TimeManager.instance.deltaTime / TimeToDry;
                garment.Humidity = (garment.Humidity - deltaHumidity);
            }
        }
    }
}
