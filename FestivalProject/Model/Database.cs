﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FestivalProject.Model
{
    class Database
    {
        private static ConnectionStringSettings ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"]; }
        }

        private static DbConnection GetConnection()
        {
            DbConnection con = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateConnection();
            con.ConnectionString = ConnectionString.ConnectionString;
            con.Open();

            return con;
        }

        public static void ReleaseConnection(DbConnection con)
        {
            if (con != null)
            {
                con.Close();
                con = null;
            }
        }

        private static DbCommand BuildCommand(String sql, params DbParameter[] parameters)
        {
            DbCommand command = GetConnection().CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }
            return command;
        }

        public static DbDataReader GetData(String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;

            try
            {
                command = BuildCommand(sql, parameters);
                reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (reader != null)
                {
                    reader.Close();
                }
                if (command != null)
                {
                    ReleaseConnection(command.Connection);
                }
                throw;
            }
        }

        public static int ModifyData(String sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(sql, parameters);
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (command != null)
                {
                    ReleaseConnection(command.Connection);
                }
                return 0;
            }
        }

        public static DbParameter AddParameter(String name, object value)
        {
            DbParameter par = DbProviderFactories.GetFactory(ConnectionString.ProviderName).CreateParameter();
            par.ParameterName = name;
            par.Value = value;
            return par;
        }

        public static DbTransaction BeginTransaction()
        {
            DbConnection con = null;
            try
            {
                con = GetConnection();
                return con.BeginTransaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ReleaseConnection(con);
                throw;
            }
        }
        private static DbCommand BuildCommand(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = trans.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = sql;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            return command;
        }
        public static DbDataReader GetData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            DbDataReader reader = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                reader = command.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public static int ModifyData(DbTransaction trans, string sql, params DbParameter[] parameters)
        {
            DbCommand command = null;
            try
            {
                command = BuildCommand(trans, sql, parameters);
                command.Transaction = trans;
                return command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
