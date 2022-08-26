using System.Diagnostics;

namespace HAL {
    public partial class HALMain {

        // ============
        // === Firewall

        private bool Firewall_CheckConfig(bool verbose = true) {
            bool resultFW = false;

            if (verbose) AddLog("CHECK // Firewall Port", PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh advfirewall firewall show rule name=\"Home Assistant Listener\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                if (line.Contains("Home Assistant Listener")) resultFW = true;
            }

            if (resultFW) {
                if (verbose) AddLog("Firewall Port " + PORT + " Configuration: Correct", PREFIXES.MIDDLE_LOG);
                btnConfigure_Firewall.Content = "Remove Firewall rule";
            } else {
                btnConfigure_Firewall.Content = "Add Firewall rule";
                if (verbose) AddLog("Firewall Port " + PORT + " Configuration: NOT FOUND", PREFIXES.MIDDLE_LOG);
            }

            if (verbose) AddLog("", PREFIXES.END_LOG);

            return resultFW;
        }

        private void Firewall_ADD() {

            AddLog("   === ADD // Firewall port rule on port " + PORT, PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh advfirewall firewall add rule name=\"Home Assistant Listener\" dir=in action=allow protocol=TCP localport=" + PORT,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                AddLog(line, PREFIXES.MIDDLE_LOG);
            }

            AddLog("", PREFIXES.END_LOG);
        }

        private void Firewall_DELETE() {

            AddLog("   === DELETE // Firewall port rule on port " + PORT, PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh advfirewall firewall delete rule name=\"Home Assistant Listener\"", // protocol=TCP localport=" + PORT,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                AddLog(line, PREFIXES.MIDDLE_LOG);
            }

            AddLog("", PREFIXES.END_LOG);
        }


        // ====================
        // === ACL Reserved URL

        private bool ACLReservedUrl_CheckConfig(bool verbose = true) {
            bool resultACL = false;

            if (verbose) AddLog("CHECK // ACL Reserved URL", PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh http show urlacl url=http://*:" + PORT + "/HAL/",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                if (line.Contains("http://*:" + PORT + "/HAL/")) resultACL = true;
            }

            if (resultACL) {
                if (verbose) AddLog("Reserved URL " + URL + " configuration: Correct", PREFIXES.MIDDLE_LOG);
                btnConfigure_ACL.Content = "Remove ACL reserved URL";
            } else {
                if (verbose) AddLog("Reserved URL " + URL + " configuration: NOT FOUND", PREFIXES.MIDDLE_LOG);
                btnConfigure_ACL.Content = "Add ACL reserved URL";
            }

            if (verbose) AddLog("", PREFIXES.END_LOG);

            return resultACL;
        }

        private void ACLReservedUrl_ADD() {

            AddLog("   === ADD // Reserved URL http://*:" + PORT + "/HAL/", PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh http add urlacl url=http://*:" + PORT + "/HAL/ user=Administrator",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                AddLog(line, PREFIXES.MIDDLE_LOG);
            }

            AddLog("", PREFIXES.END_LOG);
        }

        private void ACLReservedUrl_DELETE() {
            AddLog("DELETE // Reserved URL http://*:" + PORT + "/HAL/", PREFIXES.START_LOG);

            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = "cmd.exe",
                    Arguments = "/C netsh http delete urlacl url=http://*:" + PORT + "/HAL/",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            while (!proc.StandardOutput.EndOfStream) {
                string line = proc.StandardOutput.ReadLine();
                AddLog(line, PREFIXES.MIDDLE_LOG);
            }

            AddLog("", PREFIXES.END_LOG);
        }



    }


}
