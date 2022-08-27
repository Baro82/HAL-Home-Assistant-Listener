using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindowsInput.Events;

namespace HAL {
    public partial class HALMain {


        private void ProcessData(string url) {

            AddLog(url, PREFIXES.START_LOG);

            string key = url.Substring(url.LastIndexOf("/") + 1);
            DataRow dr = ds.Tables[0].Select("key = '" + key + "'").DefaultIfEmpty(null).FirstOrDefault();

            if (dr != null) {

                string action = dr["action"].ToString();
                string parameters = dr["parameters"].ToString();

                AddLog("Found action: " + action + " " + parameters, PREFIXES.MIDDLE_LOG);


                if (action.ToLower() == "keyboard") {
                    ExecuteWindowsKeys(parameters);
                } else {
                    ExecuteProcess(action, parameters);
                }

                

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

        
        private async Task ExecuteWindowsKeys(string parameters) {

            // https://github.com/MediatedCommunications/WindowsInput

            try {
                List<string> lst = parameters.Split('-').ToList();
                for (int i = 0; i < lst.Count; i++) {

                    string cmd = lst[i];

                    if (cmd.EndsWith("ms")) {
                        await WindowsInput.Simulate.Events().Wait(Convert.ToInt32(cmd.Replace("ms", "").Trim())).Invoke();
                    } else {
                        List<string> keys = cmd.Split('+').ToList();
                        KeyCode[] keysEnum = new KeyCode[keys.Count];

                        int j = 0;
                        foreach (string k in keys) {
                            KeyCode code;
                            Enum.TryParse(k, true, out code);
                            keysEnum[j] = code;
                            j++;
                        }

                        await WindowsInput.Simulate.Events().ClickChord(keysEnum).Invoke();

                    }
                }
            } catch (Exception e) {
                AddLog("Windows key error: " + e.Message.ToString());
            }


        }


    }


}
