using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPSpeech
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click_Recognize(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;

            String[] array = {"开始", "返回", "退出", "设置", "介绍"};
            SpeechRecognitionListConstraint speechRecognitionListConstraint =
                new SpeechRecognitionListConstraint(array);

            using (SpeechRecognizer recognizer = new SpeechRecognizer())
            {
                try
                {
                    recognizer.Constraints.Add(speechRecognitionListConstraint);
                    SpeechRecognitionCompilationResult compilationResult = await recognizer.CompileConstraintsAsync();
                    if (compilationResult.Status == SpeechRecognitionResultStatus.Success)
                    {
                        SpeechRecognitionResult speechRecognitionResult = await recognizer.RecognizeAsync();
                        if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
                        {
                            tbDisplay.Text = "finished";
                            textInput.Text = speechRecognitionResult.Text;
                            this.lb.SelectedItem = speechRecognitionResult.Text;
                        }
                    }
                }
                catch (Exception exception)
                {
                    tbDisplay.Text = "Error" + exception.Message;
                    //throw;
                }
            }
            button.IsEnabled = true;
        }

        private async void Button_Click_Speech(object sender, RoutedEventArgs e)
        {
            //            throw new NotImplementedException();
            if (textInput.Text.Length == 0) return;
            Button button = sender as Button;
            button.IsEnabled = false;
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            SpeechSynthesisStream speechSynthesisStream =
                await speechSynthesizer.SynthesizeTextToStreamAsync(textInput.Text);
            Media.SetSource(speechSynthesisStream, speechSynthesisStream.ContentType);
            button.IsEnabled = true;
        }

    }
}
