using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Json_Script : MonoBehaviour
{ 
    //Initializing Input Field
    public InputField Name;
    public InputField Class;
    public InputField Course;
    
    // Saving JSON Data
    public void Save()
    {
        // Creating Dataset Object
        Format data = new Format();
        data.NameText = Name.text;
        data.ClassText = Class.text;
        data.CourseText = Course.text;
        string json = JsonUtility.ToJson(data, true);
        // Writing to the File
        File.WriteAllText(Application.dataPath + "/FormatFile.json",json);        
    }
    public void Load()
    {   
        // Reading From the File
        string json = File.ReadAllText(Application.dataPath + "/FormatFile.json");
        Format data = JsonUtility.FromJson<Format>(json);
        // Fetching from Dataset
        Name.text = data.NameText;
        Class.text = data.ClassText;
        Course.text = data.CourseText;
    }

}
