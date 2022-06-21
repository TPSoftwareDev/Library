using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Xml;

namespace Teleperformance.Data
{
    public abstract class SqlServerDataProvider
    {
        #region Construction/Destruction 
        public int timeout { get; set; }

        public SqlServerDataProvider(string connectionString, int timeout = 45)

        {
            this.ConnectionString = connectionString;
            this.timeout = timeout;
        }

        public SqlServerDataProvider()
            : this(null) { }

        #endregion Construction/Destruction 

        #region Protected Property(ies) 

        /// <summary>
        /// Gets or sets the connection string to use when creating a new <see cref="SqlConnection"/>.
        /// </summary>
        protected string ConnectionString { get; set; }

        #endregion Protected Property(ies) 

        #region Protected Method(s) 

        protected IEnumerable<SqlParameter> BuildParameters(IDictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> pair in parameters)
            {
                yield return new SqlParameter(pair.Key, pair.Value);
            }

            yield break;
        }

        protected SqlCommand BuildStoredProcedureCommand(string storedProcedure, IDictionary<string, object> parameters)
        {
            return this.BuildStoredProcedureCommand(storedProcedure, this.BuildParameters(parameters).ToArray());
        }

        protected SqlCommand BuildStoredProcedureCommand(string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(storedProcedure)
            {
                CommandType = CommandType.StoredProcedure
            };

            foreach (SqlParameter parameter in parameters)
            {
                if (null != parameter)
                {
                    command.Parameters.Add(parameter);
                }
            }
            if (this.timeout != 0)
            {
                command.CommandTimeout = this.timeout;
            }
            return command;
        }

        protected SqlCommand BuildStoredProcedureCommand(SqlTransaction transaction, string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = this.BuildStoredProcedureCommand(storedProcedure, parameters);

            command.Transaction = transaction;

            return command;
        }

        protected SqlCommand BuildStoredProcedureCommand(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = this.BuildStoredProcedureCommand(storedProcedure, parameters);

            command.Connection = connection;

            return command;
        }

        protected SqlCommand BuildStoredProcedureCommand(SqlConnection connection, SqlTransaction transaction, string storedProcedure, params SqlParameter[] parameters)
        {
            SqlCommand command = this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters);

            command.Connection = connection;

