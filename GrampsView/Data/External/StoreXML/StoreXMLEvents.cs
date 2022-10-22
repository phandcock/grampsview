﻿namespace GrampsView.Data.ExternalStorage
{
    using GrampsView.Data.DataView;
    using GrampsView.Models.DataModels;
    using GrampsView.Models.HLinks.Models;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using static GrampsView.Common.CommonEnums;

    public partial class StoreXML : IStoreXML
    {
        public async Task LoadEventsAsync()
        {
            _iocCommonNotifications.DataLogEntryAdd("Loading Event data");
            {
                try
                {
                    // Run query
                    var de =
                        from el in localGrampsXMLdoc.Descendants(ns + "event")
                        select el;

                    // get event fields TODO

                    // Loop through results to get the Persons Uri _baseUri = new Uri("ms-appx:///");
                    foreach (XElement pname in de)
                    {
                        EventModel loadEvent = new EventModel();

                        // Event attributes
                        loadEvent.LoadBasics(GetBasics(pname));

                        if (loadEvent.Id == "E0001")
                        {
                        }

                        // Event fields
                        loadEvent.GAttribute = GetAttributeCollection(pname);

                        loadEvent.GCitationRefCollection = GetCitationCollection(pname);

                        loadEvent.GDate = SetDate(pname);

                        loadEvent.GDescription = GetElement(pname.Element(ns + "description"));

                        loadEvent.GMediaRefCollection = await GetObjectCollection(pname).ConfigureAwait(false);

                        loadEvent.GNoteRefCollection = GetNoteCollection(pname);

                        XElement tt = pname.Element(ns + "place");
                        if (!(tt is null))
                        {
                            HLinkPlaceModel t = new HLinkPlaceModel();
                            t.HLinkKey = GetHLinkKey(tt);
                            loadEvent.GPlace = t;
                        }

                        loadEvent.GTagRefCollection = GetTagCollection(pname);

                        loadEvent.GType = GetElement(pname.Element(ns + "type"));

                        loadEvent.EventType = EventModelType.UNKNOWN;
                        if (Enum.TryParse(loadEvent.GType, out EventModelType loadEventType))
                        {
                            loadEvent.EventType = loadEventType;
                        }

                        // save the event
                        DV.EventDV.EventData.Add(loadEvent);
                    }
                }
                catch (Exception e)
                {
                    // TODO handle this
                    _iocCommonNotifications.DataLogEntryAdd(e.Message);

                    _iocCommonNotifications.NotifyException("LoadEventsAsync", e);

                    throw;
                }
            }

            _iocCommonNotifications.DataLogEntryReplace("Event load complete");
            return;
        }
    }
}