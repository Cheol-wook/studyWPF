using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfMvvmApp.ViewMadels
{

    /// <summary>
    /// viewModel과 view를 glue하기 위함
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute;         //실행 처리를 위한 Generic
        private readonly Predicate<T> canExecute;   //실행 여부를 판단하는 Generic

        //실행 여부에 따라 이벤트를 추가하거나 삭제하는 이벤트핸들러
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke((T)parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            execute((T)parameter);
        }
        /// <summary>
        /// execute만 파라미터로 받음
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<T> execute) : this(execute, null) { }

        /// <summary>
        /// execute, canexecute를 파라미터로 받음
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentException(nameof(execute));
            this.canExecute = canExecute;
        }
    }
}
