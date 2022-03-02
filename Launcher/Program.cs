
using Squirrel;
using System.Diagnostics;

namespace Launcher;

class Program
{
    static void Main()
    {
        var wnd = new UpdateWnd();
        string updatePath = "https://github.com/caesay/SquirrelCustomLauncherTestApp";

        var portableDirName = "portable";
        var portablePath = Path.Combine(AppContext.BaseDirectory, portableDirName);
        var updateExe = Path.Combine(portablePath, "Update.exe");

        if (File.Exists(updateExe))
        {
            Process.Start(updateExe, "--processStart LTestAppWpf.exe");
            return;
        }

        wnd.InstallAppData += (s, e) =>
        {
            Task.Run(() =>
            {
                try
                {
                    using var updateMgr = new GithubUpdateManager(updatePath);
                    updateMgr.KillAllExecutablesBelongingToPackage();
                    updateMgr.FullInstall(progress: (p) => wnd.DisplayText = $"Installing {p}%").GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    wnd.DisplayText = ex.ToString();
                }
            });
        };

        wnd.InstallPortable += (s, e) =>
        {
            Task.Run(() =>
            {
                try
                {
                    // we want squirrel to install to the current folder, so it's a little hacky
                    using var updateMgr = new GithubUpdateManager(
                        updatePath, applicationIdOverride: portableDirName, localAppDataDirectoryOverride: AppContext.BaseDirectory);
                    updateMgr.KillAllExecutablesBelongingToPackage();
                    if (Directory.Exists(portablePath)) Directory.Delete(portablePath, true);
                    updateMgr.FullInstall(progress: (p) => wnd.DisplayText = $"Installing {p}%").GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    wnd.DisplayText = ex.ToString();
                }
            });
        };

        wnd.Run();
    }
}