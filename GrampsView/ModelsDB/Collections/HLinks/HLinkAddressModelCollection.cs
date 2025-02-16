﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common.CustomClasses;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GrampsView.Data.Collections
{
    /// <summary>
    /// Collection of HLinks to Address models.
    /// </summary>

    [KnownType(typeof(ObservableCollection<HLinkAddressDBModel>))]
    public class HLinkAddressDBModelCollection : HLinkDBBaseCollection<HLinkAddressDBModel>
    {
        public HLinkAddressDBModelCollection()
        {
            Title = "Address Collection";
        }

        public override void SetGlyph()
        {
            foreach (HLinkAddressDBModel argHLink in this)
            {
                ItemGlyph t = DV.AddressDV.GetGlyph(argHLink.HLinkKey);

                argHLink.HLinkGlyphItem.ImageType = t.ImageType;
                argHLink.HLinkGlyphItem.ImageHLink = t.ImageHLink;
                argHLink.HLinkGlyphItem.ImageSymbol = t.ImageSymbol;
                argHLink.HLinkGlyphItem.ImageSymbolColour = t.ImageSymbolColour;
            }

            base.SetGlyph();
        }

        /// <summary>
        /// Sort by address date rather than the default text
        /// </summary>
        public override void Sort()
        {
            List<HLinkAddressDBModel> t = this.OrderBy(HLinkAdressModel => HLinkAdressModel.DeRef.GDate).ToList();

            Items.Clear();

            foreach (HLinkAddressDBModel item in t)
            {
                Items.Add(item);
            }
        }
    }
}