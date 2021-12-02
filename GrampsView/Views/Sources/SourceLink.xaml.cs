﻿namespace GrampsView.UserControls
{
    using GrampsView.ViewModels;
    using GrampsView.Views;

    using Microsoft.Extensions.DependencyInjection;

    public partial class SourceLink : ViewBase
    {
        private SourceDetailViewModel _viewModel { get; set; }

        public SourceLink()
        {
            InitializeComponent(); BindingContext = _viewModel = App.Current.Services.GetService<SourceDetailViewModel>();
        }
    }
}