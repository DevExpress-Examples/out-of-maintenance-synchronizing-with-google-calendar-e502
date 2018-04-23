using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using Google.Apis.Calendar.v3;

namespace GoogleCalendarAPI {
    public partial class AppointmentSynchronizer : System.Web.UI.UserControl {
        CalendarService calendarService;
        string calendarId;
        string timeZoneId;
        public ObjectDataSource AppointmentDataSource {
            get {
                return innerAppointmentDataSource;
            }
        }
        public CalendarService CalendarService {
            get {
                return calendarService;
            }
            set {
                value = calendarService;
            }
        }
        public string CalendarId {
            get {
                return calendarId;
            }
            set {
                calendarId = value;
            }
        }

        public void SetGoogleRelatedData(CalendarService service) {
            this.calendarService = service;
        }
        public void AttachTo(ASPxScheduler control) {
            timeZoneId = control.Storage.TimeZoneId;
            control.AppointmentDataSource = this.AppointmentDataSource;
            control.AppointmentRowInserted += new ASPxSchedulerDataInsertedEventHandler(ControlOnAppointmentRowInserted);
            control.AppointmentsInserted += new PersistentObjectsEventHandler(ControlOnAppointmentsInserted);
            control.AppointmentRowUpdated += new ASPxSchedulerDataUpdatedEventHandler(ControlOnAppointmentRowUpdated);
            control.AppointmentsChanged += new PersistentObjectsEventHandler(ControlOnAppointmentsInserted);
            control.DataBind();
        }

        protected void innerAppointmentDataSource_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
            CustomEventDataSource customObjectDataSource = new CustomEventDataSource(CalendarService, CalendarId, timeZoneId);
            e.ObjectInstance = customObjectDataSource;
        }
        string lastInsertedAppointmentId;

        void ControlOnAppointmentRowInserted(object sender, ASPxSchedulerDataInsertedEventArgs e) {
            e.KeyFieldValue = this.lastInsertedAppointmentId;
        }
        void ControlOnAppointmentRowUpdated(object sender, ASPxSchedulerDataUpdatedEventArgs e) {
            e.Keys["Id"] = this.lastInsertedAppointmentId;
        }
        protected void AppointmentsDataSource_Inserted(object sender, ObjectDataSourceStatusEventArgs e) {
            this.lastInsertedAppointmentId = (string)e.ReturnValue;
        }
        void ControlOnAppointmentsInserted(object sender, PersistentObjectsEventArgs e) {
            for(int i = 0; i < e.Objects.Count; i++) {
                Appointment apt = (Appointment)e.Objects[i];
                ASPxSchedulerStorage storage = (ASPxSchedulerStorage)sender;
                storage.SetAppointmentId(apt, lastInsertedAppointmentId);
            }

        }
    }
}