namespace Mono.Android.Crasher
{
    public enum ReportingInteractionMode
    {
        /**
        * No interaction, reports are sent silently and a "Force close" dialog
        * terminates the app.
        */
        Silent,
        /**
         * A status bar notification is triggered when the application crashes, the
         * Force close dialog is not displayed. When the user selects the
         * notification, a dialog is displayed asking him if he is ok to send a
         * report.
         */
        Notification,
        /**
         * A simple Toast is triggered when the application crashes, the Force close
         * dialog is not displayed.
         */
        Toast
    }
}