namespace GrampsView.Data.DataView
{
    using GrampsView.Common;
    using GrampsView.Common.CustomClasses;
    using GrampsView.Data.Collections;
    using GrampsView.Data.Model;
    using GrampsView.Data.Repositories;
    using GrampsView.Data.Repository;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Data view into the Notes Repository.
    /// </summary>

    public class NoteDataView : DataViewBase<NoteModel, HLinkNoteModel, HLinkNoteModelCollection>, INoteDataView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDataView"/> class.
        /// </summary>
        public NoteDataView()
        {
        }

        public override IReadOnlyList<NoteModel> DataDefaultSort
        {
            get
            {
                return DataViewData.OrderBy(NoteModel => NoteModel.GStyledText.GText).ToList();
            }
        }

        /// <summary>
        /// Gets the data view data.
        /// </summary>
        /// <value>
        /// The data view data.
        /// </value>
        public override IReadOnlyList<NoteModel> DataViewData
        {
            get
            {
                return NoteData.Values.ToList();
            }
        }

        public override HLinkNoteModelCollection GetLatestChanges

        {
            get
            {
                DateTime lastSixtyDays = DateTime.Now.Subtract(new TimeSpan(60, 0, 0, 0, 0));

                IEnumerable tt = DataViewData.OrderByDescending(GetLatestChangest => GetLatestChangest.Change).Where(GetLatestChangestt => GetLatestChangestt.Change > lastSixtyDays).Take(3);

                HLinkNoteModelCollection returnCardGroup = new HLinkNoteModelCollection();

                foreach (NoteModel item in tt)
                {
                    returnCardGroup.Add(item.HLink);
                }

                returnCardGroup.Title = "Latest Note Changes";

                return returnCardGroup;
            }
        }

        public RepositoryModelDictionary<NoteModel, HLinkNoteModel> NoteData
        {
            get
            {
                return DataStore.Instance.DS.NoteData;
            }
        }

        public override HLinkNoteModelCollection GetAllAsCardGroupBase()
        {
            HLinkNoteModelCollection t = new HLinkNoteModelCollection();

            foreach (var item in DataDefaultSort)
            {
                t.Add(item.HLink);
            }

            // Sort TODO Sort t = HLinkCollectionSort(t);

            return t;
        }

        public override Group<HLinkNoteModelCollection> GetAllAsGroupedCardGroup()
        {
            Group<HLinkNoteModelCollection> t = new Group<HLinkNoteModelCollection>();

            var query = from item in DataViewData
                        orderby item.GType, item.ToString()
                        group item by (item.GType) into g
                        select new
                        {
                            GroupName = g.Key,
                            Items = g
                        };

            foreach (var g in query)
            {
                HLinkNoteModelCollection info = new HLinkNoteModelCollection
                {
                    Title = g.GroupName,
                };

                foreach (var item in g.Items)
                {
                    info.Add(item.HLink);
                }

                t.Add(info);
            }

            return t;
        }

        /// <summary>
        /// </summary>
        /// <value>
        /// The note data.
        /// </value>
        /// <returns>
        /// HLInknotemodel collection
        /// </returns>
        public HLinkNoteModelCollection GetAllAsHLink()
        {
            HLinkNoteModelCollection t = new HLinkNoteModelCollection();

            foreach (var item in DataDefaultSort)
            {
                t.Add(item.HLink);
            }

            return t;
        }

        /// <summary>
        /// Gets all models of a particlar note type.
        /// </summary>
        /// <param name="argType">
        /// Note type to search for
        /// </param>
        /// <returns>
        /// </returns>
        public CardGroupModel<NoteModel> GetAllOfType(string argType)
        {
            CardGroupModel<NoteModel> t = new CardGroupModel<NoteModel>();

            IEnumerable<NoteModel> q = DataViewData.Where(NoteModel => NoteModel.GType == argType);

            foreach (var item in q)
            {
                t.Add(item);
            }

            return new CardGroupModel<NoteModel>(q);
        }

