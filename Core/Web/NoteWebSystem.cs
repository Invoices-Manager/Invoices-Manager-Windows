using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesManager.Core.Web
{
    public class NoteWebSystem
    {
        public static string GetAll()
        {
            //get all notes (list of notes)
            //pack that notes into a notebook
            //convert back into json
            //return json

            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_NOTE_GETALL);
          
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

            NotebookModel notebook = new NotebookModel();
            notebook.Notebook.AddRange(JsonConvert.DeserializeObject<List<NoteModel>>(response.Args["notes"].ToString()));

            return JsonConvert.SerializeObject(notebook);
        }
        
        public static int Add(NoteModel newNote)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_NOTE);

            _wr.SetRequestMethod("POST");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });
            _wr.SetBody(JsonConvert.SerializeObject(newNote));

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            return JsonConvert.DeserializeObject<NoteModel>(response.Args["note"].ToString()).Id;
        }

        public static bool Edit(NoteModel newNote)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_NOTE);

            _wr.SetRequestMethod("PUT");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "bearerToken", EnvironmentsVariable.BearerToken },
                { "Content-Type", "application/json" }
            });
            _wr.SetBody(JsonConvert.SerializeObject(newNote));

            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                throw new Exception("Error: " + statusCode + " " + responseBody);
            }

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);
            
            return _wr.IsSuccess();
        }

        public static bool Delete(int id)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_NOTE + $"?Id={id}");

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

            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            return _wr.IsSuccess();
        }
    }
}
