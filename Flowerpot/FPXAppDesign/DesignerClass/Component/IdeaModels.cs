using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class IdeaModel
    {
        public IdeaModel()
        {
            Columns = new List<ColumnInIdeaModel>();
            IdeaDescription = string.Empty;
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public List<ColumnInIdeaModel> Columns { get; set; }
    }

    public class ColumnInIdeaModel
    {
        public ColumnInIdeaModel()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int ColumnId { get; set; }

        public string ColumnName { get; set; }

        public DataTypeId MyDataTypeId { get; set; }

        public int ReferedIdeaId { get; set; }

        public string ReferedIdeaIdName { get; set; }

        public List<string> ReferedColumnIds { get; set; }

        public string ReferedColumnNames { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int IdeaId { get; set; }
    }

    public class IdeaDetailModel
    {
        public IdeaDetailModel()
        {
            Rows = new List<RowDesigner>();
            Columns = new List<ColumnInIdeaModel>();
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public List<RowDesigner> Rows { get; set; }

        public List<ColumnInIdeaModel> Columns { get; set; }
    }

    public enum DataTypeId
    {
        [StringValue("Money")]
        Money = 0,
        [StringValue("Number")]
        Number = 1,
        [StringValue("DateTime")]
        Datetime = 2,
        [StringValue("LongText")]
        LongText = 3,
        [StringValue("ShortText")]
        ShortText = 4,
        [StringValue("IdeaType")]
        IdeaType = 5,
        [StringValue("Status")]
        Status = 6
    }

    public class StringValue : System.Attribute
    {
        public StringValue()
        {
        }

        public StringValue(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            var type = value.GetType();

            //Check first in our cached results...

            //Look for our 'StringValueAttribute' 

            //in the field's custom attributes

            var fi = type.GetField(value.ToString());
            var attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}
