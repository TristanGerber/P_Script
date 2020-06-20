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
using System.Windows.Threading;

namespace RAM
{
    /// <summary>
    /// Logique d'interaction pour RamUserControl.xaml
    /// </summary>
    public partial class RamUserControl : UserControl
    {
        private double RAMFree = 0;
        private int RAMUSageValueCount = 0;
        private byte scaleSize = 10;
        private byte maxValue = 9;

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
                               $pctFree = [math]::Round(($os.FreePhysicalMemory/$os.TotalVisibleMemorySize)*100,4);
                               return $pctFree -as [double]");
            Collection<PSObject> results = script.Invoke();

            RAMFree = Convert.ToDouble(results[0].BaseObject);

            if (RAMUSageValueCount <= maxValue)
            {
                Graph.Points.Add(new Point(RAMUSageValueCount * scaleSize, RAMFree));
                RAMUSageValueCount++;
            }
            else
            {
                for (int i = 0; i < maxValue; i++)
                    Graph.Points[i] = new Point(i * scaleSize, Graph.Points[i + 1].Y);

                Graph.Points.RemoveAt(maxValue);
                Graph.Points.Add(new Point(maxValue * scaleSize, RAMFree));
            }

            this.lblPercentil.Dispatcher.Invoke(DispatcherPriority.Normal,
                new Action(() => { lblPercentil.Content = (100 - RAMFree).ToString().Substring(0,4) + "%"; }));
        }

        private void RamUsageUpdaterElapsed(object o, ElapsedEventArgs e)
        {
            UpdateRamUsage();
        }
    }
}
