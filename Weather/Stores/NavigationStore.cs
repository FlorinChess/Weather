﻿using System;
using Weather.ViewModels;

namespace Weather.Stores
{
    public sealed class NavigationStore
    {
        public event Action CurrentViewModelChanged;

        private BaseViewModel _currentViewModel;
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
