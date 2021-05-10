using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButton : MonoBehaviour
{
    public void BackOut() {
        LaundryTaskController.instance.BackOut();
    }
}
