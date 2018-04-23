<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="GoogleCalendarAPI.Default" %>

<%@ Register assembly="DevExpress.Web.ASPxScheduler.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxScheduler" tagprefix="dxwschs" %>
<%@ Register assembly="DevExpress.XtraScheduler.v16.1.Core, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraScheduler" tagprefix="cc1" %>

<%@ Register Src="AppointmentSynchronizer.ascx" TagName="AppointmentSynchronizer" TagPrefix="dxex" %>
<%@ Register assembly="DevExpress.Web.v16.1, Version=16.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Google Calendar API</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dxex:AppointmentSynchronizer runat="server" ID="AppointmentSynchronizer1" InitAppointments="false" />
            <table>
                <tr>
                    <td>
                        <dx:aspxroundpanel ID="ASPxRoundPanel2" runat="server"  HeaderText="Choose calendar">
                            <panelcollection>
                                <dx:PanelContent runat="server">
                                    <asp:ListBox runat="server" ID="lbCalendars" AutoPostBack="true"></asp:ListBox>
                                </dx:PanelContent>
                            </panelcollection>
                        </dx:aspxroundpanel></td>
                    <td valign="top">
                        <dx:aspxroundpanel ID="ASPxRoundPanel1" runat="server" HeaderText="Choose the Client Time Zone">
                            <panelcollection>
                                <dx:PanelContent runat="server">
                                    <dxwschs:ASPxTimeZoneEdit runat="server" MasterControlID="ASPxScheduler1" ID="timeZoneEdit1" Width="300PX"></dxwschs:ASPxTimeZoneEdit>
                                </dx:PanelContent>
                            </panelcollection>
                        </dx:aspxroundpanel>
                    </td>
                </tr>
            </table>
            <br />
            <dxwschs:ASPxScheduler runat="server" ID="ASPxScheduler1" ActiveViewType="WorkWeek" ClientInstanceName="scheduler" Start="2015-10-26" Visible="False"
                OnAfterExecuteCallbackCommand="ASPxScheduler1_AfterExecuteCallbackCommand">
                <Views>
                    <DayView>
                        <TimeRulers>
                            <cc1:TimeRuler>
                            </cc1:TimeRuler>
                        </TimeRulers>
                    </DayView>
                    <WorkWeekView>
                        <TimeRulers>
                            <cc1:TimeRuler>
                            </cc1:TimeRuler>
                        </TimeRulers>
                    </WorkWeekView>
                </Views>
            </dxwschs:ASPxScheduler>
        </div>
    </form>
</body>
</html>