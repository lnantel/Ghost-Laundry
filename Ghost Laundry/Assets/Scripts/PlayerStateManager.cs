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
    public bool Carrying { private set; get; }
    public bool Walking { private set; get; }

    public Collider2D[] Rooms;
    public int CurrentRoomIndex;

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
        for(int i = 0; i < Rooms.Length; i++) {
            if (Rooms[i].OverlapPoint(transform.position)) {
                CurrentRoomIndex = i;
            }
        }
    }

    public void StartDash() {
        Dashing = true;
        Lock(ref moveLocks);
        Lock(ref dashLocks);
    }

    public IEnumerator DashCooldown(float duration) {
        Lock(ref dashLocks);
        yield return new WaitForSeconds(duration);
        Unlock(ref dashLocks);
    }

    public void EndDash() {
        Dashing = false;
        Unlock(ref moveLocks);
        Unlock(ref dashLocks);
    }

    public void StartCarry() {
        Carrying = true;
    }

    public void EndCarry() {
        Carrying = false;
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

    public void StartWalk() {
        Walking = true;
    }

    public void EndWalk() {
        Walking = false;
    }
}
