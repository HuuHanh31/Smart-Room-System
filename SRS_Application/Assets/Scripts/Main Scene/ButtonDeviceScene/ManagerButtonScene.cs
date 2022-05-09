using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerButtonScene : MonoBehaviour
{
    public GameObject btn_light, btn_fan, btn_heater, btn_door;
    private bool flag_light = false, flag_fan = false, flag_heater = false, flag_door = false;
    private bool state_light = false, state_fan = false, state_heater = false, state_door = false;
    void Update() {
        if (flag_light) {
            if (state_light == ManagerConnect.instance.light_state) {
                flag_light = false;
                btn_light.SetActive(true);
            }
        }
        if (flag_fan) {
            if (state_fan == ManagerConnect.instance.fan_state) {
                flag_fan = false;
                btn_fan.SetActive(true);
            }
        }
        if (flag_heater) {
            if (state_heater == ManagerConnect.instance.heater_state) {
                flag_heater = false;
                btn_heater.SetActive(true);
            }
        }
        if (flag_door) {
            if (state_door == ManagerConnect.instance.door_state) {
                flag_door = false;
                btn_door.SetActive(true);
            }
        }
    }
    public void changeLight() {
        flag_light = true;
        state_light = !ManagerConnect.instance.light_state;
        btn_light.SetActive(false);

        ManagerConnect.instance.changeState(1);
    }
    public void changeFan() {
        flag_fan = true;
        state_fan = !ManagerConnect.instance.fan_state;
        btn_fan.SetActive(false);

        ManagerConnect.instance.changeState(2);
    }
    public void changeHeater() {
        flag_heater = true;
        state_heater = !ManagerConnect.instance.heater_state;
        btn_heater.SetActive(false);

        ManagerConnect.instance.changeState(3);
    }

    public void changeDoor() {
        flag_door = true;
        state_door = !ManagerConnect.instance.door_state;
        btn_door.SetActive(false);

        ManagerConnect.instance.changeState(5);
    }
}
