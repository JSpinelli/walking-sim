using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class Interactable: MonoBehaviour
{
    public BoxCollider promptCollider;
    public GameObject keyPrompt;

    private bool isColliding = false;
    protected bool wasActivated = false;


    private void OnTriggerEnter(Collider other)
    {
        if (isColliding) return;
        isColliding = true;
        keyPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isColliding) return;
        isColliding = false;
        keyPrompt.SetActive(false);
    }
    
    public  abstract void OnInteract();
}
