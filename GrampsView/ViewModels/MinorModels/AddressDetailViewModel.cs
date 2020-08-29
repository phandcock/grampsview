﻿// <copyright file="AddressDetailViewModel.cs" company="MeMyselfAndI">
//     Copyright (c) MeMyselfAndI. All rights reserved.
// </copyright>
namespace GrampsView.ViewModels
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using Prism.Navigation;

    using System.Diagnostics.Contracts;
    using System.Threading.Tasks;

    using static GrampsView.Common.CommonEnums;

    /// <summary>
    /// ViewModel for the Address Detail page.
    /// </summary>
    public class AddressDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// The current address.
        /// </summary>
        private AddressModel _AddressObject = new AddressModel();

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// The common logging service.
        /// </param>
        public AddressDetailViewModel(ICommonLogging iocCommonLogging)
            : base(iocCommonLogging)
        {
            BaseTitle = "Address Detail";
            BaseTitleIcon = CommonConstants.IconAddress;
        }

        /// <summary>
        /// Gets or sets the View Current Person.
        /// </summary>
        /// <value>
        /// The current person ViewModel.
        /// </value>
        public AddressModel AddressObject
        {
            get
            {
                return _AddressObject;
            }

            set
            {
                SetProperty(ref _AddressObject, value);
            }
        }

        /// <summary>
        /// Called when [navigating from].
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void OnNavigatingFrom(INavigationParameters parameters)
        {
            OnNavigatedFrom(parameters);
        }

        /// <summary>
        /// Populates the view ViewModel.
        /// </summary>
        /// <returns>
        /// </returns>
        public override void PopulateViewModel()
        {
            BaseCL.LogRoutineEntry("AddressDetailViewModel");

            AddressObject = DV.AddressDV.GetModelFromHLink(BaseNavParamsHLink);

            if (AddressObject.Valid)
            {
                BaseTitle = AddressObject.GetDefaultText;

                // Get media image
                HLinkHomeImageModel AddressImage = AddressObject.HomeImageHLink;
                AddressImage.CardType = DisplayFormat.MediaCardLarge;
                BaseDetail.Add(AddressImage);

                // Get Header Details
                CardGroup headerCardGroup = new CardGroup { Title = "Address Details" };

                // Get the Name Details
                CardListLineCollection nameDetails = new CardListLineCollection
                {
                    new CardListLine("Card Type:", "Address Detail"),

                    new CardListLine("Street:", AddressObject.GStreet),
                    new CardListLine("City:", AddressObject.GCity),
                    new CardListLine("Locality:", AddressObject.GLocality),
                    new CardListLine("County:", AddressObject.GCounty),
                    new CardListLine("State:", AddressObject.GState),
                    new CardListLine("Country:", AddressObject.GCountry),

                    new CardListLine("Date:", AddressObject.GDate.ShortDate),
                    new CardListLine("Postal:", AddressObject.GPostal),
                    new CardListLine("Phone:", AddressObject.GPhone),
                };


                // Get date card
                headerCardGroup.Add(AddressObject.GDate.AsCardListLine());

                headerCardGroup.Add(nameDetails);

                // Add Standard details
                headerCardGroup.Add(DV.PersonDV.GetModelInfoFormatted(AddressObject));

                // Add map card
                headerCardGroup.Add(TurnAddressToURLModel());

                // Add header
                BaseDetail.Add(headerCardGroup);

                BaseDetail.Add(AddressObject.GCitationRefCollection.GetCardGroup());
                BaseDetail.Add(AddressObject.GNoteRefCollection.GetCardGroup());

                BaseDetail.Add(AddressObject.BackHLinkReferenceCollection.GetCardGroup());
            }

            return;
        }

        private URLModel TurnAddressToURLModel()
        {
            URLModel mapModel = new URLModel
            {
                GDescription = AddressObject.GetDefaultText,
                URLType = URIType.Map,
            };

            mapModel.MapLocation.Thoroughfare = AddressObject.GStreet;
            mapModel.MapLocation.Locality = AddressObject.GCity;
            mapModel.MapLocation.AdminArea = AddressObject.GState;
            mapModel.MapLocation.CountryName = AddressObject.GCountry;

            return mapModel;
        }
    }
}