using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DataService.MySql
{
    public class MysqlInsertQueryBuilder
    {
        private readonly List<string> fieldNames;
        private readonly List<string> fieldValues;

        private readonly string tableName;

        public void AddField(string fieldName, string fieldValue)
        {
            fieldNames.Add(fieldName);
            fieldValues.Add(String.Format("'{0}'", fieldValue));
        }

        public void AddField(string fieldName, Int32 fieldValue)
        {
            fieldNames.Add(fieldName);
            fieldValues.Add(fieldValue.ToString());
        }

        public void AddField(string fieldName, Int64 fieldValue)
        {
            fieldNames.Add(fieldName);
            fieldValues.Add(fieldValue.ToString());
        }

        public void AddField(string fieldName, bool fieldValue)
        {
            fieldNames.Add(fieldName);
            fieldValues.Add(GetBit(fieldValue).ToString());
        }

        public MysqlInsertQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            fieldNames = new List<string>();
            fieldValues = new List<string>();
        }

        public string GetInsertQuery()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO ");

            queryBuilder.Append(tableName);

            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (i != fieldNames.Count - 1)
                {
                    names.AppendFormat("`{0}`,", fieldNames[i]);
                    values.AppendFormat(" {0} ,", fieldValues[i]);
                }
                else
                {
                    names.AppendFormat("`{0}`", fieldNames[i]);
                    values.AppendFormat(" {0} ", fieldValues[i]);
                }
            }
            return String.Format("INSERT INTO {0} ({1}) VALUES ({2}); ", tableName, names, values);
        }

        public string GetInsertQueryWithInsertId()
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append("INSERT INTO ");

            queryBuilder.Append(tableName);

            StringBuilder names = new StringBuilder();
            StringBuilder values = new StringBuilder();

            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (i != fieldNames.Count - 1)
                {
                    names.AppendFormat("`{0}`,", fieldNames[i]);
                    values.AppendFormat(" {0} ,", fieldValues[i]);
                }
                else
                {
                    names.AppendFormat("`{0}`", fieldNames[i]);
                    values.AppendFormat(" {0} ", fieldValues[i]);
                }
            }
            return String.Format("INSERT INTO {0} ({1}) VALUES ({2}); SELECT @@IDENTITY;", tableName, names, values);
        }


        private static int GetBit(bool Value)
        {
            if (Value)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
