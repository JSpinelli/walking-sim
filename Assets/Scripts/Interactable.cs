using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class Interactable : MonoBehaviour {
    public BoxCollider promptCollider;
    public GameObject keyPrompt;

    private bool isColliding = false;
    protected bool wasActivated = false;

    protected string location;

    protected NarratorPace narrator;

    private void Awake () {
        Transform t = this.transform;
        while (t.parent != null) {
            //Debug.Log(t.parent);
            if (t.parent.tag == "Location") {
                
                location = t.parent.name;
                narrator = GameObject.Find ("Narrator").GetComponent<NarratorPace> ();
                narrator.Register ( location, "activable", ""+this.GetInstanceID());
            }
            t = t.parent.transform;
        }
        return; // Could not find a parent with given tag.
    }

    private void OnTriggerEnter (Collider other) {
        if (isColliding) return;
        isColliding = true;
        keyPrompt.SetActive (true);
    }

    private void OnTriggerExit (Collider other) {
        if (!isColliding) return;
        isColliding = false;
        keyPrompt.SetActive (false);
    }

    public abstract void OnInteract ();
}