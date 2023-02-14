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

    public GameObject nextDayTaskPrefab;
    public Transform nextDayTaskParent;

    public GameObject doneTaskTaskPrefab;
    public Transform doneTaskParent;


    // Update is called once per frame
    void Start()
    {
        foreach (Tache task in saveManager._taches_a_faire)
        {
            checkTask(task);
        }

    }

    public void checkTask(Tache task)
    {
        if (task.liste_de_sessions[0].horaire.Date.DayOfYear == DateTime.Today.DayOfYear)
        {
            bool todo = false;
            foreach (Session session in task.liste_de_sessions)
            {
                if (!session.faite)
                {
                    todo = true;
                    taskToDo(task, session.horaire.Hour);
                    break;
                }
            }

            if (!todo)
            {
                taskDone(task);
            }
        }
        else
        {
            taskForNextDay(task);
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

    public void addTestTaskToDo()
    {
        Tache testTask = new Tache("Faire des pompes pendant {1} secondes", 2, 1, "*%", DateTime.Today.AddHours(20));
        testTask.addSession(DateTime.Today.AddHours(21));
        testTask.addSession(DateTime.Today.AddHours(22));
        addTask(testTask);
        checkTask(testTask);
    }
    public void addTestTaskDone()
    {
        Tache testTask = new Tache("Faire des pompes pendant {1} secondes", 2, 1, "*%", DateTime.Today.AddHours(20));
        testTask.liste_de_sessions[0].faite = true;
        addTask(testTask);
        checkTask(testTask);
    }
    public void addTestTaskForNextDay()
    {
        Tache testTask = new Tache("Faire des pompes pendant {1} secondes", 2, 1, "*%", DateTime.Today.AddDays(1).AddHours(20));
        addTask(testTask);
        checkTask(testTask);
    }

    public void taskToDo(Tache task, int hour)
    {
        GameObject taskObject = Instantiate(taskPrefab, tasksParent);
        taskObject.GetComponent<TaskConstructor>().initToDo(task, hour);
    }

    public void taskForNextDay(Tache task)
    {
        GameObject taskObject = Instantiate(nextDayTaskPrefab, nextDayTaskParent);
        taskObject.GetComponent<TaskConstructor>().initForNextDay(task);
    }

    public void taskDone(Tache task)
    {
        GameObject taskObject = Instantiate(doneTaskTaskPrefab, doneTaskParent);
        taskObject.GetComponent<TaskConstructor>().initDone(task);
    }
}
