﻿using Newtonsoft.Json;
using System.IO;

namespace Categorizer.Data.JsonProviders
{
    public class JsonProvider<T> : IDataProvider<T>
    {
        public string Filepath { get; set; }

        virtual public void Save(T obj)
        {
            JsonConvert.SerializeObject(obj);
            File.WriteAllText(Filepath, JsonConvert.SerializeObject(obj));
        }

        virtual public T Read()
        {
            string json = string.Empty;
            json = File.ReadAllText(Filepath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
