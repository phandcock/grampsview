﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Common.CustomClasses;
using GrampsView.Data.Model;
using GrampsView.Data.Repository;
using GrampsView.Models.HLinks;

using SharedSharp.Errors;
using SharedSharp.Errors.Interfaces;

using System.Runtime.Serialization;
using System.Text.Json;

namespace GrampsView.Data.Collections
{
    /// <summary>
    /// </summary>

    public class HLinkBackLinkDBModelCollection : HLinkDBBaseCollection<HLinkDBBackLink>
    {
        /// <summary>
        /// Returns a CardGroup of HLinkBase and not HLinkBackLink.
        /// </summary>
        /// <value>
        /// The card group as property.
        /// </value>
        public DBCardGroupHLink<HLinkDBBase> AsCardGroup
        {
            get
            {
                DBCardGroupHLink<HLinkDBBase> t = new DBCardGroupHLink<HLinkDBBase>();

                foreach (HLinkDBBackLink item in Items)
                {
                    item.HLink.DisplayAs = CommonEnums.DisplayFormat.SmallCard;
                    t.Add(item.HLink);
                }

                t.Title = Title;

                return t;
            }
        }

        /// <summary>
        /// Returns a CardGroup of HLinkBase and not HLinkBackLink.
        /// </summary>
        /// <value>
        /// The card group as property.
        /// </value>
        public DBCardGroupHLink<HLinkDBBase> AsCardGroupLink
        {
            get
            {
                DBCardGroupHLink<HLinkDBBase> t = new DBCardGroupHLink<HLinkDBBase>();

                foreach (HLinkDBBackLink item in Items)
                {
                    item.HLink.DisplayAs = CommonEnums.DisplayFormat.LinkCardCell;
                    t.Add(item.HLink);
                }

                t.Title = Title;

                return t;
            }
        }

        private string SerialisationName { get; }

        public HLinkBackLinkDBModelCollection()
        {
            // Only for seralisation
        }

        public HLinkBackLinkDBModelCollection(string argSerialisationName)
        {
            Title = "BackLink Collection";

            SerialisationName = argSerialisationName;
        }

        public async Task DeSerialize()
        {
            try
            {
                Ioc.Default.GetRequiredService<ILog>().DataLogEntryAdd($"DeSerialising {SerialisationName}");

                DataContractSerializer ser = new(typeof(DataInstance));

                FileInfo[] ttt = DataStore.Instance.AD.CurrentDataFolder.FolderasDirInfo.GetFiles(CommonRoutines.GetSerialFile(SerialisationName));

                // Check of the file exists
                if (ttt.Length != 1)
                {
                    ErrorInfo tt = new("DeSerializeRepository", "File Does not exist.  Reload the GPKG file")
                                {
                                    { "File", CommonRoutines.GetSerialFile(CommonRoutines.GetSerialFile(SerialisationName)) },
                                };

                    Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyError(tt);
                    SharedSharpSettings.DataSerialised = false;
                    return;
                }

                //byte[] buffer = new byte[1024];

                FileStream isoStream = new FileStream(CommonRoutines.GetSerialFileFull(SerialisationName), FileMode.Open);

                //var ttt = await isoStream.ReadAsync(buffer, 0, 100);

                JsonSerializerOptions serializerOptions = CommonRoutines.GetSerializerOptions();

                HLinkBackLinkDBModelCollection t = await JsonSerializer.DeserializeAsync<HLinkBackLinkDBModelCollection>(isoStream, serializerOptions);
                this.Clear();
                foreach (HLinkDBBackLink item in t)
                {
                    this.Add(item);
                }

                return;
            }
            catch (Exception ex)
            {
                Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyException(ex, new ErrorInfo("Trying to deserialise object")
                {
                    new CardListLine("Data area",SerialisationName),
                });

                SharedSharpSettings.DataSerialised = false;

                return;
            }
        }

        public async Task Serialize()
        {
            try
            {
                Ioc.Default.GetRequiredService<ILog>().DataLogEntryAdd($"Serialising {CommonRoutines.GetSerialFile(SerialisationName)}");

                JsonSerializerOptions serializerOptions = CommonRoutines.GetSerializerOptions();

                FileStream stream = new(CommonRoutines.GetSerialFileFull(SerialisationName), FileMode.Create);

                await JsonSerializer.SerializeAsync(stream, this, serializerOptions);

                return;
            }
            catch (Exception ex)
            {
                Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyException(ex, new ErrorInfo("Trying to Serialise object")
                {
                    new CardListLine("Data area",SerialisationName),
                });
                SharedSharpSettings.DataSerialised = false;
            }
        }

        public override void SetGlyph()
        {
            // Back Reference HLinks
            foreach (HLinkDBBackLink argHLink in this)
            {
                ItemGlyph t = new ItemGlyph();

                switch (argHLink.HLinkType)
                {
                    // TODO fix this once DB access finalised
                    //
                    ////case HLinkBackLinkEnum.HLinkAddressModel:
                    ////    {
                    ////        t = DV.AddressDV.GetGlyph(argHLink.HLinkKey);
                    ////        break;
                    ////    }

                    //case HLinkBackLinkEnum.HLinkCitationModel:
                    //    {
                    //        t = DL.CitationDL.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkEventModel:
                    //    {
                    //        t = DL.EventDL.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkFamilyModel:
                    //    {
                    //        t = DL.FamilyDL.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkMediaModel:
                    //    {
                    //        t = DV.MediaDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkNameMapModel:
                    //    {
                    //        t = DV.NameMapDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkNoteModel:
                    //    {
                    //        t = DL.NoteDL.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkPersonModel:
                    //    {
                    //        t = DV.PersonDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkPersonNameModel:
                    //    {
                    //        t = DV.PersonNameDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkPlaceModel:
                    //    {
                    //        t = DV.PlaceDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkRepositoryModel:
                    //    {
                    //        t = DV.RepositoryDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkSourceModel:
                    //    {
                    //        t = DV.SourceDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.HLinkTagModel:
                    //    {
                    //        t = DV.TagDV.GetGlyph(argHLink.HLinkKey);
                    //        break;
                    //    }

                    //case HLinkBackLinkEnum.Unknown:
                    //    break;

                    default:

                        break;
                }

                argHLink.HLinkGlyphItem = t;
            }

            //// Set the first image link. Assumes main image is manually set to the first image in
            //// Gramps if we need it to be, e.g. Citations.
            SetFirstImage();

            base.Sort();
        }
    }
}