using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskConstructor : MonoBehaviour
{
    Tache _task;
    int remains;
    // Start is called before the first frame update
    public void init(Tache task)
    {
        _task = task;

        DateTime now = DateTime.Now;
        int hour = -1;
        foreach (Session session in _task.liste_de_sessions)
        {
            if (!session.faite)
            {
                hour = session.horaire.Hour;
                break;
            }
        }
        if(hour == -1)
        {
            Destroy(this.gameObject);
            return;
        }

        remains = (hour - now.Hour + 24) % 24;
        Transform information = transform.GetChild(0);
        information.GetChild(0).GetComponent<TextMeshProUGUI>().text = _task.description.Replace("{1}", "" + _task.valeur_actuelle);
        information.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + _task.combo;
        information.GetChild(1).GetComponent<TextMeshProUGUI>().text = hour + "h (h-" + remains + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (_task != null)
        {
            DateTime now = DateTime.Now;
            int hour = -1;
            foreach (Session session in _task.liste_de_sessions)
            {
                if (!session.faite)
                {
                    hour = session.horaire.Hour;
                    break;
                }
            }
            if (hour == -1)
            {
                return;
            }

            float newRemains = (hour - now.Hour + 24) % 24;
            if (remains != newRemains)
            {
                transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = hour + "h (h-" + newRemains + ")";
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
            Destroy(this.gameObject);
            return;
        }

        Transform information = transform.GetChild(0);
        information.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + _task.combo;


    }
}
