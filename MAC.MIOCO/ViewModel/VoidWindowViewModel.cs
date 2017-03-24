using MAC.MIOCO.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class VoidWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public VoidWindowViewModel(Window window)
        {
            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });
        }

        public DelegateCommand CloseCommand { get; private set; }
    }
}
