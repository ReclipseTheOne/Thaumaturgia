using System.Collections.Generic;

namespace Thaumaturgia.Modding
{
    public interface IModdableObject
    {
        Dictionary<string, IField> GetModFields();
        void AddField<T>(string name, Field<T> field);
        Field<T> GetField<T>(string name);
    }
    
    public abstract class ModdableObject : IModdableObject
    {
        private Dictionary<string, IField> _modFields = new Dictionary<string, IField>();
        
        public Dictionary<string, IField> GetModFields()
        {
            return _modFields;
        }
        
        public void AddField<T>(string name, Field<T> field)
        {
            _modFields[name] = field;
        }
        
        public Field<T> GetField<T>(string name)
        {
            if (_modFields.TryGetValue(name, out IField field) && field is Field<T> typedField)
            {
                return typedField;
            }
            throw new KeyNotFoundException($"Field '{name}' not found or wrong type.");
        }
    }
}