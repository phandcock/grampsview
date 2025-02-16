﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Models.DataModels;
using GrampsView.Models.HLinks;
using GrampsView.ModelsDB.HLinks.Models;

namespace GrampsView.Data.Model
{
    /// <summary>
    /// <para> HLink to an Attribute model. </para>
    /// <list type="table">
    /// <listheader>
    /// <term> Item </term>
    /// <term> Status </term>
    /// </listheader>
    /// <item>
    /// <description> XML 1.71 check </description>
    /// <description> Not in XML 1.71 so use the base HLink. </description>
    /// </item>
    /// </list>
    /// </summary>

    public class HLinkFamilyGraphModel : HLinkBase, IHLinkParentLinkModel
    {
        public HLinkFamilyGraphModel()
        {
            HLinkGlyphItem.Symbol = Constants.IconAttribute;
            HLinkGlyphItem.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundAttribute");
        }

        /// <summary>
        /// Gets the dereference.
        /// </summary>
        /// <value>
        /// The dereference.
        /// </value>

        public PersonModel DeRef
        {
            get;

            set;
        } = new PersonModel();

        public Group<object> Families
        {
            get
            {
                Group<object> returnValue = new Group<object>();
                foreach (HLinkFamilyDBModel currentFamily in DeRef.GParentInRefCollection)
                {
                    currentFamily.DisplayAs = CommonEnums.DisplayFormat.LinkCardMedium;
                    returnValue.Add(currentFamily);
                }

                return returnValue;
            }
        }

        public override bool Valid
        {
            get
            {
                if (!HLinkGlyphItem.Valid)
                {
                }

                return HLinkGlyphItem.Valid;
            }
        }
    }
}