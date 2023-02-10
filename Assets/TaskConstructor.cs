using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskConstructor : MonoBehaviour
{
    Task _task;
    int remains;
    // Start is called before the first frame update
    public void init(Task task)
    {
        _task = task;
        Transform information = transform.GetChild(0);
        information.GetChild(0).GetComponent<TextMeshProUGUI>().text = _task.description.Replace("{1}", "" + _task.currentValue);
        information.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + _task.jours_valides.Count;

        DateTime now = DateTime.Now;
        float hour = _task.horaire.Hour;
        remains = (_task.horaire.Hour - now.Hour + 24) % 24;
        information.GetChild(1).GetComponent<TextMeshProUGUI>().text = hour + "h (h-" + remains + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (_task != null)
        {
            DateTime now = DateTime.Now;
            float hour = _task.horaire.Hour;
            float newRemains = (_task.horaire.Hour - now.Hour + 24) % 24;
            if (remains != newRemains)
            {
                transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = hour + "h (h-" + newRemains + ")";
            }
        }
    }
}
