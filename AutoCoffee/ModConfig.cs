using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AutoCoffee
{
    class ModConfig
    {
        public string coffeeButton { get; set; } = Keys.Q.ToString();
        public int stackAmount { get; set; } = 2;
        public bool autoDrink { get; set; } = false;
        public string autoToggleButton { get; set; } = Keys.Space.ToString();
        public bool logToCosole { get; set; } = true;
        public int baseBuffID { get; set; } = 880;

        public int[][] coffeeDeffinition = new int[][]
        {
            new int[] { 395, 120, 2 },
            new int[] { 253 , 300, 2 }
        };

        

    }
}
