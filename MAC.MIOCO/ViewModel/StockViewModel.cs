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

        private string Id;
        private bool IsRepeat = false;

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

            BindData();

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
                    UpdateTime = DateTime.Now,
                    Color = Color
                };

                if (SqlServerCompactService.InsertItemMaster(item))
                {
                    InitialControlValue();
                    BindData();
                }

            }, CanExcute);

            UpdateCommand = new DelegateCommand(() =>
            {
                var item = new ItemMaster()
                {
                    Id = Id,
                    ItemId = ItemId,
                    ItemName = ItemName,
                    ItemSize = int.Parse(ItemSize),
                    ItemType = ItemType.Type,
                    StockCount = Count,
                    StockPrice = decimal.Parse(StockPrice),
                    Price = decimal.Parse(Price),
                    UpdateTime = DateTime.Now,
                    Color = Color
                };


                if (SqlServerCompactService.UpdateItemMaster(item))
                {
                    InitialControlValue();
                    BindData();
                    InsertCommandVisibility = Visibility.Visible;
                    UpdateCommandVisibility = Visibility.Collapsed;
                }

            }, CanExcute);

            ClearCommand = new DelegateCommand(() =>
            {
                InitialControlValue();
            });

            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (PageIndex + 1) * PAGESIZE < SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Count() ? true : false; });

            SearchCommand = new DelegateCommand(() =>
            {
                var s = SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0);
                PageIndex = 0;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(s.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });

            AllCommand = new DelegateCommand(() =>
            {
                SearchItemId = "";
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });

            SelectCommand = new DelegateCommand<ItemMaster>(s =>
            {
                if (s != null)
                {
                    InsertCommandVisibility = Visibility.Collapsed;
                    UpdateCommandVisibility = Visibility.Visible;

                    Id = s.Id;
                    ItemId = s.ItemId;
                    ItemName = s.ItemName;
                    ItemSize = s.ItemSize.ToString();
                    ItemType = ItemTypeCollection.FirstOrDefault(t=>t.Type == s.ItemType);
                    StockPrice = s.StockPrice.ToString();
                    Price = s.Price.ToString();
                    Count = s.StockCount;
                    Color = s.Color;

                    IsRepeat = false;
                }
            });

            DeleteCommand = new DelegateCommand<ItemMaster>(s =>
            {
                if (s != null)
                {
                    if (MessageBox.Show(window, "是否确认删除该货品？", "确认删除点“Yes”，否则点“No”", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        SqlServerCompactService.DeleteItemMaster(s);
                        BindData();
                        InitialControlValue();
                    }
                }
            });
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
            Count = 1;
            StockPrice = "";
            Price = "";
            Color = "";
            ItemType = ItemTypeCollection.FirstOrDefault();
        }

        private void BindData()
        {
            SOURCE = new ObservableCollection<ItemMaster>(SqlServerCompactService.GetData("ItemMaster").Cast<ItemMaster>());
            SOURCE = new ObservableCollection<ItemMaster>(SOURCE.OrderByDescending(s => s.UpdateTime));
            ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        public DelegateCommand<ItemMaster> SelectCommand { get; private set; }

        public DelegateCommand<ItemMaster> DeleteCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

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

                if (SOURCE.FirstOrDefault(s => string.Compare(s.ItemId, value, true) == 0) != null)
                {
                    IsRepeat = true;
                }
                else
                {
                    IsRepeat = false;
                }
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

        public int _Count = 1;
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

        public string _Color;
        public string Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private int PAGESIZE = 14;
        private int _PageIndex = 0;
        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                if (value >= 0)
                {
                    _PageIndex = value;
                }
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

        private ObservableCollection<ItemMaster> SOURCE = new ObservableCollection<ItemMaster>();

        private string _SearchItemId = "";
        public string SearchItemId
        {
            get { return _SearchItemId; }
            set
            {
                _SearchItemId = value;
                OnPropertyChanged(nameof(SearchItemId));
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
                    case nameof(ItemId):
                        if(IsRepeat && !string.IsNullOrEmpty(ItemId))
                        {
                            ret = " * 商品编号重复请检查！！！";
                        }
                        break;
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
