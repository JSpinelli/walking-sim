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
    public List<Location> location = new List<Location> ();
    public string type = "activable";
    public string prompt = "Fill ${amount} glasses with water in ${location}";
    public string category = "amount";

    public bool active = false;

    protected NarratorPace narrator;

    private void Awake () {
        Transform t = this.transform;
        while (t.parent != null) {
            //Debug.Log(t.parent);
            if (t.parent.tag == "Location") {

                location.Insert (0, t.parent.GetComponent<Location> ());
            }
            t = t.parent.transform;
        }
        narrator = GameObject.Find ("Narrator").GetComponent<NarratorPace> ();
        id = this.GetInstanceID ();
        narrator.Register (this);
        return; // Could not find a parent with given tag.
    }

    private void OnTriggerEnter (Collider other) {
        if (isColliding && active) return;
        isColliding = true;
        keyPrompt.SetActive (true);
    }

    private void OnTriggerExit (Collider other) {
        if (!isColliding && active) return;
        isColliding = false;
        keyPrompt.SetActive (false);
    }

    public abstract void OnInteract ();
}