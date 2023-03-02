using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public timePart part;

    // Start is called before the first frame update
    void Awake()
    {
        DateTime now = DateTime.Now;
        string res = "" ;
        switch (part)
        {
            case timePart.DAY: res += now.Day; break;
            case timePart.MONTH: res += now.Month; break;
            case timePart.HOUR: res += now.Hour; break;
            case timePart.MINUTES: res += now.Minute; break;
            case timePart.YEAR: res += now.Year; break;
        }

        if (res.Length < 2)
            res = "0" + res;
        this.GetComponent<TMP_InputField>().text = res;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum timePart
{
    DAY,
    MONTH,
    HOUR,
    MINUTES,
    YEAR
}
