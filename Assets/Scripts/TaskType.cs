using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskType
{
    public string name;
    public bool performed = false;

    public TaskType(string _name){
        name= _name;
        performed = false;
    }
}
