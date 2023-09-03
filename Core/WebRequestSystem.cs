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

            try
            {
#pragma warning disable SYSLIB0014 // Type or element is obsolete
                request = WebRequest.Create(endpoint);
#pragma warning restore SYSLIB0014 // Type or element is obsolete
                request.Method = method;
            }
            catch (UriFormatException ex)
            {
                MessageBox.Show(ex.ToString());
               throw new UriFormatException("The endpoint is not valid. Please check the endpoint in the config file.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new Exception(ex.Message);
            }
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
            Exception _ex = default;
            try
            {
                //This bypass the SSL certificate validation
                //Only for testing purposes
#if DEBUG
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
#endif
                response = request.GetResponse();
                isSuccess = true;
            }
            catch (WebException ex)
            {
                isSuccess = false;
                response = ex.Response;
                if (ex.Message.Contains("The SSL connection"))
                {
                    MessageBox.Show(ex.ToString());
                    _ex = ex;
                    return;
                }
            }
            //TODO: WHEN API IS NOT REACHEABLE, THEN IS THE OBJ NULL
            if (_ex is not null)
                if (_ex.Message.Contains("The SSL connection"))
                {
                    responseBody = "[]";
                    return;
                }

            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))

                responseBody = reader.ReadToEnd();
                responseModel = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);
            }
            catch { }
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
            if (responseModel is not null)
                if (responseModel.message is not null)
                    return responseModel.message;

            //check if 401
            if (GetStatusCode() is HttpStatusCode.Unauthorized)
                return "Error: 401 (Unauthorized)";
            //check if 413 (Payload Too Large)
            if (GetStatusCode() is HttpStatusCode.RequestEntityTooLarge)
                return "Error: 413 (Payload Too Large)";


            //if the response model is from asp, then take all the errors and return them
            try
            {
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
            catch (Exception)
            {
                LoggerSystem.Log(LogStateEnum.Fatal, LogPrefixEnum.WebRequest_System, responseBody);
                return "FATAL: " + responseBody;
            }

        }

        public bool IsSuccess()
        {
            return isSuccess;
        }
    }
}
