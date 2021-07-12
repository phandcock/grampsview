﻿// TODO Needs XML 1.71 check

//// gramps XML 1.71
////  primary-object
////  type
////  ptitle
////  placename-content
////  code
////  coord
////     long
////     lat
//// placeref
//// location
//// objref
//// url
//// noteref
//// noteref-content
//// citationref
//// tagref
////

namespace GrampsView.Data.Model
{
    using GrampsView.Common;
    using GrampsView.Data.Collections;

    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    /// <summary>
    /// </summary>
    /// <seealso cref="GrampsView.Data.ViewModel.ModelBase"/>
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// <seealso cref="GrampsView.Data.ViewModel.IPlaceModel"/>
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// <seealso cref="System.IComparable"/>
    /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// /// ///
    /// <seealso cref="System.Collections.IComparer"/>
    [DataContract]
    public sealed class PlaceModel : ModelBase, IPlaceModel, IComparable, IComparer
    {
        /// <summary>
        /// The local g code field.
        /// </summary>
        private string _GCodeField;

        /// <summary>
        /// The local media collection.
        /// </summary>
        private HLinkMediaModelCollection _GMediaCollection = new HLinkMediaModelCollection();

        /// <summary>
        /// The local note reference.
        /// </summary>
        private HLinkNoteModelCollection _GNoteReference = new HLinkNoteModelCollection();

        private PlaceNameModelCollection _GPlaceNames = new PlaceNameModelCollection();

        /// <summary>
        /// The local place reference.
        /// </summary>
        private HLinkPlaceModelCollection _GPlaceReference = new HLinkPlaceModelCollection();

        /// <summary>
        /// The local ptitle field.
        /// </summary>
        private string _GPTitle;

        /// <summary>
        /// The local place type field.
        /// </summary>
        private string _GTypeField;

        private HLinkPlaceModelCollection _PlaceChildCollection = new HLinkPlaceModelCollection();

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaceModel"/> class.
        /// </summary>
        public PlaceModel()
        {
            ModelItemGlyph.Symbol = CommonConstants.IconPlace;
            ModelItemGlyph.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundPlace");

            PlaceChildCollection.Title = "Enclosed Places";
            GPlaceParentCollection.Title = "Enclosing Place";
        }

        /// <summary>
        /// Gets or sets the g citation reference collection.
        /// </summary>
        [DataMember]
        public HLinkCitationModelCollection GCitationRefCollection { get; set; } = new HLinkCitationModelCollection();

        /// <summary>
        /// Gets or sets the gp code.
        /// </summary>
        /// <value>
        /// The gp code.
        /// </value>
        [DataMember]
        public string GCode
        {
            get => _GCodeField;

            set => SetProperty(ref _GCodeField, value);
        }

        [DataMember]
        public string GCoordLat
        {
            get; set;
        }

        [DataMember]
        public string GCoordLong
        {
            get; set;
        }

        /// <summary>
        /// Gets the default text for this Model.
        /// </summary>
        /// <value>
        /// The default text.
        /// </value>
        public override string GetDefaultText
        {
            get
            {
                if (!string.IsNullOrEmpty(GPTitle))
                {
                    return GPTitle;
                }

                return GPlaceNames.GetDefaultText;
            }
        }

        [DataMember]
        public PlaceLocationCollection GLocation { get; set; } = new PlaceLocationCollection();

        /// <summary>
        /// Gets or sets the media reference collection.
        /// </summary>
        /// <value>
        /// The media reference collection.
        /// </value>
        [DataMember]
        public HLinkMediaModelCollection GMediaRefCollection
        {
            get => _GMediaCollection;

            set => SetProperty(ref _GMediaCollection, value);
        }

        /// <summary>
        /// Gets or sets the g note reference collection.
        /// </summary>
        /// <value>
        /// The g note reference collection.
        /// </value>
        [DataMember]
        public HLinkNoteModelCollection GNoteRefCollection
        {
            get => _GNoteReference;

            set => SetProperty(ref _GNoteReference, value);
        }

        [DataMember]
        public PlaceNameModelCollection GPlaceNames
        {
            get => _GPlaceNames;

            set => SetProperty(ref _GPlaceNames, value);
        }

        /// <summary>
        /// Gets or sets the g place reference collection.
        /// </summary>
        /// <value>
        /// The g place reference collection.
        /// </value>
        [DataMember]
        public HLinkPlaceModelCollection GPlaceParentCollection
        {
            get => _GPlaceReference;

            set => SetProperty(ref _GPlaceReference, value);
        }

        ///// <summary>
        ///// Gets or sets the place title.
        ///// </summary>
        ///// <value>
        ///// The gp title.
        ///// </value>
        [DataMember]
        public string GPTitle
        {
            get => _GPTitle;

            set => SetProperty(ref _GPTitle, value);
        }

        /// <summary>
        /// Gets or sets the tag reference collection.
        /// </summary>
        /// <value>
        /// The g tag reference collection.
        /// </value>
        [DataMember]
        public HLinkTagModelCollection GTagRefCollection
        {
            get
            ;

            set
          ;
        }

        [DataMember]
        public string GType
        {
            get => _GTypeField;

            set => SetProperty(ref _GTypeField, value);
        }

        /// <summary>
        /// Gets or sets uRL model collection.
        /// </summary>
        [DataMember]
        public HLinkURLModelCollection GURLCollection
        {
            get; set;
        }

        = new HLinkURLModelCollection();

        /// <summary>
        /// Gets the get h link.
        /// </summary>
        /// <value>
        /// The get h link.
        /// </value>
        public HLinkPlaceModel HLink
        {
            get
            {
                HLinkPlaceModel t = new HLinkPlaceModel
                {
                    HLinkKey = HLinkKey,
                    HLinkGlyphItem = ModelItemGlyph,
                };
                return t;
            }
        }

        [DataMember]
        public HLinkPlaceModelCollection PlaceChildCollection
        {
            get => _PlaceChildCollection;

            set => SetProperty(ref _PlaceChildCollection, value);
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
            PlaceModel firstEvent = (PlaceModel)a;
            PlaceModel secondEvent = (PlaceModel)b;

            int testFlag = string.Compare(firstEvent.GetDefaultText, secondEvent.GetDefaultText, StringComparison.CurrentCulture);

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
            PlaceModel secondEvent = (PlaceModel)obj;

            int testFlag = string.Compare(GetDefaultText, secondEvent.GetDefaultText, StringComparison.CurrentCulture);

            return testFlag;
        }
    }
}