﻿// TODO Needs XML 1.71 check

/// <summary>
/// </summary>
namespace GrampsView.Data.Model
{
    using GrampsView.Common;

    using System;
    using System.Collections.ObjectModel;
    using System.Runtime.Serialization;

    using Xamarin.CommunityToolkit.ObjectModel;
    using Xamarin.Forms;

    /// <summary>
    /// Styled Text model collection.
    /// </summary>

    [KnownType(typeof(StyledTextModel))]
    public class StyledTextModel : ObservableObject, IStyledTextModel
    {
        private readonly ObservableCollection<GrampsStyle> _Styles

            = new ObservableCollection<GrampsStyle>();

        private string _GText = string.Empty;

        private FormattedString _TextFormatted = new FormattedString();

        public StyledTextModel()
        {
        }

        public string GText
        {
            get => _GText;

            set => SetProperty(ref _GText, value);
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public ObservableCollection<GrampsStyle> Styles
        {
            get
            {
                return _Styles;
            }
        }

        public FormattedString TextFormatted
        {
            get
            {
                if (_TextFormatted.Spans.Count == 0)
                {
                    _TextFormatted = GrampsTextToXamarinText.GetFormattedString(this, SharedSharp.Common.SharedSharpFontSize.FontMedium);
                }

                return _TextFormatted;
            }
        }

        /// <summary>
        /// Gets the shortened form of the text. Maximum length is 100.
        /// </summary>
        /// <value>
        /// The text short.
        /// </value>
        public string TextShort
        {
            get
            {
                return GText.Substring(0, Math.Min(GText.Length, 100));
            }
        }
    }
}