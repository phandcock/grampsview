﻿namespace GrampsView.Data.Model
{
    using GrampsView.Common;
    using GrampsView.Models.DataModels.Minor;
    using GrampsView.Models.HLinks;

    using System.Threading.Tasks;

    /// <summary>
    /// HLink to a URL
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
    /// <para> <br/> </para>
    /// </summary>

    public class HLinkURLModel : HLinkBase, IHLinkURLModel
    {
        public HLinkURLModel()
        {
            HLinkGlyphItem.Symbol = Constants.IconURL;
            HLinkGlyphItem.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundUtility");
        }

        /// <summary>
        /// Gets the reference.
        /// </summary>
        /// <value>
        /// The de reference.
        /// </value>

        public URLModel DeRef
        {
            get;

            set;
        } = new URLModel();

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

        /// <summary>
        /// No detail page to navigate to, just open the URL externally.
        /// </summary>
        public override async Task UCNavigate()
        {
            await DeRef.OpenURL();
            return;
        }
    }
}