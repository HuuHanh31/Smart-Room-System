using UnityEngine;

public class RollButton : MonoBehaviour
{
    int rot;
    void Start()
    {
        rot = 360;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        rot--;
        if (rot < 0) rot = 360;
        GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, rot);
    }
}
