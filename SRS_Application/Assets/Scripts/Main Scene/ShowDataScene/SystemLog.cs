using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemLog : MonoBehaviour
{
    public static SystemLog instance;
    public Text txt;
    Queue<string> queueLog;
    int count;
    void Awake() {
        if (instance == null) instance = this;
        queueLog = new Queue<string>();
        count = 0;
        txt.text = "";
    }
    public void EnQueue(string mess) {
        queueLog.Enqueue(mess);
        if (count < 8) count++;
        else queueLog.Dequeue();
        printQueue();
    }
    void printQueue() {
        string finalMess = "";
        string mess;
        int totalCount = count;
        while (totalCount != 0) {
            totalCount--;
            mess = queueLog.Dequeue();
            finalMess += mess;
            if (totalCount != 0) finalMess += '\n';
            queueLog.Enqueue(mess);
        }
        txt.text = finalMess;
    }
    
}
