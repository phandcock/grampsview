﻿// TODO Needs XML 1.71 check


using GrampsView.Common.CustomClasses;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;

using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace GrampsView.Data.Collections
{
    /// <summary>
    /// Attribute model collection.
    /// </summary>

    [KnownType(typeof(ObservableCollection<HLinkPersonNameModel>))]
    public class HLinkPersonNameModelCollection : HLinkBaseCollection<HLinkPersonNameModel>
    {
        public HLinkPersonNameModelCollection()
        {
            Title = "Person Names";
        }

        ///// <summary>
        ///// Gets the married name if recorded otherwise just the primary name.
        ///// </summary>
        ///// <value>
        ///// The married name.
        ///// </value>
        //public HLinkPersonNameModel GetMarriedName
        //{
        //    get
        //    {
        //        HLinkPersonNameModel t = this.FirstOrDefault(x => x.DeRef.GType == Constants.NameTypeMarried);

        // // If no married name then return the primary name if (t ==
        // default(HLinkPersonNameModel)) { return GetPrimaryName; }

        //        return t;
        //    }
        //}

        public HLinkPersonNameModel GetPrimaryName
        {
            get
            {
                // Should always have a name but just in case
                if (Items.Count == 0)
                {
                    return new HLinkPersonNameModel();
                }

                // Return the primary name if it exists
                return Items.Count > 0 ? Items[0] : new HLinkPersonNameModel();
            }
        }

        public override void SetGlyph()
        {
            foreach (HLinkPersonNameModel argHLink in this)
            {
                ItemGlyph t = DV.PersonNameDV.GetGlyph(argHLink.HLinkKey);

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
        //    List<HLinkPersonNameModel> t = this.OrderBy(HLinkPersonNameModel => HLinkPersonNameModel.DeRef.ToString()).ToList();

        // Items.Clear();

        //    foreach (HLinkPersonNameModel item in t)
        //    {
        //        Items.Add(item);
        //    }
        //}
    }
}