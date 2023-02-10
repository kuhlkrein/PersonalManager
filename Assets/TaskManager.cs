using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TaskManager : MonoBehaviour
{

    public SaveSerial saveManager;
    public GameObject taskPrefab;
    public Transform tasksParent;


    // Update is called once per frame
    void Update()
    {
        if (saveManager.updated)
        {
            saveManager.updated = false;
            foreach (Task task in saveManager._taskList)
            {
                addTaskToUI(task);
                saveManager.updated = false;
            }
        }

    }

    public void addTestTask()
    {
        Task testTask = new Task("test description : {1}", 2, 1, "*%", new DateTime(2022, 02, 11, 00, 00, 00));
        saveManager.addTask(testTask);
        addTaskToUI(testTask);
    }

    public void addTaskToUI(Task task)
    {
        GameObject taskObject = Instantiate(taskPrefab, tasksParent);
        taskObject.GetComponent<TaskConstructor>().init(task);
    }
}
