using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

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

        public string[] coffeeTypes = new string[]
        {
            JsonConvert.SerializeObject(new Dictionary<string,string>() {
                { "name", "Coffee"},
                { "id", "395"},
                { "millisecondsDuration", "120000"},
                { "speed", "2"}
            }),
            JsonConvert.SerializeObject(new Dictionary<string,string>() {
                { "name", "Espresso"},
                { "id", "253"},
                { "millisecondsDuration", "300000"},
                { "speed", "2"}
            })
        };

    }
}
