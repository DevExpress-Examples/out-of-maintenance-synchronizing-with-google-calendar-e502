Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Google.Apis.Calendar.v3.Data
Imports DevExpress.XtraScheduler.iCalendar.Native

Public NotInheritable Class CalendarApiHelper

    Private Sub New()
    End Sub

    Public Shared Function ConvertDateTime(ByVal start As EventDateTime) As Date
        If start Is Nothing Then
            Return Date.MinValue
        End If
        If start.DateTime.HasValue Then
            Return start.DateTime.Value
        End If
        Return Date.Parse(start.Date)
    End Function
    Public Shared Function IsAllDay(ByVal start As EventDateTime) As Boolean
        If start Is Nothing Then
            Return False
        End If
        Return Not start.DateTime.HasValue
    End Function
    Public Shared Function GetRawDate(ByVal [date] As Date) As String
        Return [date].ToString("yyyy-MM-dd")
    End Function
    Public Shared Sub MakeAllDay(ByVal eventDate As EventDateTime)
        Dim dateTime As Date = eventDate.DateTime.Value
        eventDate.DateTime = Nothing
        eventDate.DateTimeRaw = CalendarApiHelper.GetRawDate(dateTime.Date)
    End Sub

    Public Shared Function GetVTimeZone(ByVal timeZoneId As String) As String
        Return TimeZoneConverter.ConvertToVTimeZone(TimeZoneInfo.FindSystemTimeZoneById(timeZoneId)).TimeZoneIdentifier.Value
    End Function
    Friend Shared Sub MakeDate(ByVal eventDate As EventDateTime)
        Dim dateTime As Date = ConvertDateTime(eventDate)
        eventDate.DateTime = dateTime
    End Sub
End Class