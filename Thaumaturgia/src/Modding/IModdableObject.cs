using System.Collections.Generic;

namespace Thaumaturgia.Modding
{    public interface IModdableObject
    {
        Dictionary<string, IField> GetFields();
        void AddField<T>(string name, Field<T> field);
        Field<T> GetField<T>(string name);
    }
    
    public abstract class ModdableObject : IModdableObject
    {
        private Dictionary<string, IField> _fields = new Dictionary<string, IField>();
        
        public Dictionary<string, IField> GetFields()
        {
            return _fields;
        }
        
        public void AddField<T>(string name, Field<T> field)
        {
            _fields[name] = field;
        }
          public Field<T> GetField<T>(string name)
        {
            if (_fields.TryGetValue(name, out IField? field) && field is Field<T> typedField)
            {
                return typedField;
            }
            throw new KeyNotFoundException($"Field '{name}' not found or wrong type.");
        }
    }
}