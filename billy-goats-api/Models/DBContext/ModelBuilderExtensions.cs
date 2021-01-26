using BillyGoats.Api.Utils;
using BillyGoats.Api.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Reflection;

namespace BillyGoats.Api.Models.DBContext
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Set default value or default sql function if property is tagged with DefaultValue or DefaultSqlValue
        /// </summary>
        /// <param name="modelBuilder"></param>
        //public static void SetDefault(this ModelBuilder modelBuilder)
        //{
        //    foreach (var entity in modelBuilder.Model.GetEntityTypes())
        //    {
        //        // set column default if default tag is set
        //        foreach (var property in entity.GetProperties())
        //        {
        //            var memberInfo = property.PropertyInfo ?? (MemberInfo)property.FieldInfo;
        //            var defaultValue = memberInfo?.GetCustomAttribute<DefaultValueSqlAttribute>();
        //            if (defaultValue != null)
        //            {
        //                if (defaultValue.IsSqlFunction)
        //                {
        //                    property.Npgsql().DefaultValueSql = defaultValue.Value.ToString();
        //                }
        //                else
        //                {
        //                    property.Npgsql().DefaultValue = defaultValue.Value;
        //                }
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// Convert DB Table and Column name, Index name etc from Pascal case to snake case
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void NamePascalToSnake(this ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.SetTableName(entity.GetTableName().CamelToSnakeCase());
                // entity.Relational().TableName = entity.Relational().TableName.CamelToSnakeCase();

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.SetColumnName(property.GetColumnName().CamelToSnakeCase());
                    // property.Relational().ColumnName = property.Relational().ColumnName.CamelToSnakeCase();
                }

                foreach (var key in entity.GetKeys())
                {
                    key.SetName(key.GetName().CamelToSnakeCase());
                    //key.Relational().Name = key.Relational().Name.CamelToSnakeCase();

                    // keep the primary key name convention to be consistant with the exiting db
                    // key.Relational().Name = key.Relational().Name.TrimStart("pk_".ToCharArray()) + "_pkey";
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.SetConstraintName(key.GetConstraintName().CamelToSnakeCase());
                    //key.Relational().Name = key.Relational().Name.CamelToSnakeCase();
                }

                //foreach (var index in entity.GetIndexes())
                //{
                //    index.Set (index.GetDatabaseName().CamelToSnakeCase());
                //    //index.Relational().Name = index.Relational().Name.CamelToSnakeCase();
                //}
            }
        }
    }
}

