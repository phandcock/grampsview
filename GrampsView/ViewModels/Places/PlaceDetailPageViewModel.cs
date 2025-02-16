﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;
using GrampsView.Models.DataModels;
using GrampsView.Models.DataModels.Interfaces;
using GrampsView.Models.HLinks.Models;

namespace GrampsView.ViewModels.Places
{
    /// <summary>
    /// Defines the Place Detail Page View ViewModel.
    /// </summary>
    public class PlaceDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// </param>
        /// <param name="iocEventAggregator">
        /// The ioc event aggregator.
        /// </param>
        [Obsolete]
        public PlaceDetailViewModel(ILog iocCommonLogging, IMessenger iocEventAggregator)
            : base(iocCommonLogging)
        {
        }

        public PlaceModel PlaceObject
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the public Place ViewModel.
        /// </summary>
        /// <summary>
        /// Handles navigation inwards and sets up the place model parameter.
        /// </summary>
        public override void HandleViewModelParameters()
        {
            if (base.NavigationParameter is not null && base.NavigationParameter.Valid)
            {
                HLinkPlaceModel HLinkObject = base.NavigationParameter as HLinkPlaceModel;

                PlaceObject = HLinkObject.DeRef;

                if (PlaceObject != null)
                {
                    BaseModelBase = PlaceObject;
                    BaseTitleIcon = Constants.IconPlace;

                    // TODO Display all details

                    BaseDetail.Clear();

                    BaseDetail.Add(new CardListLineCollection("Place Detail")
                    {
                        new CardListLine("Title:", PlaceObject.GPName),
                        new CardListLine("Type:", PlaceObject.GType),
                        new CardListLine("Code:", PlaceObject.GCode),
                });

                    foreach (HLinkPlaceLocationModel thePlaceLocation in PlaceObject.GLocation)
                    {
                        BaseDetail.Add(new CardListLineCollection("Location")
                    {
                        new CardListLine("Street:", thePlaceLocation.DeRef.GStreet),
                        new CardListLine("City:", thePlaceLocation.DeRef.GCity),
                        new CardListLine("County:", thePlaceLocation.DeRef.GCounty),
                        new CardListLine("Locality:", thePlaceLocation.DeRef.GLocality),
                        new CardListLine("Parish:", thePlaceLocation.DeRef.GParish),
                        new CardListLine("State:", thePlaceLocation.DeRef.GState),
                        new CardListLine("Country:", thePlaceLocation.DeRef.GCountry),
                        new CardListLine("Phone:", thePlaceLocation.DeRef.GPhone),
                        new CardListLine("City:", thePlaceLocation.DeRef.GPostal),
                    });
                    }

                    if (PlaceObject.GCoordLat != 0 || PlaceObject.GCoordLong != 0)
                    {
                        BaseDetail.Add(new CardListLineCollection("Coordinates")
                    {
                        new CardListLine("Lat:", PlaceObject.GCoordLat),
                        new CardListLine("Long:", PlaceObject.GCoordLong),
                  });
                    }

                    if (HLinkObject.Date.Valid)
                    {
                        BaseDetail.Add(new CardListLineCollection("Place Date Ref")
                    {
                        new CardListLine("Date:", HLinkObject.Date.ShortDate),
                });
                    }

                    // Add Map card
                    IMapModel t = PlaceObject.ToMapModel();
                    BaseDetail.Add(t.HLink);

                    BaseDetail.Add(DV.PlaceDV.GetModelInfoFormatted(PlaceObject));
                }
            }
        }
    }
}