﻿namespace GrampsView.Data.Collections
{
    using GrampsView.Common.CustomClasses;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;
    using GrampsView.Models.HLinks.Models;
    using GrampsView.ModelsDB.HLinks.Models;

    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Data model for a citation hlink collection.
    /// <list type="table">
    /// <listheader>
    /// <term> Item </term>
    /// <term> Status </term>
    /// </listheader>
    /// <item>
    /// <description> XML 1.71 check </description>
    /// <description> Done </description>
    /// </item>
    /// </list>
    /// </summary>

    [KnownType(typeof(ObservableCollection<HLinkCitationDBModel>))]
    public class HLinkPlaceModelCollection : HLinkBaseCollection<HLinkPlaceModel>
    {
        public HLinkPlaceModelCollection()
        {
            Title = "Place Collection";
        }

        public override void SetGlyph()
        {
            foreach (HLinkPlaceModel argHLink in this)
            {
                ItemGlyph t = DV.PlaceDV.GetGlyph(argHLink.HLinkKey);

                argHLink.HLinkGlyphItem.ImageType = t.ImageType;
                argHLink.HLinkGlyphItem.ImageHLink = t.ImageHLink;
                argHLink.HLinkGlyphItem.ImageSymbol = t.ImageSymbol;
                argHLink.HLinkGlyphItem.ImageSymbolColour = t.ImageSymbolColour;
            }

            base.SetGlyph();
        }
    }
}