using System.Data;
using System.Data.SqlClient;
using ToolBoxLibrary.InternalFunc;

namespace ToolBoxLibrary
{
    public class DatabaseBox
    {
        private SqlConnection conn;
        private bool IsDbValid = false;
        public DatabaseBox(string conStr)
        {
            try
            {
                this.conn = new(conStr);
            }
            catch(Exception e)
            {
                ErrorManager.PrintException("ERRORE GENERICO DI CONNESSIONE AL DB\n", e);
            }

        }

        public List<Dictionary<string,string>> Query(string query)
        {
            List<Dictionary<string,string>> returnList = new();
            try
            {
                CheckDB();
                if(IsDbValid)
                {
                    using SqlCommand stmt = new(query, this.conn); ;

                    SqlDataReader result = stmt.ExecuteReader();

                    if(result.HasRows)
                    {
                        DataTable dataTable = result.GetSchemaTable();
                        while (result.Read())
                        {
                            Dictionary<string, string> record = new();               
                            for(int i=0; i<result.FieldCount; i++)
                                record.Add(dataTable.Rows[i]["ColumName"].ToString(), result[i].ToString());

                            returnList.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ErrorManager.PrintException("ERRORE NELL'ESECUZIONE DELLA QUERY\n", e);
            }
            return returnList;
        }

        private void CheckDB()
        {
            this.IsDbValid = false;
            try
            {
                if(this.conn.State.Equals(ConnectionState.Closed))
                    conn.Open();
                this.IsDbValid = true;
            }
            catch (Exception e)
            {
                ErrorManager.PrintException("ERRORE GENERICO DI CONNESSIONE AL DB\n", e);
            }
        }
    }
}
