﻿// TODO Needs XML 1.71 check


using GrampsView.Common.CustomClasses;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GrampsView.Models.Collections.HLinks
{
    /// <summary>
    /// Contains pointers to family models.
    /// </summary>

    [KnownType(typeof(ObservableCollection<HLinkPersonRefModel>))]
    public class HLinkPersonRefModelCollection : HLinkBaseCollection<HLinkPersonRefModel>
    {
        public HLinkPersonRefModelCollection()
        {
            Title = "People Ref Collection";
        }

        public override void SetGlyph()
        {
            foreach (HLinkPersonRefModel argHLink in this)
            {
                // TODO check this
                ItemGlyph t = DV.PersonDV.GetGlyph(argHLink.HLinkKey);

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
            List<HLinkPersonRefModel> t = this.OrderBy(HLinkEventModel => HLinkEventModel.DeRef.GPersonNamesCollection.GetPrimaryName.DeRef).ToList();

            Items.Clear();

            foreach (HLinkPersonRefModel item in t)
            {
                Items.Add(item);
            }
        }
    }
}