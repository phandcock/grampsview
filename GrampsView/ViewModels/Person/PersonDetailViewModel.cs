﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Common.CustomClasses;
using GrampsView.Data.Collections;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;
using GrampsView.DBModels;
using GrampsView.Models.Collections.HLinks;
using GrampsView.Models.DataModels;
using GrampsView.ModelsDB.Collections.HLinks;
using GrampsView.ModelsDB.HLinks.Models;

using System.ComponentModel;

namespace GrampsView.ViewModels.Person
{
    /// <summary>
    /// ViewModel for the Person Detail page.
    /// </summary>
    public class PersonDetailViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PersonDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// The common logging service.
        /// </param>
        [Obsolete]
        public PersonDetailViewModel(ILog iocCommonLogging)
            : base(iocCommonLogging)
        {
            BaseTitleIcon = Constants.IconPeople;
        }

        public HLinkNoteDBModel BioNote
        {
            get; set;
        } = new HLinkNoteDBModel();

        /// <summary>
        /// Gets the person's events and those of any families they were in.
        /// </summary>
        /// <returns>
        /// CardGroup
        /// </returns>
        public HLinkEventDBModelCollection EventsIncFamily
        {
            get
            {
                // Get the personal events
                HLinkEventDBModelCollection t = new();

                t.AddRange(PersonObject.GEventRefCollection);

                // Get Family events
                foreach (HLinkFamilyDBModel families in PersonObject.GParentInRefCollection)
                {
                    foreach (HLinkEventDBModel familyEvent in families.DeRef.GEventRefCollection)
                    {
                        t.Add(familyEvent);
                    }
                }

                // TODO fix    t.Sort(x => x.DeRef.GDate.SortDate);

                return t;
            }
        }

        public ItemGlyph MediaCard
        {
            get; set;
        }

                = new ItemGlyph();

        public HLinkNoteDBModelCollection NotesWithoutHighlight
        {
            get; set;
        } = new HLinkNoteDBModelCollection();

        public HLinkPersonNameModelCollection PersonNameMultipleDetails =>
                // If only one name then its already displayed in the detail section
                PersonObject.GPersonNamesCollection.Count == 1 ? new HLinkPersonNameModelCollection() : PersonObject.GPersonNamesCollection;

        /// <summary>
        /// Gets or sets the Person to be shown on the page.
        /// </summary>
        /// <value>
        /// The current person ViewModel.
        /// </value>
        public PersonModel PersonObject
        {
            get; set;
        }
        = new PersonModel();

        /// <summary>
        /// Populates the view ViewModel.
        /// </summary>
        /// <returns>
        /// </returns>
        public override void HandleViewModelParameters()
        {
            BaseCL.RoutineEntry("PersonDetailViewModel");

            if (base.NavigationParameter is not null && base.NavigationParameter.Valid)
            {
                HLinkPersonModel HLinkObject = base.NavigationParameter as HLinkPersonModel;

                PersonObject = HLinkObject.DeRef;

                if (PersonObject is not null)
                {
                    BaseModelBase = PersonObject;

                    // Get media image
                    MediaCard = PersonObject.ModelItemGlyph;

                    BaseDetail.Clear();

                    // Get the Name Details
                    BaseDetail.Add(PersonObject.GPersonNamesCollection.GetPrimaryName);

                    // Get the Person Details
                    CardListLineCollection nameDetails = GetExtraPersonDetails();
                    nameDetails.Title = "Person Detail";
                    BaseDetail.Add(nameDetails);

                    // Get date card
                    if (PersonObject.BirthDate.Valid)
                    {
                        BaseDetail.Add(PersonObject.BirthDate.AsHLink("Birth Date"));
                    }

                    // Add Standard details
                    BaseDetail.Add(DV.PersonDV.GetModelInfoFormatted(PersonObject));

                    // Get Family Graph details
                    BaseDetail.Add(
                        new HLinkFamilyGraphModel
                        {
                            DeRef = PersonObject,
                        });

                    // If Bio note, display it while showing the full list further below.
                    BioNote = PersonObject.GNoteRefCollection.GetBio;

                    NotesWithoutHighlight = PersonObject.GNoteRefCollection.GetCollectionWithoutOne(BioNote);

                    // Add PersonRefDetails - TODO
                    //if (BaseNavParamsHLink is HLinkPersonRefModel)
                    //{
                    //    HLinkPersonRefModel personRef = (BaseNavParamsHLink as HLinkPersonRefModel);

                    // Contract.Assert(personRef != null);

                    //    BaseDetail.Add(personRef.GCitationCollection.GetCardGroup("PersonRef Citations"));
                    //    BaseDetail.Add(personRef.GNoteCollection.GetCardGroup("PersonRef Notes"));
                    //}
                }

                return;
            }
        }

        private CardListLineCollection GetExtraPersonDetails()
        {
            // Get extra details
            CardListLineCollection extraDetailsCard = new()
            {
                        new CardListLine("Gender:", PersonObject.GGender.ToString()),
                };

            if (PersonObject.BirthDate.Valid)
            {
                if (PersonObject.IsLiving)
                {
                    extraDetailsCard.Add(new CardListLine("Age:", PersonObject.BirthDate.GetAge));
                }
                else
                {
                    extraDetailsCard.Add(new CardListLine("Years Since Birth:", PersonObject.BirthDate.GetAge));

                    EventDBModel ageAtDeath = DL.EventDL.GetEventType(PersonObject.GEventRefCollection, "Death");
                    if (ageAtDeath.Valid)
                    {
                        extraDetailsCard.Add(new CardListLine("Age at Death:", ageAtDeath.GDate.DateDifferenceDecoded(PersonObject.BirthDate)));
                    }
                }
            }
            else
            {
                extraDetailsCard.Add(new CardListLine("Birth Date:", "Unknown"));
            }

            extraDetailsCard.Add(new CardListLine("Is Living:", PersonObject.IsLivingAsString));

            return extraDetailsCard;
        }
    }
}
