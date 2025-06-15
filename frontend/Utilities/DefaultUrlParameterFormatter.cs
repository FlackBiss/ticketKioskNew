using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lastik.Utilities
{
    class DefaultUrlParameterFormatter : IUrlParameterFormatter
    {
        public string? Format(object? value, ICustomAttributeProvider attributeProvider, Type type)
        {
            if (value == null) return null;
            if (type.IsPrimitive) return value.ToString();
            return value switch
            {
                DateTime dt => dt.ToString("yyyy-MM-dd"),
                IConvertible convertible => convertible.ToString(CultureInfo.InvariantCulture),
                _ => JsonConvert.SerializeObject(value)
            };
        }
    }
}
