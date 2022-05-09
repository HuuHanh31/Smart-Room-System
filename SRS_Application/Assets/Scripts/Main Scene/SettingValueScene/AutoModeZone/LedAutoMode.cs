using UnityEngine;

public class LedAutoMode : device_state
{
    void FixedUpdate() {
        if (ManagerConnect.instance.isAuto != this.state) {
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
