using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverableInteractable : Interactable
{
    public GameObject objectToModify;
    public override void OnInteract()
    {
        if (active)
        {
            objectToModify.SetActive(false);
            narrator.notifyComplete(this);
        }
    }

    
    public override void OnActivate()
    {
        active = true;
    }
    public override void OnDeactivate()
    {
        active = false;
    }
}
