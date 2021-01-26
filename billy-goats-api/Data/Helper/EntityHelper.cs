using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BillyGoats.Api.Utils;
using Newtonsoft.Json;

namespace BillyGoats.Api.Data.Helper
{
    public static class EntityHelper
    {
        public static void ConvertKeyValues<T>(params object[] keyValues) where T : class
        {
            var keys = typeof(T)
                 .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            int i = 0;
            foreach (var key in keys)
            {
                keyValues[i] = Convert.ChangeType(keyValues[i], key.PropertyType);
                i++;
            }
        }

        public static object[] GetKeyValues(this object entity)
        {
            var keys = entity.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            var keyValues = new List<object>();
            foreach (var key in keys)
            {
                keyValues.Add(key.GetValue(entity, null));
            }

            return keyValues.ToArray();
        }

        public static bool HasKey(this object entity)
        {
            var keys = entity.GetType()
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(KeyAttribute), true).Length != 0);
            var keyValues = new List<object>();

            // no key field
            if (!keys.Any())
            {
                return false;
            }

            foreach (var key in keys)
            {
                var keyType = key.PropertyType;
                var keyVal = key.GetValue(entity, null);
                if (keyVal == null)
                {
                    return false;
                }

                if ((keyType == typeof(int) || keyType == typeof(long)) && (int)keyVal == 0)
                {
                    return false;
                }

                if (keyType == typeof(Guid) && (keyVal == null || (Guid)keyVal == Guid.Empty))
                {
                    return false;
                }

                if (keyType == typeof(string) && keyVal.ToString() == string.Empty)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Copies the json ignored values from origin.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <param name="from">From.</param>
        public static void CopyJsonIgnoredValuesFromOrigin(this object entity, object from)
        {
            var props = from.GetType()
               .GetProperties()
               .Where(p => 
                   p.CanWrite
                   && !p.GetCustomAttributes(typeof(NotMappedAttribute), true).Any() 
                   && p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any()
               );

            foreach (var prop in props)
            {
                var colAttribute = prop.GetCustomAttributes(typeof(ColumnAttribute), true).FirstOrDefault() as ColumnAttribute;
                // exluding jsonb field
                if (colAttribute == null || colAttribute.TypeName.ToLower () != "jsonb")
                {
                    var val = prop.GetValue(from);
                    prop.SetValue(entity, val);
                }
            }
        }
    }
}
