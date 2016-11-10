using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Data;

namespace MvcDWZ.Base
{
    public class AppEnum
    {
        #region 工具函数
        public static SortedList GetStatus(System.Type t)
        {
            SortedList list = new SortedList();

            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                list.Add(enumKey, enumDescription);
            }
            return list;
        }

        public static string GetEnumsStr(Type enumName)
        {
            StringBuilder sb = new StringBuilder();
            // get enum fileds
            FieldInfo[] fields = enumName.GetFields();
            bool isFirst = true;

            sb.Append("[");
            foreach (FieldInfo field in fields)
            {
                if (!field.FieldType.IsEnum)
                {
                    continue;
                }
                // get enum value
                int value = (int)enumName.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                string text = field.Name;
                string description = string.Empty;
                object[] array = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (array.Length > 0)
                {
                    description = ((DescriptionAttribute)array[0]).Description;
                }
                else
                {
                    description = ""; //none description,set empty
                }
                //add to list


                if (isFirst)
                {
                    isFirst = false;
                }
                else
                { sb.Append(","); }


                sb.Append("{Name:'" + description + "'");
                sb.Append(",");
                sb.Append("Value:'" + value + "'");
                sb.Append("}");
            }
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// 检查类型名是否在当前类型中存在
        /// </summary>
        /// <param name="t"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsCodeInExistsType(System.Type t, string code)
        {
            SortedList list = new SortedList();
            Array a = Enum.GetValues(t);
            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                if (enumName == code)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载枚举类型名称
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DataTable GetEnumKeyName(System.Type t)
        {
            SortedList list = new SortedList();
            Array a = Enum.GetValues(t);
            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("text", System.Type.GetType("System.String"));
            dt.Columns.Add("value", System.Type.GetType("System.String"));

            for (int i = 0; i < a.Length; i++)
            {
                string enumName = a.GetValue(i).ToString();
                string enumKeyName = System.Enum.Parse(t, enumName).ToString();
                int enumKey = (int)System.Enum.Parse(t, enumName);
                string enumDescription = GetDescription(t, enumKey);
                dr = dt.NewRow();
                dr[0] = enumDescription;
                dr[1] = enumKey;
                //ddl.Items.Insert(i + 1, new System.Web.UI.WebControls.ListItem(enumDescription,enumKeyName));
                dt.Rows.Add(dr);

            }
            return dt;
        }

        private static string GetName(System.Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        public static string GetDescription(System.Type t, object v)
        {
            try
            {
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }

        public static string _GetDescription(System.Type t, object v)
        {
            try
            {
                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "UNKNOWN";
            }
        }
        #endregion

        public enum UserType : int
        {
            [Description("机构用户")]
            Organ = 1,
            [Description("商户用户")]
            Vendor = 2,
        }
    }
}