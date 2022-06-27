﻿namespace GrampsView.Data.Model
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;

    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    /// <summary>
    /// Data model for a source reference.
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

    public class HLinkSourceModel : HLinkBase, IHLinkSourceModel
    {
        private SourceModel _Deref = new SourceModel();

        private bool DeRefCached;

        public HLinkSourceModel()
        {
            HLinkGlyphItem.Symbol = Constants.IconSource;
            HLinkGlyphItem.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundSource");
        }

        /// <summary>
        /// Gets the model from the HLink.
        /// </summary>
        /// <value>
        /// The HLink reference.
        /// </value>
        [JsonIgnore]
        public SourceModel DeRef
        {
            get
            {
                if (Valid && (!DeRefCached))
                {
                    _Deref = DV.SourceDV.GetModelFromHLinkKey(HLinkKey);

                    if (_Deref.Valid)
                    {
                        DeRefCached = true;
                    }
                }

                return _Deref;
            }
        }

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="argOobj">
        /// The object.
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(HLinkSourceModel argOobj) => DeRef.CompareTo(argOobj);

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="argObj">
        /// The object.
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(object obj)
        {
            // Null objects go first
            if (obj is null)
            {
                return 1;
            }

            // Can only compare if they are the same type so assume equal
            if (obj.GetType() != typeof(HLinkSourceModel))
            {
                return 0;
            }

            return DeRef.CompareTo((obj as HLinkSourceModel).DeRef);
        }

        public override async Task UCNavigate()
        {
            await UCNavigateBase(this, "SourceDetailPage");
        }

        //protected override IModelBase GetDeRef()
        //{
        //    return this.DeRef;
        //}
    }
}