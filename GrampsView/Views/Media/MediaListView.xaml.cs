﻿namespace GrampsView.Views
{
    public sealed partial class MediaListPage : ViewBase
    {
        public MediaListPage()
        {
            InitializeComponent(); BindingContext = _viewModel = App.Current.Services.GetService<ItemsViewModel>();
        }
    }
}