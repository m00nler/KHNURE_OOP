﻿using System.IO;
using System.Threading.Tasks;
using DAL.Interfaces;
using Newtonsoft.Json;

namespace Core.Services
{
    class SerializationWorker : ISerializationWorker
    {
        public void Serialize<TEntity>(TEntity obj, string jsonFileName)
        {
            var json = JsonConvert.SerializeObject(obj);
            File.WriteAllText(jsonFileName, json);
        }

        public TEntity Deserialize<TEntity>(string fileName)
        {
            var json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<TEntity>(json);
        }
    }
}
