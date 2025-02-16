﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Data.StoreDB;

using SharedSharp.Common.Interfaces;

namespace GrampsView.ViewModels.StartupPages
{
    /// <summary>
    /// <c> First Run View Model </c>
    /// </summary>
    public class FirstRunViewModel : ViewModelBase
    {
        private readonly ISharedSharpAppInit _AppInit;

        /// <summary>Initializes a new instance of the <see cref="FirstRunViewModel" /> class.</summary>
        /// <param name="iocCommonLogging">Common logger</param>
        /// <param name="iocAppInit">Initialisation Code</param>
        public FirstRunViewModel(ILog iocCommonLogging, ISharedSharpAppInit iocAppInit)
            : base(iocCommonLogging)
        {
            LoadDataCommand = new AsyncRelayCommand(FirstRunLoadAFileButton);

            BaseTitle = "First Run";

            BaseTitleIcon = Constants.IconSettings;

            _AppInit = iocAppInit;
        }

        public AsyncRelayCommand LoadDataCommand
        {
            get;
        }

        /// <summary>Gramps export XML plus media.</summary>
        public async Task FirstRunLoadAFileButton()
        {
            await Ioc.Default.GetRequiredService<IStoreDB>().InitialiseDB();

            await App.Current.MainPage.Navigation.PopAsync();

            await _AppInit.Init();
        }
    }
}