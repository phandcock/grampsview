﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Data.Model;
using GrampsView.Views;

using PropertyChanged;

using SharedSharp.Sizes;

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

            MediaDetailImageHeight = Ioc.Default.GetRequiredService<ISharedCardSizes>().MediaDetailImageHeight;
            MediaDetailImageWidth = Ioc.Default.GetRequiredService<ISharedCardSizes>().MediaDetailImageWidth;
        }

        public HLinkMediaModel HLinkMM
        {
            get => (HLinkMediaModel)GetValue(HLinkMMProperty);
            set => SetValue(HLinkMMProperty, value);
        }

        void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
        {
            Navigation.PushAsync(new MediaDetailPage(args.Parameter as HLinkMediaModel));
        }

        public double MediaDetailImageHeight { get; set; } = 100;
        public double MediaDetailImageWidth { get; set; } = 100;
        public HLinkMediaModel TheModel { get; set; } = new HLinkMediaModel();

        [SuppressPropertyChangedWarnings]
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