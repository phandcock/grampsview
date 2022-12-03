﻿using GrampsView.Data.Model;

using SharedSharp.Common.Interfaces;

using System.Diagnostics.Contracts;

namespace GrampsView.UserControls
{
    public partial class MediaCardLarge : Grid
    {
        public static readonly BindableProperty HLinkMMProperty
          = BindableProperty.Create(returnType: typeof(HLinkMediaModel), declaringType: typeof(MediaCardLarge), propertyName: nameof(HLinkMM), propertyChanged: OnHLinkMMChanged);

        public MediaCardLarge()
        {
            InitializeComponent();

            MediaDetailImageHeight = Ioc.Default.GetService<ISharedSharpCardSizes>().MediaDetailImageHeight;
            MediaDetailImageWidth = Ioc.Default.GetService<ISharedSharpCardSizes>().MediaDetailImageWidth;
        }

        public HLinkMediaModel HLinkMM
        {
            get => (HLinkMediaModel)GetValue(HLinkMMProperty);
            set => SetValue(HLinkMMProperty, value);
        }

        public double MediaDetailImageHeight { get; set; } = 100;
        public double MediaDetailImageWidth { get; set; } = 100;
        public HLinkMediaModel TheModel { get; set; } = new HLinkMediaModel();

        private static void OnHLinkMMChanged(BindableObject bindable, object oldValue, object newValue)
        {
            Contract.Assert(bindable != null);

            MediaCardLarge? thisCard = bindable as MediaCardLarge;

            if (newValue != null)
            {
                thisCard.TheModel = newValue as HLinkMediaModel;

                thisCard.AnchorImage.BindingContext = thisCard.TheModel.HLinkGlyphItem;
            }
        }
    }
}