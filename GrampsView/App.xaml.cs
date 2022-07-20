﻿namespace GrampsView
{
    using GrampsView.Common;
    using GrampsView.Data;
    using GrampsView.Data.External.StoreSerial;
    using GrampsView.Data.ExternalStorage;
    using GrampsView.Data.Model;
    using GrampsView.Data.Repository;
    using GrampsView.ViewModels;

    using Microsoft.AppCenter;
    using Microsoft.Extensions.DependencyInjection;

    using SharedSharp.Common;
    using SharedSharp.Interfaces;
    using SharedSharp.Services;

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;

    using Xamarin.Essentials;
    using Xamarin.Forms;

    public sealed partial class App : Application
    {
        private static HLinkFamilyModel FamilyStartModel = null;

        private static HLinkPersonModel PersonStartPage = null;

        public App()
        {
            Services = ConfigureServices();
            ShardSharpCore.Init(Services);

            InitializeComponent();

            MainPage = new AppShell();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        protected override void OnAppLinkRequestReceived(System.Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);
        }

        protected override void OnResume()
        {
            Debug.WriteLine("Resuming");

            base.OnResume();

            // Handle when your app resumes
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("Sleeping");

            base.OnSleep();

            // Handle when your app sleeps
        }

        protected override void OnStart()
        {
            // This lookup NOT required for Windows platforms - the Culture will be automatically set
            if (Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.iOS || Xamarin.Forms.Device.RuntimePlatform == Xamarin.Forms.Device.Android)
            {
                // Determine the correct, supported .NET culture
                DependencyService.Get<ILocalize>();

                CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();

                // Assets.Strings.AppResources.Culture = ci; TODO set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            // Setup various support frameworks
            // Any updates?

            SharedSharpGeneral.MSAppCenterInit(Secret.AndroidSecret, Secret.IOSSecret, Secret.UWPSecret, argLogLevel: LogLevel.Verbose);

            VersionTracking.Track();

            Services.GetService<IErrorNotifications>();

            Services.GetService<IDataRepositoryManager>();

            //// Subscribe to changes of screen metrics
            DeviceDisplay.MainDisplayInfoChanged += (s, a) =>
            {
                OnMainDisplayInfoChanged(s, a);
            };
            SharedSharp.Common.SharedSharpGeneral.ScreenSizeInit();

            // App Setup
            Application.Current.UserAppTheme = CommonLocalSettings.ApplicationTheme;

            CardSizes.Current.ReCalculateCardWidths();

            CommonLocalSettings.DatabaseVersionMin = Common.Constants.GrampsViewDatabaseVersion;

            // Get Going
            StartAtDetailPage().GetAwaiter().GetResult();

            if (DataStore.Instance.DS.IsDataLoaded)
            {
                SharedSharp.Common.SharedSharpNavigation.NavigateHub();

                return;
            }

            // Get Going
            Services.GetService<IAppInit>().Init().ConfigureAwait(false);
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Add Services
            services.AddSingleton<IAppInit, AppInit>();
            services.AddSingleton<IDatabaseReloadDisplayService, DatabaseReloadDisplayService>();
            services.AddSingleton<IDataRepositoryManager, DataRepositoryManager>();

            //     services.AddSingleton<IFirstRunDisplayService, FirstRunDisplayService>();

            services.AddSingleton<IGrampsStoreSerial, GrampsStoreSerial>();

            //      services.AddSingleton<IMessenger, WeakReferenceMessenger>();

            //     services.AddSingleton<ISharedLogging, SharedLogging>();
            //     services.AddSingleton<ISharedMessageLog, SharedMessageLog>();
            services.AddSingleton<IStoreFile, StoreFile>();
            services.AddSingleton<IStorePostLoad, StorePostLoad>();
            services.AddSingleton<IStoreXML, StoreXML>();

            //       services.AddSingleton<IWhatsNewDisplayService, WhatsNewDisplayService>();

            // Viewmodels
            services.AddTransient<AboutViewModel>();
            services.AddTransient<AddressDetailViewModel>();
            services.AddTransient<AttributeDetailViewModel>();

            services.AddTransient<BookMarkListViewModel>();

            services.AddTransient<ChildRefDetailViewModel>();
            services.AddTransient<CitationDetailViewModel>();
            services.AddTransient<CitationListViewModel>();

            services.AddTransient<DateRangeDetailViewModel>();
            services.AddTransient<DateSpanDetailViewModel>();
            services.AddTransient<DateStrDetailViewModel>();
            services.AddTransient<DateValDetailViewModel>();

            services.AddTransient<EventDetailViewModel>();
            services.AddTransient<EventListViewModel>();

            services.AddTransient<FamilyDetailViewModel>();
            services.AddTransient<FamilyListViewModel>();
            services.AddTransient<FileInputHandlerViewModel>();
            services.AddTransient<FirstRunViewModel>();

            services.AddTransient<HubViewModel>();

            services.AddTransient<MediaDetailViewModel>();
            services.AddTransient<MediaListViewModel>();

            services.AddTransient<NeedDatabaseReloadViewModel>();
            services.AddTransient<NoteDetailViewModel>();
            services.AddTransient<NoteListViewModel>();

            services.AddTransient<PeopleGraphViewModel>();
            services.AddTransient<PersonBirthdayViewModel>();
            services.AddTransient<PersonDetailViewModel>();
            services.AddTransient<PersonListViewModel>();
            services.AddTransient<PersonNameDetailViewModel>();
            services.AddTransient<PlaceDetailViewModel>();
            services.AddTransient<PlaceListViewModel>();

            services.AddTransient<RepositoryDetailViewModel>();
            services.AddTransient<RepositoryRefDetailViewModel>();
            services.AddTransient<RepositoryListViewModel>();

            services.AddTransient<SearchPageViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SourceDetailViewModel>();
            services.AddTransient<SourceListViewModel>();

            services.AddTransient<TagDetailViewModel>();
            services.AddTransient<TagListViewModel>();

            services.AddTransient<WhatsNewViewModel>();

            services.AddTransient<NavigationPage>();

            ShardSharpCore.InitServicesAdd(ref services);

            return services.BuildServiceProvider();
        }

        // TODO This code currently runs one rotation behind and does not set the window size properly on UWP.
        //See CardSizxes for the current hack fix.

        private static void OnMainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            SharedSharp.Common.SharedSharpGeneral.ScreenSizeInit();

            // // Process changes // EventAggregator ea = this.Container.Resolve<EventAggregator>();

            // //if (!(ea is null)) // { //var t = DeviceDisplay.MainDisplayInfo; // //
            // ea.GetEvent<OrientationChanged>().Publish(e.DisplayInfo.Orientation); because seems
            // // to be one rotation behind on emulator. Try the old school way until fixed if
            // (e.DisplayInfo.Width > e.DisplayInfo.Height) {
            // DataStore.Instance.AD.CurrentOrientation = DisplayOrientation.Landscape; } else { //
            // DataStore.Instance.AD.CurrentOrientation = DisplayOrientation.Portrait; }

            // // // Card width reset CardSizes.Current.ReCalculateCardWidths();
            ////     }
        }

        private static async Task StartAtDetailPage()
        {
            if (PersonStartPage != null)
            {
                await PersonStartPage.UCNavigate();
            }

            if (FamilyStartModel != null)
            {
                await FamilyStartModel.UCNavigate();
            }
        }
    }
}