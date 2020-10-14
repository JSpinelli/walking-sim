using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NarratorPace : MonoBehaviour
{

    public GameObject objectivesUI;
    public TextMeshProUGUI currentObjective;
    private int currentCount = 0;
    private int objectiveCount = 2;
    // Start is called before the first frame update
    void Start()
    {
        objectivesUI.SetActive(true);
        currentObjective.text = "Fill 2 Glasse with Water ("+currentCount+"/"+objectiveCount+")";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void notifyComplete(string location, string type, string id){
        currentCount++;
        currentObjective.text = "Fill 2 Glasse with Water ("+currentCount+"/"+objectiveCount+")";
    }

    public void Register(string location, string type, string id){
        Debug.Log("Register by location: "+location+ "Type: "+type+" Id:"+id);
    }
}
