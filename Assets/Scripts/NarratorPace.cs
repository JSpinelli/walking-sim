using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorPace : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void notifyComplete(string location, string type){

    }

    public void Register(string location, string type, string id){
        Debug.Log("Register by location: "+location+ "Type: "+type+" Id:"+id);
    }
}
