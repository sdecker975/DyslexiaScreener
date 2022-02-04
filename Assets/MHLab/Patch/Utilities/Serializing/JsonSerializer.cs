using System;
using MHLab.Patch.Core.Serializing;

namespace MHLab.Patch.Utilities.Serializing
{
    public class JsonSerializer : ISerializer
    {
        public JsonSerializer()
        {
        }
        
        public string Serialize<TObject>(TObject obj) where TObject : IJsonSerializable
        {
            return JsonUtility.ToJson(obj);
        }

        public TObject Deserialize<TObject>(string data) where TObject : IJsonSerializable
        {
            var obj = Activator.CreateInstance<TObject>();
            JsonUtility.Parse(obj, data);
            return obj;
        }

        public TObject DeserializeOn<TObject>(TObject obj, string data) where TObject : IJsonSerializable
        {
            JsonUtility.Parse(obj, data);
            return obj;
        }
    }
}