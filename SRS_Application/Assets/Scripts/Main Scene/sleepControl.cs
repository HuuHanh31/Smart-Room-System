using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class sleepControl : MonoBehaviour
{
    public Text field;
    public GameObject mainCanvas, isSleeping;
    public static sleepControl instance;
    bool isCount;
    int time, count;
    void Awake() {
        if (instance == null) instance = this;
        isCount = false;
        time = -1;
    }
    public void StartRunning() {
        if (time < 0 && isCount == false) {
            isCount = true;
            time = 10;
            count = 0;

            field.text = time.ToString();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isCount) {
            count++;
            if (count == 10) {
                time--;
                field.text = time.ToString();
                count = 0;
            }

            if (time == 0 && count == 5) turnOff();
        }
        
    }
    void turnOff() {
        mainCanvas.SetActive(true);
        isSleeping.SetActive(false);
        isCount = false;
        ManagerConnect.instance.changeState(1);
    }
    public void stopCount() {
        time = -1;
    }
}
