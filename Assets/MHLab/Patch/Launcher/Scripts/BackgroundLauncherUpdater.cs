using System;
using System.IO;
using System.Threading.Tasks;
using MHLab.Patch.Core;
using MHLab.Patch.Core.Client;
using MHLab.Patch.Core.Client.IO;
using MHLab.Patch.Core.IO;
using MHLab.Patch.Launcher.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace MHLab.Patch.Launcher.Scripts
{
    public class BackgroundLauncherUpdater : LauncherBase
    {
        public GameObject DebugSection;

        public float TimeBetweenEachCheckInSeconds = 60f;

        public UnityEvent RestartNeeded;

        private PatcherUpdater _patcherUpdater;
        private float          _timeFromLastCheck;
        private bool           _isPreviousUpdateCompleted;

        protected override string UpdateProcessName => "Background Launcher Updating";

        protected override void OverrideSettings(ILauncherSettings settings)
        {
            string rootPath = string.Empty;

#if UNITY_EDITOR
            rootPath = Path.Combine(Path.GetDirectoryName(Application.dataPath), LauncherData.WorkspaceFolderName,
                                    "TestLauncher");
            Directory.CreateDirectory(rootPath);
#elif UNITY_STANDALONE_WIN
            rootPath = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
#elif UNITY_STANDALONE_LINUX
            rootPath = Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName;
#elif UNITY_STANDALONE_OSX
            rootPath =
 Directory.GetParent(Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName).FullName;
#endif

            settings.RootPath = FilesManager.SanitizePath(rootPath);
        }

        protected override void Initialize(UpdatingContext context)
        {
            context.OverrideSettings<SettingsOverride>((originalSettings, settingsOverride) =>
            {
                originalSettings.DebugMode              = settingsOverride.DebugMode;
                originalSettings.PatcherUpdaterSafeMode = settingsOverride.PatcherUpdaterSafeMode;
            });

            context.Downloader.DownloadComplete += Data.DownloadComplete;

            NetworkChecker = new NetworkChecker();

            _patcherUpdater = new PatcherUpdater(context);

            context.RegisterUpdateStep(_patcherUpdater);

            context.Runner.PerformedStep += (sender, updater) =>
            {
                if (context.IsDirty(out var reasons, out var data))
                {
                    var stringReasons = "";

                    foreach (var reason in reasons)
                    {
                        stringReasons += $"{reason}, ";
                    }

                    stringReasons = stringReasons.Substring(0, stringReasons.Length - 2);

                    context.Logger.Debug(
                        $"Context is set to dirty: updater restart required. The files {stringReasons} have been replaced.");

                    if (data.Count > 0)
                    {
                        if (data[0] is UpdaterSafeModeDefinition)
                        {
                            var definition = (UpdaterSafeModeDefinition)data[0];
                            UpdateRestartNeeded(definition.ExecutableToRun);
                            return;
                        }
                    }

                    UpdateRestartNeeded();
                }
            };

            _timeFromLastCheck         = TimeBetweenEachCheckInSeconds;
            _isPreviousUpdateCompleted = true;
            Data.ProgressBar.gameObject.SetActive(false);
        }

        private void CheckForDebugInfoEnabling()
        {
            Context.Settings.DebugMode = Data.DebugMode;

            DebugSection.SetActive(Context.Settings.DebugMode && Data.ProgressBar.gameObject.activeSelf);
        }

        private void Update()
        {
            CheckForDebugInfoEnabling();

            _timeFromLastCheck += Time.deltaTime;

            if (_timeFromLastCheck >= TimeBetweenEachCheckInSeconds)
            {
                if (_isPreviousUpdateCompleted)
                {
                    Task.Run(StartCheckingForLauncherUpdates);
                }

                _timeFromLastCheck = 0;
            }
        }

        protected override void OnStart()
        {
        }

        private void StartCheckingForLauncherUpdates()
        {
            _isPreviousUpdateCompleted = false;

            if (FilesManager.IsDirectoryWritable(Context.Settings.GetLogsDirectoryPath()))
            {
                try
                {
                    Context.Logger.Info($"===> [{UpdateProcessName}] process STARTED! <===");

                    if (!CheckForNetworkAvailability())
                    {
                        Data.Dispatcher.Invoke(
                            () =>
                                Data.Dialog.ShowCloseDialog(
                                    Context.LocalizedMessages.NotAvailableNetwork,
                                    string.Empty,
                                    () => Data.Dialog.CloseDialog())
                        );

                        return;
                    }

                    if (!CheckForRemoteServiceAvailability())
                    {
                        Data.Dispatcher.Invoke(
                            () =>
                                Data.Dialog.ShowCloseDialog(
                                    Context.LocalizedMessages.NotAvailableServers,
                                    string.Empty,
                                    () => Data.Dialog.CloseDialog())
                        );

                        return;
                    }

                    Context.Initialize();

                    if (!_patcherUpdater.IsUpdateAvailable())
                    {
                        _isPreviousUpdateCompleted = true;
                        Data.Dispatcher.Invoke(
                            () => Data.ProgressBar.gameObject.SetActive(false)
                        );
                        return;
                    }

                    Task.Run(CheckForUpdates);
                }
                catch (Exception ex)
                {
                    UpdateFailed(ex);
                }
            }
            else
            {
                Data.Dispatcher.Invoke(() =>
                {
                    Data.Log(Context.LocalizedMessages.LogsFileNotWritable);
                    Data.Dialog.ShowDialog(Context.LocalizedMessages.LogsFileNotWritable,
                                           Context.Settings.GetLogsFilePath(),
                                           () => Data.Dialog.CloseDialog(),
                                           () => Data.Dialog.CloseDialog());
                    Data.ProgressBar.gameObject.SetActive(false);
                });

                Context.Logger.Error(
                    null, "Updating process FAILED! The Launcher has not enough privileges to write into its folder!");

                _isPreviousUpdateCompleted = true;
            }
        }

        protected override void UpdateStarted()
        {
            _isPreviousUpdateCompleted = false;
            Data.Dispatcher.Invoke(() =>
            {
                Data.StartTimer(UpdateDownloadSpeed);
                Data.ProgressBar.gameObject.SetActive(true);
            });
        }

        protected override void UpdateCompleted()
        {
            Data.Dispatcher.Invoke(() =>
            {
                Data.ProgressBar.Progress    = 1;
                Data.ProgressPercentage.text = "100%";
            });

            var repairer = new Repairer(Context);
            var updater  = new Updater(Context);

            if (repairer.IsRepairNeeded() || updater.IsUpdateAvailable())
            {
                UpdateRestartNeeded();
                return;
            }

            Data.Dispatcher.Invoke(() => { Data.Log(Context.LocalizedMessages.UpdateProcessCompleted); });
            Context.Logger.Info($"===> [{UpdateProcessName}] process COMPLETED! <===");

            _isPreviousUpdateCompleted = true;
        }

        protected override void UpdateFailed(Exception e)
        {
            Data.Dispatcher.Invoke(() =>
            {
                Data.Log(Context.LocalizedMessages.UpdateProcessFailed);
                Data.Dialog.ShowDialog(Context.LocalizedMessages.UpdateProcessFailed,
                                       e.Message,
                                       () => Data.Dialog.CloseDialog(),
                                       () => Data.Dialog.CloseDialog());

                Data.ProgressBar.gameObject.SetActive(false);

                Debug.LogException(e);
            });

            Context.Logger.Error(e, $"===> [{UpdateProcessName}] process FAILED! <=== - {e.Message} - {e.StackTrace}");

            _isPreviousUpdateCompleted = true;
        }

        protected override void UpdateRestartNeeded(string executableName = "")
        {
            Data.Dispatcher.Invoke(() => { Data.Log(Context.LocalizedMessages.UpdateRestartNeeded); });
            Context.Logger.Info($"===> [{UpdateProcessName}] process INCOMPLETE: restart is needed! <===");

            EnsureExecutePrivileges(PathsManager.Combine(Context.Settings.RootPath, Data.LauncherExecutableName));

            string filePath;

            if (!string.IsNullOrWhiteSpace(executableName))
            {
                filePath = PathsManager.Combine(Context.Settings.RootPath, executableName);
            }
            else
            {
                filePath = PathsManager.Combine(Context.Settings.RootPath, Data.LauncherExecutableName);
            }

            Data.Dispatcher.Invoke(
                () =>
                {
                    Data.Dialog.ShowDialog(
                        "Pending update!",
                        Context.LocalizedMessages.UpdateRestartNeeded,
                        () => Data.Dialog.CloseDialog(),
                        () =>
                        {
                            try
                            {
                                ApplicationStarter.StartApplication(
                                    Path.Combine(Context.Settings.RootPath,
                                                 Data.LauncherExecutableName),
                                    "");

                                Data.Dispatcher.Invoke(Application.Quit);
                            }
                            catch (Exception ex)
                            {
                                Context.Logger.Error(null, $"Unable to start the Launcher at {filePath}.");
                                UpdateFailed(ex);
                            }
                        }
                    );

                    RestartNeeded?.Invoke();
                }
            );

            _isPreviousUpdateCompleted = false;
        }

        protected override void UpdateDownloadSpeed()
        {
            Context.Downloader.DownloadSpeedMeter.Tick();

            if (Context.Downloader.DownloadSpeedMeter.DownloadSpeed > 0)
            {
                Data.Dispatcher.Invoke(() =>
                {
                    Data.DownloadSpeed.text = Context.Downloader.DownloadSpeedMeter
                                                     .FormattedDownloadSpeed;
                });
            }
            else
            {
                Data.Dispatcher.Invoke(() => { Data.DownloadSpeed.text = string.Empty; });
            }
        }

        protected override void StartApp()
        {
        }

        public void GenerateDebugReport()
        {
            GenerateDebugReport("debug_report_pregame.txt");
        }

        private void OnDisable()
        {
            Context.Downloader.Cancel();
        }
    }
}