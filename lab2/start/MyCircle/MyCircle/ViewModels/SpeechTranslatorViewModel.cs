using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinUniversity.Infrastructure;

namespace MyCircle.ViewModels
{
    public class SpeechTranslatorViewModel : SimpleViewModel
    {
        private bool isTranslating;
        private readonly Action<string> finishedCallback;

        public bool IsTranslating
        {
            get => isTranslating;
            set
            {
                if (SetPropertyValue(ref isTranslating, value))
                {
                    StopRecording.ChangeCanExecute();
                }
            }
        }

        public Command StopRecording { get; }

        public SpeechTranslatorViewModel(Action<string> finished)
        {
            finishedCallback = finished ?? throw new ArgumentNullException(nameof(finished));

            StopRecording = new Command(async () => await OnStopRecording(), () => !IsTranslating);
        }


        public Task StartRecording()
        {
            // TODO: add code to start recording.

            return Task.CompletedTask;
        }

        private async Task OnStopRecording()
        {
            IsTranslating = true;
            
            // TODO: stop recording
            await Task.Delay(2500);

            // TODO: submit text to Azure
            string text = "Hello";

            // End the screen.
            finishedCallback(text);
        }
    }
}
