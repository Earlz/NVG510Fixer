using Android.App;
using Android.OS;
using Android.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace NVG510Fixer
{
    public class TrackedActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledExceptionHandler;
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidUnhandledExceptionHandler;
            base.OnCreate(bundle);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

        protected override void OnStop()
        {
            base.OnStop();

        }

        private static void AndroidUnhandledExceptionHandler(object sender, RaiseThrowableEventArgs args)
        {
            ReportException(args.Exception);
        }

        private static void CurrentDomainUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            ReportException((Exception)e.ExceptionObject);
        }

        private static void ReportException(Exception e)
        {

            // If an unhandled exception will cause the application to terminate, you should call ApplicationStop.
        }
    }
}
