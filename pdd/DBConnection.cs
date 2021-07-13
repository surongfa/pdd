using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Threading;

namespace WechatRegster
{
    public class DBConnection
    {
        private const int MaxPool = 100;//最大连接数
        private const int MinPool = 20;//最小连接数
        private const bool Asyn_Process = true;//设置异步访问数据库
        private const bool Mars = true;
        private const int Conn_Timeout = 15;//设置连接等待时间
        private const int Conn_Lifetime = 15;//设置连接的生命周期
        private string ConnString = "";//连接字符串
        //private MySqlConnection connection = null;//连接对象

        public DBConnection(string ConnString)
        {
            this.ConnString = ConnString;//连接字符串
        }

        public bool testconnection(out string result){
            result = null;
            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConnString))
                {
                    connection.Open();
                    return true;
                }
            }catch(Exception e)
            {
                result = e.Message;
            }
            return false;
        }
        public DataTable getData(string sql, params object[] obj)//数据查询
        {
            MySqlConnection connection = new MySqlConnection(ConnString);
            //当连接处于打开状态时关闭,然后再打开,避免有时候数据不能及时更新
            try
            {
                MySqlParameter[] parameters = parseParams(sql, obj);
                //当连接处于打开状态时关闭,然后再打开,避免有时候数据不能及时更新
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }

                    }
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader != null && reader.HasRows)
                        {
                            DataTable dt = new DataTable();
                            //读取SqlDataReader里的内容
                            dt.Load(reader);
                            //关闭对象和连接
                            return dt;
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                connection.Close();
            }
        }

        // 解析sql的通配注入
        public static MySqlParameter[] parseParams(string sql , params object[] obj)
        {
            MySqlParameter[] parameters = null;
            if (!string.IsNullOrEmpty(sql) && obj != null && sql.Contains("@"))
            {
                sql = sql.Trim();
                if (sql.EndsWith(";") || sql.EndsWith(")"))
                {
                    sql = sql.Substring(0, sql.Length - 1);
                    sql = sql.Trim();
                }
                parameters = new MySqlParameter[obj.Length];
                int j = 0;
                string sqlPart = null;
                do
                {
                    int index = sql.IndexOf("@");
                    int lastIndex = sql.IndexOf(" ", index);
                    int tempIndex = sql.IndexOf("\"", index);
                    if ((lastIndex > tempIndex || lastIndex < 0) && tempIndex!=-1)
                    {
                        lastIndex = tempIndex;
                    }
                    tempIndex = sql.IndexOf(",", index);
                    if ((lastIndex > tempIndex || lastIndex < 0) && tempIndex != -1)
                    {
                        lastIndex = tempIndex;
                    }
                    lastIndex = lastIndex > 0 ? lastIndex : sql.Length;
                    sqlPart = sql.Substring(index, lastIndex - index);
                    if (!string.IsNullOrEmpty(sqlPart) && sqlPart.Contains("@"))
                    {
                        parameters[j] = new MySqlParameter(sqlPart.Trim(), obj[j]);
                        j++;
                    }
                    sql = sql.Substring(lastIndex, sql.Length- lastIndex);
                } while (!string.IsNullOrEmpty(sql) && sql.Contains("@"));
            }
            return parameters;
        }

        public int executeUpdate(string sql, params object[] obj)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnString))
            {
                MySqlParameter[] parameters = parseParams(sql, obj);
                //当连接处于打开状态时关闭,然后再打开,避免有时候数据不能及时更新
                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                command.Parameters.Add(parameter);
                            }
                        }
                        return command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    //Service.put("更新数据库数据出现异常:" + ex.StackTrace+ex.Message+ sql, true);
                    throw ex;
                }
            }
        }

        public int executeUpdate(string sql)
        {
            MySqlConnection connection = new MySqlConnection(ConnString);
            //当连接处于打开状态时关闭,然后再打开,避免有时候数据不能及时更新
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    return command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //Service.put("更新数据库数据出现异常:" + ex.StackTrace+ex.Message+ sql, true);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void executeUpdateAsync(string sql, params object[] obj)
        {
            MySqlConnection connection = new MySqlConnection(ConnString);
            MySqlParameter[] parameters = parseParams(sql, obj);
            //当连接处于打开状态时关闭,然后再打开,避免有时候数据不能及时更新
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.Add(parameter);
                        }

                    }
                    command.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                //Service.put("更新数据库数据出现异常:" + ex.StackTrace+ex.Message+ sql, true);
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
