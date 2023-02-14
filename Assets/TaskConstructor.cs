using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskConstructor : MonoBehaviour
{
    Tache _task;
    int remains;
    int prev;
    string type = "toDo";

    // Start is called before the first frame update
    public void initToDo(Tache task, int initHour)
    {
        _task = task;
        type = "toDo";
        prev = DateTime.Now.Hour;
        remains = (prev - DateTime.Now.Hour + 24) % 24;
        Transform information = transform.GetChild(0);
        information.GetChild(0).GetComponent<TextMeshProUGUI>().text = _task.description.Replace("{1}", "" + _task.valeur_actuelle);
        information.GetChild(1).GetComponent<TextMeshProUGUI>().text = prev + "h (h-" + remains + ")";
        information.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + _task.combo;
    }

    public void initDone(Tache task)
    {
        _task = task;
        type = "done";
        Transform information = transform.GetChild(0);
        information.GetChild(0).GetComponent<TextMeshProUGUI>().text = _task.description.Replace("{1}", "" + _task.valeur_actuelle);
        information.GetChild(1).GetComponent<TextMeshProUGUI>().text = "x" + _task.combo;
    }

    public void initForNextDay(Tache task)
    {
        _task = task;
        type = "forNextDay";
        Transform information = transform.GetChild(0);
        prev = DateTime.Today.DayOfYear;
        remains = _task.liste_de_sessions[0].horaire.Date.DayOfYear - prev;
        information.GetChild(0).GetComponent<TextMeshProUGUI>().text = _task.description.Replace("{1}", "" + _task.valeur_actuelle);
        information.GetChild(1).GetComponent<TextMeshProUGUI>().text = "in " + remains + " day" + (remains == 1 ? "" : "s");
    }

    // Update is called once per frame
    void Update()
    {
        if (type == "toDo")
        {
            if (prev != DateTime.Now.Hour)
            {
                remains = remains + prev - DateTime.Now.Hour;
                prev = DateTime.Now.Hour;
                Transform information = transform.GetChild(0);
                information.GetChild(1).GetComponent<TextMeshProUGUI>().text = prev + "h (h-" + remains + ")";
            }
        }
        else if (type == "forNextDay")
        {
            if (prev != DateTime.Today.DayOfYear)
            {
                remains = remains + prev - DateTime.Today.DayOfYear;
                prev = DateTime.Today.DayOfYear;
                Transform information = transform.GetChild(0);
                information.GetChild(1).GetComponent<TextMeshProUGUI>().text = "in " + remains + " day" + (remains == 1 ? "" : "s");
            }
        }

    }

    public void endLastSession()
    {
        int nbOfSessionEnded = 0;
        foreach (Session session in _task.liste_de_sessions)
        {
            nbOfSessionEnded += 1;
            if (!session.faite)
            {
                session.faite = true;
                break;
            }
        }

        GameObject.FindObjectOfType<TaskManager>().updateTask(_task);

        if (nbOfSessionEnded == _task.liste_de_sessions.Count)
        {
            TaskDone();
        }
        else
        {
            Transform information = transform.GetChild(0);
            information.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + _task.combo;
        }


    }

    public void TaskDone()
    {
        GameObject.FindObjectOfType<TaskManager>().taskDone(_task);
        Destroy(this.gameObject);
    }

    public void TaskForNextDay()
    {
        GameObject.FindObjectOfType<TaskManager>().taskForNextDay(_task);
        Destroy(this.gameObject);
    }
}
