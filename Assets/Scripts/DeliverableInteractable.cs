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
            if (audioSource)
            {
                audioSource.Play();
            }
            keyPrompt.SetActive (false);
            objectToModify.SetActive(false);
            narrator.notifyComplete(this);
            active = false;
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
