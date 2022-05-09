using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ManagerSetTurnOff : MonoBehaviour
{
    public static ManagerSetTurnOff instance;
    public Text hour,minute,second;
    int hour_value, minute_value, second_value;
    QueueLinkedList queueTime;
    void Awake() {
        if (instance == null) instance = this;
    }
    public void setAdd(int device, int hh, int mm, int ss) {
        queueTime.addQueue(hh,mm,ss,device);
    }
    void Start() {
        queueTime = new QueueLinkedList();
    }
    void FixedUpdate() {
        while (queueTime.checkHead()) {
            queueTime.DeQueue();
        }
    }

    public void light_set() {
        if (checkValue()) {
            // make light
            queueTime.addQueue(hour_value, minute_value, second_value, 1);
        }
    }
    public void fan_set() {
        if (checkValue()) {
            // make fan
            queueTime.addQueue(hour_value, minute_value, second_value, 2);
        }
    }
    public void heater_set() {
        if (checkValue()) {
            // make heater
            queueTime.addQueue(hour_value, minute_value, second_value, 3);
        }
    }
    bool checkValue() {
        // hour
        hour_value = getValue(hour.text);
        if (hour_value > 23 || hour_value < 0 || hour_value == -1) return false;
        // minute
        minute_value = getValue(minute.text);
        if (minute_value > 59 || minute_value < -1) return false;
        else if (minute_value == -1) minute_value = 0;
        // second
        second_value = getValue(second.text);
        if (second_value > 59 || second_value < -1) return false;
        else if (second_value == -1) second_value = 0;
        
        return true;
    }
    int getValue(string input) {
        if (input.Length == 0) return -1;
        for (int i = 0; i < input.Length; i++) {   
            if (!(input[i] >= '0' && input[i] <= '9')) return 123456;
        }
        return int.Parse(input);
    }
}
