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
        private WebResponseModel responseModel;
        string responseBody = String.Empty;
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
                isSuccess = false;
            }
            finally
            {
                //TODO: WHEN API IS NOT REACHEABLE, THEN IS THE OBJ NULL
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    responseBody = reader.ReadToEnd();
                responseModel = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);
            }
        }

        public HttpStatusCode GetStatusCode()
        {
            if (response is HttpWebResponse httpWebResponse)
            {
                return httpWebResponse.StatusCode;
            }

            return HttpStatusCode.Unauthorized;
        }

        public string GetResponseBody()
        {
            return responseBody;
        }

        public string GetMessageFromResponse()
        {
            //if the response model is from my api
            if (responseModel.message is not null)
                return responseModel.message;

            //if the response model is from asp, then take all the errors and return them
            AspWebResponseModel aspResponse = JsonConvert.DeserializeObject<AspWebResponseModel>(responseBody);
            string message = "Error(s): ";
            foreach (var error in aspResponse.errors)
                message += $" {error.Value}";
            message = message
                    .Replace("[", String.Empty)
                    .Replace("]", String.Empty)
                    .Replace("\r\n", String.Empty)
                    .Replace(" \"", "\r\n\"");
            return message;
        }

        public bool IsSuccess()
        {
            return isSuccess;
        }
    }
}
