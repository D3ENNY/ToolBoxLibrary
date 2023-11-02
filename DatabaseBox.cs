using System.Data;
using System.Data.SqlClient;
using ToolBoxLibrary.InternalFunc;

namespace ToolBoxLibrary.DatabaseBox
{
    public class DatabaseBox
    {
        private readonly SqlConnection conn;
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

        public List<Dictionary<string, string>> Query(string query, Dictionary<string, string>? param = null)
        {
            List<Dictionary<string,string>> returnList = new();
            try
            {
                CheckDB();
                if(IsDbValid)
                {
                    using SqlCommand stmt = new(query, this.conn); ;

                    if(param != null)
                    {
                        foreach (KeyValuePair<string,string> item in param)
                        {
                            stmt.Parameters.Add(new()
                            {
                                Value = item.Value,
                                ParameterName = item.Key
                            });
                        }
                    }

                    using SqlDataReader result = stmt.ExecuteReader();

                    if(result.HasRows)
                    {
                        while (result.Read())
                        {
                            Dictionary<string, string> record = new();
                            for (int i = 0; i < result.FieldCount; i++)
                                record.Add(result.GetName(i).ToString() ?? "default key", result[i].ToString() ?? "default value");

                            returnList.Add(record);
                        }
                    }
                }
                this.conn.Close();
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
