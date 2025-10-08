using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlockchainApp
{
    public class ExecuteProcess
    {
        private List<string> outputLines = null;
        private Process p = null;

        public string Command { get; set; }
        public string Arguments { get; set; }
        public int ProcessID { get; set; }

        public ExecuteProcess()
        {

        }

        public void Start()
        {
            if (p != null)
            {
                return;
            }

            p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = Command;
            p.StartInfo.Arguments = Arguments;

            outputLines = new List<string>();

            p.OutputDataReceived += new DataReceivedEventHandler(this.OutputHandler);
            p.EnableRaisingEvents = true;
            p.Exited += new EventHandler(this.ExitedHandler);

            p.Start();

            p.BeginOutputReadLine();

            ProcessID = p.Id;
        }

        public void OutputHandler(object sendingProcess, DataReceivedEventArgs outputLine)
        {
            if (!string.IsNullOrEmpty(outputLine.Data))
            {
                outputLines.Add(outputLine.Data);
            }
        }

        public void ExitedHandler(object sender, System.EventArgs e)
        {
            
        }

        public string GetOutputData(int start = 0, int length = -1)
        {
            if (outputLines == null || outputLines.Count <= 0)
            {
                return "";
            }

            if (length <= 0)
            {
                length = outputLines.Count;
            }

            StringBuilder output = new StringBuilder(length * 512);

            foreach (string s in outputLines)
            {
                output.AppendLine(s);
            }

            return output.ToString();
        }

        public bool HasExited()
        {
            if (p == null)
            {
                return true;
            }

            return p.HasExited;
        }

        public void Kill()
        {
            if (p != null && !p.HasExited)
            {
                p.Kill();
            }
        }
    }
}