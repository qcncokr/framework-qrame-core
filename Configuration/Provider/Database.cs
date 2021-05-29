using System;

using Qrame.CoreFX;

namespace Qrame.CoreFX.Configuration
{
    /// <summary>
    /// .NET 응용 프로그램 구성 제공자 기본 템플릿 클래스 입니다. 현재 클래스를 이용하거나, 참조하여 새로운 클래스를 개발합니다.
    /// </summary>
    /// <typeparam name="T">데이터베이스에 응용 프로그램 구성값이 지정될 제너릭 타입입니다.</typeparam>
    public class Database<T> : ConfigurationProviderBase<T> where T : ApplicationConfig, new()
    {
        private string connectionString = "";

        private string tableName = "ConfigurationSettings";

        private int key = 1;

        /// <summary>
        /// DBMS ConnectionString를 가져오거나 설정합니다.
        /// </summary>
        public string ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }

        /// <summary>
        /// 구성에 필요한 테이블명을 가져오거나 설정합니다.(기본값 : ConfigurationSettings)
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        /// <summary>
        /// 구성에 필요한 키를 가져오거나 설정합니다.
        /// </summary>
        public int Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 데이터베이스에서 구성에 필요한 항목을 조회합니다.
        /// </summary>
        /// <typeparam name="T">제너릭 타입</typeparam>
        /// <returns></returns>
        public override T Read<T>()
        {
            //            SqlDataAccess data = new SqlDataAccess(ConnectionString);

            //            string sql = "select * from [" + TableName + "] where id=" + Key.ToString();

            //            DbDataReader reader = null;
            //            try
            //            {
            //                DbCommand command = data.CreateCommand(sql);
            //                if (command == null)
            //                {
            //                    SetError(data.ErrorMessage);
            //                    return null;
            //                }
            //                reader = command.ExecuteReader();
            //                if (reader == null)
            //                {
            //                    SetError(data.ErrorMessage);
            //                    return null;
            //                }
            //            }
            //            catch (SqlException exception)
            //            {
            //                if (ex.Number == 208)
            //                {

            //                    sql =
            //@"CREATE TABLE [" + TableName + @"]  
            //( [id] [int] , [ConfigData] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS)";
            //                    try
            //                    {
            //                        data.ExecuteNonQuery(sql);
            //                    }
            //                    catch
            //                    {
            //                        return null;
            //                    }

            //                    // try again if we were able to create the table 
            //                    return Read<T>();
            //                }

            //            }
            //            catch
            //            {
            //                return null;
            //            }

            //            string xmlConfig = null;

            //            if (reader.Read())
            //                xmlConfig = (string)reader["ConfigData"];

            //            reader.Close();
            //            data.CloseConnection();

            //            if (xmlConfig.Length == 0)
            //            {
            //                T newInstance = new T();
            //                newInstance.Provider = this;
            //                return newInstance;
            //            }

            //            T instance = Read<T>(xmlConfig);

            //            return instance;
            return null;
        }

        /// <summary>
        /// 기존에 조회한 구성 타입에서 구성에 필요한 항목을 조회합니다.
        /// </summary> 
        /// <param name="Config"></param>
        /// <returns></returns>
        public override bool Read(ApplicationConfig Config)
        {
            T newConfig = Read<T>();
            if (newConfig == null)
                return false;

            Reflector.CopyTo(newConfig, Config, "Provider, ExceptionMessage");
            return true;
        }

        /// <summary>
        /// 데이터베이스에 구성에 필요한 항목을 입력합니다.
        /// </summary>
        /// <typeparam name="Config">제너릭 타입</typeparam>
        /// <returns></returns>
        public override bool Write(ApplicationConfig Config)
        {
            //            SqlDataAccess data = new SqlDataAccess(ConnectionString);

            //            string sql = "Update [" + TableName + "] set ConfigData=@ConfigData where id=" + Key.ToString();

            //            string xml = WriteSerialize(config);

            //            int Result = 0;
            //            try
            //            {
            //                Result = data.ExecuteNonQuery(sql, data.CreateParameter("@ConfigData", xml));
            //            }
            //            catch
            //            {
            //                sql =
            //    @"CREATE TABLE [" + TableName + @"]  
            //( [id] [int] , [ConfigData] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS)";
            //                try
            //                {
            //                    data.ExecuteNonQuery(sql);
            //                }
            //                catch (Exception exception)
            //                {
            //                    SetError(ex);
            //                    return false;
            //                }
            //            }

            //            // Check for missing record
            //            if (Result == 0)
            //            {
            //                sql = "Insert [" + TableName + "] (id,configdata) values (" + Key.ToString() + ",@ConfigData)";

            //                try
            //                {
            //                    Result = data.ExecuteNonQuery(sql, data.CreateParameter("@ConfigData", xml));
            //                }
            //                catch (Exception exception)
            //                {
            //                    SetError(ex);
            //                    return false;
            //                }
            //                if (Result == 0)
            //                {
            //                    return false;
            //                }

            //            }

            return true;
        }
    }
}
