Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Calendar.v3
Imports Google.Apis.Calendar.v3.Data
Imports Google.Apis.Services

Namespace GoogleCalendarAPI
    Partial Public Class [Default]
        Inherits System.Web.UI.Page

        Private fetchInterval As TimeInterval
        Private service As CalendarService
        Private calendarId As String

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If Session("credential") Is Nothing Then
                Return
            End If

            service = New CalendarService(New BaseClientService.Initializer() With {.HttpClientInitializer = TryCast(Session("credential"), UserCredential), .ApplicationName = "GoogleCalendarAPI"})
            UpdateTimeRulers()
            If service Is Nothing Then
                Return
            End If
            If Not IsPostBack Then
                Dim calendarList As CalendarList = Me.service.CalendarList.List().Execute()
                For Each calendarItem In calendarList.Items
                    lbCalendars.Items.Add(New ListItem(calendarItem.Summary, calendarItem.Id))
                Next calendarItem
                lbCalendars.SelectedIndex = 0
            End If
            Me.calendarId = lbCalendars.SelectedValue
            ShowAndBindScheduler()
        End Sub
        Protected Sub ASPxScheduler1_FetchAppointments(ByVal sender As Object, ByVal e As DevExpress.XtraScheduler.FetchAppointmentsEventArgs)
            If Me.fetchInterval.Contains(e.Interval) OrElse e.Interval.Start = Date.MinValue Then
                Return
            End If
            SetAppointmentDataSourceSelectCommandParameters(e.Interval)
            e.ForceReloadAppointments = True
            Me.fetchInterval = e.Interval
        End Sub
        Protected Sub SetAppointmentDataSourceSelectCommandParameters(ByVal interval As TimeInterval)
            AppointmentSynchronizer1.AppointmentDataSource.SelectParameters("StartDate").DefaultValue = interval.Start.ToString()
            AppointmentSynchronizer1.AppointmentDataSource.SelectParameters("EndDate").DefaultValue = interval.End.ToString()

        End Sub
        Private Sub ShowAndBindScheduler()
            ASPxRoundPanel1.Visible = True
            ASPxScheduler1.Visible = True
            AppointmentSynchronizer1.CalendarId = Me.calendarId
            AppointmentSynchronizer1.SetGoogleRelatedData(Me.service)
            BindSchedulerToObjectDataSource()
        End Sub
        Private Sub BindSchedulerToObjectDataSource()
            DataHelper.SetupDefaultMappings(ASPxScheduler1)
            Me.fetchInterval = ASPxScheduler1.ActiveView.GetVisibleIntervals().Interval
            SetAppointmentDataSourceSelectCommandParameters(Me.fetchInterval)
            AppointmentSynchronizer1.AttachTo(ASPxScheduler1)
            AddHandler ASPxScheduler1.FetchAppointments, AddressOf ASPxScheduler1_FetchAppointments
        End Sub
        Protected Sub ASPxScheduler1_AfterExecuteCallbackCommand(ByVal sender As Object, ByVal e As SchedulerCallbackCommandEventArgs)
            If e.CommandId = SchedulerCallbackCommandId.ChangeTimeZone Then
                UpdateTimeRulers()
            End If
        End Sub
        Private Sub UpdateTimeRulers()
            ASPxScheduler1.BeginUpdate()
            Try
                UpdateClientTimeRuler(ASPxScheduler1.DayView)
                UpdateClientTimeRuler(ASPxScheduler1.WorkWeekView)
            Finally
                ASPxScheduler1.EndUpdate()
            End Try
        End Sub
        Private Sub UpdateClientTimeRuler(ByVal view As DevExpress.Web.ASPxScheduler.DayView)
            Dim ruler As TimeRuler = view.TimeRulers(0)
            Dim tzId As String = ASPxScheduler1.OptionsBehavior.ClientTimeZoneId
            ruler.TimeZoneId = tzId
            ruler.Caption = String.Format("Client: {0}", tzId)
        End Sub
    End Class
End Namespace