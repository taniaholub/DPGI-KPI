﻿using System;
using System.Windows.Input;

namespace WpfTimerApp
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => execute(parameter);
        public event EventHandler CanExecuteChanged { add { } remove { } }
    }
}
