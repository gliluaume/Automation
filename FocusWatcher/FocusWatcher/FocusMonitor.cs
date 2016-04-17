using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using Microsoft.Win32;

namespace FocusWatcher
{
    public class FocusMonitor
    {
        public int LongDurationIgnore { get; set; }
        private ProcessDesc _previousProcess;
        private ProcessDesc _currentProcess;
        public FocusMonitor()
        {
            Logger.WriteHeader();
            AutomationFocusChangedEventHandler focusHandler = OnFocusChanged;
            Automation.AddAutomationFocusChangedEventHandler(focusHandler);
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(OnSessionSwitched);
        }
        private void OnSessionSwitched(object sender, SessionSwitchEventArgs e)
        {
            DateTime now = DateTime.Now;
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    this._currentProcess = ProcessDesc.LockedSession;
                    this._currentProcess.StartTime = DateTime.Now;
                    break;
                case SessionSwitchReason.SessionUnlock:
                    this._currentProcess.EndTime = DateTime.Now;
                    break;
            }
        }
        private void OnFocusChanged(object sender, AutomationFocusChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            AutomationElement focusedElement = sender as AutomationElement;
            if (focusedElement != null)
            {
                
                if (null != this._currentProcess)
                {
                    this._previousProcess = this._currentProcess;
                    this._previousProcess.EndTime = now;
                }

                int processId = focusedElement.Current.ProcessId;
                using (Process process = Process.GetProcessById(processId))
                {
                    this._currentProcess = new ProcessDesc() { Name = process.ProcessName, Title = process.MainWindowTitle, StartTime = now };
                    if (null != this._previousProcess)
                    {
                        LogPreviousProcess();
                    }
                }
            }
        }

        private void LogPreviousProcess()
        {
            int duration = (int)(Math.Floor(_previousProcess.Duration.TotalSeconds));
            if (LongDurationIgnore > duration)
            {
                Logger.Log("{0};{1};{2};{3}", _previousProcess.StartTime, duration, _previousProcess.Name, _previousProcess.Title);
            }
        }
    }
}
