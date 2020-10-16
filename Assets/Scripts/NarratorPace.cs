using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NarratorPace : MonoBehaviour {

    public GameObject objectivesUI;
    public TextMeshProUGUI currentObjective;
    private int currentCount = 0;
    private int objectiveCount = 2;
    private List<Interactable> interactableRepo = new List<Interactable> ();
    private Dictionary<string, List<Interactable>> interactableRepoByType = new Dictionary<string, List<Interactable>> ();
    private Dictionary<string, Dictionary<string, List<Interactable>>> interactableByLocation = new Dictionary<string, Dictionary<string, List<Interactable>>> ();

    private int currentTaskId;
    private string currentLocationTask;
    // Start is called before the first frame update
    void Start () {
        objectivesUI.SetActive (true);
        currentObjective.text = "Fill 2 Glasse with Water (" + currentCount + "/" + objectiveCount + ")";
    }

    // Update is called once per frame
    void Update () {

    }

    public void notifyComplete (string location, string type, string id) {
        currentCount++;
        currentObjective.text = "Fill 2 Glasse with Water (" + currentCount + "/" + objectiveCount + ")";
    }

    public void Register (Interactable subscriber) {
        // interactableRepo.Add (subscriber);
        // if (!interactableRepoByType.ContainsKey (subscriber.type)) {
        //     var list = new List<Interactable> ();
        //     list.Add (subscriber);
        //     interactableRepoByType.Add (subscriber.type, list);
        // } else {
        //     List<Interactable> list;
        //     interactableRepoByType.TryGetValue (subscriber.type, out list);
        //     list.Add (subscriber);
        // }

        if (!interactableByLocation.ContainsKey (subscriber.location[0])) {
            var location = new Dictionary<string, List<Interactable>> ();
        } else {
            Dictionary<string, List<Interactable>> location;
            interactableByLocation.TryGetValue (subscriber.location[0], out location);
            if (!location.ContainsKey (subscriber.type)) {
                var list = new List<Interactable> ();
                list.Add (subscriber);
                location.Add (subscriber.type, list);
            } else {
                List<Interactable> list;
                location.TryGetValue (subscriber.type, out list);
                list.Add (subscriber);
            }
        }

        Debug.Log ("Register by location: " + subscriber.location[0] + " Type: " + subscriber.type + " Id:" + subscriber.id);
    }
}