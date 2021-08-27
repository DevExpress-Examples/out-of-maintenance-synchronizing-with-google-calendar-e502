<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128547943/16.1.4%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E502)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [AppointmentSynchronizer.ascx](./CS/WebApplication2/AppointmentSynchronizer.ascx) (VB: [AppointmentSynchronizer.ascx](./VB/WebApplication2/AppointmentSynchronizer.ascx))
* [AppointmentSynchronizer.ascx.cs](./CS/WebApplication2/AppointmentSynchronizer.ascx.cs) (VB: [AppointmentSynchronizer.ascx.vb](./VB/WebApplication2/AppointmentSynchronizer.ascx.vb))
* [CalendarApiHelper.cs](./CS/WebApplication2/CalendarApiHelper.cs) (VB: [CalendarApiHelper.vb](./VB/WebApplication2/CalendarApiHelper.vb))
* [CustomEvents.cs](./CS/WebApplication2/CustomEvents.cs) (VB: [CustomEvents.vb](./VB/WebApplication2/CustomEvents.vb))
* [DataHelper.cs](./CS/WebApplication2/DataHelper.cs) (VB: [DataHelper.vb](./VB/WebApplication2/DataHelper.vb))
* [Default.aspx](./CS/WebApplication2/Default.aspx) (VB: [Default.aspx](./VB/WebApplication2/Default.aspx))
* **[Default.aspx.cs](./CS/WebApplication2/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebApplication2/Default.aspx.vb))**
* [GoogleHelper.cs](./CS/WebApplication2/GoogleHelper.cs) (VB: [GoogleHelper.vb](./VB/WebApplication2/GoogleHelper.vb))
* [CalendarApiHelper.cs](./CS/WebApplication2/Helpers/CalendarApiHelper.cs) (VB: [CalendarApiHelper.vb](./VB/WebApplication2/Helpers/CalendarApiHelper.vb))
* [CustomEvents.cs](./CS/WebApplication2/Helpers/CustomEvents.cs) (VB: [CustomEvents.vb](./VB/WebApplication2/Helpers/CustomEvents.vb))
* [DataHelper.cs](./CS/WebApplication2/Helpers/DataHelper.cs) (VB: [DataHelper.vb](./VB/WebApplication2/Helpers/DataHelper.vb))
* [GoogleHelper.cs](./CS/WebApplication2/Helpers/GoogleHelper.cs) (VB: [GoogleHelper.vb](./VB/WebApplication2/Helpers/GoogleHelper.vb))
* [oauth2callback.aspx](./CS/WebApplication2/oauth2callback.aspx) (VB: [oauth2callback.aspx](./VB/WebApplication2/oauth2callback.aspx))
* [oauth2callback.aspx.cs](./CS/WebApplication2/oauth2callback.aspx.cs) (VB: [oauth2callback.aspx.vb](./VB/WebApplication2/oauth2callback.aspx.vb))
<!-- default file list end -->
# Synchronizing with Google Calendar


<p>This example illustrates how to use <strong>Google Calendar API</strong> to synchronize ASPxScheduler with Google Calendar. Google provides the corresponding guidelines regarding the use of this API:</p>
<p><a href="https://developers.google.com/google-apps/calendar/quickstart/dotnet">Google Calendar API</a> </p>
<p>Before using this API, make certain you have read and are in compliance with <a href="https://developers.google.com/site-policies">Google’s licensing terms</a>. Next, you’ll need to generate a <strong>JSON file</strong> with OAuth 2.0 credentials to enable the <strong>Google Calendar API.</strong></p>
<p><br>We have a corresponding KB article which contains a step-by-step description of how to generate this <strong>JSON file </strong>for the <a href="https://developers.google.com/identity/protocols/OAuth2">installed application</a> type:</p>
<p><a href="https://www.devexpress.com/Support/Center/p/T267842">How to enable the Google Calendar API to use it in your application</a></p>
<p><br>The the OAuth 2.0 flow both for authentication and for obtaining authorization is similar to the one for <a href="https://developers.google.com/api-client-library/python/auth/web-app">Web Server Applications</a> except three points:</p>
<p>1) When creating a client ID, you specify that your application is an Installed application. This results in a different value for the <em><code>redirect_uri</code></em> parameter. <br>2) The client ID and client secret obtained from the API Console are embedded in the source code of your application. In this context, the client secret is obviously not treated as a secret. <br>3) The authorization code can be returned to your application in the title bar of the browser or in the query string of an HTTP request to the local host. <br><br>Please refer to <a href="https://developers.google.com/api-client-library/python/auth/installed-app">Google OAuth 2.0</a> documentation for more information.<br><br>After you generate this JSON file, start the<em> "oauth2callback.aspx"</em> page for authorization. <br><br>1. Enter the email address you used to generate the JSON file.<br>2. Select the JSON file on the client machine by clicking the "Browse" button.<br>3. Click the "Get 'Client ID' and 'Client secret' from file" button to upload the selected file and enable the Google Calendar API.<br>4. The application should be navigated to the <em>"Default.aspx"</em> page.<br>5. Select a corresponding calendar to synchronize with the scheduler storage.<br><br></p>
<p><strong>P.S. To run this example's solution, include the corresponding "Google Calendar API" assemblies into the project.</strong></p>
<p><strong>For this, open the "Package Manager Console" (Tools - NuGet Package Manager) and execute the following command:</strong><br><br></p>
<pre class="prettyprint notranslate"><code>Install-Package Google.Apis.Calendar.v3</code></pre>
<p><br><br></p>
<p>The synchronization procedure implemented in this example has the following limitations: </p>
<p>-     Recurring appointments are excluded<br>-     Statuses, labels, reminders, and resource associations are not synchronized.</p>

<br/>


