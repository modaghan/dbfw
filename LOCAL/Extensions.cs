using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LOCAL
{
    public static class Extensions
    {
        public class DisplayNameLocalizedAttribute : DisplayNameAttribute
        {
            private string _displayName;
            public string Name { get; set; }

            public DisplayNameLocalizedAttribute(string resourceKey, Type resourceType)
            {
                try
                {
                    PropertyInfo propInfo = resourceType.GetProperty(resourceKey,
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    _displayName = (string)propInfo.GetValue(propInfo.DeclaringType, null);
                    Name = _displayName;
                }
                catch (Exception)
                {
                    _displayName = resourceKey;
                }
            }

            public override string DisplayName
            {
                get
                {
                    return _displayName;
                }
            }
        }
        public class DisplayFormatLocalizedAttribute : DisplayFormatAttribute
        {
            public DisplayFormatLocalizedAttribute(string resourceKey, Type resourceType)
            {                
                PropertyInfo propInfo = resourceType.GetProperty(resourceKey,
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                base.ConvertEmptyStringToNull = true;
                base.NullDisplayText = (string)propInfo.GetValue(propInfo.DeclaringType, null);
            }
            
        }

        public class DefaultingAttribute : DefaultValueAttribute
        {
            public DefaultingAttribute(Type type, string value = null) : base(type, value)
            {
                if (type == typeof(DateTime))
                    base.SetValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm.ss.fff"));
            }

        }

        public class NotListingAttribute : Attribute
        {
            public NotListingAttribute()
            {

            }
        }
    }
}
