using System;
using System.Windows.Input;

namespace DS.Interfaces
{
    public class RelayCommand : ICommand
    {
        private readonly Func<Boolean> canExecute;
        private readonly Action<object> execute;

        public RelayCommand(Action<object> execute)
          : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Func<Boolean> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        public Boolean CanExecute(Object parameter)
        {
            return canExecute == null ? true : canExecute();
        }

        public void Execute(Object parameter)
        {
            execute(parameter);
        }
    }
}
