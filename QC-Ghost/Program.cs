using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace QC_Ghost
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string bethesdaNetLauncherPath = "BethesdaNetLauncher.exe"; // Relative

            if (args.Length > 0)
                bethesdaNetLauncherPath = args[0]; // Makes debuggin easier and allows users to place the exe wherever they want

            if (!File.Exists(bethesdaNetLauncherPath)) {
                Console.Error.WriteLine($"'{bethesdaNetLauncherPath}' is not a valid path!");
                Console.ReadLine();
                Environment.Exit(1);
            }

            // We need to start the launcher ourself for the Steam Overlay to work, so lets start by killing it if it's already running
            CloseBethesdaLauncher();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = bethesdaNetLauncherPath;
            startInfo.Arguments = "bethesdanet://run/11"; // 11 = Quake Champions
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            Process.Start(startInfo);

            DateTime startedAt = DateTime.Now;

            // Wait for Quake Champions to launch or just continue after 20 seconds have passed
            while (!IsQuakeChampionsRunning && (DateTime.Now - startedAt).TotalSeconds < 20)
                Thread.Sleep(1000);

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
