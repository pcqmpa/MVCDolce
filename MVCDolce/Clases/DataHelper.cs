﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCDolce.Clases
{
    public class DataHelper
    {
        private readonly SqlConnectionStringBuilder _conexion;
        private string _nameSp;

        public DataHelper(SqlConnectionStringBuilder conexion)
        {
            _conexion = conexion;
        }

        public T EjecutarSp<T>(string spName, List<SqlParameter> parametros)
        {
            //creo la conexion
            SqlConnection strcon = null;
            _nameSp = spName;

            try
            {
                strcon = new SqlConnection(_conexion.ConnectionString);

                var cmd = new SqlCommand(spName, strcon) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 0;
                if (parametros != null)
                {
                    cmd.Parameters.AddRange(parametros.ToArray());
                }

                //valido el tipo de dato a retornar
                if (typeof(T) == typeof(DataSet))
                {
                    var ds = new DataSet();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                    return (T)(Object)ds;
                }
                if (typeof(T) == typeof(DataTable))
                {
                    var ds = new DataTable();
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                    return (T)(Object)ds;
                }
                if (typeof(T) == typeof(SqlDataReader))
                {
                    strcon.Open();
                    var reader = cmd.ExecuteReader();
                    return (T)(Object)reader;
                }
                if (typeof(T) == typeof(int))
                {
                    strcon.Open();
                    var entero = cmd.ExecuteNonQuery();
                    return (T)(Object)entero;
                }
                if (typeof(T) == typeof(bool))
                {
                    strcon.Open();
                    bool retorno = Convert.ToBoolean(cmd.ExecuteNonQuery());
                    return (T)(object)retorno;
                }
                else
                {
                    throw new NotSupportedException(string.Format("El tipo {0} no es soportado", typeof(T).Name));
                }
            }
            finally
            {
                if (strcon != null && strcon.State == ConnectionState.Open)
                {
                    strcon.Close();
                }
            }
        }
    }
}
