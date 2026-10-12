using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Portal.core.GenericRepository
{
    public class GenericReflection<TEntity> : IGenericReflection<TEntity> where TEntity : class, new()
    {
        public IList<TEntity> ToListReflection(DataTable table)
        {
            List<TEntity> result = new List<TEntity>();
            IList<PropertyInfo> properties = typeof(TEntity).GetProperties().ToList();

            foreach (var row in table.Rows)
                result.Add(CreateItemFromRow((DataRow)row, properties));

            return result;
        }

        public TEntity CreateItemFromRow(DataRow row, IList<PropertyInfo> properties)
        {
            TEntity item = new TEntity();

            foreach (var property in properties)
                if (row.Table.Columns[property.Name] != null)
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                        CreateColumnFromRowTypeCode(item, property, row[property.Name]);

            return item;
        }

        public void CreateColumnFromRowTypeCode(TEntity item, PropertyInfo property, object value)
        {
            try
            {
                if (property != null && value != null)
                {
                    TypeCode typeCode = Type.GetTypeCode(property.PropertyType);

                    switch (typeCode)
                    {
                        case TypeCode.Int16:
                            property.SetValue(item, Convert.ToInt16(value), null);
                            break;
                        case TypeCode.Int32:
                            property.SetValue(item, Convert.ToInt32(value), null);
                            break;
                        case TypeCode.Int64:
                            property.SetValue(item, Convert.ToInt64(value), null);
                            break;
                        case TypeCode.Decimal:
                            property.SetValue(item, Convert.ToDecimal(value), null);
                            break;
                        case TypeCode.Boolean:
                            if (value.ToString() == "1")
                                property.SetValue(item, true, null);
                            else if (value.ToString() == "0")
                                property.SetValue(item, false, null);
                            else
                                property.SetValue(item, Convert.ToBoolean(value), null);
                            break;
                        case TypeCode.DateTime:
                            property.SetValue(item, Convert.ToDateTime(value), null);
                            break;
                        default:
                            CreateColumnFromRowPropType(item, property, value);
                            break;
                    }
                }
            }
            catch (Exception ex) { throw new Exception("GenericReflection - CreateColumnFromRowTypeCode", ex); }
        }

        public void CreateColumnFromRowPropType(TEntity item, PropertyInfo property, object value)
        {
            try
            {
                if (property != null && value != null)
                {
                    string propertyType = property.PropertyType.ToString().ToLower();

                    switch (propertyType)
                    {
                        case string a when propertyType.Contains("int16"):
                            property.SetValue(item, Convert.ToInt16(value), null);
                            break;
                        case string a when propertyType.Contains("int32"):
                            property.SetValue(item, Convert.ToInt32(value), null);
                            break;
                        case string a when propertyType.Contains("int64"):
                            property.SetValue(item, Convert.ToInt64(value), null);
                            break;
                        case string a when propertyType.Contains("decimal"):
                            property.SetValue(item, Convert.ToDecimal(value), null);
                            break;
                        case string a when propertyType.Contains("bool"):
                            if (value.ToString() == "1")
                                property.SetValue(item, true, null);
                            else if (value.ToString() == "0")
                                property.SetValue(item, false, null);
                            else
                                property.SetValue(item, Convert.ToBoolean(value), null);
                            break;
                        case string a when propertyType.Contains("datetime"):
                            property.SetValue(item, Convert.ToDateTime(value), null);
                            break;
                        case string a when propertyType.Contains("binary"):
                            if (value is string stringValue && !string.IsNullOrEmpty(value.ToString()))
                            {
                                // Convert the string to a byte array. You can use a specific encoding if necessary.
                                byte[] binaryValue = Convert.FromBase64String(stringValue);
                                property.SetValue(item, binaryValue, null);
                            }
                

                            property.SetValue(item, null, null);
                            break;
                        default:
                            property.SetValue(item, value, null);
                            break;
                    }
                }
            }
            catch (Exception ex) { throw new Exception("GenericReflection - CreateColumnFromRowPropType", ex); }
        }
    }

}
