﻿using CommunityToolkit.Mvvm.Messaging;

using GrampsView.Common;

using SharedSharp.Common.Interfaces;

using System.Threading.Tasks;

using Xamarin.CommunityToolkit.ObjectModel;

namespace GrampsView.ViewModels.StartupPages
{
    /// <summary>
    /// <c> viewmodel </c> for the About <c> Flyout </c>.
    /// </summary>
    public class NeedDatabaseReloadViewModel : ViewModelBase
    {
        private readonly ISharedSharpAppInit _AppInit;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedDatabaseReloadViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// Injected common loggeing.
        /// </param>
        /// <param name="iocEventAggregator">
        /// Injected event aggregator.
        /// </param>
        public NeedDatabaseReloadViewModel(SharedSharp.Logging.Interfaces.ILog iocCommonLogging, IMessenger iocEventAggregator, ISharedSharpAppInit iocAppInit)
            : base(iocCommonLogging)
        {
            BaseTitle = "Database reload needed";

            BaseTitleIcon = Constants.IconSettings;

            LoadDataCommand = new AsyncCommand(LoadDataAction);

            _AppInit = iocAppInit;
        }

        public AsyncCommand LoadDataCommand
        {
            get; private set;
        }

        public async Task LoadDataAction()
        {
            _ = await Xamarin.Forms.Shell.Current.Navigation.PopModalAsync();

            await _AppInit.Init();
        }
    }
}