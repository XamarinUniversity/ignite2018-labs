using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Plugin.SimpleAudioRecorder;
using Plugin.SpeechToText;
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

        ISimpleAudioRecorder recorder;

        public async Task StartRecording()
        {
            if (!await RequestAudioPermissions())
            {
                finishedCallback(null);
                return;
            }

            recorder = CrossSimpleAudioRecorder.CreateSimpleAudioRecorder();
            if (recorder.CanRecordAudio)
            {
                await recorder.RecordAsync().ConfigureAwait(false);
            }
            else
            {
                finishedCallback(null);
            }
        }

        private async Task<bool> RequestAudioPermissions()
        {
            if (Device.RuntimePlatform == Device.macOS)
                return true;

            PermissionStatus status = PermissionStatus.Unknown;

            try
            {
                status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Microphone);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Microphone))
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Microphone);
                        if (results.ContainsKey(Permission.Microphone))
                            status = results[Permission.Microphone];
                    }
                }
            }
            catch
            {
            }

            return status == PermissionStatus.Granted;
        }

        const string SpeechApiKey = "fb3c4e67a81242f794cb56ebb279271d";

        private async Task OnStopRecording()
        {
            IsTranslating = true;

            // TODO: stop recording
            var recorderResult = await recorder.StopAsync();

            // TODO: submit text to Azure
            SpeechToText speechClient = new SpeechToText(SpeechApiKey);
            var speechResult = await speechClient.RecognizeSpeechAsync(recorderResult.GetFilePath());

            // End the screen.
            finishedCallback(speechResult.DisplayText);
        }
    }
}
