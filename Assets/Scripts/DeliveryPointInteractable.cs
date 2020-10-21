using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPointInteractable : Interactable
{
    private bool activated = false;
    public GameObject objectToModify;
    public override void OnInteract()
    {
        if (!activated)
        {
            objectToModify.SetActive(true);
            narrator.notifyComplete(this);
        }
    }
}
