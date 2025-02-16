﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Common;
using GrampsView.Common.CustomClasses;
using GrampsView.Data.Collections;
using GrampsView.Data.Model;
using GrampsView.Models.Collections.HLinks;
using GrampsView.Models.DataModels.Date;
using GrampsView.ModelsDB.Collections.HLinks;

namespace GrampsView.Models.DataModels.Interfaces
{
    public interface IMediaModel : IModelBase
    {
        FileInfoEx CurrentStorageFile
        {
            get;

            set;
        }

        string FileContentType
        {
            get;
            set;
        }

        string FileMimeSubType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file MIME.
        /// </summary>
        /// <value>
        /// The file MIME.
        /// </value>
        string FileMimeType
        {
            get;
            set;
        }

        HLinkCitationDBModelCollection GCitationRefCollection
        {
            get;
        }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        /// <value>
        /// The date value.
        /// </value>
        DateObjectModelBase GDateValue
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the file description.
        /// </summary>
        /// <value>
        /// The file description.
        /// </value>
        string GDescription
        {
            get;
            set;
        }

        HLinkNoteDBModelCollection GNoteRefCollection
        {
            get;
        }

        HLinkTagModelCollection GTagRefCollection
        {
            get;
        }

        /// <summary>
        /// Gets the get h link Media Model that points to this ViewModel.
        /// </summary>
        /// <value>
        /// The get h link.
        /// </value>
        HLinkMediaModel HLink
        {
            get;
        }

        HLinkKey InternalMediaFileOriginalHLink
        {
            get;
            set;
        }

        bool IsImage
        {
            get;
        }

        bool IsInternalMediaFile
        {
            get; set;
        }

        bool IsOriginalFilePathValid
        {
            get;
        }

        CommonEnums.HLinkGlyphType MediaDisplayType
        {
            get;
        }

        /// <summary>
        /// Gets or sets the height of the meta data.
        /// </summary>
        /// <value>
        /// The height of the meta data.
        /// </value>
        double MetaDataHeight
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the width of the meta data.
        /// </summary>
        /// <value>
        /// The width of the meta data.
        /// </value>
        double MetaDataWidth
        {
            get; set;
        }

        string OriginalFilePath
        {
            get;

            set;
        }
    }
}