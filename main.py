from Adafruit_IO import MQTTClient
from dotenv import load_dotenv
import sys
import time
import json
import random
import serial
import os

AIO_FEED_DEVICE = ["microbit-led", "microbit-fan", "microbit-buzzer", "microbit-door"]

load_dotenv()

AIO_USERNAME = "HanhHuynh"
AIO_KEY = os.getenv('API_KEY')


def connected(client):
    print("Connected successfully ...")
    for feed in AIO_FEED_DEVICE:
        client.subscribe(feed)

def subscribe(client, userdata, mid, granted_qos):
    print("Subscribed successfully ...")


def disconnected(client):
    print("Disconnect ...")
    sys.exit(1)


def message(client, feed_id, payload):  
    print (feed_id + ": " + payload)      
    if isMicrobitConnected:
        ser.write(("#" + str(payload)).encode())

client = MQTTClient(AIO_USERNAME, AIO_KEY)
client.on_connect = connected
client.on_disconnect = disconnected
client.on_message = message
client.on_subscribe = subscribe
client.connect()
client.loop_background()

def getPort():
    return "COM6"

isMicrobitConnected = False
if getPort() != "None":
    ser = serial.Serial(port=getPort(), baudrate=115200)
    isMicrobitConnected = True
 
def processData(data):
    data = data.replace("!", "")
    data = data.replace("#", "")
    splitData = data.split(":")
    print(splitData)
    try:
        if splitData[0] == "1":
            if splitData[1] == "TEMP":
                client.publish("microbit-temp", splitData[2])
            elif splitData[1] == "HUMI":
                client.publish("microbit-humid", splitData[2])   
        elif splitData[0] == "2":
            if splitData[1] == "LIGHT":
                client.publish("microbit-light", splitData[2])   
        elif splitData[0] == "3":
            if splitData[1] == "GAS":
                client.publish("microbit-gas", splitData[2])               
    except: 
        pass        

mess = ""
def readSerial():
    bytesToRead = ser.inWaiting()
    if (bytesToRead > 0):
        global mess
        mess = mess + ser.read(bytesToRead).decode("UTF-8")
        while ("#" in mess) and ("!" in mess):
            start = mess.find("!")
            end = mess.find("#")
            processData(mess[start:end + 1])
            if (end == len(mess)):
                mess = ""
            else:
                mess = mess[end+1:]           

# def test():
#     temp = random.randint(0, 50)
#     humi = random.randint(0, 100)
#     light = random.randint(0, 1023)
#     gas = random.randint(0, 1023)
#     client.publish("microbit-temp", temp)
#     client.publish("microbit-humid", humi)
#     client.publish("microbit-light", light)
#     client.publish("microbit-gas", gas)

#     data_x = {
#         "Temp": temp,
#         "Humid": humi,
#         "Light": light,
#         "Gas": gas  
#     }

#     data_y = json.dumps(data_x)
#     print("Time:", time.ctime(time.time()))
#     print(data_y)

#     time.sleep(10)

while True:
    # test()
    if isMicrobitConnected:
        readSerial()

    time.sleep(1)
