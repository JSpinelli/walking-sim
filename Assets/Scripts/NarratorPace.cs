using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NarratorPace : MonoBehaviour
{

    public GameObject objectivesUI;
    public TextMeshProUGUI currentObjective;

    public GameObject narratorPrompter;
    public TextMeshProUGUI currentDialog;
    private int currentCount = 0;
    private int objectiveCount = 2;
    private List<Interactable> interactableRepo = new List<Interactable>();
    private List<Location> mainLocations = new List<Location>();
    private Dictionary<string, List<Interactable>> interactableRepoByType = new Dictionary<string, List<Interactable>>();
    private Dictionary<string, Dictionary<string, List<Interactable>>> interactableByLocation = new Dictionary<string, Dictionary<string, List<Interactable>>>();

    private int currentTaskId;
    private string currentLocationTask;

    private string templateObjective;

    private int taskCompletedCounter = 0;
    private int taskFailedCounter = 0;

    private int fillerDialogCounter = 0;
    private int completeTaskDialogCounter = 0;
    private int incompleteTaskDialogCounter = 0;
    private string[] fillerDialog = {
        "Do you know how depressing it is to be a know-all narrator all the time? I even rearrange the task from time to time to change things up",
        "Surprise! It's another task!"
    };
    private string[] completeTaskDialog = {
        "Hi, welcome to me, welcome to an office game. Let me give you another task",
        "You may have realised that this game is a game about choices, yes, another metagame, my developers are not as original as they think",
        "It seems you need to perform another task",
        "Good job, It seems I have a good player this time",
        "You are getting good at clicking a button"
    };

    private string[] incompleteTaskDialog = {
        "Ohhh, a defiant player. You know that this was also expected right?",
        "I can literally keep giving you tasks all day",
        "Watch out! Another task is coming your way",
        "If you are expecting another witty response you are out of luck. This is a student project. Limited time and all that"
    };
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < interactableRepo.Count; i++)
        {
            this.constructReferences(interactableRepo[i]);
        }
        // Dictionary<string, List<Interactable>> typeIterator;
        // List<Interactable> activityIterator;

        objectivesUI.SetActive(true);
        // Debug.Log(interactableRepo.Count);
        // interactableByLocation.TryGetValue("Office B", out typeIterator);
        // typeIterator.TryGetValue("glass", out activityIterator);
        // this.makeObjective(activityIterator);

        this.findObjective();
    }

    private void findObjective()
    {
        Dictionary<string, List<Interactable>> typeIterator;
        List<Interactable> activityIterator;
        var newLocation = mainLocations.Find(x => !x.visited);
        newLocation.visited = true;
        interactableByLocation.TryGetValue(newLocation.name, out typeIterator);
        typeIterator.TryGetValue("glass", out activityIterator);
        this.makeObjective(activityIterator);
    }



    private void taskComplete()
    {
        taskCompletedCounter++;
        narratorPrompter.SetActive(true);
        currentDialog.text = completeTaskDialog[completeTaskDialogCounter];
        completeTaskDialogCounter++;
        this.findObjective();
    }

    private void taskFailed()
    {
        taskFailedCounter++;
    }

    private void makeObjective(List<Interactable> activities)
    {
        string partialText;
        switch (activities[0].category)
        {
            case "amount":
                objectiveCount = Random.Range(2, activities.Count);
                currentCount = 0;
                Debug.Log(activities[0].prompt);
                partialText = activities[0].prompt.Replace("${amount}", "" + objectiveCount) + "( ${currentCount} /" + objectiveCount + ")";
                Debug.Log(partialText);
                templateObjective = partialText.Replace("${location}", activities[0].location[0].name);
                currentObjective.text = templateObjective.Replace("${currentCount}", "" + currentCount);
                foreach (var item in activities)
                {
                    item.active = true;
                }
                break;
            case "delivery":
                break;
            case "interact":
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void notifyComplete(Interactable subscriber)
    {
        switch (subscriber.category)
        {
            case "amount":
                currentCount++;
                currentObjective.text = templateObjective.Replace("${currentCount}", "" + currentCount);
                if (currentCount == objectiveCount)
                {
                    this.taskComplete();
                }
                break;
            case "delivery":
                break;
            case "interact":
                break;
        }
    }

    public void Register(Interactable subscriber)
    {
        interactableRepo.Add(subscriber);
        Debug.Log("Register by location: " + subscriber.location[0].name + " Type: " + subscriber.type + " Id:" + subscriber.id);
    }

    private void constructReferences(Interactable subscriber)
    {
        if (!interactableRepoByType.ContainsKey(subscriber.type))
        {
            var list = new List<Interactable>();
            list.Add(subscriber);
            interactableRepoByType.Add(subscriber.type, list);
        }
        else
        {
            List<Interactable> list;
            interactableRepoByType.TryGetValue(subscriber.type, out list);
            list.Add(subscriber);
        }

        //Find if location already exists on Dictionary
        if (!interactableByLocation.ContainsKey(subscriber.location[0].name))
        {
            Debug.Log("Creating entry for location: " + subscriber.location[0].name);
            mainLocations.Add(subscriber.location[0]); // TO-DO check for starting location and secondary location (those should be treated as visited)
            //If it doesnt exist create it
            var location = new Dictionary<string, List<Interactable>>();
            //Create list of Interactables
            var list = new List<Interactable>();
            // Add current (first to be added)
            list.Add(subscriber);
            // Add List with it's location to the new Dictionary Entry
            Debug.Log("Creating entry for type: " + subscriber.type);
            location.Add(subscriber.type, list);
            interactableByLocation.Add(subscriber.location[0].name, location);
        }
        else
        {
            //If location already exists fetch it, and add new value
            Dictionary<string, List<Interactable>> location;
            interactableByLocation.TryGetValue(subscriber.location[0].name, out location);
            //Find if type already exsits on sub-Dictionary
            if (!location.ContainsKey(subscriber.type))
            {
                Debug.Log("Creating entry for type: " + subscriber.type);
                //If it doesnt create list
                var list = new List<Interactable>();
                // Add current (first to be added)
                list.Add(subscriber);
                // Add List with it's location to the new Dictionary Entry
                location.Add(subscriber.type, list);
            }
            else
            {
                //If it already exists, fetch list and add new subscriber
                List<Interactable> list;
                location.TryGetValue(subscriber.type, out list);
                list.Add(subscriber);
            }
        }
    }
}