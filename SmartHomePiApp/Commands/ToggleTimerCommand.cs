////////////////////////////////////////////////////////
// Copyright (c) 2017 Sameer Khandekar                //
// License: MIT License.                              //
////////////////////////////////////////////////////////
using System;

using MvvmAtom;

using SmartHomePiApp.ViewModels;

namespace SmartHomePiApp.Commands
{
    /// <summary>
    /// Toggles the timer on/off
    /// </summary>
    public class ToggleTimerCommand : AtomCommandBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="viewModel">The viee model</param>
        public ToggleTimerCommand(AtomViewModelBase viewModel)
            :base(viewModel)
        {
        }

        /// <summary>
        /// Can always execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Toggle the timer
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            MainViewModel.IsTimerOn = !MainViewModel.IsTimerOn;
        }

        private MainPageViewModel MainViewModel => (MainPageViewModel)ViewModel;
    }
}
