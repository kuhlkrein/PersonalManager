using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SaveSerial : MonoBehaviour
{
    public bool updated = false;

    public List<Task> _taskList;
    public int _currentIndex;

    private void Start()
    {
        LoadGame();
    }

    public void addTask(Task task)
    {
        task.id = _currentIndex;
        _currentIndex += 1;
        _taskList.Add(task);
        SaveGame();
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/mainSettings.dat");
        TaskList data = new TaskList();
        data.task_list = _taskList;
        data.currentIndex = _currentIndex;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
                       + "/mainSettings.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/mainSettings.dat", FileMode.Open);
            TaskList data = (TaskList)bf.Deserialize(file);
            file.Close();
            _taskList = data.task_list;
            _currentIndex = data.currentIndex;
            Debug.Log("Game data loaded!");
        }
        else { 
            Debug.LogError("There is no save data! Reseting.");
            ResetData();
        }
        updated = true;
    }

    void ResetData()
    {
            _taskList = new List<Task>();
            _currentIndex = 0;
        if (File.Exists(Application.persistentDataPath  + "/mainSettings.dat"))
        {
            File.Delete(Application.persistentDataPath  + "/mainSettings.dat");
            Debug.Log("Data reset complete!");
        }
        else { 
            Debug.LogError("No save data to delete, new data created!");
        }
        SaveGame();
    }
}

[Serializable]
class TaskList
{
    public List<Task> task_list = new List<Task>();
    public int currentIndex = 0;
}

[Serializable]
public class Task
{
    public int id = -1;
    public String description = "description";
    public float currentValue = 1;
    public float modificateur_d_amelioration = 1;
    public String modificateur_type = "*%"; // {* || +}{%}
    public List<DateTime> jours_valides = new List<DateTime>();
    public DateTime horaire = new DateTime(2022, 02, 10, 23, 00, 00);

    public Task(String descript, float initialValue, float modificateur, string modifType, DateTime horaire_A_Repeter)
    {
        description = descript;
        currentValue = initialValue;
        modificateur_d_amelioration = modificateur;
        modificateur_type = modifType;
        horaire = horaire_A_Repeter;
}
}