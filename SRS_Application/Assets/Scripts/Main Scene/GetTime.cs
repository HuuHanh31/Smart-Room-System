using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTime : MonoBehaviour
{
    public Text text;

    // Update is called once per frame
    void Update() {
        text.text = System.DateTime.Now.ToString("dd/MM/yyyy   HH:mm:ss ");
    }
    public static int getDay() {
        string value = System.DateTime.Now.ToString("dd");
        return Int32.Parse(value);
    }
    public static int getMonth() {
        string value = System.DateTime.Now.ToString("MM");
        return Int32.Parse(value);
    }
    public static int getYear() {
        string value = System.DateTime.Now.ToString("yyyy");
        return Int32.Parse(value);
    }
    public static int getHour() {
        string value = System.DateTime.Now.ToString("HH");
        return Int32.Parse(value);
    }
    public static int getMinute() {
        string value = System.DateTime.Now.ToString("mm");
        return Int32.Parse(value);
    }
    public static int getSec() {
        string value = System.DateTime.Now.ToString("ss");
        return Int32.Parse(value);
    }
    public static bool isLeapYear() {
        int year = getYear();
        return ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0));
    } 
    public static int getNextDay() {
        int day = getDay();
        if (day <= 27) return day + 1;
        int month = getMonth();
        switch (month) {
            case 2:
                if (day == 28 && isLeapYear()) return 29;
                return 1;
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                if (day == 31) return 1;
                return day + 1;
            default:
                if (day == 30) return 1;
                return day + 1;
        }
    }
}