using System;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace HAL {
    public partial class HALMain {


        private void ProcessData(string url) {

            AddLog(url, PREFIXES.START_LOG);

            string key = url.Substring(url.LastIndexOf("/") + 1);
            DataRow dr = ds.Tables[0].Select("key = '" + key + "'").DefaultIfEmpty(null).FirstOrDefault();

            if (dr != null) {
                AddLog("Found action: " + dr["action"].ToString() + " " + dr["parameters"].ToString(), PREFIXES.MIDDLE_LOG);

                ExecuteProcess(dr["action"].ToString(), dr["parameters"].ToString());

            } else {
                AddLog("Action not Found ", PREFIXES.MIDDLE_LOG);
            }



            AddLog("", PREFIXES.END_LOG);
        }


        private void ExecuteProcess(string action, string parameters) {

            try {
                var proc = new Process {
                    StartInfo = new ProcessStartInfo {
                        FileName = action + " ",
                        Arguments = parameters,
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
            } catch (Exception e) {
                AddLog("Errore: " + e.Message, PREFIXES.MIDDLE_LOG);
            }

        }


    }


}
