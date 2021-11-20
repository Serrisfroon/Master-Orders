using System;
using System.IO;
using System.Windows.Forms;

namespace YoutubeLibrary
{
    public class VodMonitor
    {
        private FileSystemWatcher vodDirectoryMonitor = new FileSystemWatcher();
        private string uploadButtonText = "Upload to YouTube";
        public string directoryToWatch
        {
            get { return vodDirectoryMonitor.Path; }
            set { vodDirectoryMonitor.Path = value; }
        }

        private bool _enableFunctions;
        public bool enableFunctions 
        {
            get
            {
                return _enableFunctions;
            }
            set
            {
                if(value == false)
                {
                    targetControl.Text = uploadButtonText;
                    targetControl.Enabled = true;
                }
                _enableFunctions = value;
            }
        }
        public string newFileName { get; set; }
        public bool enableUpload { get; set; }
        public Control targetControl { get; set; }
        public string streamSoftware { get; set; }
        public VodMonitor(string toDirectoryToWatch, Control toTargetControl, bool toEnableFuctions)
        {
            directoryToWatch = toDirectoryToWatch;
            targetControl = toTargetControl;
            enableFunctions = toEnableFuctions;

            InitializeMonitor();
        }

        public void Disable()
        {
            vodDirectoryMonitor.Dispose();
        }

        private void InitializeMonitor()
        {
            vodDirectoryMonitor.EnableRaisingEvents = true;                       //Enable to monitor to trigger these events
            vodDirectoryMonitor.Created += FileSystemWatcher_Created;             //Associate the file creation event to the monitor
            vodDirectoryMonitor.Deleted += FileSystemWatcher_Deleted;             //Associate the file deletion event to the monitor
            vodDirectoryMonitor.Renamed += FileSystemWatcher_Renamed;             //Associate the file deletion event to the monitor
        }

        public string GetDetectedVod()
        {
            if(newFileName == null)
            {
                return null;
            }
            return directoryToWatch + @"\" + newFileName;
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (enableFunctions == true)
            {
                if (Path.GetExtension(e.Name) == @".mp4")
                {

                    newFileName = e.Name;
                    try
                    {
                        targetControl.BeginInvoke((Action)delegate ()
                        {
                            if (streamSoftware == "XSplit")
                            {
                                enableUpload = false;
                                targetControl.Text = "Recording in Progress";
                                targetControl.Enabled = false;
                            }
                        });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            /*
            if (enableYoutubeFunctions == true)
            {
                if (e.OldName == newFileName)
                {
                    enableUpload = true;
                    targetControl.BeginInvoke((Action)delegate ()
                    {
                        targetControl.Text = "Upload to YouTube";
                        targetControl.Enabled = true;
                    });
                }
            }
            */
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (enableFunctions == true)
            {
                if (e.OldName == newFileName)
                {
                    newFileName = e.Name;
                    enableUpload = true;
                    try
                    {
                        targetControl.Invoke((Action)delegate ()
                        {
                            if (streamSoftware == "XSplit")
                            {
                                targetControl.Text = uploadButtonText;
                                targetControl.Enabled = true;
                            }
                        });
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }
    }
}
