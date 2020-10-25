using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomTrigger : MonoBehaviour
{
    private bool triggered = false;
    public NarratorPace narrator;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            narrator.triggerDarkRoom();
            triggered = true;
        }

    }
}
