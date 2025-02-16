﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common.CustomClasses;
using GrampsView.Data.DataView;
using GrampsView.Data.StoreXML;
using GrampsView.DBModels;

using System.Diagnostics;
using System.Xml.Linq;

namespace GrampsView.Data.ExternalStorage
{
    public partial class StoreXML : IStoreXML
    {
        /// <summary>
        /// load families from external storage.
        /// </summary>
        /// <returns>
        /// Flag indicating if the family data was loaded.
        /// </returns>
        public async Task<bool> LoadFamiliesAsync()
        {
            // RepositoryModelType<FamilyModel, HLinkFamilyModel>
            MyLog.DataLogEntryAdd("Loading Family data");
            {
                // Load notes
                try
                {
                    // Run query
                    System.Collections.Generic.IEnumerable<XElement> de =
                        from el in LocalGrampsXMLdoc.Descendants(ns + "family")
                        select el;

                    // get family fields TODO

                    // Loop through results to get the Families
                    foreach (XElement familyElement in de)
                    {
                        FamilyDBModel loadFamily = new();

                        // Family attributes
                        loadFamily.LoadBasics(GetDBBasics(familyElement));

                        if (loadFamily.Id == "F0151")
                        {
                        }

                        // Family fields

                        // relationship type
                        XElement tempRelationship = familyElement.Element(ns + "rel");
                        if (tempRelationship != null)
                        {
                            loadFamily.GFamilyRelationship = (string)tempRelationship.Attribute("type");
                        }

                        // father element
                        XElement tempFather = familyElement.Element(ns + "father");
                        if (tempFather != null)
                        {
                            loadFamily.GFather.HLinkGlyphItem.ImageType = Common.CommonEnums.HLinkGlyphType.TempLoading;
                            loadFamily.GFather.HLinkKey = new HLinkKey((string)tempFather.Attribute("hlink"));
                        }

                        // mother element
                        XElement tempMother = familyElement.Element(ns + "mother");
                        if (tempMother != null)
                        {
                            loadFamily.GMother.HLinkGlyphItem.ImageType = Common.CommonEnums.HLinkGlyphType.TempLoading;
                            loadFamily.GMother.HLinkKey = new HLinkKey((string)tempMother.Attribute("hlink"));
                        }

                        loadFamily.GDate = GetDate(familyElement.Element(ns + "date"));

                        loadFamily.GAttributeCollection = GetAttributeCollection(familyElement);

                        loadFamily.GChildRefCollection = GetChildRefCollection(familyElement);

                        // Citation References
                        loadFamily.GCitationRefCollection = GetCitationCollection(familyElement);

                        // Event References
                        loadFamily.GEventRefCollection = GetEventCollection(familyElement);

                        loadFamily.GLDSOrdCollection = GetLDSOrdCollection(familyElement);

                        // ObjectRef loading
                        loadFamily.GMediaRefCollection = await GetObjectCollection(familyElement).ConfigureAwait(false);

                        loadFamily.GNoteRefCollection = GetNoteCollection(familyElement);

                        loadFamily.GTagRefCollection = GetTagCollection(familyElement);

                        // set the Home image or symbol now that everything is laoded loadFamily = SetHomeImage(loadFamily);

                        // save the family
                        FamilyDBModel t = new FamilyDBModel(loadFamily as FamilyDBModel);
                        Debug.WriteLine(t.HLinkKeyValue);
                        DL.FamilyDL.FamilyAccess.Add(t);
                    }
                }
                catch (Exception ex)
                {
                    MyNotifications.NotifyException("Load Families", ex);
                }
            }

            MyLog.DataLogEntryReplace("Family load complete");

            return true;
        }
    }
}