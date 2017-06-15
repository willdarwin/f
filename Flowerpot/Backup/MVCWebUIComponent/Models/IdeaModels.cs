using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace MVCWebUIComponent.Models
{
    public class IdeaModel
    {
        public IdeaModel()
        {
            Columns = new List<ColumnInIdeaModel>();
            IdeaDescription = "";
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int IdeaId { get; set; }

        [Required(ErrorMessage = "A name is required!")]
        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<ColumnInIdeaModel> Columns { get; set; }
    }

    public class ColumnInIdeaModel
    {
        public ColumnInIdeaModel()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int ColumnId { get; set; }

        [Required(ErrorMessage = "A name is required!")]
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
            Rows = new List<RowModel>();
            Columns = new List<ColumnInIdeaModel>();
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int IdeaId { get; set; }

        [Required(ErrorMessage = "A name is required!")]
        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<RowModel> Rows { get; set; }

        public IList<ColumnInIdeaModel> Columns { get; set; }
    }

    public enum DataTypeId
    {
        [StringValue("Money")]
        Money = 0,
        [StringValue("Number")]
        Number = 1,
        [StringValue("Datetime")]
        Datetime = 2,
        [StringValue("LongText")]
        LongText = 3,
        [StringValue("ShortText")]
        ShortText = 4,
        [StringValue("Entity")]
        Entity = 5,
        [StringValue("Status")]
        Status = 6
    }

    public class StringValue : System.Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            //Check first in our cached results...

            //Look for our 'StringValueAttribute' 

            //in the field's custom attributes

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
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
