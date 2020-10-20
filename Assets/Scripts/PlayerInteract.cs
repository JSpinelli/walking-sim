using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        RaycastHit hit;
        //Ray ray = GetComponent<Camera>().ScreenPointToRay();
        Transform cam = Camera.main.transform;
        Ray ray = new Ray(cam.position, cam.forward);

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Transform objectHit = hit.transform;
            Interactable interactable = objectHit.GetComponent<Interactable>();
            if (interactable)
            {
                interactable.OnInteract();
            }

            // Do something with the object that was hit by the raycast.
        }
    }
}
