using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twotorial : MonoBehaviour
{
    private bool SortingTooltip;
    private bool InspectTooltip;
    private bool TempTooltip;
    private bool TumbleTooltip;

    private bool WoolGrabbed;
    private bool WoolInspected;

    private void OnEnable() {
        WorkStation.RequestCarriedBasket += OnLaundryViewOpened;
        LaundryObject.Grabbed += OnLaundryObjectGrabbed;
        LaundryObject.Inspected += OnLaundryObjectInspected;
    }

    private void OnDisable() {
        WorkStation.RequestCarriedBasket -= OnLaundryViewOpened;
        LaundryObject.Grabbed -= OnLaundryObjectGrabbed;
        LaundryObject.Inspected -= OnLaundryObjectInspected;
    }

    private void OnLaundryViewOpened() {
        if(LaundryTaskController.instance.activeWorkStation is TableWorkstation) {
            if (!SortingTooltip) {
                SortingTooltip = true;
                ToastManager.instance.SayLine("The TABLE is perfect for SORTING clothes into different baskets.", 1.0f);
                ToastManager.instance.SayLine("For today, since yer gonna be washin' with HOT water a lot, make sure to separate WHITE clothes from COLORED ones!", 2.0f);
                ToastManager.instance.SayLine("And don't worry about mixin' customers' clothes together. The BAGGER will sort them for ya!", 2.0f);
                ToastManager.instance.SayLine("To better identify baskets at a glance, you can change their COLOR TAG with LEFT CLICK.", 1.0f, true);
            }
        }else if (LaundryTaskController.instance.activeWorkStation is WashingMachine) {
            if (!TempTooltip) {
                TempTooltip = true;
                ToastManager.instance.SayLine("The temperature dial is fixed, so ya can set it to HOT now.", 1.0f);
                ToastManager.instance.SayLine("When ya do, though, make sure not to mix WHITE and COLORED clothing!", 1.0f);
                ToastManager.instance.SayLine("Otherwise, the WHITE clothes'll be RUINED!", 1.0f);
                ToastManager.instance.SayLine("The best place to sort 'em is the TABLE.", 1.0f);
            }
        }else if (LaundryTaskController.instance.activeWorkStation is Dryer) {
            if (!TumbleTooltip) {
                TumbleTooltip = true;
                ToastManager.instance.SayLine("Have ya got any WOOL to dry? If so, careful not to TEAR it!", 1.0f);
                ToastManager.instance.SayLine("Make sure to set the dial to LOW TUMBLE. It'll take a bit longer to dry, though!", 1.0f);
            }
        }
    }

    private void OnLaundryObjectGrabbed(LaundryObject laundryObject) {
        if(laundryObject is LaundryGarment laundryGarment) {
            if (laundryGarment.garment.fabric.name.Equals("Wool") && !WoolGrabbed) {
                WoolGrabbed = true;
                ToastManager.instance.SayLine("Hey, that's WOOL! It's a tricky fabric, so make sure to check its LAUNDRY TAG with RIGHT CLICK!", 1.0f);
            }
        }
    }

    private void OnLaundryObjectInspected(LaundryObject laundryObject) {
        if (laundryObject is LaundryGarment laundryGarment) {
            if (laundryGarment.garment.fabric.name.Equals("Wool") && !WoolInspected) {
                WoolInspected = true;
                ToastManager.instance.SayLine("See that first icon? It means ya should ONLY wash wool with HOT WATER.", 1.0f);
                ToastManager.instance.SayLine("The second icon means ya should set the DRYER to LOW TUMBLE.", 1.0f);
                ToastManager.instance.SayLine("Otherwise, the wool will get ripped to shreds!", 1.0f);
                ToastManager.instance.SayLine("I'll let you guess what the last icon means.", 1.0f);
            }
        }
    }
}
