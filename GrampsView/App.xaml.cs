﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Data.Repository;
using GrampsView.Views;

using SharedSharp.Common.Interfaces;

namespace GrampsView
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new BaseNavigation();

            StartUp();

            _ = Ioc.Default.GetRequiredService<IMessenger>().Send(new SharedSharp.Events.AppStartEvent(true));

        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            window.Created += (s, e) =>
            {
                // Custom logic
                _ = Ioc.Default.GetRequiredService<ISharedSharpAppInit>().Init();

                Ioc.Default.GetRequiredService<ISharedSharpSizes>().HandleWindowSizeChanged((s as Window).Width, (s as Window).Height);
            };

            return window;
        }


        private void StartUp()
        {
            // Setup various support frameworks
            VersionTracking.Track();

            _ = Ioc.Default.GetRequiredService<IDataRepositoryManager>();

            // App Setup
            Current.UserAppTheme = SharedSharpSettings.ApplicationTheme;

            SharedSharpSettings.DatabaseVersionMin = Common.Constants.GrampsViewDatabaseVersion;




        }
    }
}