using System;
using System.Data.SqlTypes;

namespace Hasseware.ComponentModel.Mapping.Converters
{
    internal sealed class SafeSqlConvert

    {
        public static DateTime ToDateTime(SqlDateTime sqldate)
        {
            return sqldate.Value;
        }

        public static SqlDateTime ToSqlDateTime(DateTime date)
        {
            return date < (DateTime)SqlDateTime.MinValue ? SqlDateTime.MinValue : date;
        }

        public static DateTime? ToNullableDateTime(SqlDateTime sqldate)
        {
            return sqldate.Value;
        }

        public static SqlDateTime? ToNullableSqlDateTime(DateTime date)
        {
            return date < (DateTime)SqlDateTime.MinValue ? SqlDateTime.MinValue : date;
        }
    }
}

