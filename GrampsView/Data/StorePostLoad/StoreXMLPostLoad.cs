﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;
using GrampsView.Data.Repository;
using GrampsView.Data.StoreDB;
using GrampsView.Data.StorePostLoad;
using GrampsView.DBModels;
using GrampsView.Models.DataModels;
using GrampsView.Models.DataModels.Minor;
using GrampsView.Models.HLinks.Models;
using GrampsView.ModelsDB.HLinks.Models;

namespace GrampsView.Data.ExternalStorage
{
    /// <summary>
    /// Creates a collection of entities with content read from a GRAMPS XML file.
    /// </summary>
    public partial class StorePostLoad : ObservableObject, IStorePostLoad
    {
        /// <summary>
        /// Organises the address repository.
        /// </summary>
        private bool OrganiseAddressRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Address data");

            foreach (AddressDBModel argModel in DL.AddressDL.DataAsList)
            {
                argModel.GCitationRefCollection.SetGlyph();

                // Citation Collection
                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetAddressImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        private bool OrganiseBookMarkRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising BookMark data");

            DV.BookMarkCollection.SetGlyph();

            return true;
        }

        /// <summary>
        /// Organises the Citation Repository.
        /// </summary>
        private bool OrganiseCitationRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Citation data");

            foreach (CitationDBModel argModel in DL.CitationDL.DataAsList)
            {
                if (argModel.Id == "C0144")
                {
                }

                if (argModel.HLinkKey.Value == "_e672fba539b3fa55a9523e1d52c")
                {
                }

                argModel.GNoteRefCollection.SetGlyph();

                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                // Media Collection - Create backlinks in media models to citation models
                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    //mediaRef.HLinkGlyphItem = DV.MediaDV.GetGlyph(mediaRef.HLinkKey);

                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));

                    // Save to original Hlink as well
                    if (mediaRef.OriginalMediaHLink.Valid)
                    {
                        DataStore.Instance.DS.MediaData[mediaRef.OriginalMediaHLink.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                    }
                }

                // Note Collection
                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;

                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Source Link
                argModel.GSourceRef.HLinkGlyphItem = DV.SourceDV.GetGlyph(argModel.GSourceRef.HLinkKey);

