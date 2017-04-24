using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class StockViewModel : ViewModelBase, IDataErrorInfo
    {
        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand UpdateCommand { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public StockViewModel(Window window)
        {
            ItemTypeCollection = new ObservableCollection<ItemTypeModel>(ItemTypeModel.ItemTypeModelList);
            ItemType = ItemTypeCollection.FirstOrDefault();
            InsertCommandVisibility = Visibility.Visible;
            UpdateCommandVisibility = Visibility.Collapsed;

            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });

            InsertCommand = new DelegateCommand(() =>
            {
                var item = new ItemMaster()
                {
                    Id = Guid.NewGuid().ToString(),
                    ItemId = ItemId,
                    ItemName = ItemName,
                    ItemSize = int.Parse(ItemSize),
                    ItemType = ItemType.Type,
                    StockCount = Count,
                    StockPrice = decimal.Parse(StockPrice),
                    Price = decimal.Parse(Price),
                    UpdateTime = DateTime.Now
                };

                var sql = @"INSERT INTO [ItemMaster]([ItemId],[ItemName],[ItemSize],[ItemType],[StockCount],[SalesCount],[StockPrice],[Price],[Id],[UpdateTime])
                            VALUES
                            ('" + item.ItemId + "','" + item.ItemName + "'," + item.ItemSize + "," + item.ItemType + "," + item.StockCount + ",0," + item.StockPrice + "," + item.Price + ",'" + item.Id + "','" + item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") + "'); ";
                if(SqlServerCompactService.Insert(sql))
                {
                    InitialControlValue();
                }

            }, CanExcute);

            UpdateCommand = new DelegateCommand(() =>
            {
            }, CanExcute);
        }

        private bool CanExcute()
        {
            var ret = false;
            var i = 0;
            decimal j = 0;

            if (!string.IsNullOrEmpty(ItemId) && !string.IsNullOrEmpty(ItemName) 
                && int.TryParse(ItemSize, out i) && decimal.TryParse(StockPrice, out j) && decimal.TryParse(Price, out j))
            {
                ret = true;
            }

            return ret;
        }

        private void InitialControlValue()
        {
            ItemId = "";
            ItemName = "";
            ItemSize = "";
            Count = 0;
            StockPrice = "";
            Price = "";
        }

        private Visibility _InsertCommandVisibility = Visibility.Visible;
        public Visibility InsertCommandVisibility
        {
            get { return _InsertCommandVisibility; }
            set
            {
                _InsertCommandVisibility = value;
                OnPropertyChanged(nameof(InsertCommandVisibility));
            }
        }

        private Visibility _UpdateCommandVisibility = Visibility.Collapsed;
        public Visibility UpdateCommandVisibility
        {
            get { return _UpdateCommandVisibility; }
            set
            {
                _UpdateCommandVisibility = value;
                OnPropertyChanged(nameof(UpdateCommandVisibility));
            }
        }

        private string _ItemId;
        public string ItemId
        {
            get { return _ItemId; }
            set
            {
                _ItemId = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }

        public string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                _ItemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }

        public string _ItemSize;
        public string ItemSize
        {
            get { return _ItemSize; }
            set
            {
                _ItemSize = value;
                OnPropertyChanged(nameof(ItemSize));
            }
        }

        public ObservableCollection<ItemTypeModel> _ItemTypeCollection = new ObservableCollection<ItemTypeModel>();
        public ObservableCollection<ItemTypeModel> ItemTypeCollection
        {
            get { return _ItemTypeCollection; }
            set
            {
                _ItemTypeCollection = value;
                OnPropertyChanged(nameof(ItemTypeCollection));
            }
        }

        public ItemTypeModel _ItemType;
        public ItemTypeModel ItemType
        {
            get { return _ItemType; }
            set
            {
                _ItemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }

        public int _Count;
        public int Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));
            }
        }

        public string _StockPrice;
        public string StockPrice
        {
            get { return _StockPrice; }
            set
            {
                _StockPrice = value;
                OnPropertyChanged(nameof(StockPrice));
            }
        }

        public string _Price;
        public string Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                OnPropertyChanged(nameof(Price));
            }
        }


        #region IDataErrorInfo

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var ret = string.Empty;
                var i = 0;
                decimal j = 0;
                switch (columnName)
                {
                    case nameof(ItemSize):
                        if (!string.IsNullOrEmpty(ItemSize) && !int.TryParse(ItemSize, out i))
                        {
                            ret = " * 请输入整数！！！";
                        }
                        break;
                    case nameof(StockPrice):
                        if (!string.IsNullOrEmpty(StockPrice) && !decimal.TryParse(StockPrice, out j))
                        {
                            ret = " * 请输入整数！！！";
                        }
                        break;
                    case nameof(Price):
                        if (!string.IsNullOrEmpty(Price) && !decimal.TryParse(Price, out j))
                        {
                            ret = " * 请输入整数！！！";
                        }
                        break;
                }
                return ret;
            }
        }


        #endregion
    }
}
