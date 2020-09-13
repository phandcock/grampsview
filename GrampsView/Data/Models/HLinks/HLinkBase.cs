﻿//-----------------------------------------------------------------------
// <copyright file="HLinkBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace GrampsView.Data.Model
{
    using GrampsView.Common;

    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Runtime.Serialization;

    using static GrampsView.Common.CommonEnums;

    /// <summary>
    /// GRAMPS $$(Hlink)$$ element class.
    /// </summary>
    [DataContract]
    public class HLinkBase : CommonBindableBase, IHLinkBase
    {
        /// <summary>
        /// The local h link key.
        /// </summary>
        private string _HLinkKey = string.Empty;

        /// <summary>
        /// The local priv.
        /// </summary>
        private bool _Priv = default(bool);

        public HLinkBase()
        {
        }

        public DisplayFormat CardType { get; set; } = DisplayFormat.Default;

        [DataMember]
        public bool GPriv
        {
            get
            {
                return _Priv;
            }

            set
            {
                SetProperty(ref _Priv, value);
            }
        }

        /// <summary>
        /// Gets or sets the h link key.
        /// </summary>
        /// <value>
        /// The h link key.
        /// </value>
        [DataMember]
        public string HLinkKey
        {
            get
            {
                return _HLinkKey;
            }

            set
            {
                SetProperty(ref _HLinkKey, value);
            }
        }

        /// <summary>
        /// Gets the priv as string.
        /// </summary>
        /// <value>
        /// The priv as string.
        /// </value>
        public string PrivAsString
        {
            get
            {
                return _Priv.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets a value indicating whether gets boolean showing if the $$(HLink)$$ is valid.
        /// </summary>
        /// <value>
        /// Boolean showing if $$(HLink)$$ is valid.
        /// </value>
        public virtual bool Valid
        {
            get
            {
                return (!string.IsNullOrEmpty(HLinkKey));
            }
        }

        public static bool operator !=(HLinkBase left, HLinkBase right)
        {
            return !(left == right);
        }

        public static bool operator <(HLinkBase left, HLinkBase right)
        {
            return left is null ? right is object : left.CompareTo(right) < 0;
        }

        public static bool operator <=(HLinkBase left, HLinkBase right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator ==(HLinkBase left, HLinkBase right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator >(HLinkBase left, HLinkBase right)
        {
            return left is object && left.CompareTo(right) > 0;
        }

        public static bool operator >=(HLinkBase left, HLinkBase right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// Compares the specified x.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// </returns>
        int IComparer.Compare(object x, object y)
        {
            return Compare(x, y);
        }

        /// <summary>
        /// Compares to. Bases it on the HLInkKey for want of anything else that makes sense.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            return string.Compare(HLinkKey, (obj as HLinkBase).HLinkKey, true, System.Globalization.CultureInfo.CurrentCulture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return (this.HLinkKey == (obj as HLinkBase).HLinkKey);
        }

        public override int GetHashCode()
        {
            return HLinkKey.GetHashCode();
        }

        /// <summary>
        /// Sets the base fields.
        /// </summary>
        /// <param name="arg">
        /// The argument.
        /// </param>
        public void SetBase(HLinkBase arg)
        {
            Contract.Assert(arg != null);

            HLinkKey = arg.HLinkKey;
        }

        /// <summary>
        /// Compares the specified x. Bases it on the HLInkKey for want of anything else that makes sense.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// </returns>
        protected int Compare(object x, object y)
        {
            Contract.Requires(y != null);

            if (x is null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y is null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            return string.Compare((x as HLinkBase).HLinkKey, (y as HLinkBase).HLinkKey, StringComparison.CurrentCulture);
        }
    }
}