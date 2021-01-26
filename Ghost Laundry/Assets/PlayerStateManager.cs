using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager instance;

    //Constraints
    private int moveLocks;
    private int dashLocks;

    //States
    public bool Dashing { private set; get; }

    private void Awake() {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        moveLocks = 0;
        dashLocks = 0;
    }

    void Update()
    {
        
    }

    public void StartDash() {
        Dashing = true;
        Lock(ref moveLocks);
        Lock(ref dashLocks);
    }

    public void EndDash() {
        Dashing = false;
        Unlock(ref moveLocks);
        Unlock(ref dashLocks);
    }

    private void Lock(ref int lockCounter) {
        lockCounter++;
    }

    private void Unlock(ref int lockCounter) {
        lockCounter = Mathf.Clamp(lockCounter - 1, 0, lockCounter);
    }

    public bool CanMove() {
        return moveLocks == 0;
    }

    public bool CanDash() {
        return dashLocks == 0;
    }
}
