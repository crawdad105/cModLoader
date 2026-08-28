using cModLoader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cModLoader.ModComponents {
    internal class ModItem {

        public Dynamic Item;

        /// <summary> Item stack. </summary>
        public int stack {
            get => Item.GetValue<int>("stack"); 
            set => Item.SetValue<int>("stack", value); 
        }
        /// <summary> Item ID. </summary>
        public int type { 
            get => Item.GetValue<int>("type");
            set => Item.SetValue<int>("type", value);
        }
        /// <summary> Item name. </summary>
        public string Name {
            get {
                if (Terraria.VersionChecks._1_3AndUp) {
                    if (Terraria.VersionChecks._1_3_5AndUp) {
                        return Item.GetValue<string>("Name");
                    }
                    // the Name property did not exist
                    return Terraria.StaticReference.Lang.Invoke<object>("GetItemName", type).ToString();
                }
                return Item.GetValue<string>("name");
            }
        }

    }
}
