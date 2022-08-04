﻿namespace GrampsView.ViewModels
{
    using GrampsView.Common;
    using GrampsView.Data.DataView;
    using GrampsView.Data.Model;

    using CommunityToolkit.Mvvm.Messaging;

    using SharedSharp.Logging;
    using SharedSharp.Model;

    /// <summary>
    /// Defines the EVent Detail Page View ViewModel.
    /// </summary>
    public class RepositoryDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// The ioc common logging.
        /// </param>
        /// <param name="iocEventAggregator">
        /// The ioc event aggregator.
        /// </param>
        public RepositoryDetailViewModel(ISharedLogging iocCommonLogging, IMessenger iocEventAggregator)
            : base(iocCommonLogging)
        {
        }

        public HLinkRepositoryModel RepositoryHLink
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the repository object.
        /// </summary>
        /// <value>
        /// The repository object.
        /// </value>
        public RepositoryModel RepositoryObject
        {
            get; set;
        }

        /// <summary>
        /// Handles navigation inwards and sets up the repository model parameter.
        /// </summary>
        public override void HandleViewDataLoadEvent()
        {
            RepositoryHLink = CommonRoutines.GetHLinkParameter<HLinkRepositoryModel>(BaseParamsHLink);

            RepositoryObject = RepositoryHLink.DeRef;

            if (!(RepositoryObject == null))
            {
                BaseModelBase = RepositoryObject;
                BaseTitleIcon = Constants.IconRepository;

                BaseDetail.Add(new CardListLineCollection("Repository Detail")
                    {
                        new CardListLine("Name:", RepositoryObject.GRName),
                        new CardListLine("Type:", RepositoryObject.GType),
                    });

                BaseDetail.Add(DV.RepositoryDV.GetModelInfoFormatted(RepositoryObject));
            }
        }
    }
}