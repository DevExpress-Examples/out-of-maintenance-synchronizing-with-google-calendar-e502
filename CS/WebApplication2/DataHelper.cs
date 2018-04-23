using System;
using DevExpress.Web.ASPxScheduler;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public static class DataHelper {
	public static void SetupDefaultMappings(ASPxScheduler control) {
		SetupDefaultMappingsSiteMode(control);
	}
	//
	public static void SetupCustomEventsMappings(ASPxScheduler control) {
		SetupDefaultMappingsSiteMode(control);
	}

	private static void SetupDefaultMappingsSiteMode(ASPxScheduler control) {
		ASPxSchedulerStorage storage = control.Storage;
		storage.BeginUpdate();
		try {
			ASPxAppointmentMappingInfo appointmentMappings = storage.Appointments.Mappings;
			appointmentMappings.AppointmentId = "Id";
			appointmentMappings.Start = "StartTime";
			appointmentMappings.End = "EndTime";
			appointmentMappings.Subject = "Subject";
			appointmentMappings.AllDay = "AllDay";
			appointmentMappings.Description = "Description";
			appointmentMappings.Label = "Label";
			appointmentMappings.Location = "Location";
			appointmentMappings.Status = "Status";
			appointmentMappings.Type = "EventType";
		}
		finally {
			storage.EndUpdate();
		}
	}
	
}
