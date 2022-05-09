using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class device_state : MonoBehaviour
{
    public GameObject red_led, green_led;
    protected bool state, check;
    int count;
    protected Color color_dark, color_light;
    protected SpriteRenderer red_ren, green_ren;
    void Start() {
        red_ren = red_led.GetComponent<SpriteRenderer>();
        green_ren = green_led.GetComponent<SpriteRenderer>();
        color_light = new Color(1f, 1f, 1f, 1f);
        color_dark = new Color(0.7f, 0.7f, 0.7f, 1f);
        count = 0;
        check = false;
        state = false;
    }

    // Update is called once per frame
    protected void FixedUpdate() {
        if (count == 5) {
            if (state) {
                // turn on
                red_ren.color = color_dark;
                if (check) green_ren.color = color_light;
                else green_ren.color = color_dark;
            }
            else {
                // turn off
                green_ren.color = color_dark;
                if (check) red_ren.color = color_light;
                else red_ren.color = color_dark;
            }

            check = !check;
            count = -1;
        }
        count++;
    }
}
