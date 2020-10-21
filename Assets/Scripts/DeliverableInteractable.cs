using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverableInteractable : Interactable
{
    private bool activated = false;
    public GameObject objectToModify;
    public override void OnInteract()
    {
        
        if (active)
        {
            objectToModify.SetActive(false);
            narrator.notifyComplete(this);
        }
    }
}
