using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace UtilityComponent.DataFactory
{
    public class SqlDataAccess : IDataAccess
    {

        public SqlConnection Connection { get; set; }

        private SqlTransaction transaction;

        string ConnectionString { get; set; }

        public SqlDataAccess(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            ConnectionString = connectionString;
        }
        #region IDataAccess member

        

        public void Open()
        {
            if (this.Connection.State.Equals(ConnectionState.Closed))
            {
                this.Connection.Open();
            }
        }

        public void Close()
        {
            if (this.Connection != null && this.Connection.State.Equals(ConnectionState.Open))
            {
                this.Connection.Close();
            }
        }

        public void BeginTran()
        {
            transaction = this.Connection.BeginTransaction();
        }

        public void CommitTran()
        {
            this.transaction.Commit();
        }

        public void RollbackTran()
        {
            this.transaction.Rollback();
        }

        public int ExecuteNonQuery(string sql, params QueryParameter[] list)
        {
            var command = new SqlCommand(sql, Connection);
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            PrepareCommand(command, CommandType.Text, sql, list);
            var i = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return i;
        }

        public object ExecuteScalar(string sql, params QueryParameter[] list)
        {
            var command = new SqlCommand();
            PrepareCommand(command, CommandType.Text, sql, list);
            var obj = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj;
        }

        public DataTable GetTable(string sql, params QueryParameter[] list)
        {
            var table = new DataTable();
            var command = new SqlCommand();
            PrepareCommand(command, CommandType.Text, sql, list);
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            command.Parameters.Clear();
            return table;
        }

        public DataSet GetSet(string sql, string tableName)
        {
            var myda = new SqlDataAdapter(sql, this.Connection);
            var result = new DataSet();
            myda.Fill(result, tableName);
            return result;
        }

        public IDataReader GetReader(string sql, params QueryParameter[] list)
        {
            var command = new SqlCommand();
            PrepareCommand(command, CommandType.Text, sql, list);
            var reader = command.ExecuteReader();
            command.Parameters.Clear();
            return reader;
        }

        public DataTable ExecuteSql(string sql)
        {
            var myda = new SqlDataAdapter(sql, this.Connection);
            var table = new DataTable();
            myda.Fill(table);
            return table;
        }
        #endregion

        private void PrepareCommand(SqlCommand command, CommandType type, string commandText, params QueryParameter[] list)
        {
            command.CommandText = commandText;
            command.CommandType = type;
            command.Connection = this.Connection;
            if (list != null && list.Length > 0)
            {
                for (var i = 0; i < list.Length; i++)
                {
                    command.Parameters.AddWithValue(list[i].Name, list[i].Value);
                }
            }
        }



        #region Updated method members for repositpry

        private void PrepareCommand_new(SqlCommand command, CommandType type, string commandText, params QueryParameter[] list)
        {
            command.CommandText = commandText;
            command.CommandType = type;
            if (list != null && list.Length > 0)
            {
                for (var i = 0; i < list.Length; i++)
                {
                    command.Parameters.AddWithValue(list[i].Name, list[i].Value);
                }
            }
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public int ExecuteNonQuery(SqlCommand command, string sql, params QueryParameter[] list)
        {
            PrepareCommand_new(command, CommandType.Text, sql, list);
            var i = command.ExecuteNonQuery();
            command.Parameters.Clear();
            return i;
        }

        public object ExecuteScalar(SqlCommand command, string sql, params QueryParameter[] list)
        {
            PrepareCommand_new(command, CommandType.Text, sql, list);
            var obj = command.ExecuteScalar();
            command.Parameters.Clear();
            return obj;
        }

        public DataTable GetTable(SqlCommand command, string sql, params QueryParameter[] list)
        {
            PrepareCommand_new(command, CommandType.Text, sql, list);
            var table = new DataTable();
            var adapter = new SqlDataAdapter(command);
            adapter.Fill(table);
            command.Parameters.Clear();
            return table;
        }
        
        #endregion
    }
}

