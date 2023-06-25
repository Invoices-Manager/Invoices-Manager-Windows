using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InvoicesManager.Core
{
    public class UserWebSystem
    {
        public bool Login(string username, string password)
        {
            //check if the data is vaild
            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
                return false;

            //GetBearerToken
            EnvironmentsVariable.BearerToken = GetBearerToken(username, password);

            //if the login was not sucessful, then return a false
            if (String.IsNullOrEmpty(EnvironmentsVariable.BearerToken)) 
                return false;

            //if the login was sucessful, then enable the main window buttons
            EnvironmentsVariable.MainWindowInstance.UI_Login();
            EnvironmentsVariable.MainWindowInstance.Bttn_Open_Login.Visibility = Visibility.Collapsed;
            EnvironmentsVariable.MainWindowInstance.Bttn_Open_Logout.Visibility = Visibility.Visible;
            return true;
        }

        public bool Logout()
        {
            if (!String.IsNullOrEmpty(EnvironmentsVariable.BearerToken))
                if (!LogoutFromApi())
                    return false;

            EnvironmentsVariable.ClearBearerToken();
            EnvironmentsVariable.MainWindowInstance.UI_Logout();
            EnvironmentsVariable.MainWindowInstance.Bttn_Open_Login.Visibility = Visibility.Visible;
            EnvironmentsVariable.MainWindowInstance.Bttn_Open_Logout.Visibility = Visibility.Collapsed;
            

            return true;
        }

        public bool Create(dynamic userData)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_USER);

            _wr.SetRequestMethod("POST");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "Content-Type", "application/json" }
            });
            _wr.SetBody(JsonConvert.SerializeObject(userData));
            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.User_System, "Error: " + statusCode + " " + responseBody);
                MessageBox.Show(_wr.GetMessageFromResponse(), "Error", (MessageBoxButton)MessageBoxButtons.OK, (MessageBoxImage)MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool LogoutFromApi()
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_USER_LOGOUT);

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
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.User_System, "Error: " + statusCode + " " + responseBody);
                MessageBox.Show(_wr.GetMessageFromResponse());
                return false;
            }

            return isSuccess;
        }

        private string GetBearerToken(string username, string password)
        {
            WebRequestSystem _wr = new WebRequestSystem(EnvironmentsVariable.API_ENDPOINT_USER_LOGIN);

            _wr.SetRequestMethod("POST");
            _wr.SetHeaders(new WebHeaderCollection()
            {
                { "Content-Type", "application/json" }
            });
            _wr.SetBody(JsonConvert.SerializeObject(new { username, password }));
            _wr.SendRequest();

            HttpStatusCode statusCode = _wr.GetStatusCode();
            string responseBody = _wr.GetResponseBody();
            bool isSuccess = _wr.IsSuccess();

            if (!isSuccess)
            {
                LoggerSystem.Log(LogStateEnum.Error, LogPrefixEnum.User_System, "Error: " + statusCode + " " + responseBody);
                MessageBox.Show(_wr.GetMessageFromResponse());
                return String.Empty;
            }
            
            WebResponseModel response = JsonConvert.DeserializeObject<WebResponseModel>(responseBody);

            return JsonConvert.SerializeObject(response.Args["token"].ToString()).Trim('"');
        }
    }
}
