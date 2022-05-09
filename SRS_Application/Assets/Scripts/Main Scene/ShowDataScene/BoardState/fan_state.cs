using UnityEngine;

public class fan_state : device_state
{
    void FixedUpdate() {
        if (ManagerConnect.instance.fan_state != this.state) {
            if (this.state) {
                //turn on --> off
                green_ren.color = color_dark;
            }
            else {
                // turn off
                red_ren.color = color_dark;
            }
            this.state = !this.state;
        }
        base.FixedUpdate();
    }
}
