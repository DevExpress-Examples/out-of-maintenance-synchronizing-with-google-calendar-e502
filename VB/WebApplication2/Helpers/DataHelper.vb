Imports System
Imports DevExpress.Web.ASPxScheduler

''' <summary>
''' Summary description for DataHelper
''' </summary>
Public NotInheritable Class DataHelper

    Private Sub New()
    End Sub

    Public Shared Sub SetupDefaultMappings(ByVal control As ASPxScheduler)
        SetupDefaultMappingsSiteMode(control)
    End Sub
    '
    Public Shared Sub SetupCustomEventsMappings(ByVal control As ASPxScheduler)
        SetupDefaultMappingsSiteMode(control)
    End Sub

    Private Shared Sub SetupDefaultMappingsSiteMode(ByVal control As ASPxScheduler)
        Dim storage As ASPxSchedulerStorage = control.Storage
        storage.BeginUpdate()
        Try
            Dim appointmentMappings As ASPxAppointmentMappingInfo = storage.Appointments.Mappings
            appointmentMappings.AppointmentId = "Id"
            appointmentMappings.Start = "StartTime"
            appointmentMappings.End = "EndTime"
            appointmentMappings.Subject = "Subject"
            appointmentMappings.AllDay = "AllDay"
            appointmentMappings.Description = "Description"
            appointmentMappings.Label = "Label"
            appointmentMappings.Location = "Location"
            appointmentMappings.Status = "Status"
            appointmentMappings.Type = "EventType"
        Finally
            storage.EndUpdate()
        End Try
    End Sub

End Class
