using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class device_Waiting {
    private int hour, minute, sec, day, month, year, type;
    public device_Waiting(int hour, int minute, int sec, int day, int month, int year, int type) {
        this.hour = hour;
        this.minute = minute;
        this.sec = sec;
        this.day = day;
        this.month = month;
        this.year = year;
        this.type = type;
    }
    public void printData() {
        Debug.Log("Head queue:");
        Debug.Log(hour.ToString() + ":" + minute.ToString() + ":" + sec.ToString() + "     " + day.ToString() + "/" + month.ToString() + '/' + year.ToString());
        Debug.Log("Type: " + type.ToString());
    }
    public void activeEvent() {
        switch (type) {
            case 1:
                if (ManagerConnect.instance.light_state) ManagerConnect.instance.changeState(1);
                return;
            case 2:
                if (ManagerConnect.instance.fan_state) ManagerConnect.instance.changeState(2);
                return;
            case 3:
                if (ManagerConnect.instance.heater_state) ManagerConnect.instance.changeState(3);
                return;
        }
    }
    public bool checkTime() {
        int d = GetTime.getDay(), mo = GetTime.getMonth(), y = GetTime.getYear(), h = GetTime.getHour(), mi = GetTime.getMinute(), s = GetTime.getSec();
        // Debug.Log("Time get");
        // Debug.Log(h.ToString() + ":" + mi.ToString() + ":" + s.ToString() + "     " + d.ToString() + "/" + mo.ToString() + '/' + y.ToString());

        if (this.year < y)      return true;
        else if (this.year == y) {
            if (this.month < mo) return true;
            else if (this.month == mo) {
                if (this.day < d) return true;
                else if (this.day == d) {
                    if (this.hour < h) return true;
                    else if (this.hour == h) {
                        if (this.minute < mi) return true;
                        else if (this.minute == mi && this.sec <= s) return true;
                    }
                }
            }
        }

        return false;
    }
    public bool isGreatThan(device_Waiting another) {
        int d = another.day, mo = another.month, y = another.year, h = another.hour, mi = another.minute, s = another.sec;
        // Debug.Log("Time get");
        // Debug.Log(h.ToString() + ":" + mi.ToString() + ":" + s.ToString() + "     " + d.ToString() + "/" + mo.ToString() + '/' + y.ToString());

        if (this.year < y)      return false;
        else if (this.year == y) {
            if (this.month < mo) return false;
            else if (this.month == mo) {
                if (this.day < d) return false;
                else if (this.day == d) {
                    if (this.hour < h) return false;
                    else if (this.hour == h) {
                        if (this.minute < mi) return false;
                        else if (this.minute == mi && this.sec <= s) return false;
                    }
                }
            }
        }

        return true;

        // if (this.year   < another.year)   return false;
        // if (this.month  < another.month)  return false;
        // if (this.day    < another.day)    return false;
        // if (this.hour   < another.hour)   return false;
        // if (this.minute < another.minute) return false;
        // if (this.sec    < another.sec)    return false;
        // return true;
    }
}
public class node_device {
    public device_Waiting child;
    public node_device next;
    public node_device(device_Waiting child, node_device next = null) {
        this.child = child;
        this.next = next;
    }
    public bool addNode(node_device new_node) {
        if (this.child.isGreatThan(new_node.child)) return true;
        else {
            if (this.next == null) {
                this.next = new_node;
            }
            else if (this.next.addNode(new_node)) {
                new_node.next = this.next;
                this.next = new_node;
            }
        }
        return false;
    }
}
public class QueueLinkedList : MonoBehaviour
{
    
    node_device head;
    int length;
    public QueueLinkedList() {
        head = null;
        length = 0;
    }
    bool isToday(int hour, int minute, int sec) {
        int cur_h = GetTime.getHour(), cur_m = GetTime.getMinute(), cur_s = GetTime.getSec();
        if (cur_h > hour) {
            Debug.Log("Fail hour");
            return false;
        } 
        if (cur_h == hour) {
            if (cur_m > minute)
            {
                Debug.Log("Fail minute");
                return false;
            }
            if (cur_m == minute && cur_s + 29 > sec)
            {
                Debug.Log("Fail sec");
                return false;
            }
        }
        return true;
    }
    public void addQueue(int hour, int minute, int sec, int type) {
        // get time value for node
        int day = GetTime.getDay(), month = GetTime.getMonth(), year = GetTime.getYear();
        if (!this.isToday(hour,minute,sec)) {
            day = GetTime.getNextDay();
            if (day == 1) {
                month++;
                if (month == 13) {
                    month = 1;
                    year++;
                }
            }
        }
        // create node
        node_device new_node = new node_device(new device_Waiting(hour, minute, sec, day, month, year, type));
        if (length == 0) {
            head = new_node;
        }
        else {
            if (head == null) {
                Debug.Log("Miss head");
            }

            node_device curr = head, pre = null;

            while (true){ 
                if (curr.child.isGreatThan(new_node.child)) {
                    if (pre == null) {
                        new_node.next = head;
                        head = new_node;
                    }
                    else {
                        pre.next = new_node;
                        new_node.next = curr;
                    }
                    break;
                }
                else {
                    pre = curr;
                    curr = curr.next;
                    if (curr == null) {
                        pre.next = new_node;
                        new_node.next = null;
                        break;
                    }
                }
            }

        }
        length++;

        head.child.printData();
    }
    public bool checkHead() {
        if (head == null) return false;
        if (head.child.checkTime()) {
            Debug.Log("Delete head");
            head.child.activeEvent();
            return true;
        }
        else return false;
    }
    public void DeQueue() {
        head = head.next;
        if (head != null) head.child.printData();
        else Debug.Log("Queue empty");
    }
}
