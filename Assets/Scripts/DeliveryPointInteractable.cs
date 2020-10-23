using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPointInteractable : Interactable
{
    private bool activated = false;
    public GameObject objectToModify;
    public override void OnInteract()
    {
        if (!activated && active)
        {
            objectToModify.SetActive(true);
            narrator.notifyComplete(this);
            activated = true;
        }
    }

    public override void OnActivate()
    {
        active = true;
        objectToModify.SetActive(false);
    }
    public override void OnDeactivate()
    {
        active = false;
        objectToModify.SetActive(true);
    }
}