            return command;
        }

        protected IEnumerable<T> ExecuteMultipleResultSetCommand<T>(SqlCommand command, CreatorFromMultipleResultSetSqlDataReader<T> creatorFromReader)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return new List<T>(this.ExecuteMultipleResultSetCommand<T>(connection, command, creatorFromReader));
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected IEnumerable<T> ExecuteMultipleResultSetCommand<T>(SqlConnection connection, SqlCommand command, CreatorFromMultipleResultSetSqlDataReader<T> creatorFromReader)
        {
            this.setupCommandConnection(connection, command);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                IEnumerable<T> objects = creatorFromReader(reader);
                IEnumerator<T> objectEnumerator = objects.GetEnumerator();

                while (objectEnumerator.MoveNext())
                {
                    yield return objectEnumerator.Current;
                }

                reader.Close();
            }

            yield break;
        }

        protected IEnumerable<T> ExecuteMultipleResultSetStoredProcedure<T>(string storedProcedure, CreatorFromMultipleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteMultipleResultSetCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected IEnumerable<T> ExecuteMultipleResultSetStoredProcedure<T>(SqlConnection connection, string storedProcedure, CreatorFromMultipleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteMultipleResultSetCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected IEnumerable<T> ExecuteMultipleResultSetStoredProcedure<T>(SqlConnection connection, SqlTransaction transaction, string storedProcedure, CreatorFromMultipleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteMultipleResultSetCommand(connection, this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters), creatorFromReader);
        }

        protected int ExecuteNonQueryCommand(SqlCommand command)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return this.ExecuteNonQueryCommand(connection, command);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected int ExecuteNonQueryCommand(SqlConnection connection, SqlCommand command)
        {
            this.setupCommandConnection(connection, command);
            return command.ExecuteNonQuery();
        }

        protected int ExecuteNonQueryStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteNonQueryCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected int ExecuteNonQueryStoredProcedure(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteNonQueryCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected int ExecuteNonQueryStoredProcedure(SqlConnection connection, SqlTransaction transaction, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteNonQueryCommand(connection, this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters));
        }

        protected SqlDataReader ExecuteReaderStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteReaderCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected SqlDataReader ExecuteReaderStoredProcedure(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteReaderCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected SqlDataReader ExecuteReaderStoredProcedure(SqlConnection connection, SqlTransaction transaction, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteReaderCommand(connection, this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters));
        }

        protected SqlDataReader ExecuteReaderCommand(SqlCommand command)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return this.ExecuteReaderCommand(connection, command);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected SqlDataReader ExecuteReaderCommand(SqlConnection connection, SqlCommand command)
        {
            this.setupCommandConnection(connection, command);
            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes a <see cref="SqlCommand"/> and returns the value of the first column on the first record.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>The value of the first column on the first record.</returns>
        /// <remarks>This will call <see cref="GetConnection"/> to get the connection to execute the command on.</remarks>
        protected object ExecuteScalarCommand(SqlCommand command)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return this.ExecuteScalarCommand(connection, command);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes a <see cref="SqlCommand"/> and returns the value of the first column on the first record.
        /// </summary>
        /// <param name="connection">The connection to execute the command on.</param>
        /// <param name="command">The command to execute.</param>
        /// <returns>The value of the first column on the first record.</returns>
        protected object ExecuteScalarCommand(SqlConnection connection, SqlCommand command)
        {
            this.setupCommandConnection(connection, command);

            return command.ExecuteScalar();
        }

        protected object ExecuteScalarStoredProcedure(string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteScalarCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected object ExecuteScalarStoredProcedure(SqlConnection connection, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteScalarCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters));
        }

        protected object ExecuteScalarStoredProcedure(SqlConnection connection, SqlTransaction transaction, string storedProcedure, params SqlParameter[] parameters)
        {
            return this.ExecuteScalarCommand(connection, this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters));
        }

        /// <summary>
        /// Executes a <see cref="SqlCommand"/> and returns an object created using only the first record.
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <param name="creatorFromReader">The function that will create the object from the <see cref="SqlDataReader"/>. This can also be a lambda expression.</param>
        /// <returns>The object created by the function passed in using only the first record of the commands record set.</returns>
        /// <remarks>This will call <see cref="GetConnection"/> to get the connection to execute the command on.</remarks>
        protected T ExecuteSingleObjectCommand<T>(SqlCommand command, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return this.ExecuteSingleObjectCommand<T>(connection, command, creatorFromReader);
                }
                catch (Exception ex)
                {                    
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Executes a <see cref="SqlCommand"/> and returns an object created using only the first record.
        /// </summary>
        /// <typeparam name="T">The type of object to create</typeparam>
        /// <param name="connection">The connection to execute the command on.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="creatorFromReader">The function that will create the object from the <see cref="SqlDataReader"/>. This can also be a lambda expression.</param>
        /// <returns>The object created by the function passed in using only the first record of the commands record set.</returns>
        protected T ExecuteSingleObjectCommand<T>(SqlConnection connection, SqlCommand command, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader)
        {
            T @object = default(T);
            this.setupCommandConnection(connection, command);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    @object = creatorFromReader(reader);
                }

                reader.Close();
            }

            return @object;
        }

        protected T ExecuteSingleObjectStoredProcedure<T>(string storedProcedure, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteSingleObjectCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected T ExecuteSingleObjectStoredProcedure<T>(SqlConnection connection, string storedProcedure, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteSingleObjectCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected T ExecuteSingleObjectStoredProcedure<T>(SqlConnection connection, SqlTransaction transaction, string storedProcedure, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteSingleObjectCommand(connection, this.BuildStoredProcedureCommand(transaction, storedProcedure, parameters), creatorFromReader);
        }

        protected IEnumerable<T> ExecuteSingleResultSetCommand<T>(SqlCommand command, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return new List<T>(this.ExecuteSingleResultSetCommand<T>(connection, command, creatorFromReader));
                }             
                finally
                {
                    connection.Close();
                }
            }
        }

        protected IEnumerable<T> ExecuteSingleResultSetCommand<T>(SqlConnection connection, SqlCommand command, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader)
        {
            this.setupCommandConnection(connection, command);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    yield return creatorFromReader(reader);
                }

                reader.Close();
            }
        }

        protected IEnumerable<T> ExecuteSingleResultSetStoredProcedure<T>(string storedProcedure, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteSingleResultSetCommand(this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected IEnumerable<T> ExecuteSingleResultSetStoredProcedure<T>(SqlConnection connection, string storedProcedure, CreatorFromSingleResultSetSqlDataReader<T> creatorFromReader, params SqlParameter[] parameters)
        {
            return this.ExecuteSingleResultSetCommand(connection, this.BuildStoredProcedureCommand(storedProcedure, parameters), creatorFromReader);
        }

        protected XmlReader ExecuteXmlReaderCommand(SqlCommand command)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                try
                {
                    connection.Open();

                    return this.ExecuteXmlReaderCommand(connection, command);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        protected XmlReader ExecuteXmlReaderCommand(SqlConnection connection, SqlCommand command)
        {
            this.setupCommandConnection(connection, command);
            return command.ExecuteXmlReader();
        }

        protected virtual SqlConnection GetConnection()
        {
            if (null == this.ConnectionString)
            {
                throw new InvalidOperationException("Attempt to get a connection when the ConnectionString hasn't been set.");
            }

            return new SqlConnection(this.ConnectionString);
        }

        #endregion Protected Method(s) 

        #region Private Method(s) 

        private void setupCommandConnection(SqlConnection connection, SqlCommand command)
        {
            if (command.Connection == null)
            {
                command.Connection = connection;
            }
            else if (command.Connection != connection)
            {
                throw new ArgumentException("Miss-matched connections, the command has a different connection already associated with it.", "connection");
            }
        }

        #endregion Private Method(s) 
    }
}