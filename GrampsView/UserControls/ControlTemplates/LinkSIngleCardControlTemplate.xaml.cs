﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Models.HLinks;

namespace GrampsView.UserControls
{


    public partial class LinkSingleCardControlTemplate : ContentView
    {
        public LinkSingleCardControlTemplate()
        {
            InitializeComponent();
        }

        private void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
        {
            Navigation.PushAsync((args.Parameter as HLinkBase).NavigationPage());
        }
    }
}