using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagerAutoMode : MonoBehaviour
{
    public Text minTemp, maxTemp;
    float min_temp, max_temp;
    public void clicked() {
        if (checkValue())
        ManagerConnect.instance.updateAutoMode(min_temp, max_temp);
    }
    public void turnAutoLight() {
        ManagerConnect.instance.updateAutoLight();
    }
    bool checkValue() {
        string min_temp_value = minTemp.text;
        string max_temp_value = maxTemp.text;

        min_temp = getValue(min_temp_value);
        if (min_temp == 10000000) return false;

        max_temp = getValue(max_temp_value);
        if (max_temp == 10000000) return false;
        
        return true;
    }
    float getValue(string input) {
        if (input.Length == 0) return 10000000;
        for (int i = 0; i < input.Length; i++) {
            if ((input[i] == '-' && i != 0) ||
                (!(input[i] == '.' || (input[i] >= '0' && input[i] <= '9'))))
                return 10000000;
        }
        return float.Parse(input);
    }
}
