using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;

namespace GoogleCalendarAPI {
    public partial class Default : System.Web.UI.Page {
        TimeInterval fetchInterval;
        CalendarService service;
        string calendarId;

        protected void Page_Load(object sender, EventArgs e) {
            if(Session["credential"] == null)
                return;

            service = new CalendarService(new BaseClientService.Initializer() {
                HttpClientInitializer = Session["credential"] as UserCredential,
                ApplicationName = "GoogleCalendarAPI"
            });
            UpdateTimeRulers();
            if(service == null)
                return;
            if(!IsPostBack) {
                CalendarList calendarList = this.service.CalendarList.List().Execute();
                foreach(var calendarItem in calendarList.Items) {
                    lbCalendars.Items.Add(new ListItem(calendarItem.Summary, calendarItem.Id));
                }
                lbCalendars.SelectedIndex = 0;
            }
            this.calendarId = lbCalendars.SelectedValue;
            ShowAndBindScheduler();
        }
        protected void ASPxScheduler1_FetchAppointments(object sender, DevExpress.XtraScheduler.FetchAppointmentsEventArgs e) {
            if(this.fetchInterval.Contains(e.Interval) || e.Interval.Start == DateTime.MinValue)
                return;
            SetAppointmentDataSourceSelectCommandParameters(e.Interval);
            e.ForceReloadAppointments = true;
            this.fetchInterval = e.Interval;
        }
        protected void SetAppointmentDataSourceSelectCommandParameters(TimeInterval interval) {
            AppointmentSynchronizer1.AppointmentDataSource.SelectParameters["StartDate"].DefaultValue = interval.Start.ToString();
            AppointmentSynchronizer1.AppointmentDataSource.SelectParameters["EndDate"].DefaultValue = interval.End.ToString();

        }
        void ShowAndBindScheduler() {
            ASPxRoundPanel1.Visible = true;
            ASPxScheduler1.Visible = true;
            AppointmentSynchronizer1.CalendarId = this.calendarId;
            AppointmentSynchronizer1.SetGoogleRelatedData(this.service);
            BindSchedulerToObjectDataSource();
        }
        void BindSchedulerToObjectDataSource() {
            DataHelper.SetupDefaultMappings(ASPxScheduler1);
            this.fetchInterval = ASPxScheduler1.ActiveView.GetVisibleIntervals().Interval;
            SetAppointmentDataSourceSelectCommandParameters(this.fetchInterval);
            AppointmentSynchronizer1.AttachTo(ASPxScheduler1);
            ASPxScheduler1.FetchAppointments += new FetchAppointmentsEventHandler(ASPxScheduler1_FetchAppointments);
        }
        protected void ASPxScheduler1_AfterExecuteCallbackCommand(object sender, SchedulerCallbackCommandEventArgs e) {
            if(e.CommandId == SchedulerCallbackCommandId.ChangeTimeZone)
                UpdateTimeRulers();
        }
        void UpdateTimeRulers() {
            ASPxScheduler1.BeginUpdate();
            try {
                UpdateClientTimeRuler(ASPxScheduler1.DayView);
                UpdateClientTimeRuler(ASPxScheduler1.WorkWeekView);
            } finally {
                ASPxScheduler1.EndUpdate();
            }
        }
        void UpdateClientTimeRuler(DevExpress.Web.ASPxScheduler.DayView view) {
            TimeRuler ruler = view.TimeRulers[0];
            string tzId = ASPxScheduler1.OptionsBehavior.ClientTimeZoneId;
            ruler.TimeZoneId = tzId;
            ruler.Caption = String.Format("Client: {0}", tzId);
        }
    }
}