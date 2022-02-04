using System;
using System.Threading;
using MHLab.Patch.Core.Client.Progresses;
using MHLab.Patch.Core.Utilities;
using MHLab.Patch.Launcher.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MHLab.Patch.Launcher.Scripts
{
    public sealed class LauncherData : MonoBehaviour
    {
        public string RemoteUrl;
        public string LauncherExecutableName;
        public string GameExecutableName;

        public bool LaunchAnywayOnError;
        public bool DebugMode;
        
        public Dispatcher  Dispatcher;
        public ProgressBar ProgressBar;
        public Text        DownloadSpeed;
        public Text        ProgressPercentage;
        public Text        Logs;
        public Text        ElapsedTime;
        public Dialog      Dialog;
        public Text        SizeProgress;
        public Text        SoftwareVersion;
        public Button      ResumeButton;
        public Button      PauseButton;
        
        public const string WorkspaceFolderName = "PATCHWorkspace";
        
        private Timer _timer;
        private int _elapsed;

        private void Start()
        {
            ResetComponents();
        }

        public void DownloadComplete(object sender, EventArgs e)
        {
            
        }

        public void UpdateProgressChanged(UpdateProgress e)
        {
            Dispatcher.Invoke(() =>
            {
                var totalSteps = Math.Max(e.TotalSteps, 1);
                ProgressBar.Progress = (float) e.CurrentSteps / totalSteps;

                ProgressPercentage.text = (e.CurrentSteps * 100 / totalSteps) + "%";

                SizeProgress.text = FormatUtility.FormatSizeDecimal(e.CurrentSteps, 2) + "/" +
                                    FormatUtility.FormatSizeDecimal(e.TotalSteps, 2);
            });
            
            Log(e.StepMessage);
        }
        
        public void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                Logs.text = message;
            });
        }

        public void ResetComponents()
        {
            ProgressPercentage.text = "0%";
            DownloadSpeed.text      = "0B/s";
            ElapsedTime.text        = "00:00";
            Logs.text               = string.Empty;
            SizeProgress.text       = "0B/0B";
            
            ProgressBar.Progress = 0;
        }

        public void StartTimer(Action updateDownloadSpeed)
        {
            _timer = new Timer((state) =>
            {
                _elapsed++;
                Dispatcher.Invoke(() =>
                {
                    var minutes = _elapsed / 60;
                    var seconds = _elapsed % 60;

                    ElapsedTime.text = string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
                    
                    updateDownloadSpeed.Invoke();
                });
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        public void StopTimer()
        {
            _timer.Dispose();
        }
    }
}