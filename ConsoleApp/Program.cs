using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace ConsoleApp
{
    class Program
    {
        /*
         * OracleParameter 參數名稱不要用 ":"
         * Ex: 
         * "Name"   (o)
         * ":Name"  (x)
         * 
         * OracleParameter return Integer type => OracleDbType.Decimal
         */
        const string connectionString = "Data source=localhost:49161;User id=system;Password=oracle;";

        static void Main(string[] args)
        {
            using (OracleConnection _conn = new OracleConnection(connectionString))
            {
                var _cmd = _conn.CreateCommand();

                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.BindByName = true;

                _conn.Open();

                //Uncomment line to run sample method
                //CallTotalCount(_cmd);
                //QueryProduct(_cmd);
                CallGetProduct(_cmd);

                //CallCreate(_cmd);
                //CallUpdate(_cmd);
                //CallDelete(_cmd);

                _conn.Close();
            }

            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }


        //Call Function PKG_PRODUCT.TOTALCOUNT
        private static void CallTotalCount(OracleCommand cmd)
        {
            cmd.CommandText = "PKG_PRODUCT.TOTALCOUNT";
            cmd.Parameters.Add(":RETURN", OracleDbType.Int32);
            cmd.Parameters[":RETURN"].Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();

            var _returnValue = (cmd.Parameters[":RETURN"].Value);

            Console.WriteLine($"Type: \"{_returnValue.GetType()}\"");
            Console.WriteLine($"Value: \"{_returnValue}\"");
            Console.WriteLine();
        }

        //Call Procedure PKG_PRODUCT.QUERY_PRODUCT
        private static void QueryProduct(OracleCommand cmd)
        {
            cmd.CommandText = "PKG_PRODUCT.QUERY_PRODUCT";
            cmd.Parameters.Add("P_CODE", OracleDbType.Varchar2, 20);
            cmd.Parameters.Add("P_NAME", OracleDbType.NVarchar2, 50);
            cmd.Parameters.Add("O_COUNT", OracleDbType.RefCursor);
            cmd.Parameters.Add("O_RETURN", OracleDbType.RefCursor);

            cmd.Parameters["O_COUNT"].Direction = ParameterDirection.Output;
            cmd.Parameters["O_RETURN"].Direction = ParameterDirection.Output;

            cmd.Parameters["P_CODE"].Value = DBNull.Value;
            cmd.Parameters["P_NAME"].Value = DBNull.Value;

            Console.Write("Total records : ");
            var _reader = cmd.ExecuteReader();
            while (_reader.Read())
            {
                Console.WriteLine(_reader[0]);
            }

            _reader.NextResult();

            Console.WriteLine("Display records : ");
            while (_reader.Read())
            {
                Console.Write(_reader[0] + "\t");
                Console.Write(_reader[1].ToString().PadRight(20));
                Console.WriteLine(_reader[2]);
            }
        }

        //Call Procedure PKG_PRODUCT.GET_PRODUCT
        private static void CallGetProduct(OracleCommand cmd)
        {
            var _identity = 1;

            cmd.CommandText = "PKG_PRODUCT.GET_PRODUCT";

            cmd.Parameters.Add("P_ID", OracleDbType.Int32);
            cmd.Parameters.Add("O_RETURN", OracleDbType.RefCursor);

            cmd.Parameters["O_RETURN"].Direction = ParameterDirection.Output;

            cmd.Parameters["P_ID"].Value = _identity;

            var _reader = cmd.ExecuteReader();

            Console.WriteLine($"Get Id: {_identity}");
            Console.WriteLine("Display records: ");
            while (_reader.Read())
            {
                Console.Write(_reader[0] + "\t");
                Console.Write(_reader[1].ToString().PadRight(20));
                Console.WriteLine(_reader[2]);
            }
        }

        //Call Procedure PKG_PRODUCT.CREATE_PRODUCT
        private static void CallCreate(OracleCommand cmd)
        {
            cmd.CommandText = "PKG_PRODUCT.CREATE_PRODUCT";

            cmd.Parameters.Add("P_CODE", OracleDbType.Varchar2, 20);
            cmd.Parameters.Add("P_NAME", OracleDbType.NVarchar2, 50);
            cmd.Parameters.Add("O_ID", OracleDbType.Int32);

            cmd.Parameters["O_ID"].Direction = ParameterDirection.Output;

            var _guid = Guid.NewGuid().ToString();
            _guid = _guid.Replace("-", "");

            //set input parameters
            cmd.Parameters["P_CODE"].Value = $"CODE-{_guid}";
            cmd.Parameters["P_NAME"].Value = $"NAME-{_guid}";

            var _effectRows = cmd.ExecuteNonQuery();
            var _returnValue = cmd.Parameters["O_ID"].Value;

            Console.WriteLine($"Effect rows {_effectRows}");
            Console.WriteLine($"New Id {_returnValue}");
            Console.WriteLine();
        }

        //Call Procedure PKG_PRODUCT.UPDATE_PRODUCT
        private static void CallUpdate(OracleCommand cmd)
        {
            cmd.CommandText = "PKG_PRODUCT.UPDATE_PRODUCT";

            cmd.Parameters.Add("P_CODE", OracleDbType.Varchar2, 20);
            cmd.Parameters.Add("P_NAME", OracleDbType.NVarchar2, 50);
            cmd.Parameters.Add("P_ID", OracleDbType.Int32);
            cmd.Parameters.Add("O_RESULT", OracleDbType.Char, 1);

            cmd.Parameters["O_RESULT"].Direction = ParameterDirection.Output;

            //set input parameters
            cmd.Parameters["P_CODE"].Value = "P_CODE";
            cmd.Parameters["P_NAME"].Value = "P_NAME";
            cmd.Parameters["P_ID"].Value = 8;

            var _effectRows = cmd.ExecuteNonQuery();
            var _returnValue = cmd.Parameters["O_RESULT"].Value;

            Console.WriteLine($"Effect rows {_effectRows}");
            Console.WriteLine($"Execute result {_returnValue}");
            Console.WriteLine();
        }

        //Call Procedure PKG_PRODUCT.DELETE_PRODUCT
        private static void CallDelete(OracleCommand cmd)
        {
            cmd.CommandText = "PKG_PRODUCT.DELETE_PRODUCT";

            cmd.Parameters.Add("P_ID", OracleDbType.Int32);
            cmd.Parameters.Add("O_RESULT", OracleDbType.Char, 1);

            cmd.Parameters["O_RESULT"].Direction = ParameterDirection.Output;

            //set input parameters
            cmd.Parameters["P_ID"].Value = 10;

            var _effectRows = cmd.ExecuteNonQuery();
            var _returnValue = cmd.Parameters["O_RESULT"].Value;

            Console.WriteLine($"Effect rows {_effectRows}");
            Console.WriteLine($"Execute result {_returnValue}");
            Console.WriteLine();
        }
    }
}
