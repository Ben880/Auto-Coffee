using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace AutoCoffee
{

    

    class JsonWrapper
    {
        public static string emptyJson = "{}";
        private Dictionary<string, string> jsonValues;

        public JsonWrapper(string json)
        {
            jsonValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }

        public void trySet(string key, string value)
        {
            if (jsonValues.ContainsKey(key))
                jsonValues[key] = value;
            else
                jsonValues.Add(key, value);
        }

        public void trySet(string key, int i)
        {
            trySet(key, i.ToString());
        }

        public int tryGetInt(string name, int fallback)
        {
            try {return Int32.Parse(jsonValues[name]);} 
            catch (Exception e) {return fallback;}
        }

        public int tryGetInt(string name)
        {
            return tryGetInt(name, 0);
        }

        public string tryGetString(string name, string fallback)
        {
            if (jsonValues.ContainsKey(name))
                return jsonValues[name];
            return fallback;
        }

        public string tryGetString(string name)
        {
            return tryGetString(name, "");
        }

    }
}
