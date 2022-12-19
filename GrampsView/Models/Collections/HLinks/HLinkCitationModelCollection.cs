﻿using GrampsView.Common.CustomClasses;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace GrampsView.Data.Collections
{
    /// <summary>
    /// Observable collection of Citation HLinks.
    ///  // XML 1.71 check Done
    /// </summary>

    [KnownType(typeof(ObservableCollection<HLinkCitationModel>))]
    public class HLinkCitationModelCollection : HLinkBaseCollection<HLinkCitationModel>
    {
        public HLinkCitationModelCollection()
        {
            Title = "Citation Collection";
        }

        //public override CardGroup GetCardGroup()
        //{
        //    CardGroup t = base.GetCardGroup();

        // t.Title = Title;

        //    return t;
        //}

        public override void SetGlyph()
        {
            // Back Reference Citation HLinks
            foreach (HLinkCitationModel argHLink in this)
            {
                ItemGlyph t = DV.CitationDV.GetGlyph(argHLink.HLinkKey);

                argHLink.HLinkGlyphItem.ImageType = t.ImageType;
                argHLink.HLinkGlyphItem.ImageHLink = t.ImageHLink;
                argHLink.HLinkGlyphItem.ImageSymbol = t.ImageSymbol;
                argHLink.HLinkGlyphItem.ImageSymbolColour = t.ImageSymbolColour;
            }

            base.SetGlyph();
        }

        /// <summary>
        /// Helper method to sort and set the firt image link.
        /// </summary>
        public override void Sort()
        {
            // Sort the collection
            List<HLinkCitationModel> t = this.OrderBy(HLinkCitationModel => HLinkCitationModel.DeRef.GDateContent.SortDate).ToList();

            Items.Clear();

            foreach (HLinkCitationModel item in t)
            {
                Items.Add(item);
            }
        }
    }
}