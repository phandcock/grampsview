﻿namespace GrampsView.Views
{
    using GrampsView.ViewModels;

    using Microsoft.Extensions.DependencyInjection;

    public sealed partial class SettingsPage : ViewBase
    {
        private SettingsViewModel _viewModel { get; set; }

        public SettingsPage()
        {
            InitializeComponent(); BindingContext = _viewModel = App.Current.Services.GetService<SettingsViewModel>();
        }
    }
}