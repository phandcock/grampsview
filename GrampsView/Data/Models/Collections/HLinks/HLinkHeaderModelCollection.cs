﻿namespace GrampsView.Data.Collections
{
    using GrampsView.Common.CustomClasses;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary> Collection of Header HLinks <list type="table"><listheader><term><th> Status
    /// </term></listheader><item><description> XML 1.71 check </description><description> Done
    /// </description></item></list><para><br /></para> </summary>
    [CollectionDataContract]
    [KnownType(typeof(ObservableCollection<HLinkHeaderModel>))]
    public class HLinkHeaderModelCollection : HLinkBaseCollection<HLinkHeaderModel>
    {
        public override void SetGlyph()
        {
            foreach (HLinkHeaderModel argHLink in this)
            {
                ItemGlyph t = DV.HeaderDV.GetGlyph(argHLink.HLinkKey);

                argHLink.HLinkGlyphItem.ImageType = t.ImageType;
                argHLink.HLinkGlyphItem.ImageHLink = t.ImageHLink;
                argHLink.HLinkGlyphItem.ImageSymbol = t.ImageSymbol;
                argHLink.HLinkGlyphItem.ImageSymbolColour = t.ImageSymbolColour;
            }

            base.SetGlyph();
        }

        ///// <summary>
        ///// Helper method to sort and set the firt image link.
        ///// </summary>
        //public override void Sort()
        //{
        //    // Sort the collection
        //    List<HLinkHeaderModel> t = this.OrderBy(HLinkHeaderModel => HLinkHeaderModel.DeRef.ToString()).ToList();

        // Items.Clear();

        //    foreach (HLinkHeaderModel item in t)
        //    {
        //        Items.Add(item);
        //    }
        //}
    }
}