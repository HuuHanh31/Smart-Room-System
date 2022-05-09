function Sensor () {
    cnt_sensor += 1
    if (cnt_sensor >= 300) {
        cnt_sensor = 0
        NPNBitKit.DHT11Read(DigitalPin.P0)
        serial.writeString("!1:TEMP:" + NPNBitKit.DHT11Temp() + "#")
        basic.pause(100)
        serial.writeString("!1:HUMI:" + NPNBitKit.DHT11Hum() + "#")
        basic.pause(100)
        serial.writeString("!2:LIGHT:" + pins.analogReadPin(AnalogPin.P3) + "#")
        basic.pause(100)
        serial.writeString("!3:GAS:" + pins.analogReadPin(AnalogPin.P10) + "#")
    }
}
function Gas_Alarm () {
    if (pins.analogReadPin(AnalogPin.P10) >= 700) {
        pins.digitalWritePin(DigitalPin.P6, 1)
        pins.digitalWritePin(DigitalPin.P7, 0)
        pins.analogWritePin(AnalogPin.P4, 1000)
    } else if (pins.analogReadPin(AnalogPin.P10) >= 500) {
        pins.digitalWritePin(DigitalPin.P6, 1)
        pins.digitalWritePin(DigitalPin.P7, 1)
    } else {
        pins.digitalWritePin(DigitalPin.P6, 0)
        pins.digitalWritePin(DigitalPin.P7, 1)
    }
}
input.onButtonPressed(Button.A, function () {
    entry = "" + entry + "A"
})
function Light () {
    cnt_lightss += 1
    if (cnt_lightss >= 20) {
        if (pins.analogReadPin(AnalogPin.P3) <= 350) {
            cnt_light = 0
            cnt_dark += 1
            if (cnt_dark >= 5) {
                pins.digitalWritePin(DigitalPin.P2, 1)
            }
        } else {
            cnt_dark = 0
            cnt_light += 1
            if (cnt_light >= 5) {
                pins.digitalWritePin(DigitalPin.P2, 0)
            }
        }
    }
}
input.onButtonPressed(Button.AB, function () {
    if (entry == pwd) {
        NPNLCD.ShowString("Welcome", 0, 0)
    } else {
        NPNLCD.ShowString("Wrong password", 0, 0)
    }
    basic.pause(500)
    entry = ""
})
input.onButtonPressed(Button.B, function () {
    entry = "" + entry + "B"
})
serial.onDataReceived(serial.delimiters(Delimiters.Hash), function () {
    if (serial.readString() == "#0") {
        pins.digitalWritePin(DigitalPin.P2, 0)
    }
    if (serial.readString() == "#1") {
        pins.digitalWritePin(DigitalPin.P2, 1)
    }
    if (serial.readString() == "#2") {
        pins.digitalWritePin(DigitalPin.P9, 0)
    }
    if (serial.readString() == "#3") {
        pins.digitalWritePin(DigitalPin.P9, 1)
    }
    if (serial.readString() == "#4") {
        pins.analogWritePin(AnalogPin.P4, 0)
    }
    if (serial.readString() == "#5") {
        pins.analogWritePin(AnalogPin.P4, 1023)
    }
})
let cnt_lightss = 0
let cnt_sensor = 0
let cnt_light = 0
let cnt_dark = 0
let pwd = ""
let entry = ""
NPNLCD.LcdInit()
entry = ""
pwd = "ABAB"
cnt_dark = 0
cnt_light = 0
cnt_sensor = 0
cnt_lightss = 0
pins.digitalWritePin(DigitalPin.P8, 0)
led.enable(false)
basic.forever(function () {
    Sensor()
    Light()
    Gas_Alarm()
    basic.pause(100)
})
