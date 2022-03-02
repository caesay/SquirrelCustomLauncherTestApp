using Squirrel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LTestAppWpf
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SquirrelAwareApp.HandleEvents(Installed);
        }

        void Installed(SemanticVersion ver, IAppTools tools)
        {
            // install shortcuts only if not portable?
        }
    }
}
