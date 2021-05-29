using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Qrame.CoreFX.ExtensionMethod
{
  public static class DbConnectionExtensions
  {
    public static bool IsConnectionOpen(this DbConnection @this)
    {
      return @this.State == ConnectionState.Open;
    }

    public static bool IsConnectionOpen(this IDbConnection @this)
    {
      return @this.State == ConnectionState.Open;
    }

    public static void EnsureOpen(this DbConnection @this)
    {
      if (@this.State == ConnectionState.Closed)
      {
        @this.Open();
      }
    }

    public static void EnsureOpen(this IDbConnection @this)
    {
      if (@this.State == ConnectionState.Closed)
      {
        @this.Open();
      }
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction) where T : new()
    {
      using (DbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToEntities<T>();
        }
      }
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, Action<DbCommand> commandFactory) where T : new()
    {
      using (DbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToEntities<T>();
        }
      }
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, CommandType.Text, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, CommandType.Text, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, commandType, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, CommandType commandType, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, commandType, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, DbParameter[] parameters) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, CommandType.Text, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, CommandType.Text, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, commandType, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType, IDbTransaction transaction) where T : new()
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          foreach (var item in parameters)
          {
            command.Parameters.Add(item);
          }
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToEntities<T>();
        }
      }
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, Action<IDbCommand> commandFactory) where T : new()
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToEntities<T>();
        }
      }
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, CommandType.Text, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, CommandType.Text, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, commandType, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, CommandType commandType, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, null, commandType, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, CommandType.Text, null);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, CommandType.Text, transaction);
    }

    public static IEnumerable<T> ExecuteEntities<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntities<T>(cmdText, parameters, commandType, null);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction) where T : new()
    {
      using (DbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToEntity<T>();
        }
      }
    }

    public static T ExecuteEntity<T>(this DbConnection @this, Action<DbCommand> commandFactory) where T : new()
    {
      using (DbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToEntity<T>();
        }
      }
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, CommandType.Text, null);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, CommandType.Text, transaction);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, commandType, null);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, CommandType commandType, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, commandType, transaction);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, DbParameter[] parameters) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, CommandType.Text, null);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, DbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, CommandType.Text, transaction);
    }

    public static T ExecuteEntity<T>(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, commandType, null);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType, IDbTransaction transaction) where T : new()
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          foreach (var item in parameters)
          {
            command.Parameters.Add(item);
          }
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToEntity<T>();
        }
      }
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, Action<IDbCommand> commandFactory) where T : new()
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToEntity<T>();
        }
      }
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, CommandType.Text, null);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, CommandType.Text, transaction);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, commandType, null);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, CommandType commandType, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, null, commandType, transaction);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, CommandType.Text, null);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, IDbTransaction transaction) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, CommandType.Text, transaction);
    }

    public static T ExecuteEntity<T>(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType) where T : new()
    {
      return @this.ExecuteEntity<T>(cmdText, parameters, commandType, null);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction)
    {
      using (DbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToExpandoObject();
        }
      }
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, Action<DbCommand> commandFactory)
    {
      using (DbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToExpandoObject();
        }
      }
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText)
    {
      return @this.ExecuteExpandoObject(cmdText, null, CommandType.Text, null);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, null, CommandType.Text, transaction);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, CommandType commandType)
    {
      return @this.ExecuteExpandoObject(cmdText, null, commandType, null);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, CommandType commandType, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, null, commandType, transaction);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, DbParameter[] parameters)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, CommandType.Text, null);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, DbParameter[] parameters, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, CommandType.Text, transaction);
    }

    public static dynamic ExecuteExpandoObject(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, commandType, null);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType, IDbTransaction transaction)
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          foreach (var item in parameters)
          {
            command.Parameters.Add(item);
          }
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToExpandoObject();
        }
      }
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, Action<IDbCommand> commandFactory)
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          reader.Read();
          return reader.ToExpandoObject();
        }
      }
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText)
    {
      return @this.ExecuteExpandoObject(cmdText, null, CommandType.Text, null);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, null, CommandType.Text, transaction);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, CommandType commandType)
    {
      return @this.ExecuteExpandoObject(cmdText, null, commandType, null);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, CommandType commandType, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, null, commandType, transaction);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, IDataParameter[] parameters)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, CommandType.Text, null);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, IDataParameter[] parameters, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, CommandType.Text, transaction);
    }

    public static dynamic ExecuteExpandoObject(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType)
    {
      return @this.ExecuteExpandoObject(cmdText, parameters, commandType, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType, DbTransaction transaction)
    {
      using (DbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          command.Parameters.AddRange(parameters);
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToExpandoObjects();
        }
      }
    }

   public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, Action<DbCommand> commandFactory)
    {
      using (DbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToExpandoObjects();
        }
      }
    }

   public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, transaction);
    }

  public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, CommandType commandType)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, commandType, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, CommandType commandType, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, commandType, transaction);
    }

  public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, DbParameter[] parameters)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, DbParameter[] parameters, DbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, transaction);
    }

   public static IEnumerable<dynamic> ExecuteExpandoObjects(this DbConnection @this, string cmdText, DbParameter[] parameters, CommandType commandType)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, commandType, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType, IDbTransaction transaction)
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        command.CommandText = cmdText;
        command.CommandType = commandType;
        command.Transaction = transaction;

        if (parameters != null)
        {
          foreach (var item in parameters)
          {
            command.Parameters.Add(item);
          }
        }

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToExpandoObjects();
        }
      }
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, Action<IDbCommand> commandFactory)
    {
      using (IDbCommand command = @this.CreateCommand())
      {
        commandFactory(command);

        using (IDataReader reader = command.ExecuteReader())
        {
          return reader.ToExpandoObjects();
        }
      }
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, CommandType.Text, transaction);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, CommandType commandType)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, commandType, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, CommandType commandType, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, null, commandType, transaction);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, IDataParameter[] parameters)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, null);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, IDataParameter[] parameters, IDbTransaction transaction)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, CommandType.Text, transaction);
    }

    public static IEnumerable<dynamic> ExecuteExpandoObjects(this IDbConnection @this, string cmdText, IDataParameter[] parameters, CommandType commandType)
    {
      return @this.ExecuteExpandoObjects(cmdText, parameters, commandType, null);
    }
  }
}
