using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace JebraAzureFunctions
{
    class Tools
    {
        public Tools() { }

        /// <summary>
        /// Prints to Visual Studio debug console.
        /// </summary>
        /// <param name="s">String to print.</param>
        public static void printDebug(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }
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
