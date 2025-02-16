﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Data.DataView;
using GrampsView.Data.StoreDB;
using GrampsView.Data.StoreXML;
using GrampsView.DBModels;
using GrampsView.Models.HLinks.Models;

using System.Diagnostics;
using System.Xml.Linq;

using static GrampsView.Common.CommonEnums;

namespace GrampsView.Data.ExternalStorage
{
    public partial class StoreXML : IStoreXML
    {
        public async Task LoadEventsAsync()
        {
            MyLog.DataLogEntryAdd("Loading Event data");
            {
                try
                {
                    // Run query
                    System.Collections.Generic.IEnumerable<XElement> de =
                        from el in LocalGrampsXMLdoc.Descendants(ns + "event")
                        select el;

                    // get event fields TODO

                    // Loop through results to get the Persons Uri _baseUri = new Uri("ms-appx:///");
                    foreach (XElement pname in de)
                    {
                        EventDBModel loadEvent = new();

                        // Event attributes
                        loadEvent.LoadBasics(GetDBBasics(pname));

                        if (loadEvent.Id == "E0714")
                        {
                        }

                        // Event fields
                        loadEvent.GAttribute = GetAttributeCollection(pname);

                        loadEvent.GCitationRefCollection = GetCitationCollection(pname);

                        loadEvent.GDate = SetDBDate(pname);

                        loadEvent.GDescription = GetElement(pname.Element(ns + "description"));

                        loadEvent.GMediaRefCollection = await GetObjectCollection(pname).ConfigureAwait(false);

                        loadEvent.GNoteRefCollection = GetNoteCollection(pname);

                        XElement tt = pname.Element(ns + "place");
                        if (tt is not null)
                        {
                            HLinkPlaceModel ttt = new()
                            {
                                HLinkKey = GetHLinkKey(tt)
                            };
                            loadEvent.GPlace = ttt;
                        }

                        loadEvent.GTagRefCollection = GetTagCollection(pname);

                        loadEvent.GType = GetElement(pname.Element(ns + "type"));

                        loadEvent.EventType = EventModelType.UNKNOWN;
                        if (Enum.TryParse(loadEvent.GType, out EventModelType loadEventType))
                        {
                            loadEvent.EventType = loadEventType;
                        }

                        // save the event
                        EventDBModel t = new EventDBModel(loadEvent);
                        Debug.WriteLine($"Event {t.HLinkKeyValue}");
                        Debug.WriteLine($"Date {t.GDate.HLinkKeyValue}");
                        DL.EventDL.EventAccess.Add(t);
                    }
                }
                catch (Exception ex)
                {
                    MyNotifications.NotifyException("LoadEventsAsync", ex);
                }
            }

            Ioc.Default.GetRequiredService<IStoreDB>().SaveChanges();

            MyLog.DataLogEntryReplace("Event load complete");
            return;
        }
    }
}