Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Auth.OAuth2.Flows
Imports Google.Apis.Auth.OAuth2.Requests
Imports Google.Apis.Auth.OAuth2.Responses
Imports Google.Apis.Auth.OAuth2.Web
Imports Google.Apis.Util.Store

Namespace GoogleCalendarAPI
    Partial Public Class oauth2callback
        Inherits System.Web.UI.Page

        Private UserId As String
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If IsPostBack Then
                UserId = tbUserID.Text
            End If
        End Sub
        Private Const UploadDirectory As String = "~/Secret/"
        Protected Sub ASPxUploadControl1_FileUploadComplete(ByVal sender As Object, ByVal e As DevExpress.Web.FileUploadCompleteEventArgs)
            Dim fileName As String = ASPxUploadControl1.UploadedFiles(0).FileName
            Dim fileInfo As New FileInfo(fileName)
            Dim resFileName As String = MapPath(UploadDirectory) & fileInfo.Name

            Dim credential As UserCredential = Login(resFileName)
            If credential Is Nothing Then
                Return
            End If

            Session("credential") = credential
            Const redirecturi As String = "Default.aspx"
            e.CallbackData = redirecturi
        End Sub


        Public Function Login(ByVal fileName As String) As UserCredential

            Dim SCOPES() As String = { "https://www.googleapis.com/auth/userinfo.profile", "https://www.googleapis.com/auth/userinfo.email", "https://www.googleapis.com/auth/calendar"}

            Dim credential As UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(ASPxUploadControl1.UploadedFiles(0).FileContent).Secrets, SCOPES, UserId, CancellationToken.None, New FileDataStore(fileName, True)).Result

            Return credential
        End Function
    End Class


End Namespace