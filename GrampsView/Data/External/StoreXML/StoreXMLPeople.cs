﻿namespace GrampsView.Data.ExternalStorage
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using Microsoft.Extensions.DependencyInjection;

    using SharedSharp.Interfaces;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// People load Routines.
    /// </summary>
    public partial class StoreXML : IStoreXML
    {
        /// <summary>
        /// Load the person data from the external storage XML file.
        /// </summary>
        /// <returns>
        /// Flag indicating if people data loaded successfully.
        /// </returns>
        public async Task LoadPeopleDataAsync()
        {
            _iocCommonLogging.RoutineEntry("LoadPeopleDataAsync");

            App.Current.Services.GetService<IErrorNotifications>().DataLogEntryAdd("Loading People data");
            {
                string defaultImage = string.Empty;

                // Run query
                var de =
                    from el in localGrampsXMLdoc.Descendants(ns + "person")
                    select el;

                try
                {
                    foreach (XElement pname in de)
                    {
                        PersonModel loadPerson = new PersonModel();

                        // Person attributes
                        loadPerson.LoadBasics(GetBasics(pname));

                        if (loadPerson.Id == "I1138")
                        {
                        }

                        // Address
                        loadPerson.GAddressCollection = GetAddressCollection(pname);

                        // Get attribute collection
                        loadPerson.GAttributeCollection = GetAttributeCollection(pname);

                        // Childof
                        XElement tempChildOf = pname.Element(ns + "childof");
                        if (tempChildOf != null)
                        {
                            // Force glyph valid while loading
                            loadPerson.GChildOf.HLinkGlyphItem.ImageType = CommonEnums.HLinkGlyphType.TempLoading;
                            loadPerson.GChildOf.HLinkKey = GetHLinkKey(tempChildOf);
                        }

                        // CitationRef collection
                        loadPerson.GCitationRefCollection = GetCitationCollection(pname);

                        // EventRef
                        loadPerson.GEventRefCollection = GetEventCollection(pname);

                        // gender
                        switch (GetElement(pname, "gender"))
                        {
                            case "F":
                                {
                                    loadPerson.GGender = CommonEnums.Gender.Female;
                                    break;
                                }

                            case "M":
                                {
                                    loadPerson.GGender = CommonEnums.Gender.Male;
                                    break;
                                }

                            default:
                                {
                                    loadPerson.GGender = CommonEnums.Gender.Unknown;
                                    break;
                                }
                        }

                        // TODO load LDS collection

                        // media object collection loading
                        loadPerson.GMediaRefCollection = await GetObjectCollection(pname).ConfigureAwait(false);

                        // Name
                        loadPerson.GPersonNamesCollection = GetPersonNameCollection(pname);

                        // NoteRefs Collection
                        loadPerson.GNoteRefCollection = GetNoteCollection(pname);

                        // Parentin
                        IEnumerable<XElement> localPIElement =
                            from pIElementEl in pname.Descendants(ns + "parentin")
                            select pIElementEl;

                        if (localPIElement.Any())
                        {
                            // load parentIn references
                            foreach (XElement loadPIElement in localPIElement)
                            {
                                HLinkFamilyModel t = new HLinkFamilyModel
                                {
                                    HLinkKey = GetHLinkKey(loadPIElement),
                                };

                                loadPerson.GParentInRefCollection.Add(t);
                            }

                            loadPerson.GParentInRefCollection.SetFirstImage();
                        }

                        // PersonRef
                        loadPerson.GPersonRefCollection = GetPersonRefCollection(pname);

                        // TagRef
                        loadPerson.GTagRefCollection = GetTagCollection(pname);

                        // URL
                        loadPerson.GURLCollection = GetURLCollection(pname);

                        // load the person
                        DV.PersonDV.PersonData.Add(loadPerson);
                    }

                    // let everybody know
                    _iocCommonLogging.RoutineExit("loadPeopleData");
                }
                catch (Exception ex)
                {
                    if (DV.PersonDV.PersonData.Count > 0)
                    {
                        // TODO Add this back + DV.PersonDV.PersonData[DV.PersonDV.PersonData.Count].GPersonNamesCollection.GetPrimaryName.FullName
                        App.Current.Services.GetService<IErrorNotifications>().NotifyException("Loading person from GRAMPSXML storage.  The last person successfully loaded was ", ex);
                        throw;
                    }
                    else
                    {
                        App.Current.Services.GetService<IErrorNotifications>().NotifyException("Loading people from GRAMPSXML storage.  No people have been loaded", ex);
                        throw;
                    }
                }
            }

            //var tt = (DataStore.Instance.DS.PersonNameData.Where(x => x.Value.GSurName.GetPrimarySurname == "Ainger"));
            //if (tt.Count() > 0)
            //{
            //}

            App.Current.Services.GetService<IErrorNotifications>().DataLogEntryReplace("People load complete");

            return;
        }
    }
}