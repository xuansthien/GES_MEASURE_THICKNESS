using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DataService.MySql
{
    public class MysqlUpdateQueryBuilder
    {
        private readonly List<string> fieldNames;
        private readonly List<string> fieldValues;

        private readonly string tableName;
        private string condition;

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

        public void AddCondition(string condition)
        {
            this.condition = condition;
        }

        public void AddCondition(string format, params object[] args)
        {
            this.condition = string.Format(format, args);
        }

        public MysqlUpdateQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            fieldNames = new List<string>();
            fieldValues = new List<string>();
        }

        public string GetUpdateQuery()
        {
            StringBuilder setvalues = new StringBuilder();

            for (int i = 0; i < fieldNames.Count; i++)
            {
                if (i != fieldNames.Count - 1)
                {
                    setvalues.AppendFormat("`{0}` = {1} ,", fieldNames[i], fieldValues[i]);
                }
                else
                {
                    setvalues.AppendFormat("`{0}` = {1}", fieldNames[i], fieldValues[i]);
                }
            }

            return String.Format("UPDATE {0} SET {1} WHERE {2}; ", tableName, setvalues, condition);
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
