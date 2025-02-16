﻿// Copyright (c) phandcock.  All rights reserved.

using CommunityToolkit.Mvvm.Messaging;

using GrampsView.Common;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;

using SharedSharp.Logging;

namespace GrampsView.ViewModels
{
    /// <summary>
    /// View Model for the Event Section Page.
    /// </summary>
    public class TagListViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagListViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// Common logging.
        /// </param>
        /// <param name="iocEventAggregator">
        /// Prism Event Aggregator.
        /// </param>

        public TagListViewModel(SharedSharp.Logging.Interfaces.ILog iocCommonLogging, IMessenger iocEventAggregator)
            : base(iocCommonLogging)
        {
            BaseTitle = "Tag List";
            BaseTitleIcon = Constants.IconTag;
        }

        /// <summary>
        /// Gets a Card Group of Tags for display
        /// </summary>
        /// <value>
        /// The tag source.
        /// </value>
        public CardGroupHLink<HLinkTagModel> TagSource
        {
            get
            {
                return DV.TagDV.GetAllAsCardGroupBase();
            }
        }
    }
}