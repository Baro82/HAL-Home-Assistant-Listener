using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Principal;
using System.Windows;

namespace HAL {

    public partial class HALMain : Window {

        static private int PORT = 8080;
        private string URL = "http://[YOUR_IP]:" + PORT + "/HAL/";


        public HALMain() {
            InitializeComponent();

            CheckRunAsAdmin();

            URL = URL.Replace("[YOUR_IP]", GetLocalIPAddress());
            InitServer();
            ReadDatabase();
            WindowState = WindowState.Minimized;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            
            if (Firewall_CheckConfig() && ACLReservedUrl_CheckConfig()) {
                StartStopListener();
                var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            } else {

                WindowState = WindowState.Normal;
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
                InitTrayIcon(false);
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            (sender as System.Windows.Threading.DispatcherTimer).Stop();
            InitTrayIcon(true);
        }



        // =============
        // === UI EVENTS

        private void btnStartListener_Click(object sender, RoutedEventArgs e) {
            StartStopListener();
        }

        private void btnConfigure_ACL_Click(object sender, RoutedEventArgs e) {
            if (ACLReservedUrl_CheckConfig(false)) {
                ACLReservedUrl_DELETE();
            } else {
                ACLReservedUrl_ADD();
            }
            ACLReservedUrl_CheckConfig();
        }

        private void btnConfigure_Firewall_Click(object sender, RoutedEventArgs e) {
            if (Firewall_CheckConfig(false)) {
                Firewall_DELETE();
            } else {
                Firewall_ADD();
            }
            Firewall_CheckConfig();
        }

        private void dg_RowEditEnding(object sender, System.Windows.Controls.DataGridRowEditEndingEventArgs e) {
            WriteDatabase();
        }



        // ===============
        // === LOG MANAGER

        private enum PREFIXES {
            NOTHING_LOG,
            START_LOG,
            MIDDLE_LOG,
            END_LOG,
        }

        private void AddLog(string log, PREFIXES pre = PREFIXES.NOTHING_LOG) {
            if (pre == PREFIXES.START_LOG) log = "=== " + log;
            if (pre == PREFIXES.MIDDLE_LOG) log = "    " + log;
            if (pre == PREFIXES.END_LOG) {
                log = "====================" + Environment.NewLine;
            }
            if (log.Trim().Length == 0) return;

            txtLog.Text += Environment.NewLine + DateTime.Now.ToString("t") + ": " + log;
            txtLog.ScrollToEnd();
        }



        // ===================
        // === OTHER FUNCTIONS

        private string GetLocalIPAddress() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork) {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }



        private void CheckRunAsAdmin() {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin) {
                // It is not possible to launch a ClickOnce app as administrator directly,
                // so instead we launch the app as administrator in a new process.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // The following properties run the new process as administrator
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Start the new process
                try {
                    Process.Start(processInfo);
                } catch (Exception) {
                    // The user did not allow the application to run as administrator
                    MessageBox.Show("Sorry, but I don't seem to be able to start " +
                       "this program with administrator rights!");
                }

                // Shut down the current process
                System.Windows.Application.Current.Shutdown();
            } /*else {
                // We are running as administrator
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }*/
        }
    }


}
