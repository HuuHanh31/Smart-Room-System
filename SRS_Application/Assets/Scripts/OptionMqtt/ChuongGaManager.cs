using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ChuongGa
{
    public class ChuongGaManager : MonoBehaviour
    {
        [SerializeField]
        private Text station_name;
        
        [SerializeField]
        private CanvasGroup _canvasLayer1;
        [SerializeField]
        private Toggle LampControl;
        [SerializeField]
        private Text LampStatus;
        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;
        [SerializeField]
        private Text min_temperature;
        [SerializeField]
        private Text max_temperature;
        [SerializeField]
        private CanvasGroup status_fan_lamp_on;
        [SerializeField]
        private CanvasGroup status_fan_lamp_off;
        [SerializeField]
        private Button _btn_config;
        /// <summary>
        /// Layer 2 elements
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasLayer2;
        [SerializeField]
        private InputField _input_min_tempe;
        [SerializeField]
        private InputField _input_max_tempe;
        [SerializeField]
        private Toggle ModeAuto;

        /// <summary>
        /// General elements
        /// </summary>
        [SerializeField]
        private GameObject Btn_Quit;

        private Tween twenFade;

        private bool device_status = false;

        public void Update_Status(Status_Data _status_data)
        {
            station_name.text = _status_data.station_name;
            foreach(data_ss _data in _status_data.data_ss)
            {
                switch (_data.ss_name)
                {

                    case "temperature_min":
                        min_temperature.text = _data.ss_value + "°C";
                        _input_min_tempe.text = _data.ss_value;
                        break;

                    case "temperature_max":
                        max_temperature.text = _data.ss_value + "°C";
                        _input_max_tempe.text = _data.ss_value;

                        break;

                    case "fan_temperature":
                        temperature.text = _data.ss_value + "°C";
                        break;

                    case "fan_humidity":
                        humidity.text = _data.ss_value + "%";
                        break;

                    case "mode_fan_auto":
                        if (_data.ss_value == "1") { 
                            ModeAuto.isOn = true;
                            LampControl.interactable = false;
                        }
                        else { 
                            ModeAuto.isOn = false;
                            LampControl.interactable = true;
                        }
                        break;
                    //case "device_status":
                    //    Debug.Log("_data.ss_value " + _data.ss_value);
                    //    if (_data.ss_value == "1")
                    //        _btn_config.interactable = true;
                       
                    //    break;
                }
                
            }
            if(_status_data.device_status=="1")
                _btn_config.interactable = true;

        }

        public void Update_Control(ControlFan_Data _control_data)
        {
            if (_control_data.device_status == 1)
            {
                LampControl.interactable = true;
                if (_control_data.fan_status == 1)
                    LampControl.isOn = true;
                else
                    LampControl.isOn = false;
            }

        }

        public ControlFan_Data Update_ControlFan_Value(ControlFan_Data _controlFan)
        {
            _controlFan.device_status = 0;
            _controlFan.fan_status = (LampControl.isOn ? 1 : 0);
            LampControl.interactable = false;
            return _controlFan;
        }

        public Config_Data Update_Config_Value(Config_Data _configdata)
        {
            _configdata.temperature_max = float.Parse(_input_max_tempe.text);
            _configdata.temperature_min = float.Parse(_input_min_tempe.text);
            _configdata.mode_fan_auto = ModeAuto.isOn ? 1 : 0;
           
            return _configdata;
        }

        public void OnLampValueChange()
        {
            if (LampControl.isOn == true)
            {
                LampStatus.text = "ON";
                LampStatus.color = new Color(0f, 255f, 0f);
            }
            else
            {
                LampStatus.text = "OFF";
                LampStatus.color = new Color(255f, 0f, 0f);
            }
            SwitchLamp();
        }

        public void Disable_Config_Btn()
        {
            _btn_config.interactable = false;
        }

        public void Fade(CanvasGroup _canvas, float endValue, float duration, TweenCallback onFinish)
        {
            if (twenFade != null)
            {
                twenFade.Kill(false);
            }

            twenFade = _canvas.DOFade(endValue, duration);
            twenFade.onComplete += onFinish;
        }

        public void FadeIn(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 1f, duration, () =>
            {
                _canvas.interactable = true;
                _canvas.blocksRaycasts = true;
            });
        }

        public void FadeOut(CanvasGroup _canvas, float duration)
        {
            Fade(_canvas, 0f, duration, () =>
            {
                _canvas.interactable = false;
                _canvas.blocksRaycasts = false;
            });
        }



        IEnumerator _IESwitchLayer()
        {
            if (_canvasLayer1.interactable == true)
            {
                FadeOut(_canvasLayer1, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer2, 0.25f);
            }
            else
            {
                FadeOut(_canvasLayer2, 0.25f);
                yield return new WaitForSeconds(0.5f);
                FadeIn(_canvasLayer1, 0.25f);
            }
        }

        IEnumerator _IESwitchlamp()
        {
            if (status_fan_lamp_off.interactable == true)
            {
                FadeOut(status_fan_lamp_off, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_fan_lamp_on, 0.1f);
            }
            else
            {
                FadeOut(status_fan_lamp_on, 0.1f);
                yield return new WaitForSeconds(0.15f);
                FadeIn(status_fan_lamp_off, 0.1f);
            }
        }
        public void SwitchLayer()
        {
            StartCoroutine(_IESwitchLayer());
        }

        public void SwitchLamp()
        {
            StartCoroutine(_IESwitchlamp());

        }
    }
}