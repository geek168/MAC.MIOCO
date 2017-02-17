using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAC.MIOCO.Command
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private Action methodToExecute;
        private Func<bool> canExecuteEvaluator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodToExecute"></param>
        /// <param name="canExecuteEvaluator"></param>
        public DelegateCommand(Action methodToExecute, Func<bool> canExecuteEvaluator)
        {
            this.methodToExecute = methodToExecute;
            this.canExecuteEvaluator = canExecuteEvaluator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodToExecute"></param>
        public DelegateCommand(Action methodToExecute)
            : this(methodToExecute, null)
        {
        }

        public bool CanExecute(object parameter)
        {
            if (canExecuteEvaluator == null)
            {
                return true;
            }
            else
            {
                bool result = canExecuteEvaluator.Invoke();
                return result;
            }
        }

        public void Execute(object parameter)
        {
            methodToExecute.Invoke();
        }

    }
}