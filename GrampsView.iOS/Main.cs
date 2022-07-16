﻿using Microsoft.Extensions.DependencyInjection;

using SharedSharp.Interfaces;

using System;
using System.Threading.Tasks;

using UIKit;

namespace GrampsView.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            // TODO UnhandledExceptionHandler += new Windows.UI.Xaml.UnhandledExceptionEventHandler(UnhandledExceptionHandler);

            // if you want to use a different Application Delegate class from "AppDelegate" you can
            // specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            App.Current.Services.GetService<IErrorNotifications>().NotifyException("TaskSchedulerOnUnobservedTaskException", newExc);
        }
    }
}