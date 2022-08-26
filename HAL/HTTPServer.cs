using System;
using System.Net;

namespace HAL {

    public class HttpServer {
        public event EventHandler onRequest;

        private HttpListener _listener;
        private int Port = 8080;


        public HttpServer(int _port) {
            Port = _port;
        }

        public void Start() {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + Port.ToString() + "/HAL/");
            _listener.Start();
            Receive();
        }

        public void Stop() {
            _listener.Stop();
        }

        public bool State() {
            return _listener != null && _listener.IsListening;
        }

        private void Receive() {
            _listener.BeginGetContext(new AsyncCallback(ListenerCallback), _listener);
        }

        private void ListenerCallback(IAsyncResult result) {
            if (_listener.IsListening) {
                var context = _listener.EndGetContext(result);
                var request = context.Request;

                EventHandler thisOnRequest = onRequest;
                if (thisOnRequest != null) thisOnRequest(this, new EventArgs_Response(request.Url.ToString()));

                var response = context.Response;
                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = "text/plain";
                response.OutputStream.Write(new byte[] { }, 0, 0);
                response.OutputStream.Close();



                Receive();
            }
        }
    }

    public class EventArgs_Response : EventArgs {
        public string response_text;
        public EventArgs_Response(string _response_text) {
            response_text = _response_text;
        }
    }

}