using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInteractable : Interactable
{
    public GameObject Pivot;
    public bool isReseatable;
    private bool activated = false;
    public override void OnInteract()
    {
        if (!activated)
        {
            gameObject.transform.RotateAround(Pivot.transform.position, Vector3.up, 90);
            activated = true;
        }else{
            if (isReseatable){
                gameObject.transform.RotateAround(Pivot.transform.position, Vector3.up, -90);
                activated = false;
            }
        }

    }
}
