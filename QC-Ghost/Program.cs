using System;
using System.Diagnostics;
using System.Threading;

namespace QC_Ghost
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "BethesdaNetLauncher.exe";
            startInfo.Arguments = "bethesdanet://run/11"; // 11 = Quake Champions
            Process.Start(startInfo);

            DateTime startedAt = DateTime.Now;

            // Wait for Quake Champions to launch or just continue after 20 seconds have passed
            while (!IsQuakeChampionsRunning && (DateTime.Now - startedAt).TotalSeconds < 20)
                Thread.Sleep(1000);

            // Wait for Quake Champions to exit, or just skip this step if it never started
            while (IsQuakeChampionsRunning)
                Thread.Sleep(1000);

            // And then close the Bethesda Launcher
            CloseBethesdaLauncher();
        }

        private static void CloseBethesdaLauncher()
        {
            // Close all instances of the bethesda launcher
            foreach (Process process in Process.GetProcessesByName("BethesdaNetLauncher"))
                process.Kill(); // with fire
        }

        private static bool IsQuakeChampionsRunning => Process.GetProcessesByName("QuakeChampions").Length > 0;
    }
}
