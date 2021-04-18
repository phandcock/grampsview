﻿namespace GrampsView.Data.ExternalStorage
{
    using GrampsView.Common.CustomClasses;
    using GrampsView.Data.Model;
    using GrampsView.Data.Repository;

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    /// <summary>
    /// Loads BookMark XML.
    /// </summary>
    public partial class StoreXML : IStoreXML
    {
        /// <summary>
        /// Loads the BookMark data asynchronous.
        /// </summary>
        /// <returns>
        /// True if loaded ok.
        /// </returns>
        public async Task LoadBookMarksAsync()
        {
            await DataStore.Instance.CN.DataLogEntryAdd("Loading BookMark data").ConfigureAwait(false);
            {
                try
                {
                    // Run query
                    var de =
                        from el in localGrampsXMLdoc.Descendants(ns + "bookmark")
                        select el;

                    // set BookMark count field
                    int bookMarkCount = 0;

                    // Loop through results
                    foreach (XElement argBookMark in de)
                    {
                        // BookMark Handle
                        bookMarkCount++;

                        // BookMark fields
                        string GTarget = GetAttribute(argBookMark.Attribute("target"));
                        HLinkKey GHLink = new HLinkKey(GetAttribute(argBookMark.Attribute("hlink")));
                        HLinkBackLink newHlinkBackLink = SetBookMarkTarget(GTarget, GHLink);

                        if (newHlinkBackLink.Valid)
                        {
                            DataStore.Instance.DS.BookMarkCollection.Add(newHlinkBackLink);
                        }
                        else
                        {
                            ErrorInfo t = new ErrorInfo("Bad BookMark")
                                {
                                    { "HLink",  argBookMark.ToString()}
                                };

                            await DataStore.Instance.CN.NotifyError(t);
                        }

                        await DataStore.Instance.CN.DataLogEntryReplace(string.Format("Loading bookmark: {0}", newHlinkBackLink.HLinkType));
                    }

                    DataStore.Instance.DS.BookMarkCollection.Title = string.Empty;
                }
                catch (Exception e)
                {
                    // TODO handle this
                    await DataStore.Instance.CN.DataLogEntryAdd(e.Message).ConfigureAwait(false);

                    throw;
                }
            }

            await DataStore.Instance.CN.DataLogEntryReplace("Bookmark load complete").ConfigureAwait(false);

            return;
        }

        public HLinkBackLink SetBookMarkTarget(string argGTarget, HLinkKey argHLinkKey)
        {
            switch (argGTarget)
            {
                case "person":
                    {
                        HLinkPersonModel p1 = new HLinkPersonModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "family":
                    {
                        HLinkFamilyModel p1 = new HLinkFamilyModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "event":
                    {
                        HLinkEventModel p1 = new HLinkEventModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "source":
                    {
                        HLinkSourceModel p1 = new HLinkSourceModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "citation":
                    {
                        HLinkCitationModel p1 = new HLinkCitationModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "place":
                    {
                        HLinkPlaceModel p1 = new HLinkPlaceModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "media":
                    {
                        HLinkMediaModel p1 = new HLinkMediaModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "repository":
                    {
                        HLinkRepositoryModel p1 = new HLinkRepositoryModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }

                case "note":
                    {
                        HLinkNoteModel p1 = new HLinkNoteModel
                        {
                            HLinkKey = argHLinkKey
                        };

                        return new HLinkBackLink(p1);
                    }
            }

            return new HLinkBackLink();
        }
    }
}