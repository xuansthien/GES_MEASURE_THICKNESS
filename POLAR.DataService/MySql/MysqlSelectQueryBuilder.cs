using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POLAR.DataService.MySql
{
    public class MysqlSelectQueryBuilder
    {
        private readonly List<string> fieldNames;

        private readonly string tableName;
        private string condition;

        public MysqlSelectQueryBuilder(string tableName)
        {
            this.tableName = tableName;
            fieldNames = new List<string>();
        }

        public void AddField(string fieldName)
        {
            fieldNames.Add(fieldName);
        }

        public void AddCondition(string condition)
        {
            this.condition = condition;
        }

        public void AddCondition(string format, params object[] args)
        {
            this.condition = string.Format(format, args);
        }

        public string GetQuery()
        {
            StringBuilder names = new StringBuilder();

            if (fieldNames.Count == 0)
            {
                names.AppendFormat("`{0}`", "*");
            }
            else
            {
                for (int i = 0; i < fieldNames.Count; i++)
                {
                    if (i != fieldNames.Count - 1)
                    {
                        names.AppendFormat("`{0}`,", fieldNames[i]);
                    }
                    else
                    {
                        names.AppendFormat("`{0}`", fieldNames[i]);
                    }
                }
            }
            if (!string.IsNullOrEmpty(this.condition))
            {
                return String.Format("SELECT {0} FROM {1} WHERE {2}; ", names, tableName, condition);
            }
            return String.Format("SELECT {0} FROM {1}; ", names, tableName);
        }
    }
}
