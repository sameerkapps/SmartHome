// ////////////////////////////////////////////////////////////
// // Copyright 2017 Sameer Khandekar                        //
// // License: MIT License                                   //
// ////////////////////////////////////////////////////////////
using System;
using HomeMonitor.ViewModels;

using MvvmAtom;


namespace HomeMonitor.Commands
{
    public abstract class HomeMonitorCommandBase : MvvmAtom.AtomCommandBase
    {
        public HomeMonitorCommandBase(AtomViewModelBase viewModel)
        : base(viewModel)
        {
        }

        protected MainPageViewModel MainViewModel => (MainPageViewModel)ViewModel;
    }
}
