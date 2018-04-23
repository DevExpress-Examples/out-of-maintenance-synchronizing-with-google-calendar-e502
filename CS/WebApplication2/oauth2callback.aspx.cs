using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Util.Store;

namespace GoogleCalendarAPI {
    public partial class oauth2callback : System.Web.UI.Page {
        string UserId;
        protected void Page_Load(object sender, EventArgs e) {
            if(IsPostBack)
                UserId = tbUserID.Text;
        }
        const string UploadDirectory = "~/Secret/";
        protected void ASPxUploadControl1_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e) {
            string fileName = ASPxUploadControl1.UploadedFiles[0].FileName;
            FileInfo fileInfo = new FileInfo(fileName);
            string resFileName = MapPath(UploadDirectory) + fileInfo.Name;

            UserCredential credential = Login(resFileName);
            if(credential == null)
                return;

            Session["credential"] = credential;
            const string redirecturi = @"Default.aspx";
            e.CallbackData = redirecturi;
        }


        public UserCredential Login(string fileName) {

            String[] SCOPES = new String[] {
               "https://www.googleapis.com/auth/userinfo.profile",
               "https://www.googleapis.com/auth/userinfo.email",
               "https://www.googleapis.com/auth/calendar",
            };

            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
              GoogleClientSecrets.Load(ASPxUploadControl1.UploadedFiles[0].FileContent).Secrets,
              SCOPES,
              UserId,
              CancellationToken.None,
              new FileDataStore(fileName, true)).Result;

            return credential;
        }
    }
       
    
}