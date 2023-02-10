﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Data.DataView;
using GrampsView.Data.Model;
using GrampsView.Models.DataModels;
using GrampsView.Models.DataModels.Interfaces;
using GrampsView.Models.HLinks.Interfaces;

namespace GrampsView.ViewModels.Media
{
    /// <summary>
    /// Media Detail ViewModel
    /// </summary>
    public class MediaDetailViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaDetailViewModel"/> class.
        /// </summary>
        /// <param name="iocCommonLogging">
        /// Common logger.
        /// </param>
        /// <param name="iocEventAggregator">
        /// The event aggregator.
        /// </param>
        public MediaDetailViewModel(ILog iocCommonLogging, IMessenger iocEventAggregator)
            : base(iocCommonLogging)
        {
            BaseTitleIcon = Constants.IconMedia;

            BaseCL.Progress("MediaDetailViewModel created");
        }

        public HLinkMediaModel CurrentHLinkMedia
        {
            get; set;
        } = new HLinkMediaModel();

        /// <summary>
        /// Gets or sets the current media object.
        /// </summary>
        /// <value>
        /// The current media object.
        /// </value>
        public MediaModel CurrentMediaObject
        {
            get; set;
        } = new MediaModel();

        public IHLinkMediaModel MediaCard
        {
            get; set;
        } = new HLinkMediaModel();

        /// <summary>
        /// Handles navigation inwards and gets the media model parameter.
        /// </summary>
        /// <returns>
        /// </returns>
        public override void HandleViewModelParameters()
        {
            BaseCL.RoutineEntry("MediaDetailViewModel OnNavigatedTo");

            CurrentHLinkMedia = CommonRoutines.GetHLinkParameter<HLinkMediaModel>(BasePassedArguments);

            // For cropped or internal media then show the original image
            IMediaModel tt = CurrentHLinkMedia.DeRef;
            if (tt.IsInternalMediaFile)
            {
                CurrentHLinkMedia = DV.MediaDV.GetModelFromHLinkKey(tt.InternalMediaFileOriginalHLink).HLink;
            }

            if (CurrentHLinkMedia is not null)
            {
                CurrentMediaObject = CurrentHLinkMedia.DeRef as MediaModel;

                if (CurrentMediaObject is not null)
                {
                    BaseModelBase = CurrentMediaObject;
                    BaseTitle = CurrentHLinkMedia.DeRef.DefaultTextShort;

                    MediaCard = CurrentMediaObject.ModelItemGlyph.ImageHLinkMediaModel;

                    BaseDetail.Clear();

                    // Get basic details
                    BaseDetail.Add(new CardListLineCollection("Media Detail")
                    {
                        new CardListLine("File Description:", CurrentMediaObject.GDescription),
                        new CardListLine("File Mime Type:", CurrentMediaObject.FileMimeType),
                        new CardListLine("File Content Type:", CurrentMediaObject.FileContentType),
                        new CardListLine("File Mime SubType:", CurrentMediaObject.FileMimeSubType),
                        new CardListLine("OriginalFilePath:", CurrentMediaObject.OriginalFilePath),
                    });

                    // Get date card
                    if (CurrentMediaObject.GDateValue.Valid)
                    {
                        BaseDetail.Add(CurrentMediaObject.GDateValue.AsHLink("Media Date"));
                    }

                    // Add standard details
                    MediaModel t = CurrentMediaObject;
                    BaseDetail.Add(DV.MediaDV.GetModelInfoFormatted(t));

                    // Add Media Link Card
                    HLinkMediaModel ttt = CurrentMediaObject.HLink;
                    ttt.DisplayAs = CommonEnums.DisplayFormat.LinkCardMedium;
                    BaseDetail.Add(ttt);
                }

                BaseCL.RoutineExit("MediaDetailViewModel OnNavigatedTo");
            }
        }
    }
}