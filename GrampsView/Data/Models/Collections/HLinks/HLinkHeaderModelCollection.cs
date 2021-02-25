﻿// TODO Needs XML 1.71 check

namespace GrampsView.Data.Collections
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Collection of EVent $$(HLinks)$$.
    /// </summary>
    [CollectionDataContract]
    [KnownType(typeof(ObservableCollection<HLinkHeaderModel>))]
    public class HLinkHeaderModelCollection : HLinkBaseCollection<HLinkHeaderModel>
    {
        public override CardGroup GetCardGroup()
        {
            CardGroup t = base.GetCardGroup();

            t.Title = Title;

            return t;
        }

        public override void SetGlyph()
        {
            foreach (HLinkHeaderModel argHLink in this)
            {
                argHLink.HLinkGlyphItem = DV.HeaderDV.GetGlyph(argHLink.HLinkKey);
            }

            // TODO SortAndSetFirst();
        }
    }
}