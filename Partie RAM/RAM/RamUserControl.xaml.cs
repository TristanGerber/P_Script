using Microsoft.TeamFoundation.Common.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RAM
{
    /// <summary>
    /// Logique d'interaction pour RamUserControl.xaml
    /// </summary>
    public partial class RamUserControl : UserControl
    {
        private object RAMUsage = 0;
        private int RAMUSageValueCount = 0;
        public RamUserControl()
        {
            InitializeComponent();
            GetData();

            Timer ramUsageUpdater = new Timer(1000);
            ramUsageUpdater.Elapsed += RamUsageUpdaterElapsed;
            ramUsageUpdater.Start();
        }

        private object GetData()
        {
            UpdateRamUsage();
            return null;
        }

        private void UpdateRamUsage()
        {
            PowerShell script = PowerShell.Create();
            script.AddScript(@"$os = Get-Ciminstance Win32_OperatingSystem;
                               $pctFree = 100 - [math]::Round(($os.FreePhysicalMemory/$os.TotalVisibleMemorySize)*100,2);
                               return $pctFree -as [int]");
            Collection<PSObject> results = script.Invoke();

            Graph.Points.Add(new Point(RAMUSageValueCount * 10, Convert.ToInt32(results[0].BaseObject)));
            RAMUSageValueCount++;
        }

        private void RamUsageUpdaterElapsed(object o, ElapsedEventArgs e)
        {
            UpdateRamUsage();
        }
    }
}
