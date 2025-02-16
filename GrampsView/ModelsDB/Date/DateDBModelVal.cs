﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Data.Model;
using GrampsView.Models.DataModels.Date;
using GrampsView.Models.HLinks.Models;

using SharedSharp.Errors.Interfaces;

using System.Diagnostics.Contracts;
using System.Text.Json.Serialization;

using static GrampsView.Common.CommonEnums;

namespace GrampsView.ModelsDB.Date
{
    /// <summary>
    /// Create Val version of DateObjectModel.
    /// </summary>
    /// TODO Update fields as per Schema

    //[JsonDerivedType(typeof(DateObjectModelVal), typeDiscriminator: "val")]
    public class DateDBModelVal : DateDBModelBase, IDateDBModelVal
    {
        /// <summary>
        /// $$(cformat)$$ field.
        /// </summary>
        private string _GCformat = string.Empty;

        private string _GNewYear = string.Empty;

        /// <summary>
        /// Quality field.
        /// </summary>
        private DateQuality _GQuality = DateQuality.unknown;

        private DateValType _GValType = DateValType.unknown;

        public DateDBModelVal(string aVal, string? aCFormat = null, bool aDualDated = false, string? aNewYear = null, DateQuality aQuality = DateQuality.unknown, DateValType aValType = DateValType.unknown)
        {
            {
                Contract.Requires(!string.IsNullOrEmpty(aVal));

                // Setup basics
                ModelItemGlyph.Symbol = Constants.IconDate;
                ModelItemGlyph.SymbolColour = CommonRoutines.ResourceColourGet("CardBackGroundUtility");
                //DerivedType = DateObjectModelDerivedTypeEnum.DateObjectModelVal;

                HLinkKey = Common.CustomClasses.HLinkKey.NewAsGUID();

                try
                {
                    GCformat = aCFormat;

                    GDualdated = aDualDated;

                    GNewYear = aNewYear;

                    GQuality = aQuality;

                    GVal = aVal;

                    GValType = aValType;

                    NotionalDate = ConvertRFC1123StringToDateTime(aVal);

                    DateType = DateObjectModelDerivedTypeEnum.DateObjectModelVal;
                }
                catch (Exception ex)
                {
                    Ioc.Default.GetRequiredService<IErrorNotifications>().NotifyException("Error in SetDate", ex);
                }
            }
        }

        public DateDBModelVal()
        {
        }

        public override string DefaultTextShort => ShortDate;

        public string GCformat
        {
            get => _GCformat;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _ = SetProperty(ref _GCformat, value);
                }
            }
        }

        public bool GDualdated
        {
            get;

            set;
        }

        /// <summary>
        /// Gets the $$(cformat)$$ field.
        /// </summary>
        /// <summary>
        /// Gets a value indicating whether gets or sets the $$(dualdated)$$ field.
        /// </summary>
        public override int? GetAge
        {
            get
            {
                if (Valid)
                {
                    // Calculate the age - ROUGHLY
                    DateTime today = DateTime.Today;
                    // return (today - NotionalDate).Days / 365;

                    return today.Year - NotionalDate.Year - 1 +
                            (today.Month > NotionalDate.Month ||
                            today.Month == NotionalDate.Month && today.Day >= NotionalDate.Day ? 1 : 0);
                }

                return null;
            }
        }

        public override string GetYear => Valid ? NotionalDate.Year.ToString(System.Globalization.CultureInfo.CurrentCulture) : "Unknown";

        public string GNewYear
        {
            get => _GNewYear;

            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _ = SetProperty(ref _GNewYear, value);
                }
            }
        }

        public DateQuality GQuality
        {
            get => _GQuality;

            set => SetProperty(ref _GQuality, value);
        }

        public string GVal { get; set; }

        public DateValType GValType
        {
            get => _GValType;

            set => SetProperty(ref _GValType, value);
        }

        /// <summary>
        /// Gets the New Year field.
        /// </summary>
        /// <summary>
        /// Get the Date Quality.
        /// </summary>
        /// <summary>
        /// Gets the $$(val)$$ field.
        /// </summary>
        /// <summary>
        /// Gets the type of the Val Type, e.g. Before
        /// </summary>
        /// <value>
        /// The type of the g value.
        /// </value>
        [JsonIgnore]
        public HLinkDateDBModelVal HLink
        {
            get
            {
                HLinkDateDBModelVal t = new()
                {
                    DeRef = this,
                    HLinkKey = HLinkKey,
                    HLinkGlyphItem = ModelItemGlyph,
                };

                return t;
            }
        }

        public override string LongDate
        {
            get
            {
                if (!Valid)
                {
                    return string.Empty;
                }

                string dateString = string.Empty;

                if (NotionalDate != DateTime.MinValue)
                {
                    dateString = base.LongDate;
                }

                if (!string.IsNullOrEmpty(GCformat))
                {
                    dateString += " Format: " + GCformat;
                }

                if (GValType != DateValType.unknown)
                {
                    dateString = Enum.GetName(typeof(DateValType), GValType) + " " + dateString;
                }

                // Do not display a messgae if thw quality is unknown
                if (GQuality != DateQuality.unknown)
                {
                    dateString += " " + GQuality.ToString();
                }

                if (GDualdated)
                {
                    dateString += " (Dual dated)";
                }

                if (!string.IsNullOrEmpty(GNewYear))
                {
                    dateString += " New Year: " + GNewYear;
                }

                return dateString.Trim();
            }
        }

        /// <summary>
        /// Gets the string version of the date field.
        /// </summary>
        /// <returns>
        /// a string version of the date.
        /// </returns>
        public override string ShortDate
        {
            get
            {
                if (!Valid)
                {
                    return string.Empty;
                }

                string dateString = string.Empty;

                if (NotionalDate != DateTime.MinValue)
                {
                    dateString = base.ShortDate;
                }

                if (GValType != DateValType.unknown)
                {
                    dateString = Enum.GetName(typeof(DateValType), GValType) + " " + dateString;
                }

                return dateString.Trim();
            }
        }

        public override CardListLineCollection AsCardListLine(string argTitle = "Date Detail")
        {
            CardListLineCollection DateModelCard = new();

            if (Valid)
            {
                DateModelCard = new CardListLineCollection("Date Detail")
                            {
                                new CardListLine("Date:", LongDate),
                                new CardListLine("Val:", GVal),
                                new CardListLine("C Format:", GCformat),
                                new CardListLine("Type:", GValType.ToString(),GValType != DateValType.unknown),
                                new CardListLine("Quality:", GQuality.ToString(),GQuality != DateQuality.unknown),
                                new CardListLine("Dual Dated:", GDualdated,true),
                                new CardListLine("New Year:", GNewYear),
                            };
            }

            if (!string.IsNullOrEmpty(argTitle))
            {
                DateModelCard.Title = argTitle;
            }

            return DateModelCard;
        }

        public override HLinkDateDBModelVal AsHLink(string argTitle)
        {
            HLinkDateDBModelVal t = HLink;
            t.Title = argTitle;

            return t;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            DateObjectModelBase? tempObj = obj as DateObjectModelBase;

            return NotionalDate == tempObj.NotionalDate;
        }

        public override int GetHashCode()
        {
            return HLinkKey.GetHashCode();
        }
    }
}