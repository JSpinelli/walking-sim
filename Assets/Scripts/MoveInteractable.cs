using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteractable : Interactable
{
    public GameObject Pivot;
    public GameObject objectToMove;
    public Collider collider;
    public bool isReseatable;
    private bool activated = false;
    public override void OnInteract()
    {

        if (!activated)
        {
            collider.enabled = false;
            objectToMove.transform.RotateAround(Pivot.transform.position, Vector3.up, 90);
            activated = true;
            if (audioSource){
                    audioSource.Play();
            }
        }else{
            if (isReseatable){
                if (audioSource){
                    audioSource.Play();
                }
                objectToMove.transform.RotateAround(Pivot.transform.position, Vector3.up, -90);
                collider.enabled = true;
                activated = false;
            }
        }
    }

    public override void OnActivate(){

    }
    public override void OnDeactivate(){

    }
}
