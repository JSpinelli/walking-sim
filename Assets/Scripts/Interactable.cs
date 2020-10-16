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

    public int id;
    public List<string> location = new List<string> ();
    public string type = "activable";

    protected NarratorPace narrator;

    private void Awake () {
        Transform t = this.transform;
        while (t.parent != null) {
            //Debug.Log(t.parent);
            if (t.parent.tag == "Location") {

                location.Insert (0, t.parent.name);
            }
            t = t.parent.transform;
        }
        narrator = GameObject.Find ("Narrator").GetComponent<NarratorPace> ();
        id = this.GetInstanceID ();
        narrator.Register (this);
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