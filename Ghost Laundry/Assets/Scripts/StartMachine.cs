using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMachine : MonoBehaviour
{
  public Animator WM; 

  public Animator Dryer; 


public void StartWM(){

    WM.SetTrigger("StartWM"); 
}

public void StartDryer(){

    Dryer.SetTrigger("StartD"); 
}


}
