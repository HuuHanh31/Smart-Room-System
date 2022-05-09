using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class callFunction : MonoBehaviour
{
    // TURN ON/OFF THE LIGHT/FAN
    // OPEN/CLOSE THE DOOR
    // Input:
    // - device: 0: light, 1: fan, 2: door
    // - isOn: true: ON/OPEN, false: OFF/CLOSE
    public static void turnDevice(int device, bool isOn) {
        switch(device) {
            case 0: // light
                if (ManagerConnect.instance.light_state != isOn) ManagerConnect.instance.changeState(1);
                break;
            case 1: // fan
                if (ManagerConnect.instance.fan_state != isOn) ManagerConnect.instance.changeState(2);
                break;
            case 2: // door
                if (ManagerConnect.instance.door_state != isOn) ManagerConnect.instance.changeState(5);
                break;
            default:
                break;
        }
    }
    // TURN OFF THE LIGHT/FAN AT HH:MM:SS
    // Input: device (0: light, 1: fan), hour, minute, second
    public static void turnOffDeviceAt(int device, int hh, int mm, int ss) {
        ManagerSetTurnOff.instance.setAdd(device + 1, hh, mm, ss);
    }
    // TURN ON AUTO MODE WITH TEMPERATURE FROM MIN_TEMP TO MAX_TEMP
    // TURN OFF AUTO MODE
    public static void turnAutoMode(bool isOn, float min_temp = 0, float max_temp = 0) {
        if (ManagerConnect.instance.isAuto != isOn)
            ManagerConnect.instance.updateAutoMode(min_temp, max_temp);
    }
    // TURN ON/OFF AUTO LIGHT MODE
    public static void turnAutoLightMode(bool isOn) {
        if (ManagerConnect.instance.isAutoLight != isOn)
            ManagerConnect.instance.updateAutoLight();
    }
    // EXIT/QUIT
    public static void quitSystem() {
        M2MqttUnity.Examples.ClientMQTT.instance.OnDestroy();
        Application.Quit();
    }
    // LOGOUT
    public static void logOut() {
        ManagerConnect.instance.LogOut();
    }
    // YES, I'M HERE, ... etc
    public static void iHere() {
        ManagerConnect.instance.iHere();
    }
}
