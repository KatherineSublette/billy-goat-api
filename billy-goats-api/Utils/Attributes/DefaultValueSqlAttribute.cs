using System;

namespace BillyGoats.Api.Utils
{
    /// <summary>
    /// To Specify a DefaultValue or Default SQL function for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultValueSqlAttribute : Attribute
    {
        public object Value { get; set; }

        public bool IsSqlFunction { get; set; }

        public DefaultValueSqlAttribute(object value, bool isSqlFunction = false) : base()
        {
            this.Value = value;
            this.IsSqlFunction = isSqlFunction;
        }
    }
}
