<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="AppointmentSynchronizer.ascx.vb" Inherits="GoogleCalendarAPI.AppointmentSynchronizer" %>
<asp:ObjectDataSource ID="innerAppointmentDataSource" runat="server" DataObjectTypeName="CustomEvent"  TypeName="CustomEventDataSource" DeleteMethod="DeleteMethodHandler" 
    SelectMethod="SelectMethodHandler" OnObjectCreated="innerAppointmentDataSource_ObjectCreated" OnInserted="AppointmentsDataSource_Inserted" OnUpdated="AppointmentsDataSource_Inserted" 
     UpdateMethod="UpdateMethodHandler" InsertMethod="InsertMethodHandler" >
    <SelectParameters>
        <asp:Parameter Name="StartDate" Type="dateTime" />
        <asp:Parameter Name="EndDate" Type="dateTime" />
    </SelectParameters> 

</asp:ObjectDataSource>