Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports Google.Apis.Calendar.v3
Imports Google.Apis.Calendar.v3.Data
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler

<Serializable> _
Public Class CustomEvent
    Private eventEntry As [Event]
    Private recurrenceInfo As String
    Public Sub New(ByVal eventEntry As [Event])
        Me.eventEntry = eventEntry
    End Sub
    Public Sub New()
        Me.eventEntry = New [Event]()
    End Sub

    Public ReadOnly Property SourceEvent() As [Event]
        Get
            Return eventEntry
        End Get
    End Property
    Public Property Subject() As String
        Get
            Return SourceEvent.Summary
        End Get
        Set(ByVal value As String)
            SourceEvent.Summary = value
        End Set
    End Property
    Public Property StartTime() As Date
        Get
            Return CalendarApiHelper.ConvertDateTime(SourceEvent.Start)
        End Get
        Set(ByVal value As Date)
            If eventEntry.Start Is Nothing Then
                eventEntry.Start = New EventDateTime()
            End If
            eventEntry.Start.DateTime = value
        End Set
    End Property
    Public Property EndTime() As Date
        Get
            Return CalendarApiHelper.ConvertDateTime(SourceEvent.End)
        End Get
        Set(ByVal value As Date)
            If eventEntry.End Is Nothing Then
                eventEntry.End = New EventDateTime()
            End If
            eventEntry.End.DateTime = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return SourceEvent.Description
        End Get
        Set(ByVal value As String)
            eventEntry.Description = value
        End Set
    End Property
    Public Property Location() As String
        Get
            Return SourceEvent.Location
        End Get
        Set(ByVal value As String)
            eventEntry.Location = value
        End Set
    End Property
    Public Property AllDay() As Boolean
        Get
            Return CalendarApiHelper.IsAllDay(SourceEvent.Start)
        End Get
        Set(ByVal value As Boolean)
            If AllDay = value Then
                Return
            End If
            If value Then
                CalendarApiHelper.MakeAllDay(SourceEvent.Start)
                CalendarApiHelper.MakeAllDay(SourceEvent.End)
            Else
                CalendarApiHelper.MakeDate(SourceEvent.Start)
                CalendarApiHelper.MakeDate(SourceEvent.End)
                SourceEvent.End.DateTime = SourceEvent.End.DateTime.Value.Date.AddDays(1)
            End If
        End Set
    End Property
    Public Property Id() As Object
        Get
            Return SourceEvent.Id
        End Get
        Set(ByVal value As Object)
            eventEntry.Id = If(value Is Nothing, Nothing, value.ToString())
        End Set
    End Property
    Public Property Status() As Integer
    Public Property Label() As Long
    Public Property EventType() As Integer

End Class

Public Class CustomEventList
    Inherits BindingList(Of CustomEvent)

End Class

#Region "CustomEventDataSource"
Public Class CustomEventDataSource

    Private googleHelper_Renamed As GoogleHelper = Nothing
    Public Sub New(ByVal calendarService As CalendarService, ByVal calendarId As String, ByVal timeZoneId As String)
        Me.googleHelper_Renamed = New GoogleHelper(calendarService, calendarId, timeZoneId)
    End Sub
    Public Sub New()
        Me.googleHelper_Renamed = New GoogleHelper(Nothing, String.Empty, String.Empty)
    End Sub
    Private ReadOnly Property GoogleHelper() As GoogleHelper
        Get
            Return googleHelper_Renamed
        End Get
    End Property

    #Region "ObjectDataSource methods"
    Public Function InsertMethodHandler(ByVal customEvent As CustomEvent) As String
        Return GoogleHelper.InsertCustomEvent(customEvent)
    End Function
    Public Sub DeleteMethodHandler(ByVal customEvent As CustomEvent)
        GoogleHelper.DeleteCustomEvent(customEvent)
    End Sub
    Public Function UpdateMethodHandler(ByVal customEvent As CustomEvent) As String
        Return GoogleHelper.UpdateCustomEvent(customEvent)
    End Function
    Public Function SelectMethodHandler(ByVal startDate As Date, ByVal endDate As Date) As IEnumerable
        Return GoogleHelper.SelectCustomEvent(startDate, endDate)
    End Function

    #End Region
End Class
#End Region