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
    void Start()
    {
        foreach (Tache task in saveManager._taches_a_faire)
        {
            foreach (Session session in task.liste_de_sessions)
            {
                if (!session.faite)
                {
                    addTaskToUI(task);
                    break;
                }
            }
        }

    }

    public void updateTask(Tache taskToUpdate)
    {
        foreach (Tache task in saveManager._taches_a_faire)
        {
            if (task.id == taskToUpdate.id)
            {
                bool sessionsEnded = task.updateSessions(taskToUpdate.liste_de_sessions);
                bool taskFind = false;
                foreach (HistoriqueDeTache historique in saveManager._historique)
                {
                    if (historique.id == task.id)
                    {
                        taskFind = true;
                        historique.validateObjective(task);
                    }
                }
                if (!taskFind)
                {
                    HistoriqueDeTache historiqueDeTache = new HistoriqueDeTache(task.id);
                    historiqueDeTache.validateObjective(task);
                    saveManager._historique.Add(historiqueDeTache);
                }
                saveManager.SaveGame();
                break;
            }
        }
    }

    public void addTask(Tache task)
    {
        task.id = saveManager._index_courant;
        saveManager._index_courant += 1;
        saveManager._taches_a_faire.Add(task);

        HistoriqueDeTache historiqueDeTache = new HistoriqueDeTache(task.id);
        saveManager._historique.Add(historiqueDeTache);

        saveManager.SaveGame();
    }

    public void addTestTask()
    {
        Tache testTask = new Tache("Faire des pompes pendant {1} secondes", 2, 1, "*%", new DateTime(2022, 1, 1, 20, 00, 00));
        testTask.addSession(new DateTime(2022, 1, 1, 22, 0, 0));
        addTask(testTask);
        addTaskToUI(testTask);
    }

    public void addTaskToUI(Tache task)
    {
        GameObject taskObject = Instantiate(taskPrefab, tasksParent);
        taskObject.GetComponent<TaskConstructor>().init(task);
    }
}
