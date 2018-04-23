Imports System
Imports System.Collections.Generic
Imports Google.Apis.Calendar.v3
Imports Google.Apis.Calendar.v3.Data

Public Class GoogleHelper

    Private service_Renamed As CalendarService
    Private calendarId As String
    Private timeZoneId As String

    Public Sub New(ByVal calendarService As CalendarService, ByVal calendarId As String, ByVal timeZoneId As String)
        Me.service_Renamed = calendarService
        Me.calendarId = calendarId
        Me.timeZoneId = timeZoneId
    End Sub

    Private ReadOnly Property Service() As CalendarService
        Get
            Return service_Renamed
        End Get
    End Property
    #Region "SelectCustomEvent"
    Public Function SelectCustomEvent(ByVal start As Date, ByVal [end] As Date) As CustomEventList
        Dim request As EventsResource.ListRequest = Service.Events.List(Me.calendarId)
        request.MaxResults = 2500
        request.TimeMin = start
        request.TimeMax = [end]
        Dim events As Events = request.Execute()
        Dim result As New CustomEventList()
        For Each item As [Event] In events.Items
            If item.Recurrence IsNot Nothing OrElse item.RecurringEventId IsNot Nothing Then
                Continue For
            End If

            If item.OriginalStartTime IsNot Nothing Then
                Continue For
            End If
            Dim customEvent As New CustomEvent(item)
            result.Add(customEvent)
        Next item
        Return result
    End Function
    #End Region
    #Region "UpdateCustomEvent"
    Public Function UpdateCustomEvent(ByVal customEvent As CustomEvent) As String
        Dim sourceEvent As [Event] = customEvent.SourceEvent
        Dim updatedEvent As [Event] = Me.service_Renamed.Events.Update(sourceEvent, Me.calendarId, sourceEvent.Id).Execute()
        Return updatedEvent.Id
    End Function
    #End Region
    #Region "InsertCustomEvent"

    Private Sub AssignProperties(ByVal customEvent As CustomEvent, ByVal newEvent As [Event])
        newEvent.Summary = customEvent.SourceEvent.Summary
        newEvent.Description = customEvent.SourceEvent.Description
        newEvent.Location = customEvent.SourceEvent.Location
        newEvent.Start = customEvent.SourceEvent.Start
        newEvent.Start.TimeZone =CalendarApiHelper.GetVTimeZone(timeZoneId)
        newEvent.End = customEvent.SourceEvent.End
        newEvent.End.TimeZone = CalendarApiHelper.GetVTimeZone(timeZoneId)
    End Sub

    Public Function InsertCustomEvent(ByVal customEvent As CustomEvent) As String
        Dim instance As New [Event]()
        AssignProperties(customEvent, instance)
        Dim result As [Event] = Me.service_Renamed.Events.Insert(instance, Me.calendarId).Execute()
        Return result.Id
    End Function
    #End Region
    #Region "DeleteCustomEvent"
    Public Sub DeleteCustomEvent(ByVal customEvent As CustomEvent)
        service_Renamed.Events.Delete(Me.calendarId, customEvent.SourceEvent.Id).Execute()
    End Sub
    #End Region

End Class

