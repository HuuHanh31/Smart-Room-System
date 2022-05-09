using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples {
    public class ClientMQTT : M2MqttUnityClient {
        public static ClientMQTT instance;
        public string Topic = "";
        public string Machine_Id;
        //public string Topic_to_Subcribe = "";
        public string msg_received_from_topic = "";
        private List<string> eventMessages = new List<string>();
        private string topic_temp = "", topic_led = "", topic_fan = "", topic_light = "", topic_gas = "";
        private string topic_bell = "", topic_door = "";
        protected override void Awake() {
            if (instance == null) instance = this;
            brokerAddress = PlayerPrefs.GetString("cur_broker_uri");
            mqttUserName = PlayerPrefs.GetString("cur_access_token");
            mqttPassword = PlayerPrefs.GetString("cur_pwd_access_token");
            brokerPort = 1883;

            // brokerAddress = "io.adafruit.com";
            // mqttUserName = "doantaydo";
            // mqttPassword = "aio_UGWa25DKmDrJ86aeApge3I94aB1la";
            mqttPassword = mqttPassword.Substring(0, mqttPassword.Length - 1);
            Debug.Log(brokerAddress);
            Debug.Log(mqttUserName);
            Debug.Log(mqttPassword);


            topic_temp = mqttUserName + "/feeds/microbit-temp";
            topic_led = mqttUserName + "/feeds/microbit-led";
            topic_fan = mqttUserName + "/feeds/microbit-fan";
            topic_light = mqttUserName + "/feeds/microbit-light";
            topic_gas = mqttUserName + "/feeds/microbit-gas";
            topic_bell = mqttUserName + "/feeds/microbit-buzzer";
            topic_door = mqttUserName + "/feeds/microbit-door";

            autoConnect = true;
            base.Awake();
        }

        public void TestPublish() {
            Debug.Log("Test message published");
        }

        public void SetEncrypted(bool isEncrypted) {
            this.isEncrypted = isEncrypted;
        }
        protected override void OnConnecting() {
            base.OnConnecting();
        }
        protected override void OnConnected() {
            base.OnConnected();
            SubscribeTopics();
        }
        public bool isPub = false;
        public int start_temp = 0, start_light = 2, start_gas = 1;
        public bool start_led = false, start_fan = false;
        protected override void SubscribeTopics()
        {
            if (!isPub) {
                Debug.Log("Pub");
                publishTemp(start_temp);
                publishLight(0);
                publishGas(0);
                publishLed(false);
                publishFan(false);
                publishBell(false);
                publishDoor(false);
                isPub = true;
            }
            
            if (topic_temp != "")
                client.Subscribe(new string[] { topic_temp }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_fan != "")
                client.Subscribe(new string[] { topic_fan }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_led != "")
                client.Subscribe(new string[] { topic_led }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_light != "")
                client.Subscribe(new string[] { topic_light }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_gas != "")
                client.Subscribe(new string[] { topic_gas }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_bell != "")
                client.Subscribe(new string[] { topic_bell }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            if (topic_door != "")
                client.Subscribe(new string[] { topic_door }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics() {
            client.Unsubscribe(new string[] { topic_temp });
            client.Unsubscribe(new string[] { topic_fan });
            client.Unsubscribe(new string[] { topic_led });
            client.Unsubscribe(new string[] { topic_light });
            client.Unsubscribe(new string[] { topic_gas });
            client.Unsubscribe(new string[] { topic_bell });
            client.Unsubscribe(new string[] { topic_door });
        }

        protected override void OnConnectionFailed(string errorMessage) {}

        protected override void OnDisconnected() {}

        protected override void OnConnectionLost() {}

        protected override void Start()
        {
            SystemLog.instance.EnQueue("Connecting to server!!!");
            base.Start();
            SystemLog.instance.EnQueue("Connected to server!!!");
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            msg_received_from_topic = msg;
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
            if (topic == topic_led)
                ManagerConnect.instance.light_state = (msg == "1");
            if (topic == topic_fan)
                ManagerConnect.instance.fan_state = (msg == "3");
            if (topic == topic_bell)
                ManagerConnect.instance.bell_state = (msg == "5");
            if (topic == topic_door)
                ManagerConnect.instance.door_state = (msg == "7");
            if (topic == topic_temp)
                ManagerConnect.instance.cur_temp = float.Parse(msg);
            if (topic == topic_light)
                ManagerConnect.instance.cur_light = float.Parse(msg);
            if (topic == topic_gas)
                ManagerConnect.instance.cur_gas = float.Parse(msg);
        }

        private void StoreMessage(string eventMsg) {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg) {}

        protected override void Update() {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
        }

        public void OnDestroy() {
            Disconnect();
        }

        private void OnValidate() {}
        public void publishLed(bool type) {
            if (topic_led == "") return;
            if (type) client.Publish(topic_led, System.Text.Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_led, System.Text.Encoding.UTF8.GetBytes("0"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishFan(bool type) {
            if (topic_fan == "") return;
            if (type) client.Publish(topic_fan, System.Text.Encoding.UTF8.GetBytes("3"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_fan, System.Text.Encoding.UTF8.GetBytes("2"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishBell(bool type) {
            if (topic_bell == "") return;
            if (type) client.Publish(topic_bell, System.Text.Encoding.UTF8.GetBytes("5"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_bell, System.Text.Encoding.UTF8.GetBytes("4"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishDoor(bool type) {
            if (topic_door == "") return;
            if (type) client.Publish(topic_door, System.Text.Encoding.UTF8.GetBytes("7"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            else client.Publish(topic_door, System.Text.Encoding.UTF8.GetBytes("6"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishTemp(float value) {
            if (topic_temp == "") return;
            client.Publish(topic_temp, System.Text.Encoding.UTF8.GetBytes(value.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishLight(float value) {
            if (topic_temp == "") return;
            client.Publish(topic_light, System.Text.Encoding.UTF8.GetBytes(value.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishGas(float value) {
            if (topic_temp == "") return;
            client.Publish(topic_gas, System.Text.Encoding.UTF8.GetBytes(value.ToString()), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public void publishHeater(bool type) { }
    }
}


