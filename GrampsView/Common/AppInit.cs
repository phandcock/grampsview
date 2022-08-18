﻿namespace GrampsView.Common
{
    using CommunityToolkit.Mvvm.Messaging;

    using GrampsView.Events;
    using GrampsView.Views;

    using Microsoft.Extensions.DependencyInjection;

    using SharedSharp.Common;
    using SharedSharp.Interfaces;
    using SharedSharp.Services;

    using System;
    using System.Threading.Tasks;

    public class AppInit : IAppInit
    {
        public async Task Init()
        {
            try
            {
                // First run?
                if (await App.Current.Services.GetService<IFirstRunDisplayService>().ShowIfAppropriate(nameof(FirstRunPage)))
                {
                    return;
                }

                // Need WhatNew?
                if (await App.Current.Services.GetService<IWhatsNewDisplayService>().ShowIfAppropriate(nameof(WhatsNewPage)))
                {
                    return;
                }

                if (await App.Current.Services.GetService<IDatabaseReloadDisplayService>().ShowIfAppropriate(nameof(NeedDatabaseReloadPage)))
                {
                    SharedSharpSettings.DataSerialised = false;

                    return;
                }

                // Setup Event Handling
                //App.Current.Services.GetService<IMessenger>().Register<SSharpMessageWindowSizeChanged>(this, (r, m) =>
                //{
                //    if (m.Value == null)
                //        return;

                //    SharedSharpSizes.WindowSize = m.Value;
                //    SharedSharpCardSizes.Current.ReCalculateCardWidths();
                //});

                //App.Current.Services.GetService<IMessenger>().Register<SSharpMessageOrientationChange>(this, (r, m) =>
                //{
                //    if (m == null)
                //        return;

                //    SharedSharpCardSizes.Current.ReCalculateCardWidths();
                //});

                // Load da data
                await LoadData().ConfigureAwait(false);

                //// Action startup parameters
                //if (App.Current.StartUpParameterUri != null)
                //{
                //    HyperLinkCard parameterURICard = new HyperLinkCard();
                //    parameterURICard.Create();
                //    parameterURICard.Title = DateTime.UtcNow.ToString();
                //    parameterURICard.theLink = App.Current.StartUpParameterUri;

                //    await App.Current.Services.GetService<IDataStore>().ItemAddShareTargetAsync(parameterURICard);
                //}
            }
            catch (Exception ex)
            {
                App.Current.Services.GetService<IErrorNotifications>().NotifyException("AppInit.Init", ex);

                throw;
            }
        }

        private async Task LoadData()
        {
            try
            {
                if (SharedSharpSettings.DataSerialised)
                {
                    // App.Current.Services.GetService<IDataRepositoryManager>().StartDataLoad();
                    App.Current.Services.GetService<IMessenger>().Send(new DataLoadStartEvent(true));
                    return;
                }

                // No Serialised Data and made it this far so some problem has occurred. Load
                // everything from the beginning.
                await SharedSharpNavigation.NavigateAsync(nameof(FileInputHandlerPage));
            }
            catch (Exception ex)
            {
                App.Current.Services.GetService<IErrorNotifications>().NotifyException("AppInit.LoadData", ex);

                throw;
            }
        }
    }
}