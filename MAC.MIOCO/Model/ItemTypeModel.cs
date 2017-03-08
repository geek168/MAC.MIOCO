using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAC.MIOCO;

namespace MAC.MIOCO.Model
{
    public class ItemTypeModel
    {
        public int Type { get; set; }

        public string ItemTypeName { get; set; }

        public static List<ItemTypeModel> ItemTypeModelList { get; private set; }

        static ItemTypeModel()
        {
            ItemTypeModelList = SqlServerCompactService.GetData("ItemTypeModel").Cast<ItemTypeModel>().ToList();
        }
    }
}
