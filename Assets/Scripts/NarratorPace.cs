using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NarratorPace : MonoBehaviour
{

    public AudioSource playerAudioSource;
    public AudioClip[] narratorVoiceClips;
    private int clipCounter = 0;
    public GameObject objectivesUI;
    public GameObject pauseUI;
    public TextMeshProUGUI currentObjective;
    public TextMeshProUGUI timer;

    public GameObject narratorPrompter;
    public GameObject[] objectsToDisable;
    public int escapeTreshold;
    public TextMeshProUGUI currentDialog;

    public float timerMinutes;
    private bool timerStarted;
    private float timeRemaining;
    private int currentCount = 0;
    private int objectiveCount = 2;
    private List<Interactable> interactableRepo = new List<Interactable>();
    private List<Location> mainLocations = new List<Location>();
    private List<TaskType> taskTypes = new List<TaskType>();
    private Dictionary<string, List<Interactable>> interactableRepoByType = new Dictionary<string, List<Interactable>>();
    private List<Interactable> dropOffs = new List<Interactable>();
    private Dictionary<string, Dictionary<string, List<Interactable>>> interactableByLocation = new Dictionary<string, Dictionary<string, List<Interactable>>>();
    private int currentTaskId;
    private string currentLocationTask;

    private string templateObjective;
    private string templateObjective2;

    private List<Interactable> interactablesActive = new List<Interactable>();

    private bool itemPickedUp = false;
    private bool darkRoomTriggered = false;

    private bool narratorAnnoyed = false;

    private string previousTaskType = "false";

    private Interactable deliveryPoint;

    public Interactable firstInteraction;

    private bool dialogTimer = false;
    private float dialogTimeRemaining;
    private int taskCompletedCounter = 0;
    private int taskFailedCounter = 0;
    private int completeTaskDialogCounter = 0;
    private int incompleteTaskDialogCounter = 0;
    private int exitDialogCounter = 0;
    private int quitTaskDialogCounter = 0;
    private string[] completeTaskDialog = {
        "Hi, welcome to me, welcome to an office game",
        "Great job! You clearly know what this is about. Here have another task",
        "Good job, It seems I have a good player this time",
        "Surprise! It's another task!",
        "I rearrange the tasks everytime someone new arrives, don't tell the developers",
        "You are getting good at clicking buttons",
        "Weird how you are the only person on the office and still there is chatter in the background",
        "Maybe there isn't anyone because adding npcs would be too much?",
        "Clearly they spent all the time in me, the star of the game",
        "They recycle my voicelines thought, so maybe that is not true",
        "TODO - WRITE MORE WITTY DIALOG",
        "Wow, amazing. What kind of developer writes their commentary inside the dialog?",
        "Maybe it was on purpouse, you know, fourth wall breaking and all that",
        "Incredible how you keep completing this tasks",
        "You are not tired right? No? Perfect, have another one",
        "Aaaaand another one",
        "Lots of things to do here",
        "If you are expecting another witty response you are out of luck. This is a student project. Limited time and all that",
    };

    private string[] incompleteTaskDialog = {
        "Come on, these are not difficult tasks",
        "Are you lost? It is a very simple layout you shouldn't be",
        "I can literally keep giving you tasks all day",
        "Watch out! Another task is coming your way",
        "Yep, no more responses for failing",
        "You should try finishing a task. No more dialog here",
        "Nope, none, empty",
        "No, for real, stop it",
        "This office needs you to do your tasks, please",
        "Yeah, I'm tired of you failing, go ahead, quit"
    };

    private string[] exitDialog = {
        "Well, it seems you found the unfinished section of the game",
        "What did you expect?",
        "I forgot there is no dialog option. I'll assume you said something clever",
        "Ok...There isn't much else to say. You can enjoy the void if you want",
        "I personally think I was pretty good at giving you tasks, why did you have to fall?",
        "I keep forgetting about the dialog thing, lazy developers",
        "You cannot return to the office level, you can quit if you want. I don't mind",
        "Alt + F4, they didn't even add a pause menu, that is straight up mean"
    };

    private string[] quitDialog = {
        "No no no, it's boring being me. Can you stay a little bit longer?",
        "They programmed me to say this but I really mean it, being this game is boring",
        "Okey, fine, leave"
    };

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < interactableRepo.Count; i++)
        {
            this.constructReferences(interactableRepo[i]);
        }
        SceneManager.LoadScene("Static Assets", LoadSceneMode.Additive);
        objectivesUI.SetActive(true);
        currentObjective.text = "Sign paper";
        firstInteraction.OnActivate();
    }

    public void triggerDarkRoom()
    {
        objectivesUI.SetActive(false);
        narratorPrompter.SetActive(false);
        timerStarted = false;
        darkRoomTriggered = true;
        timeRemaining = 5;

    }
    public void OnPauseGame()
    {
        // pauseUI.SetActive(true);
        // timerStarted = false;
    }

    public void OnResumeGame()
    {
        pauseUI.SetActive(false);
        timerStarted = true;
    }

    public void quitDialogTrigger()
    {
        if (narratorAnnoyed)
        {
            Application.Quit();
        }
        else
        {
            if (quitTaskDialogCounter < quitDialog.Length)
            {
                currentDialog.text = quitDialog[quitTaskDialogCounter];
                quitTaskDialogCounter++;
            }
            else
            {
                Application.Quit();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                this.DisplayTime(timeRemaining);
            }
            else
            {
                //Debug.Log("Time has run out!");

                timeRemaining = 0;
                this.DisplayTime(timeRemaining);
                timerStarted = false;
                this.taskFailed();
            }
        }
        if (dialogTimer)
        {
            if (dialogTimeRemaining > 0)
            {
                dialogTimeRemaining -= Time.deltaTime;
            }
            else
            {

                dialogTimeRemaining = 0;
                dialogTimer = false;
                narratorPrompter.SetActive(false);
            }
        }
        if (darkRoomTriggered)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                narratorPrompter.SetActive(true);
                timeRemaining = 0;
                timeRemaining = 10;
                if (exitDialogCounter < exitDialog.Length)
                {
                    this.narratorDialog(exitDialog[exitDialogCounter]);
                    exitDialogCounter++;
                }
            }
        }
    }

    private void startTimer()
    {
        timerStarted = true;
        timeRemaining = timerMinutes * 60;
        this.DisplayTime(timeRemaining);
    }

    private void findObjective()
    {
        Dictionary<string, List<Interactable>> typeIterator;
        List<Interactable> activityIterator = null; ;
        var notVistedLocations = mainLocations.FindAll(x => !x.visited);
        Location newLocation;
        if (notVistedLocations.Count > 0)
        {
            //Debug.Log("Still no visited locations");
            newLocation = notVistedLocations[Random.Range(0, notVistedLocations.Count - 1)];
            newLocation.visited = true;
            interactableByLocation.TryGetValue(newLocation.name, out typeIterator);
        }
        else
        {
            newLocation = mainLocations[Random.Range(0, mainLocations.Count - 1)];
            interactableByLocation.TryGetValue(newLocation.name, out typeIterator);
        }
        //Debug.Log("Next Objective in: " + newLocation.name);
        var index = Random.Range(0, taskTypes.Count - 1);
        var cycles = 0;
        bool foundButNotValid = false;
        while (cycles < taskTypes.Count)
        {
            if (typeIterator.ContainsKey(taskTypes[index].name))
            {

                if (previousTaskType != taskTypes[index].name)
                {
                    typeIterator.TryGetValue(taskTypes[index].name, out activityIterator);
                    cycles = taskTypes.Count;
                    previousTaskType = taskTypes[index].name;
                }
                else
                {
                    foundButNotValid = true;
                    cycles++;
                    index++;
                    if (index == taskTypes.Count)
                    {
                        index = 0;
                    }
                }

            }
            else
            {
                cycles++;
                index++;
                if (index == taskTypes.Count)
                {
                    index = 0;
                }
            }
        }
        if (activityIterator == null && !foundButNotValid)
        {
            Debug.Log(newLocation.name + " REMOVED");
            interactableByLocation.Remove(newLocation.name);
            this.findObjective();
            return;
        }
        else
        {
            if (foundButNotValid)
            {
                //newLocation.visited = false;
                this.findObjective();
                return;
            }
        }

        this.makeObjective(activityIterator);
        this.startTimer();
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    public void narratorDialog(string text)
    {
        narratorPrompter.SetActive(true);

        if (clipCounter == narratorVoiceClips.Length)
        {
            clipCounter = 0;
        }
        playerAudioSource.PlayOneShot(narratorVoiceClips[clipCounter], 1f);
        clipCounter++;
        currentDialog.text = text;
        dialogTimer = true;
        dialogTimeRemaining = 8;
    }
    private void taskComplete()
    {
        timerStarted = false;
        this.DisplayTime(0);
        taskCompletedCounter++;
        if (completeTaskDialogCounter < completeTaskDialog.Length)
        {
            this.narratorDialog(completeTaskDialog[completeTaskDialogCounter]);
        }

        if (completeTaskDialogCounter == escapeTreshold)
        {
            foreach (var item in objectsToDisable)
            {
                item.SetActive(false);
            }
        }
        completeTaskDialogCounter++;
        this.findObjective();
    }

    private void taskFailed()
    {
        taskFailedCounter++;

        if (incompleteTaskDialogCounter < incompleteTaskDialog.Length)
        {
            this.narratorDialog(incompleteTaskDialog[incompleteTaskDialogCounter]);
        }
        else
        {
            narratorAnnoyed = true;
        }
        incompleteTaskDialogCounter++;
        this.findObjective();
    }


    private void makeObjective(List<Interactable> activities)
    {
        string partialText;
        switch (activities[0].category)
        {
            case "amount":
                objectiveCount = Random.Range(2, activities.Count - 1);
                currentCount = 0;
                partialText = activities[0].prompt.Replace("${amount}", "" + objectiveCount) + "( ${currentCount} /" + objectiveCount + ")";
                templateObjective = partialText.Replace("${location}", "the " + activities[0].location[0].name);
                currentObjective.text = templateObjective.Replace("${currentCount}", "" + currentCount);
                interactablesActive = activities;
                foreach (Interactable item in activities)
                {
                    item.OnActivate();
                }
                break;
            case "deliverable":

                Interactable pickUp = activities[Random.Range(0, activities.Count - 1)];
                string locationA = "";
                foreach (Location location in pickUp.location)
                {
                    //Debug.Log(location.name);
                    locationA = locationA + " " + location.name;
                }
                templateObjective = pickUp.prompt.Replace("${locationA}", "the " + locationA);

                int dropOffIndex = Random.Range(0, (dropOffs.Count - 1));
                //Debug.Log(dropOffIndex);
                deliveryPoint = dropOffs[dropOffIndex];
                //Debug.Log("Dropoff name: " + deliveryPoint.name + " first location: " + deliveryPoint.location[0].name);
                dropOffs.RemoveAt(dropOffIndex);
                string locationB = "";
                foreach (Location location in deliveryPoint.location)
                {
                    //Debug.Log(location.name);
                    locationB = locationB + " " + location.name;
                }
                templateObjective2 = "Deliver the binder to " + "the " + locationB;
                currentObjective.text = templateObjective;
                pickUp.OnActivate();

                break;
            case "interact":
                Interactable objective = activities[Random.Range(0, activities.Count - 1)];
                string locationName = "";
                foreach (Location location in objective.location)
                {
                    //Debug.Log(location.name);
                    locationName = locationName + " " + location.name;
                }
                templateObjective = objective.prompt.Replace("${location}", "the " + locationName);
                currentObjective.text = templateObjective;
                objective.OnActivate();
                break;
        }
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
                    foreach (Interactable item in interactablesActive)
                    {
                        item.OnDeactivate();
                    }
                    this.taskComplete();
                }
                break;
            case "deliverable":
                itemPickedUp = true;
                currentObjective.text = templateObjective2;
                deliveryPoint.OnActivate();
                this.startTimer();
                break;
            case "interact":
                this.taskComplete();
                break;
            case "dropoff":
                if (itemPickedUp)
                {
                    this.taskComplete();
                }
                break;
        }
    }

    public void Register(Interactable subscriber)
    {
        interactableRepo.Add(subscriber);
    }

    private void constructReferences(Interactable subscriber)
    {
        if (subscriber.category != "dropoff")
        {
            if (!interactableRepoByType.ContainsKey(subscriber.type))
            {
                taskTypes.Add(new TaskType(subscriber.type));
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


            if (subscriber.location.Count > 0)
            {
                //Find if location already exists on Dictionary
                if (!interactableByLocation.ContainsKey(subscriber.location[0].name))
                {
                    // Debug.Log("Creating entry for location: " + subscriber.location[0].name);
                    mainLocations.Add(subscriber.location[0]); // TO-DO check for starting location and secondary location (those should be treated as visited)
                                                               //If it doesnt exist create it
                    var location = new Dictionary<string, List<Interactable>>();
                    //Create list of Interactables
                    var list = new List<Interactable>();
                    // Add current (first to be added)
                    list.Add(subscriber);
                    // Add List with it's location to the new Dictionary Entry
                    //Debug.Log("Creating entry for type: " + subscriber.type);
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
                        //Debug.Log("Creating entry for type: " + subscriber.type);
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
        else
        {
            dropOffs.Add(subscriber);
        }
    }
}