                DataStore.Instance.DS.SourceData[argModel.GSourceRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));

                // Tag Collection
                argModel.GTagRef.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRef)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            // TODO finish adding the collections to the backlinks
            SetCitationImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the event repository.
        /// </summary>
        private bool OrganiseEventRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Event data");

            foreach (EventDBModel argModel in DL.EventDL.DataAsList)
            {
                argModel.GCitationRefCollection.SetGlyph();

                // Citation Collection
                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Place Collection
                if (argModel.GPlace.Valid)
                {
                    DataStore.Instance.DS.PlaceData[argModel.GPlace.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                // Media Collection
                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    //mediaRef.HLinkGlyphItem = DV.MediaDV.GetGlyph(mediaRef.HLinkKey);

                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // NoteDBModel Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;

                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetEventImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the family repository.
        /// </summary>
        private bool OrganiseFamilyRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Family data ");

            foreach (FamilyDBModel argModel in DL.FamilyDL.DataAsList)
            {
                // Child Collection
                argModel.GChildRefCollection.SetGlyph();

                foreach (HLinkChildRefModel childRef in argModel.GChildRefCollection)
                {
                    DataStore.Instance.DS.PersonData[childRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Citation Collection
                argModel.GCitationRefCollection.SetGlyph();

                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Parents
                argModel.GFather.HLinkGlyphItem = argModel.GFather.DeRef.ModelItemGlyph;
                argModel.GMother.HLinkGlyphItem = argModel.GMother.DeRef.ModelItemGlyph;

                // Refresh the glyphs
                if (argModel.GFather.Valid)
                {
                    argModel.GFather = DataStore.Instance.DS.PersonData[argModel.GFather.HLinkKey.Value].HLink;
                }
                if (argModel.GMother.Valid)
                {
                    argModel.GMother = DataStore.Instance.DS.PersonData[argModel.GMother.HLinkKey.Value].HLink;
                }

                // EventModel Collection
                argModel.GEventRefCollection.SetGlyph();

                foreach (HLinkEventDBModel eventRef in argModel.GEventRefCollection)
                {
                    IQueryable<EventDBModel> ttt = DL.EventDL.EventAccess.Where(x => x.HLinkKeyValue == eventRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        EventDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                // Media Collection
                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    //mediaRef.HLinkGlyphItem = DV.MediaDV.GetGlyph(mediaRef.HLinkKey);

                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Note Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;
                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetFamilyImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the header repository.
        /// </summary>
        private bool OrganiseHeaderRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Header data");

            SetHeaderImages();

            return true;
        }

        /// <summary>
        /// Organises the media repository.
        /// </summary>
        private bool OrganiseMediaRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Media data");

            try
            {
                foreach (MediaModel argModel in DV.MediaDV.DataViewData)
                {
                    if (argModel.Id == "O0204")
                    {
                    }

                    // Citation Collection
                    argModel.GCitationRefCollection.SetGlyph();

                    foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                    {
                        IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                        if (ttt.Any())
                        {
                            CitationDBModel t = ttt.First();
                            t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                            ttt.First();
                        }
                    }

                    // Note Collection
                    argModel.GNoteRefCollection.SetGlyph();

                    // Back Reference Note HLinks
                    foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                    {
                        NoteDBModel ttt = noteRef.DeRef;

                        NoteDBModel t = ttt;
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                    }

                    // Tag Collection
                    argModel.GTagRefCollection.SetGlyph();

                    foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                    {
                        DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                    }

                    argModel.BackHLinkReferenceCollection.Sort();
                }
            }
            catch (Exception ex)
            {
                _commonNotifications.NotifyException("Exception in OrganiseMediaRepository", ex);
            }

            _ = SetMediaImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises misc items pending use of a dependency graph.
        /// </summary>
        private bool OrganiseMisc()
        {
            _CommonLogging.DataLogEntryAdd("Organising Misc data");

            // Family children
            foreach (FamilyDBModel argModel in DL.FamilyDL.DataAsList)
            {
                // Children Collection
                argModel.GChildRefCollection.SetGlyph();
            }

            SetAddressImages();

            return true;
        }

        /// <summary>
        /// Organises the namemap repository.
        /// </summary>
        private bool OrganiseNameMapRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising NameMap data");

            SetNameMapImages();

            return true;
        }

        /// <summary>
        /// Organises the note repository.
        /// </summary>
        private bool OrganiseNoteRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Note data");

            foreach (NoteDBModel argModel in DL.NoteDL.DataAsList)
            {
                // Note Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetNotesImages();

            return true;
        }

        private bool OrganisePersonNameRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Person Name data");

            foreach (PersonNameModel argModel in DV.PersonNameDV.DataViewData)
            {
                argModel.GNoteReferenceCollection.SetGlyph();

                // Citation Collection
                argModel.GCitationRefCollection.SetGlyph();

                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    citationRef.HLinkGlyphItem = DL.CitationDL.GetGlyph(citationRef.HLinkKey);

                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Note Collection
                foreach (HLinkNoteDBModel noteRef in argModel.GNoteReferenceCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;

                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetPersonNameImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the person repository.
        /// </summary>
        private bool OrganisePersonRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Person data");

            foreach (PersonModel argModel in DV.PersonDV.DataViewData)
            {
                if (argModel.Id == "I0469")
                {
                }

                // Address Collection
                argModel.GAddressCollection.SetGlyph();

                foreach (HLinkAddressDBModel addressRef in argModel.GAddressCollection)
                {
                    DataStore.Instance.DS.AddressData[addressRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Citation Collection
                argModel.GCitationRefCollection.SetGlyph();

                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Event Collection
                argModel.GEventRefCollection.SetGlyph();

                foreach (HLinkEventDBModel eventRef in argModel.GEventRefCollection)
                {
                    IQueryable<EventDBModel> ttt = DL.EventDL.EventAccess.Where(x => x.HLinkKeyValue == eventRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        EventDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Family Collection

                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Note Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;

                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Parent In Collection
                argModel.GParentInRefCollection.SetGlyph();

                // PersonName Collection
                argModel.GPersonNamesCollection.SetGlyph();

                // PersonName Collection
                foreach (HLinkPersonNameModel personNameRef in argModel.GPersonNamesCollection)
                {
                    DataStore.Instance.DS.PersonNameData[personNameRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // --
                if (argModel.GChildOf.Valid)
                {
                    argModel.GChildOf = DL.FamilyDL.GetModelFromHLink(argModel.GChildOf).HLink;
                }

                // set Birthdate
                EventDBModel birthDate = DL.EventDL.GetEventType(argModel.GEventRefCollection, Constants.EventTypeBirth);
                if (birthDate.Valid)
                {
                    argModel.BirthDate.NotionalDate = birthDate.GDate.NotionalDate;
                }

                // set Is Living
                argModel.IsLiving = !DL.EventDL.GetEventType(argModel.GEventRefCollection, Constants.EventTypeDeath).Valid;

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    // Set the backlinks
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();

                // DataStore.Instance.DS.PersonData[argModel.HLinkKey.Value] = argModel;
            }

            SetPersonImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the place repository.
        /// </summary>
        private bool OrganisePlaceRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Place data");

            foreach (PlaceModel argModel in DV.PlaceDV.DataViewData)
            {
                argModel.GPlaceParentCollection.SetGlyph();

                foreach (HLinkPlaceModel placeRef in argModel.GPlaceParentCollection)
                {
                    DataStore.Instance.DS.PlaceData[placeRef.HLinkKey.Value].PlaceChildCollection.Add(argModel.HLink);
                }

                // Citation Collection
                argModel.GCitationRefCollection.SetGlyph();

                foreach (HLinkCitationDBModel citationRef in argModel.GCitationRefCollection)
                {
                    IQueryable<CitationDBModel> ttt = DL.CitationDL.CitationAccess.Where(x => x.HLinkKeyValue == citationRef.HLinkKey.Value);

                    if (ttt.Any())
                    {
                        CitationDBModel t = ttt.First();
                        t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                        ttt.First();
                    }
                }

                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Note Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;
                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            // Now that all of the Enclosed Places have been added

            foreach (PlaceModel thePlaceModel in DV.PlaceDV.DataViewData)
            {
                thePlaceModel.PlaceChildCollection.SetGlyph();

                // Sort anyway into alphabetic order
                thePlaceModel.PlaceChildCollection.Sort();
            }

            SetPlaceImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the repository repository.
        /// </summary>
        private bool OrganiseRepositoryRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Repository data");

            foreach (RepositoryModel argModel in DV.RepositoryDV.DataViewData)
            {
                // Note Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;

                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetRepositoryImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the source repository backlinks.
        /// - XML 1.71 Completed
        /// </summary>
        /// <returns>
        /// true if the organisation worked.
        /// </returns>
        private bool OrganiseSourceRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Source data");

            foreach (SourceModel argModel in DV.SourceDV.DataViewData)
            {
                // Media Collection
                argModel.GMediaRefCollection.SetGlyph();

                foreach (HLinkMediaModel mediaRef in argModel.GMediaRefCollection)
                {
                    DataStore.Instance.DS.MediaData[mediaRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Note Collection
                argModel.GNoteRefCollection.SetGlyph();

                foreach (HLinkNoteDBModel noteRef in argModel.GNoteRefCollection)
                {
                    NoteDBModel ttt = noteRef.DeRef;
                    NoteDBModel t = ttt;
                    t.BackHLinkReferenceCollection.Add(new HLinkDBBackLink(argModel.HLink));
                }

                // Repository Ref Collection
                argModel.GRepositoryRefCollection.SetGlyph();

                foreach (HLinkRepositoryRefModel repositoryRef in argModel.GRepositoryRefCollection)
                {
                    DataStore.Instance.DS.RepositoryData[repositoryRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                // Tag Collection
                argModel.GTagRefCollection.SetGlyph();

                foreach (HLinkTagModel tagRef in argModel.GTagRefCollection)
                {
                    DataStore.Instance.DS.TagData[tagRef.HLinkKey.Value].BackHLinkReferenceCollection.Add(new HLinkBackLink(argModel.HLink));
                }

                argModel.BackHLinkReferenceCollection.Sort();
            }

            SetSourceImages();

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            return true;
        }

        /// <summary>
        /// Organises the tag repository.
        /// </summary>
        private bool OrganiseTagRepository()
        {
            _CommonLogging.DataLogEntryAdd("Organising Tag data");

            SetTagImages();

            return true;
        }
    }
}