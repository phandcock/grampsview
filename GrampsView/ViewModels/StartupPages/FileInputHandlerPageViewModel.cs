﻿namespace GrampsView.ViewModels
{
    using GrampsView.Common;
    using GrampsView.Data;
    using GrampsView.Data.Model;
    using GrampsView.Data.Repository;
    using GrampsView.Events;

    using Prism.Commands;
    using Prism.Events;

    using System;
    using System.Reflection;

    using Xamarin.CommunityToolkit.UI.Views;
    using Xamarin.Essentials;
    using Xamarin.Forms;

    /// <summary>
    /// View model for File Input Page.
    /// </summary>
    public partial class FileInputHandlerViewModel : ViewModelBase
    {
        private bool _CanHandleDataFolderChosen = true;

        private bool _LocalCanHandleSample = true;

        /// <summary>
        /// The local data detail list.
        /// </summary>
        private CardListLineCollection localDataDetailList = new CardListLineCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInputHandlerViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// The common logging.
        /// </param>
        /// <param name="iocEventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="iocNavigationService">
        /// Prism Navigation Service
        /// </param>
        public FileInputHandlerViewModel(ICommonLogging iocCommonLogging, IEventAggregator iocEventAggregator)
            : base(iocCommonLogging, iocEventAggregator)
        {
            BaseTitle = "File Input Handler";

            BaseTitleIcon = CommonConstants.IconSettings;

            LoadSampleCommand = new DelegateCommand(LoadSample).ObservesCanExecute(() => LocalCanHandleSample);

            PickFileCommand = new DelegateCommand(PickFile).ObservesCanExecute(() => LocalCanHandleDataFolderChosen);
        }

        /// <summary>
        /// Gets the data detail list.
        /// </summary>
        /// <value>
        /// The data detail list.
        /// </value>
        public CardListLineCollection DataDetailList
        {
            get
            {
                return localDataDetailList;
            }
        }

        public DelegateCommand LoadSampleCommand { get; private set; }

        public bool LocalCanHandleDataFolderChosen
        {
            get { return _CanHandleDataFolderChosen; }
            set { SetProperty(ref _CanHandleDataFolderChosen, value); }
        }

        public bool LocalCanHandleSample
        {
            get { return _LocalCanHandleSample; }
            set { SetProperty(ref _LocalCanHandleSample, value); }
        }

        public DelegateCommand PickFileCommand { get; private set; }

        /// <summary>
        /// Loads the sample data.
        /// </summary>
        public async void LoadSample()
        {
            BaseCL.Progress("Load sample data");

            CommonRoutines.ListEmbeddedResources();

            // Load Resource
            var assemblyExec = Assembly.GetExecutingAssembly();
            var resourceName = "GrampsView.AnythingElse.SampleData.EnglishTudorHouse.gpkg";

            DataStore.Instance.AD.CurrentInputStream = assemblyExec.GetManifestResourceStream(resourceName);

            DataStore.Instance.AD.CurrentInputStreamPath = "AnythingElse/Sample Data/EnglishTudorHouse.gpkg";

            BaseCL.Progress("Tell someone to load the file");

            // Remove the old dateTime stamps so the files get reloaded even if they have been seen before
            CommonLocalSettings.SetReloadDatabase();

            BaseEventAggregator.GetEvent<DataLoadStartEvent>().Publish(false);

            await Shell.Current.GoToAsync("..");
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
        public async void PickFile()
        {
            BaseCL.Progress("Calling folder picker");

            try
            {
                base.BaseCurrentState = LayoutState.Loading;

                if (await StoreFileUtility.PickCurrentInputFile().ConfigureAwait(false))
                {
                    BaseCL.Progress("Tell someone to load the file");

                    // Remove the old dateTime stamps so the files get reloaded even if they have
                    // been seen before
                    CommonLocalSettings.SetReloadDatabase();

                    BaseEventAggregator.GetEvent<DataLoadStartEvent>().Publish(false);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Shell.Current.GoToAsync("..");
                    });

                    await DataStore.Instance.CN.DataLogEntryAdd("File picked").ConfigureAwait(false);
                }
                else
                {
                    BaseCL.Progress("File picker error");
                    DataStore.Instance.CN.NotifyAlert("No input file was selected");

                    // Allow another pick if required
                    LocalCanHandleDataFolderChosen = true;

                    base.BaseCurrentState = LayoutState.None;
                }
            }
            catch (Exception ex)
            {
                DataStore.Instance.CN.NotifyException("Exception when using File Picker", ex);

                throw;
            }
        }

        /// <summary>
        /// Called when navigation is performed to a page. You can use this method to load state if
        /// it is available.
        /// </summary>
        public override void PopulateViewModel()
        {
            BaseEventAggregator.GetEvent<ProgressLoading>().Publish(null);

            if (DataStore.Instance.AD.CurrentDataFolderValid)
            {
                DataDetailList.Clear();

                DataDetailList.Add(
                    new CardListLine(
                        "Data Folder:",
                        DataStore.Instance.AD.CurrentDataFolder.FullName));
            }
            else
            {
                DataDetailList.Clear();

                DataDetailList.Add(
                    new CardListLine(
                         "Data Folder:",
                        "Not set"));
            }
        }
    }
}