﻿// Copyright (c) phandcock.  All rights reserved.

using GrampsView.Data.Model;
using GrampsView.Views;

namespace GrampsView.UserControls
{
    public partial class NoteCardSmall : SmallCardControlTemplate
    {
        public NoteCardSmall()
        {
            InitializeComponent();
        }

        private void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
        {
            Navigation.PushAsync(new NoteDetailPage(args.Parameter as HLinkNoteModel));
        }
    }
}