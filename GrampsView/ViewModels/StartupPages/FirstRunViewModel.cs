﻿using CommunityToolkit.Mvvm.Messaging;

using GrampsView.Common;

using SharedSharp.Common.Interfaces;

using System.Threading.Tasks;

using Xamarin.CommunityToolkit.ObjectModel;

namespace GrampsView.ViewModels.StartupPages
{
    /// <summary>
    /// <c> First Run View Model </c>
    /// </summary>
    public class FirstRunViewModel : ViewModelBase
    {
        private readonly ISharedSharpAppInit _AppInit;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstRunViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// Common logger
        /// </param>
        /// <param name="iocEventAggregator">
        /// The ioc event aggregator.
        /// </param>
        public FirstRunViewModel(SharedSharp.Logging.Interfaces.ILog iocCommonLogging, IMessenger iocEventAggregator, ISharedSharpAppInit iocAppInit)
            : base(iocCommonLogging)
        {
            LoadDataCommand = new AsyncCommand(FirstRunLoadAFileButton);

            BaseTitle = "First Run";

            BaseTitleIcon = Constants.IconSettings;

            _AppInit = iocAppInit;
        }

        public AsyncCommand LoadDataCommand
        {
            get;
        }

        /// <summary>
        /// Gramps export XML plus media.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public async Task FirstRunLoadAFileButton()
        {
            _ = await Xamarin.Forms.Shell.Current.Navigation.PopModalAsync();

            await _AppInit.Init();
        }
    }
}