
using System;
namespace BillyGoats.Api.Utils
{
    /// <summary>
    /// To Specify a DefaultValue or Default SQL function for a property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AlwaysIncludeAttribute : Attribute
    {
    }
}

