Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.Web.ASPxScheduler
Imports DevExpress.XtraScheduler
Imports Google.Apis.Calendar.v3

Namespace GoogleCalendarAPI
    Partial Public Class AppointmentSynchronizer
        Inherits System.Web.UI.UserControl


        Private calendarService_Renamed As CalendarService

        Private calendarId_Renamed As String
        Private timeZoneId As String
        Public ReadOnly Property AppointmentDataSource() As ObjectDataSource
            Get
                Return innerAppointmentDataSource
            End Get
        End Property
        Public Property CalendarService() As CalendarService
            Get
                Return calendarService_Renamed
            End Get
            Set(ByVal value As CalendarService)
                value = calendarService_Renamed
            End Set
        End Property
        Public Property CalendarId() As String
            Get
                Return calendarId_Renamed
            End Get
            Set(ByVal value As String)
                calendarId_Renamed = value
            End Set
        End Property

        Public Sub SetGoogleRelatedData(ByVal service As CalendarService)
            Me.calendarService_Renamed = service
        End Sub
        Public Sub AttachTo(ByVal control As ASPxScheduler)
            timeZoneId = control.Storage.TimeZoneId
            control.AppointmentDataSource = Me.AppointmentDataSource
            AddHandler control.AppointmentRowInserted, AddressOf ControlOnAppointmentRowInserted
            AddHandler control.AppointmentsInserted, AddressOf ControlOnAppointmentsInserted
            AddHandler control.AppointmentRowUpdated, AddressOf ControlOnAppointmentRowUpdated
            AddHandler control.AppointmentsChanged, AddressOf ControlOnAppointmentsInserted
            control.DataBind()
        End Sub

        Protected Sub innerAppointmentDataSource_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
            Dim customObjectDataSource As New CustomEventDataSource(CalendarService, CalendarId, timeZoneId)
            e.ObjectInstance = customObjectDataSource
        End Sub
        Private lastInsertedAppointmentId As String

        Private Sub ControlOnAppointmentRowInserted(ByVal sender As Object, ByVal e As ASPxSchedulerDataInsertedEventArgs)
            e.KeyFieldValue = Me.lastInsertedAppointmentId
        End Sub
        Private Sub ControlOnAppointmentRowUpdated(ByVal sender As Object, ByVal e As ASPxSchedulerDataUpdatedEventArgs)
            e.Keys("Id") = Me.lastInsertedAppointmentId
        End Sub
        Protected Sub AppointmentsDataSource_Inserted(ByVal sender As Object, ByVal e As ObjectDataSourceStatusEventArgs)
            Me.lastInsertedAppointmentId = DirectCast(e.ReturnValue, String)
        End Sub
        Private Sub ControlOnAppointmentsInserted(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs)
            For i As Integer = 0 To e.Objects.Count - 1
                Dim apt As Appointment = CType(e.Objects(i), Appointment)
                Dim storage As ASPxSchedulerStorage = DirectCast(sender, ASPxSchedulerStorage)
                storage.SetAppointmentId(apt, lastInsertedAppointmentId)
            Next i

        End Sub
    End Class
End Namespace