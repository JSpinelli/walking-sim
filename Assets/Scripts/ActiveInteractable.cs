using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInteractable : Interactable
{
    public GameObject objectToModify;
    public bool setActive;

    public override void OnInteract()
    {
        if (active)
        {
            if (!wasActivated)
            {
                objectToModify.SetActive(setActive);
                wasActivated = true;
                narrator.notifyComplete(this);
            }
        }
    }
}