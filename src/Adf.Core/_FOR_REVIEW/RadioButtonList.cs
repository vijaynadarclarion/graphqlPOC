//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Adf.Core._FOR_REVIEW
//{
//    public sealed class RadioButtonList
//    {
//        #region Constructs
//        public RadioButtonList(IEnumerable items, string dataValueField, string dataLabelField, object selectedValue = null)
//        {
//            Items = items;
//            DataValueField = dataValueField;
//            DataLabelField = dataLabelField;
//            SelectedValue = selectedValue;
//        }

//        #endregion Constructs

//        #region Property
//        public string DataValueField { get; private set; }
//        public string DataLabelField { get; private set; }
//        public object SelectedValue { get; set; }
//        public IEnumerable Items { get; private set; }
//        #endregion Property

//    }
//}


//public static RadioButtonList ToRadioButtonList<T>(this IEnumerable<T> items, Expression<Func<T, object>> dataValueField, Expression<Func<T, object>> dataLabelField, object selectedValue = null)
//{

//    return new RadioButtonList(items, dataValueField.GetPropertyName(), dataLabelField.GetPropertyName(), selectedValue);
//}
