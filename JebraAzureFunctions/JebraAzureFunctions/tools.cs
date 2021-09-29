using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace JebraAzureFunctions
{
    class tools
    {
        public tools() { }

        public static String sqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }
    }
}
