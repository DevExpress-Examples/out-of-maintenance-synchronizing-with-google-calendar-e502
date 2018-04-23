using System;
using System.Collections.Generic;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

public class GoogleHelper
{
    CalendarService service;
    string calendarId;
    string timeZoneId;

    public GoogleHelper(CalendarService calendarService, string calendarId, string timeZoneId)
    {
        this.service = calendarService;
        this.calendarId = calendarId;
        this.timeZoneId = timeZoneId;
    }

    CalendarService Service { get { return service; } }
    #region SelectCustomEvent
    public CustomEventList SelectCustomEvent(DateTime start, DateTime end)
    {
        EventsResource.ListRequest request = Service.Events.List(this.calendarId);
        request.MaxResults = 2500;
        request.TimeMin = start;
        request.TimeMax = end;
        Events events = request.Execute();
        CustomEventList result = new CustomEventList();
        foreach (Event item in events.Items)
        {
            if (item.Recurrence != null || item.RecurringEventId != null)
                continue;
                
            if (item.OriginalStartTime != null)
                continue;
            CustomEvent customEvent = new CustomEvent(item);
            result.Add(customEvent);
        }
        return result;
    }
    #endregion
    #region UpdateCustomEvent
    public string UpdateCustomEvent(CustomEvent customEvent)
    {
        Event sourceEvent = customEvent.SourceEvent;
        Event updatedEvent = this.service.Events.Update(sourceEvent, this.calendarId, sourceEvent.Id).Execute();
        return updatedEvent.Id;
    }
    #endregion
    #region InsertCustomEvent

    private void AssignProperties(CustomEvent customEvent, Event newEvent)
    {
        newEvent.Summary = customEvent.SourceEvent.Summary;
        newEvent.Description = customEvent.SourceEvent.Description;
        newEvent.Location = customEvent.SourceEvent.Location;
        newEvent.Start = customEvent.SourceEvent.Start;
        newEvent.Start.TimeZone =CalendarApiHelper.GetVTimeZone(timeZoneId);
        newEvent.End = customEvent.SourceEvent.End;
        newEvent.End.TimeZone = CalendarApiHelper.GetVTimeZone(timeZoneId);
    }
   
    public string InsertCustomEvent(CustomEvent customEvent)
    {
        Event instance  = new Event();
        AssignProperties(customEvent, instance);
        Event result = this.service.Events.Insert(instance, this.calendarId).Execute();
        return result.Id;
    }
    #endregion
    #region DeleteCustomEvent
    public void DeleteCustomEvent(CustomEvent customEvent)
    {
        service.Events.Delete(this.calendarId, customEvent.SourceEvent.Id).Execute();
    }
    #endregion
 
}

