using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

abstract public class Interactable : MonoBehaviour
{
    public BoxCollider promptCollider;
    public GameObject keyPrompt;

    private bool isColliding = false;
    protected bool wasActivated = false;

    public int id;
    public List<Location> location = new List<Location>();
    public string type = "activable";
    public string prompt = "Fill ${amount} glasses with water in ${location}";
    public string category = "amount";

    public bool subscribe = true;

    public bool active = false;

    protected NarratorPace narrator;

    private void Awake()
    {
        if (subscribe)
        {
            Transform t = this.transform;
            while (t.parent != null)
            {
                //Debug.Log(t.parent);
                if (t.parent.tag == "Location")
                {

                    location.Insert(0, t.parent.GetComponent<Location>());
                }
                t = t.parent.transform;
            }
            if (location.Count > 0)
            {
                narrator = GameObject.Find("Narrator").GetComponent<NarratorPace>();
                id = this.GetInstanceID();
                narrator.Register(this);
            }
            return;
        }
    }

    public void ShowPrompt()
    {
        if (isColliding || !active) return;
        isColliding = true;
        keyPrompt.SetActive(true);
    }

    public void HidePrompt()
    {
        if (!isColliding || !active) return;
        isColliding = false;
        keyPrompt.SetActive(false);
    }

    public abstract void OnInteract();
    public abstract void OnActivate();
    public abstract void OnDeactivate();
}