using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoiceRecognition {
    public class speech_system : MonoBehaviour {
        int t = 35, h = 10, lgio = 15, lphut = 32, lgiay = 50, agio = 14, aphut = 26, agiay = 40;

        SpeechRecognitionEngine recognizer;
        //SpeechSynthesizer synthesizer;

        Choices cmdlist, numbers, objectChoice, optionChoice, customChoice;
        GrammarBuilder setCmd, customTemp, customTime, autoLight, builder;
        void SetUp() {
            //recognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("en-US"));
            recognizer = new SpeechRecognitionEngine();
            //synthesizer = new SpeechSynthesizer();

            cmdlist = new Choices("check temp", "check humid", "check alarm", "quit");
            numbers = new Choices(); for (int i = 0; i < 10; i++) numbers.Add(new SemanticResultValue(i.ToString(), i));
            objectChoice = new Choices(new SemanticResultValue("light", "light"), new SemanticResultValue("fan", "fan"), new SemanticResultValue("heater", "heater"), new SemanticResultValue("alarm", "alarm"));
            optionChoice = new Choices(new SemanticResultValue("on", "on"), new SemanticResultValue("off", "off"));

            setCmd = new GrammarBuilder("set");
            setCmd.Append(new SemanticResultKey("object", objectChoice));
            setCmd.Append(new SemanticResultKey("option", optionChoice));
            cmdlist.Add(setCmd);

            customTemp = new GrammarBuilder("set temp by");
            customTemp.Append(new SemanticResultKey("tempnum1", numbers));
            customTemp.Append("and");
            customTemp.Append(new SemanticResultKey("tempnum2", numbers));
            cmdlist.Add(customTemp);

            customChoice = new Choices("light", "alarm");
            customTime = new GrammarBuilder("set");
            customTime.Append(customChoice);
            customTime.Append("by");
            customTime.Append(new SemanticResultKey("h1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("h2", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("m1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("m2", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("s1", numbers));
            customTime.Append("and");
            customTime.Append(new SemanticResultKey("s2", numbers));
            cmdlist.Add(customTime);

            autoLight = new GrammarBuilder("set light automatic");
            cmdlist.Add(autoLight);

            builder = new GrammarBuilder(cmdlist);
            recognizer.LoadGrammar(new Grammar(builder));
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechHandler);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
            recognizer.EndSilenceTimeout = TimeSpan.FromSeconds(1);
            recognizer.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(1.5);
            
            // using (SpeechSynthesizer a = new SpeechSynthesizer()) {
            //     a.SetOutputToDefaultAudioDevice();
            //     a.Speak("ABC");
            // }


            // if (synthesizer == null) Debug.Log("NULL LISTEN");
            // try {
            //     synthesizer.SetOutputToWavFile(@"Speech.wav");
            // }
            // catch {
            //     Debug.Log("CATCH");
            // }
            
        }
        bool isSetUp = false;
        // Update is called once per frame
        void Update() {
            if (!isSetUp) {
                SetUp();
                isSetUp = true;
            }
            Console.ReadLine();
        }
        //bool unset = true;
        void SpeechHandler(object sender, SpeechRecognizedEventArgs e)
        {
            Debug.Log(e.Result.Text);
            
            string str = "non";
            if (e.Result.Text == "check temp")
            {
            //     if (unset) {
            //     synthesizer.SetOutputToDefaultAudioDevice();
            //     unset = false;
            // }
                //synthesizer.Speak("Current temperature is " + t);
                str = "Current temperature is " + ManagerConnect.instance.cur_temp;
            }
            else if (e.Result.Text == "check humid")
            {
                //synthesizer.Speak("Current humidity is " + h);
                str = "Current humidity is 0";
            }
            else if (e.Result.Text == "check alarm")
            {
                //synthesizer.Speak("Alarm will set off at " + agio + "hour" + aphut + "minute" + agiay + "second");
            }
            // else
            // {
            //     if (e.Result.Text.Contains("on") || e.Result.Text.Contains("off"))
            //     {
            //         synthesizer.Speak(e.Result.Semantics["object"].Value + "turned" + e.Result.Semantics["option"].Value);
            //     }
            //     else
            //     {
            //         if (e.Result.Text.Contains("set temp by"))
            //         {
            //             t = (int)e.Result.Semantics["tempnum1"].Value * 10 + (int)e.Result.Semantics["tempnum2"].Value;
            //             synthesizer.Speak("Current temperature is set to" + t.ToString());
            //         }
            //         else if (e.Result.Text.Contains("set alarm by"))
            //         {
            //             agio = (int)e.Result.Semantics["h1"].Value * 10 + (int)e.Result.Semantics["h2"].Value;
            //             aphut = (int)e.Result.Semantics["m1"].Value * 10 + (int)e.Result.Semantics["m2"].Value;
            //             agiay = (int)e.Result.Semantics["s1"].Value * 10 + (int)e.Result.Semantics["s2"].Value;
            //             synthesizer.Speak("Alarm will set off at " + agio + "hour" + aphut + "minute" + agiay + "second");
            //         }
            //         else if (e.Result.Text.Contains("set light by"))
            //         {
            //             lgio = (int)e.Result.Semantics["h1"].Value * 10 + (int)e.Result.Semantics["h2"].Value;
            //             lphut = (int)e.Result.Semantics["m1"].Value * 10 + (int)e.Result.Semantics["m2"].Value;
            //             lgiay = (int)e.Result.Semantics["s1"].Value * 10 + (int)e.Result.Semantics["s2"].Value;
            //             synthesizer.Speak("Light will turn off at " + lgio + "hour" + lphut + "minute" + lgiay + "second");
            //         }
            //         else if (e.Result.Text == "set light automatic")
            //         {
            //             synthesizer.Speak("Light will turn off automatically at " + lgio + "hour" + lphut + "minute" + lgiay + "second");
            //         }
            //     }
            // }

            if (e.Result.Text == "quit")
            {
                //Console.WriteLine("Exits");
                //recognizer.RecognizeAsyncStop();
                str = "Exit";
                //Application.Quit();
            }
            SystemLog.instance.EnQueue(str);
        }
    }
}

