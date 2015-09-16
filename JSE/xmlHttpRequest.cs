using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace JSE
{
    public delegate void XHREventHandler();

    public sealed class XMLHttpRequest
    {
        readonly Dictionary<string, string> headers = new Dictionary<string, string>();
        Uri uri;
        string httpMethod;
        private int _readyState;

        public int readyState
        {
            get { return _readyState; }
            private set
            {
                _readyState = value;

                onreadystatechange?.Invoke();
            }
        }

        public string responseText
        {
            get; private set;
        }

        public string responseType
        {
            get; private set;
        }

        public bool withCredentials { get; set; }

        public XHREventHandler onreadystatechange { get; set; }

        public void setRequestHeader(string key, string value)
        {
            headers[key] = value;
        }

        public string getResponseHeader(string key)
        {
            if (headers.ContainsKey(key))
            {
                return headers[key];
            }

            return null;
        }

        public void open(string method, string url)
        {
            httpMethod = method;
            uri = new Uri(url);

            readyState = 1;
        }

        public void send()
        {
            SendAsync();
        }

        async void SendAsync()
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                readyState = 2;

                switch (httpMethod)
                {
                    case "GET":
                    default:
                        using (var response = await httpClient.GetAsync(uri))
                        {
                            using (var content = response.Content)
                            {
                                responseType = "text";
                                responseText = await content.ReadAsStringAsync();
                                readyState = 4;
                            }
                        }
                        break;
                }                
            }
        }
    }
}
