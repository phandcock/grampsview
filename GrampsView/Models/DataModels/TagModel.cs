﻿using GrampsView.Common;
using GrampsView.Models.DataModels;

using System;
using System.Collections;

namespace GrampsView.Data.Model
{
    /// <summary>
    /// Data model for a Tag item. XML 1.71 check complete
    /// </summary>

    public sealed class TagModel : ModelBase, ITagModel, IComparable, IComparer
    {
        /// <summary>
        /// The color.
        /// </summary>
        private Color _GColor = Colors.White;

        /// <summary>
        /// The name.
        /// </summary>
        private string _GName = string.Empty;

        /// <summary>
        /// The priority.
        /// </summary>
        private int _GPriority;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagModel"/> class.
        /// </summary>
        public TagModel()
        {
            ModelItemGlyph.Symbol = Constants.IconTag;
            ModelItemGlyph.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundUtility");
        }

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>

        public Color GColor
        {
            get => _GColor;

            set => SetProperty(ref _GColor, value);
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>

        public string GName
        {
            get => _GName;

            set => SetProperty(ref _GName, value);
        }

        /// <summary>
        /// Gets or sets the Priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>

        public int GPriority
        {
            get => _GPriority;

            set => SetProperty(ref _GPriority, value);
        }

        /// <summary>
        /// Gets the get h link.
        /// </summary>
        /// <value>
        /// The get h link.
        /// </value>
        public HLinkTagModel HLink
        {
            get
            {
                HLinkTagModel t = new()
                {
                    HLinkKey = HLinkKey,
                    HLinkGlyphItem = ModelItemGlyph,
                };
                return t;
            }
        }

        /// <summary>
        /// Compares two objects.
        /// </summary>
        /// <param name="a">
        /// object A.
        /// </param>
        /// <param name="b">
        /// object B.
        /// </param>
        /// <returns>
        /// One, two or three.
        /// </returns>
        int IComparer.Compare(object a, object b)
        {
            TagModel firstEvent = (TagModel)a;
            TagModel secondEvent = (TagModel)b;

            // compare on Priority first
            int testFlag = string.Compare(firstEvent.GName, secondEvent.GName, StringComparison.CurrentCulture);

            return testFlag;
        }

        /// <summary>
        /// Implement IComparable CompareTo method.
        /// </summary>
        /// <param name="obj">
        /// The object to compare.
        /// </param>
        /// <returns>
        /// One, two or three.
        /// </returns>
        int IComparable.CompareTo(object obj)
        {
            TagModel secondEvent = (TagModel)obj;

            // compare on Name first
            int testFlag = string.Compare(GName, secondEvent.GName, StringComparison.CurrentCulture);

            return testFlag;
        }

        public override string ToString()
        {
            return GName;
        }
    }
}