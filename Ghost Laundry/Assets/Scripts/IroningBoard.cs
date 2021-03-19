using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IroningBoard : WorkStation
{
    public Animator animator;

    protected override void Start() {
        areaPrefab = (GameObject)Resources.Load("IroningBoardArea");
        base.Start();
    }
}
