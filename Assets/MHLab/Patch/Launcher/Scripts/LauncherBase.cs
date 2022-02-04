using System;
using System.IO;
using System.Threading.Tasks;
using MHLab.Patch.Core.Client;
using MHLab.Patch.Core.Client.IO;
using MHLab.Patch.Core.IO;
using MHLab.Patch.Core.Logging;
using MHLab.Patch.Core.Utilities;
using MHLab.Patch.Launcher.Scripts.Localization;
using MHLab.Patch.Launcher.Scripts.Utilities;
using MHLab.Patch.Utilities;
using MHLab.Patch.Utilities.Serializing;
using UnityEngine;

namespace MHLab.Patch.Launcher.Scripts
{
    public abstract class LauncherBase : MonoBehaviour
    {
        public LauncherData Data;
        
        protected UpdatingContext Context;
        protected INetworkChecker NetworkChecker;
        protected abstract string UpdateProcessName { get; }
        
        private ILauncherSettings CreateSettings()
        {
            var settings = new LauncherSettings();
            settings.RemoteUrl = Data.RemoteUrl;
            settings.PatchDownloadAttempts = 3;
            settings.AppDataPath = Application.persistentDataPath;
            
#if DEBUG
            settings.DebugMode = true;
#else
            settings.DebugMode = false;
#endif

            Data.DebugMode = settings.DebugMode;
            
            OverrideSettings(settings);

            return settings;
        }

        protected abstract void OverrideSettings(ILauncherSettings settings);

        private UpdatingContext CreateContext(ILauncherSettings settings)
        {
            var progress = new ProgressReporter();
            progress.ProgressChanged.AddListener(Data.UpdateProgressChanged);
            
            var context = new UpdatingContext(settings, progress);
            context.Logger     = new SimpleLogger(context.FileSystem, settings.GetLogsFilePath(), settings.DebugMode);
            context.Serializer = new JsonSerializer();
            context.LocalizedMessages = new EnglishUpdaterLocalizedMessages();

            return context;
        }
        
        private void Initialize(ILauncherSettings settings)
        {
            Context = CreateContext(settings);

            if (Data.SoftwareVersion != null)
            {
                Data.SoftwareVersion.text = $"v{settings.SoftwareVersion}";
            }

            Initialize(Context);
        }

        protected abstract void Initialize(UpdatingContext context);
        
        protected void GenerateDebugReport(string path)
        {
            var system = DebugHelper.GetSystemInfo();
            var report = Debugger.GenerateDebugReport(Context.Settings, system, new JsonSerializer());
            
            File.WriteAllText(path, report);
        }

        private void Awake()
        {
            Initialize(CreateSettings());
        }

        private void Start()
        {
            OnStart();
        }
        
        protected virtual void OnStart()
        {
            if (FilesManager.IsDirectoryWritable(Context.Settings.GetLogsDirectoryPath()))
            {
                StartUpdateProcess();
            }
            else
            {
                Data.Log(Context.LocalizedMessages.LogsFileNotWritable);
                Context.Logger.Error(null, "Updating process FAILED! The Launcher has not enough privileges to write into its folder!");

                if (Data.LaunchAnywayOnError == false)
                {
                    Data.Dialog.ShowDialog(Context.LocalizedMessages.LogsFileNotWritable,
                        Context.Settings.GetLogsFilePath(),
                        Application.Quit,
                        StartUpdateProcess);
                }
                else
                {
                    StartApp();
                }
            }
        }

        protected void StartUpdateProcess()
        {
            try
            {
                Context.Logger.Info($"===> [{UpdateProcessName}] process STARTED! <===");

                if (CheckForNetworkAvailability() == false)
                {
                    if (Data.LaunchAnywayOnError)
                    {
                        StartApp();
                    }
                    else
                    {
                        Data.Dialog.ShowCloseDialog(Context.LocalizedMessages.NotAvailableNetwork,
                                                    string.Empty,
                                                    Application.Quit);
                    }
                    
                    return;
                }

                if (CheckForRemoteServiceAvailability() == false)
                {
                    if (Data.LaunchAnywayOnError)
                    {
                        StartApp();
                    }
                    else
                    {
                        Data.Dialog.ShowCloseDialog(Context.LocalizedMessages.NotAvailableServers,
                                                    string.Empty,
                                                    Application.Quit);
                    }

                    return;
                }

                Context.Initialize();

                Task.Run(CheckForUpdates);
            }
            catch (Exception ex)
            {
                UpdateFailed(ex);
                
                if (Data.LaunchAnywayOnError)
                {
                    StartApp();
                }
            }
        }

        protected bool CheckForNetworkAvailability()
        {
            if (!NetworkChecker.IsNetworkAvailable())
            {
                Data.Log(Context.LocalizedMessages.NotAvailableNetwork);
                Context.Logger.Error(null, $"[{UpdateProcessName}] process FAILED! Network is not available or connectivity is low/weak... Check your connection!");

                return false;
            }

            return true;
        }

        protected bool CheckForRemoteServiceAvailability()
        {
            if (!NetworkChecker.IsRemoteServiceAvailable(Context.Settings.GetRemoteBuildsIndexUrl(), out var exception))
            {
                Data.Log(Context.LocalizedMessages.NotAvailableServers);
                Context.Logger.Error(exception, $"[{UpdateProcessName}] process FAILED! Our servers are not responding... Wait some minutes and retry!");
                
                return false;
            }

            return true;
        }
        
        protected void CheckForUpdates()
        {
            UpdateStarted();

            try
            {
                Context.Update();

                UpdateCompleted();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                UpdateFailed(ex);
            }
            finally
            {
                Data.StopTimer();
            }
        }

        protected abstract void UpdateStarted();
        
        protected abstract void UpdateCompleted();
        
        protected abstract void UpdateFailed(Exception e);
        
        protected abstract void UpdateRestartNeeded(string executableName = "");

        protected abstract void UpdateDownloadSpeed();

        protected abstract void StartApp();
        
        protected void EnsureExecutePrivileges(string filePath)
        {
            try
            {
                PrivilegesSetter.EnsureExecutePrivileges(filePath);
            }
            catch (Exception ex)
            {
                Context.Logger.Error(ex, "Unable to set executing privileges on {FilePath}.", filePath);
            }
        }
    }
}