using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Chakra
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

                try
                {
                    onreadystatechange?.Invoke();
                }
                catch
                {
                }
            }
        }

        public string response => responseText;

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

        public void send(string data)
        {
            SendAsync(data);
        }

        async void SendAsync(string data)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var header in headers)
                {
                    if (header.Key.StartsWith("Content"))
                    {
                        continue;
                    }
                    httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                readyState = 2;

                HttpResponseMessage responseMessage = null;

                switch (httpMethod)
                {
                    case "DELETE":
                        responseMessage = await httpClient.DeleteAsync(uri);
                        break;
                    case "PATCH":
                    case "POST":
                        responseMessage = await httpClient.PostAsync(uri, new StringContent(data));
                        break;
                    case "GET":
                        responseMessage = await httpClient.GetAsync(uri);
                        break;
                }

                if (responseMessage != null)
                {
                    using (responseMessage)
                    {
                        using (var content = responseMessage.Content)
                        {
                            responseType = "text";
                            responseText = await content.ReadAsStringAsync();
                            readyState = 4;
                        }
                    }
                }
            }
        }
    }
}
