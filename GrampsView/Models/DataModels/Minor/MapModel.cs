﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Models.DataModels.Interfaces;
using GrampsView.Models.HLinks.Models;

using SharedSharp.Errors.Interfaces;

using System.Diagnostics.Contracts;

using static GrampsView.Common.CommonEnums;

namespace GrampsView.Models.DataModels.Minor
{
    /// <summary>
    /// Data model for a Map reference.
    /// <list type="table">
    /// <listheader>
    /// <term> Item </term>
    /// <term> Status </term>
    /// </listheader>
    /// <item>
    /// <description> XML 1.71 check </description>
    /// <description> NA </description>
    /// </item>
    /// </list>
    /// </summary>
    public class MapModel : ModelBase, IMapModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapModel"> MapModel </see> class.
        /// </summary>
        public MapModel()
        {
            OpenMapCommand = new AsyncRelayCommand(OpenMap);

            ModelItemGlyph.Symbol = Constants.IconMap;
            ModelItemGlyph.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundUtility");
        }

        public string Description
        {
            get;
            set;
        } = string.Empty;

        public HLinkMapModel HLink
        {
            get
            {
                HLinkMapModel t = new()
                {
                    DeRef = this,
                    HLinkKey = HLinkKey,
                    HLinkGlyphItem = ModelItemGlyph,
                };

                return t;
            }
        }

        public MapType MapType
        {
            get;
            set;
        } = MapType.Unknown;

        public Location MyLocation
        {
            get;
            set;
        } = new Location();

        public Placemark MyPlaceMark
        {
            get;
            set;
        } = new Placemark();

        /// <summary>
        /// Gets or sets the Lat / Long GPS location..
        /// </summary>
        /// <value>
        /// The xamarin essentials location object.
        /// </value>
        /// <summary>
        /// Gets or sets the PlaceMark location description.
        /// </summary>
        /// <value>
        /// The xamarin essentials placemark.
        /// </value>
        public IAsyncRelayCommand OpenMapCommand
        {
            get; private set;
        }

        /// <summary>
        /// Opens the Map.
        /// </summary>
        public async Task OpenMap()
        {
            switch (MapType)
            {
                case MapType.LatLong:
                    {
                        try
                        {
                            await Map.OpenAsync(MyLocation);
                        }
                        catch (Exception ex)
                        {
                            Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyException("No map application available to open", ex);
                        }

                        break;
                    }

                case MapType.Place:
                    {
                        try
                        {
                            MapLaunchOptions mapOptions = new()
                            {
                                Name = ToString(),
                            };

                            await MyPlaceMark.OpenMapsAsync(mapOptions);
                        }
                        catch (Exception ex)
                        {
                            Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyException("No map application available to open", ex);
                        }

                        break;
                    }

                default:
                    {
                        Contract.Assert(false, "Bad Map Type");
                        break;
                    }
            }
        }

        /// <summary>
        /// Gets the default text.
        /// </summary>
        /// <value>
        /// The default text.
        /// </value>
        public override string ToString()
        {
            return Description;
        }
    }
}