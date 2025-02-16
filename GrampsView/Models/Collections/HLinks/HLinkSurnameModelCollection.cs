﻿// TODO Needs XML 1.71 check

namespace GrampsView.Data.Collections
{
    using GrampsView.Data.Model;

    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Surname model collection.
    /// </summary>

    [KnownType(typeof(ObservableCollection<SurnameModel>))]
    public class HLinkSurnameModelCollection : HLinkBaseCollection<HLinkSurnameModel>
    {
        public HLinkSurnameModelCollection()
        {
            Title = "Surname Model Collection";
        }

        public string GetPrimarySurname
        {
            get
            {
                // TODO Handle multiple surnames
                if ((Items.Count > 1) && (Items[1].Valid))
                {
                }

                if ((Items.Count > 0) && (Items[0].Valid))
                {
                    return Items[0].DeRef.GText;
                }

                return "Unknown";
            }
        }
    }
}