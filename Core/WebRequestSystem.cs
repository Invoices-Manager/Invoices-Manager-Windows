using System;
using System.IO;
using System.Net;

namespace InvoicesManager.Core
{
    public class WebRequestSystem
    {
        private readonly string endpoint;
        private WebRequest request;
        private WebResponse response;
        private bool isSuccess;

        public WebRequestSystem(string endpoint)
        {
            this.endpoint = endpoint;
            isSuccess = false;
        }

        public void SetRequestMethod(string method)
        {
#pragma warning disable SYSLIB0014 // Type or element is obsolete
            request = WebRequest.Create(endpoint);
#pragma warning restore SYSLIB0014 // Type or element is obsolete
            request.Method = method;
        }

        public void SetHeaders(WebHeaderCollection headers)
        {
            request.Headers = headers;
        }

        public void SetBody(string body)
        {
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(body);
            request.ContentType = "application/json";
            request.ContentLength = byteData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(byteData, 0, byteData.Length);
            }
        }

        public void SendRequest()
        {
            try
            {
                response = request.GetResponse();
                isSuccess = true;
            }
            catch (WebException ex)
            {
                response = ex.Response;
            }
        }

        public HttpStatusCode GetStatusCode()
        {
            if (response is HttpWebResponse httpWebResponse)
            {
                return httpWebResponse.StatusCode;
            }

            throw new InvalidOperationException("The response is not an HTTP response.");
        }

        public string GetResponseBody()
        {
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

        public bool IsSuccess()
        {
            return isSuccess;
        }
    }
}
