﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance;
    public BoxCollider promptCollider;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        //Ray ray = GetComponent<Camera>().ScreenPointToRay();
        Transform cam = Camera.main.transform;
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(cam.position, cam.forward, interactDistance);
        //if (Physics.Raycast(ray, out hit, interactDistance))
        if (hits.Length > 0)
        {

            for (var i = 0; i < hits.Length; i++)
            {
                Transform objectHit = hits[i].transform;
                Interactable interactable = objectHit.GetComponent<Interactable>();
                if (interactable)
                {
                    interactable.OnInteract();
                }
            }
            // Do something with the object that was hit by the raycast.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.ShowPrompt();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.gameObject.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.HidePrompt();
        }
    }
}