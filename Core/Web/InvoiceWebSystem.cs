using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesManager.Core.Web
{
    public class InvoiceWebSystem
    {
        public static string GetAll()
        {
            //get all invoices (list of invoices)
            //convert back into json
            //return json

            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_INVOICE_GETALL);

            _wr.SetRequestMethod("GET");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            List<InvoiceModel> invoices = JsonConvert.DeserializeObject<List<InvoiceModel>>(response.Args["invoices"].ToString());

            return JsonConvert.SerializeObject(invoices);
        }
        
        public static string GetFile(int id)
        {
            //get file (in base64)
            //return base64

            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_INVOICE_GETFILE + $"?id={id}");

            _wr.SetRequestMethod("GET");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            return response.Args["base64"].ToString();
        }
        
        public static int Add(InvoiceModel newInvoice, string base64)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_INVOICE);

            _wr.SetRequestMethod("POST");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });

            var requestBody = new
            {
                NewInvoice = newInvoice,
                InvoiceFileBase64 = base64
            };

            _wr.SetBody(JsonConvert.SerializeObject(requestBody));

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            return JsonConvert.DeserializeObject<NoteModel>(response.Args["invoice"].ToString()).Id;
        }

        public static bool Edit(InvoiceModel newInvoice)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_INVOICE);

            _wr.SetRequestMethod("PUT");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });

            _wr.SetBody(JsonConvert.SerializeObject(newInvoice));

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            return isSuccess;
        }

        public static bool Remove(int id)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_INVOICE + $"?id={id}");

            _wr.SetRequestMethod("DELETE");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            return isSuccess;
        }
    }
}
