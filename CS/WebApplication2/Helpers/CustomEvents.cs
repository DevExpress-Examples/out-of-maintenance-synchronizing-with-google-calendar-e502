
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

[Serializable]
public class CustomEvent {
    Event eventEntry;
    string recurrenceInfo;
    public CustomEvent(Event eventEntry) {
        this.eventEntry = eventEntry;
    }
    public CustomEvent() {
        this.eventEntry = new Event();
    }

    public Event SourceEvent { get { return eventEntry; } }
    public string Subject { get { return SourceEvent.Summary; } set { SourceEvent.Summary = value; } }
    public DateTime StartTime {
        get { return CalendarApiHelper.ConvertDateTime(SourceEvent.Start); }
        set {
            if (eventEntry.Start == null)
                eventEntry.Start = new EventDateTime();
            eventEntry.Start.DateTime = value;
        }
    }
    public DateTime EndTime {
        get { return CalendarApiHelper.ConvertDateTime(SourceEvent.End); }
        set {
            if (eventEntry.End == null)
                eventEntry.End = new EventDateTime();
            eventEntry.End.DateTime = value;
        }
    }
    public string Description { get { return SourceEvent.Description; } set { eventEntry.Description = value; } }
    public string Location { get { return SourceEvent.Location; } set { eventEntry.Location = value; } }
    public bool AllDay {
        get { return CalendarApiHelper.IsAllDay(SourceEvent.Start); }
        set {
            if (AllDay == value)
                return;
            if (value) {
                CalendarApiHelper.MakeAllDay(SourceEvent.Start);
                CalendarApiHelper.MakeAllDay(SourceEvent.End);
            } else {
                CalendarApiHelper.MakeDate(SourceEvent.Start);
                CalendarApiHelper.MakeDate(SourceEvent.End);
                SourceEvent.End.DateTime = SourceEvent.End.DateTime.Value.Date.AddDays(1);
            }
        }
    }
    public object Id { get { return SourceEvent.Id; } set { eventEntry.Id = (value == null)? null : value.ToString(); } }
    public int Status { get; set; }
    public long Label { get; set; }
    public int EventType { get; set; }
  
}

public class CustomEventList : BindingList<CustomEvent> {
}

#region CustomEventDataSource
public class CustomEventDataSource {
    GoogleHelper googleHelper = null;
    public CustomEventDataSource(CalendarService calendarService, string calendarId, string timeZoneId)
    {
        this.googleHelper = new GoogleHelper(calendarService, calendarId, timeZoneId);
    }
    public CustomEventDataSource() {
        this.googleHelper = new GoogleHelper(null, String.Empty, string.Empty);
    }
    GoogleHelper GoogleHelper { get { return googleHelper; } }

    #region ObjectDataSource methods
    public string InsertMethodHandler(CustomEvent customEvent) {
        return GoogleHelper.InsertCustomEvent(customEvent);
    }
    public void DeleteMethodHandler(CustomEvent customEvent) {
        GoogleHelper.DeleteCustomEvent(customEvent);
    }
    public string UpdateMethodHandler(CustomEvent customEvent) {
        return GoogleHelper.UpdateCustomEvent(customEvent);
    }
    public IEnumerable SelectMethodHandler(DateTime startDate, DateTime endDate)
    {
        return GoogleHelper.SelectCustomEvent(startDate, endDate);
    }
   
    #endregion
}
#endregion