using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class SalesWindowViewModel : ViewModelBase
    {

        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }
        Window WINDOW;
        public event EventHandler AfterLogoutHandler;

        private string _SalesDate;
        public string SalesDate
        {
            get { return _SalesDate; }
            set
            {
                _SalesDate = value;
                OnPropertyChanged(nameof(SalesDate));
            }
        }

        private ObservableCollection<ItemMaster> _ItemMasterColletion;
        public ObservableCollection<ItemMaster> ItemMasterColletion
        {
            get { return _ItemMasterColletion; }
            set
            {
                _ItemMasterColletion = value;
                OnPropertyChanged(nameof(ItemMasterColletion));
            }
        }

        public SalesWindowViewModel(Window window)
        {
            SalesDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Timer timer = new Timer(1000);
            timer.Elapsed += (s, e) => { SalesDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); };
            timer.Start();

            WINDOW = window;
            LogoutCommand = new DelegateCommand(Execute, () => { return true; });
            CloseCommand = new DelegateCommand(() => { Application.Current.Shutdown(); }, () => { return true; });

            ItemMasterColletion = new ObservableCollection<ItemMaster>
            {
                new ItemMaster {ItemId="MIC123456789", ItemName="春装上衣--ABC110", ItemSize =1, StockCount=10, SalesCount=1, StockPrice=250.00 },
                new ItemMaster {ItemId="MIC123456ABC", ItemName="夏季装上衣--GDA110", ItemSize =2, StockCount=0, SalesCount=1, StockPrice=116.88 },
                new ItemMaster {ItemId="MIC12345633A", ItemName="冬装大衣--ABC110", ItemSize =1, StockCount=9, SalesCount=1, StockPrice=56.47 },
                new ItemMaster {ItemId="MIC123456891", ItemName="秋装连衣裙ABC110", ItemSize =4, StockCount=3, SalesCount=1, StockPrice=218.66 },
                new ItemMaster {ItemId="MIC123456301", ItemName="春装坎肩ABC110", ItemSize =6, StockCount=0, SalesCount=1, StockPrice=380.00 },
                new ItemMaster {ItemId="MIC123456EFT", ItemName="夏装披风加99生命ABC110", ItemSize =7, StockCount=5, SalesCount=1, StockPrice=158.00 },
                new ItemMaster {ItemId="MIC123456GHJ", ItemName="秋装上衣ABC110", ItemSize =6, StockCount=6, SalesCount=1, StockPrice=188 },
                new ItemMaster {ItemId="MIC12345699A", ItemName="冬装上衣ABC110", ItemSize =3, StockCount=9, SalesCount=1, StockPrice=66 },
                new ItemMaster {ItemId="MIC123456QWR", ItemName="完美装上衣ABC110", ItemSize =6, StockCount=0, SalesCount=1, StockPrice=999 },
                new ItemMaster {ItemId="MIC123456GTL", ItemName="无敌漂亮美少女上衣11111111111ABC110", ItemSize =1, StockCount=2, SalesCount=1, StockPrice=19999 },
                new ItemMaster {ItemId="MIC123456GTL", ItemName="巴拉巴拉小魔仙ABC110", ItemSize =1, StockCount=2, SalesCount=1, StockPrice=9999 },
                new ItemMaster {ItemId="MIC123456GTL", ItemName="旅居下ABC110", ItemSize =1, StockCount=2, SalesCount=1, StockPrice=8888 },
                new ItemMaster {ItemId="MIC123456GTL", ItemName="无敌浩克ABC110", ItemSize =1, StockCount=2, SalesCount=1, StockPrice=6666 },
                new ItemMaster {ItemId="MIC123456GTL", ItemName="钢铁侠ABC110", ItemSize =1, StockCount=2, SalesCount=1, StockPrice=454547 }
            };
        }

        public void Execute()
        {
            WINDOW.Close();
            AfterLogoutHandler(null, null);
        }



    }
}
