using UnityEngine;
using UnityEngine.UI;

public class RollEff : MonoBehaviour
{
    float amount = 0;
    Image img;
    void Start() {
        img = GetComponent<Image>();
        img.fillAmount = amount;
    }

    void Update()
    {
        amount += 0.005f;
        if (amount > 1f) amount = 0f;
        img.fillAmount = amount;
    }
}