        public override NoteModel GetModelFromHLinkKey(HLinkKey argHLinkKey)
        {
            return NoteData[argHLinkKey.Value];
        }

        public override NoteModel GetModelFromId(string argId)
        {
            NoteModel t = DataViewData.FirstOrDefault(X => X.Id == argId);

            if (t is null)
            {
                return new NoteModel();
            }

            return t;
        }

        /// <summary>
        /// hes the link collection sort.
        /// </summary>
        /// <param name="collectionArg">
        /// The collection argument.
        /// </param>
        /// <returns>
        /// Sorted hlink collection.
        /// </returns>
        public override HLinkNoteModelCollection HLinkCollectionSort(HLinkNoteModelCollection collectionArg)
        {
            if (collectionArg == null)
            {
                return null;
            }

            IOrderedEnumerable<HLinkNoteModel> t = collectionArg.OrderBy(HLinkNoteModel => HLinkNoteModel.DeRef.TextShort);

            HLinkNoteModelCollection tt = new HLinkNoteModelCollection();

            foreach (HLinkNoteModel item in t)
            {
                tt.Add(item);
            }

            return tt;
        }

        //public override HLinkNoteModelCollection Search(string argQuery)
        //{
        //    HLinkNoteModelCollection itemsFound = new HLinkNoteModelCollection
        //    {
        //        Title = "Notes"
        //    };

        //    if (string.IsNullOrEmpty(argQuery))
        //    {
        //        return itemsFound;
        //    }

        //    // Get list of notes
        //    HLinkNoteModelCollection tt = DV.NoteDV.Search(argQuery);

        //    // Convert to HlinkNote
        //    List<HLinkNoteModel> ttt = new List<HLinkNoteModel>();

        //    foreach (var item in tt)
        //    {
        //        foreach (HLinkBackLink item1 in item.DeRef.BackHLinkReferenceCollection)
        //        {
        //            if (item1.HLinkType == HLinkBackLink.HLinkBackLinkEnum.HLinkPersonModel)
        //            {
        //                ttt.Add(item1.HLink as HLinkNoteModel);
        //            }
        //        }
        //    }

        //    // Get Distinct
        //    foreach (var item2 in ttt.Distinct())
        //    {
        //        itemsFound.Add(item2);
        //    }

        //    return itemsFound;
        //}

        public override HLinkNoteModelCollection Search(string queryString)
        {
            HLinkNoteModelCollection itemsFound = new HLinkNoteModelCollection();

            if (string.IsNullOrEmpty(queryString))
            {
                return itemsFound;
            }

            var temp = DataViewData.Where(x => x.GStyledText.GText.ToLower(CultureInfo.CurrentCulture).Contains(queryString)).OrderBy(y => y.ToString()).Distinct();

            if (temp.Any())
            {
                foreach (NoteModel tempMO in temp)
                {
                    itemsFound.Add(tempMO.HLink);
                }
            }

            return itemsFound;
        }

        public List<SearcHandlerItem> SearchShell(string argQuery)
        {
            List<SearcHandlerItem> returnValue = new List<SearcHandlerItem>();

            foreach (var item in Search(argQuery))
            {
                returnValue.Add(new SearcHandlerItem(item));
            }

            return returnValue;
        }

        public CardGroupHLink<HLinkNoteModel> SearchTag(string argQuery)
        {
            CardGroupHLink<HLinkNoteModel> itemsFound = new CardGroupHLink<HLinkNoteModel>
            {
                Title = "Notes"
            };

            if (string.IsNullOrEmpty(argQuery))
            {
                return itemsFound;
            }

            var temp = from gig in DataViewData
                       where gig.GTagRefCollection.Any(act => act.DeRef.GName == argQuery)
                       select gig;

            if (temp.Any())
            {
                foreach (NoteModel tempMO in temp)
                {
                    itemsFound.Add(tempMO.HLink);
                }
            }

            return itemsFound;
        }
    }
}