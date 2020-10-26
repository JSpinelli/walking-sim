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
                if (audioSource){
                    audioSource.Play();
                }
                objectToModify.SetActive(setActive);
                wasActivated = true;
                active = false;
                keyPrompt.SetActive (false);
                narrator.notifyComplete(this);
            }
        }
    }

    
    public override void OnActivate()
    {
        active = true;
        wasActivated = false;
        objectToModify.SetActive(!setActive);
        keyPrompt.SetActive (true);
    }
    public override void OnDeactivate()
    {
        active = false;
        keyPrompt.SetActive (false);
    }
}