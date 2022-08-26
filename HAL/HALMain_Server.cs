using System;

namespace HAL {
    public partial class HALMain {

        HttpServer httpServer;

        private void InitServer() {
            httpServer = new HttpServer(PORT);
        }

        private void StartStopListener() {
            if (httpServer.State()) {
                httpServer.Stop();
                httpServer.onRequest -= onRequest;
                btnStartListener.Content = "Start server";
                AddLog("Server stopped...", PREFIXES.START_LOG);
            } else {
                httpServer.Start();
                httpServer.onRequest += onRequest;
                btnStartListener.Content = "Stop server";
                AddLog("Server started...", PREFIXES.START_LOG);
            }

        }

        private void onRequest(Object o, EventArgs e) {
            Dispatcher.BeginInvoke(new Action(() => {
                string url = ((EventArgs_Response)e).response_text;
                ProcessData(url);
            }));
        }


    }


}
