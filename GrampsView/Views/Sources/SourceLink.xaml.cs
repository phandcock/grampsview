﻿namespace GrampsView.UserControls
{
    using GrampsView.Views;

    public partial class SourceLink : ViewBase
    {
        public SourceLink()
        {
            InitializeComponent(); BindingContext = _viewModel = App.Current.Services.GetService<ItemsViewModel>();
        }
    }
}