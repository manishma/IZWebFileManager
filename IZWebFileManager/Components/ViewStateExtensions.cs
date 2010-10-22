using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace IZ.WebFileManager.Components
{
    static class ViewStateExtensions
    {
        public static T GetValue<T>(this StateBag stateBag, string key, T defaultValue)
        {
            return (T) (stateBag[key] ?? defaultValue);
        }

        public static void SetValue<T>(this StateBag stateBag, string key, T value)
        {
            stateBag[key] = value;
        }
    }
}
