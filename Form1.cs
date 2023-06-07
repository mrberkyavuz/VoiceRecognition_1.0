using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Diagnostics;
using System.Threading;


namespace VoiceRecognition_1._0
{
    public partial class LstCommands : Form
    {

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Claire = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
        DateTime TimeNow = DateTime.Now;

        public LstCommands()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
        }


        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            if (speech == "Hello")
            {
                Claire.SpeakAsync("Hello, I am here");
            }

            if (speech == "How are you")
            {
                Claire.SpeakAsync("I am doing great ,how are you");
            }

            if (speech == "What time is it")
            {
                Claire.SpeakAsync(DateTime.Now.ToString("h mm tt"));
            }
            if (speech == "Stop listening") 
            {

                Claire.SpeakAsync("If you need me just ask");
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }
            if (speech == "Open google") 
            {
                Claire.SpeakAsync("Okey Sir I'm opening Google now");
                Process.Start("chrome", "https://www.google.com/");
            }

            if (speech == "Open youtube")
            {
                Claire.SpeakAsync("Okey Sir I'm opening Youtube now");
                Process.Start("chrome", "https://www.youtube.com/?hl=tr&gl=TR");
            }

            if (speech == "Open whatsapp")
            {
                Claire.SpeakAsync("Okey Sir I'm opening Whatsapp now");
                Process.Start("chrome", "https://web.whatsapp.com/");
            }

            if (speech == "Say hello to Enes")
            {
                Claire.SpeakAsync("Okey let's we say hello to Enes");
                Process.Start("chrome", "https://web.whatsapp.com/send?phone=+905056721903&text=Hello%20Enes");
            }

            if (speech == "Say hello to Levent")
            {
                Claire.SpeakAsync("Okey let's we say hello to Levent");
                Process.Start("chrome", "https://web.whatsapp.com/send?phone=+905332010820&text=Hello%20Levent");
            }

        }

        private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;
        }

        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if (speech == "Wake up")
            {
                startlistening.RecognizeAsyncCancel();
                Claire.SpeakAsync("Yes I am here");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecTimeOut == 10) 
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if (RecTimeOut == 11) 
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }
    }
}
