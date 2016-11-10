using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MvcDWZ.Base
{
    public abstract class SqlHelper
    {
        //private static readonly ILog _log = LogManager.GetLogger(typeof(SqlHelper));
        public static readonly string ConnectionStringLocal = ConfigurationManager.ConnectionStrings["Conn_DWZ"].ToString();


        /// <summary>
        /// 修改默认 SqlCommand Timeout 的值，默认为30s，目前设置为120s
        /// </summary>
        public static readonly int Default_Timeout_Value = 120;
        [Obsolete("强烈建议使用其他指明数据库连接串的重载方法")]
        public static object ExecuteScalar(string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;
            return ExecuteScalar(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string connectstring, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;
            return ExecuteScalar(connectstring, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string cmdText, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;

            if (paras != null && paras.Length > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteScalar(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string connectstring, string cmdText, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;

            if (paras != null && paras.Length > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteScalar(connectstring, CommandType.Text, cmd);
        }
        public static object ExecuteScalar(string connectstring, string cmdText, SqlParameterCollection paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;

            if (paras != null && paras.Count > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Count];
                for (int i = 0; i < paras.Count; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteScalar(connectstring, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string connectstring, SqlCommand cmd)
        {
            return ExecuteScalar(connectstring, CommandType.Text, cmd);
        }

        public static object ExecuteScalar(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = Default_Timeout_Value;

                return cmd.ExecuteScalar();
            }
        }
        [Obsolete("强烈建议使用其他指明数据库连接串的重载方法")]
        public static int ExecuteNonQuery(string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;
            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string connectString, string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;
            return ExecuteNonQuery(connectString, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string cmdText, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;

            if (paras != null && paras.Length > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, string cmdText, SqlParameter[] paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;

            if (paras != null && paras.Length > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Length];
                for (int i = 0; i < paras.Length; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteNonQuery(connectionString, cmd);
        }
        public static int ExecuteNonQuery(string connectionString, string cmdText, SqlParameterCollection paras)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = Default_Timeout_Value;
            if (paras != null && paras.Count > 0)
            {
                SqlParameter[] temp = new SqlParameter[paras.Count];
                for (int i = 0; i < paras.Count; i++)
                {
                    temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                }
                cmd.Parameters.AddRange(temp);
            }

            return ExecuteNonQuery(connectionString, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, SqlCommand cmd)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, SqlCommand cmd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = Default_Timeout_Value;

                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return rowsAffected;
            }
        }

        public static int ExecuteNonQuery(SqlCommand cmd, out int sysno)
        {
            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, CommandType.Text, cmd, out sysno);
        }

        public static int ExecuteNonQuery(string connectionString, SqlCommand cmd, out int sysno)
        {
            return ExecuteNonQuery(connectionString, CommandType.Text, cmd, out sysno);
        }

        public static int ExecuteNonQuery(SqlCommand cmd, CommandType cmdType)
        {
            return ExecuteNonQuery(SqlHelper.ConnectionStringLocal, cmdType, cmd);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, SqlCommand cmd, out int sysno)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = cmdType;
                cmd.CommandTimeout = Default_Timeout_Value;

                int rowsAffected = cmd.ExecuteNonQuery();

                //必须符合下面的条件
                if (cmd.Parameters.Contains("@SysNo") && cmd.Parameters["@SysNo"].Direction == ParameterDirection.Output)
                    sysno = (int)cmd.Parameters["@SysNo"].Value;
                else if (cmd.Parameters.Contains("@TransactionNumber") && cmd.Parameters["@TransactionNumber"].Direction == ParameterDirection.Output)
                    sysno = (int)cmd.Parameters["@TransactionNumber"].Value;
                else
                    throw new Exception("SqlHelper: Does not contain SysNo or ParameterDirection is Not Output");

                cmd.Parameters.Clear();
                return rowsAffected;
            }
        }
        [Obsolete("强烈建议使用其他指明数据库连接串的重载方法")]
        public static DataSet ExecuteDataSet(string cmdText)
        {
            SqlParameter[] sp1 = { };
            return ExecuteDataSet(SqlHelper.ConnectionStringLocal, cmdText, sp1);
        }

        public static DataSet ExecuteDataSet(string connectionString, string cmdText)
        {
            SqlParameter[] sp1 = { };
            return ExecuteDataSet(connectionString, cmdText, sp1);
        }

        public static DataSet ExecuteDataSet(string cmdText, SqlParameter[] paras)
        {
            return ExecuteDataSet(SqlHelper.ConnectionStringLocal, cmdText, paras);
        }

        public static DataSet ExecuteDataSet(string connectionString, string cmdText, SqlParameter[] paras)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = Default_Timeout_Value;



                if (paras != null && paras.Length > 0)
                {
                    SqlParameter[] temp = new SqlParameter[paras.Length];
                    for (int i = 0; i < paras.Length; i++)
                    {
                        temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                    }
                    cmd.Parameters.AddRange(temp);
                }

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet("mySet");
                sqlDA.Fill(dataSet, "Anonymous");

                return dataSet;
            }
        }
        public static DataSet ExecuteDataSet(string connectionString, string cmdText, SqlParameterCollection paras)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = Default_Timeout_Value;



                if (paras != null && paras.Count > 0)
                {
                    SqlParameter[] temp = new SqlParameter[paras.Count];
                    for (int i = 0; i < paras.Count; i++)
                    {
                        temp[i] = (SqlParameter)((ICloneable)paras[i]).Clone();
                    }
                    cmd.Parameters.AddRange(temp);
                }

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet("mySet");
                sqlDA.Fill(dataSet, "Anonymous");

                return dataSet;
            }
        }
        [Obsolete("强烈建议使用其他指明数据库连接串的重载方法")]
        public static DataSet ExecuteDataSet(SqlCommand cmd)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandTimeout = Default_Timeout_Value;

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet("mySet");
                sqlDA.Fill(dataSet, "Anonymous");

                return dataSet;
            }
        }

        public static DataSet ExecuteDataSet(string ConnectString, SqlCommand cmd)
        {
            using (SqlConnection conn = new SqlConnection(ConnectString))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandTimeout = Default_Timeout_Value;

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet("mySet");
                sqlDA.Fill(dataSet, "Anonymous");

                return dataSet;
            }
        }

        public static DataSet ExecuteDataSet(string cmdText, string type, SqlParameter[] paras, ref string output)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.ConnectionStringLocal))
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (type == "StoredProcedure")
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandType = CommandType.Text;
                }
                cmd.CommandTimeout = Default_Timeout_Value;

                if (paras != null && paras.Length > 0)
                    cmd.Parameters.AddRange(paras);

                SqlDataAdapter sqlDA = new SqlDataAdapter();
                sqlDA.SelectCommand = cmd;
                DataSet dataSet = new DataSet("mySet");
                sqlDA.Fill(dataSet, "Anonymous");
                output = dataSet.Tables[1].Rows[0][0].ToString();
                return dataSet;
            }
        }
        //public static void InsertTableUpdateLog(SqlParameterCollection sp,string table,string connection,int SysNo,string ColumnName)
        //{
        //    try
        //    {
        //        if (AppConfig.AllowInsertTableUpdateLog != "1")
        //            return;
        //    }
        //    catch { return; }
        //    try
        //    {
        //        string sql = "select ";
        //        for (int i = 0; i < sp.Count;i++ )
        //        {
        //            SqlParameter item = sp[i];
        //            if (i == 0)
        //            {
        //                if (item.Size < 200)
        //                {
        //                    sql += item.ParameterName.Replace("@", "") ;//Note nvarchar(2000)个字符,KeySysNo用Varchar(20)
        //                }
        //            }
        //            else
        //            {
        //                if (item.Size < 200)
        //                {
        //                    sql += "," + item.ParameterName.Replace("@", "");//Note nvarchar(2000)个字符,KeySysNo用Varchar(20)
        //                }
        //            }
        //        }
        //        sql += " from " + table + " where " + ColumnName + "=@SysNo";
        //        SqlParameter[] sp1 = {new SqlParameter("@SysNo",SysNo) };
        //        DataTable dt=SqlHelper.ExecuteDataSet(connection, sql, sp1).Tables[0];
        //        string note = "";
        //        foreach (DataColumn dc in dt.Columns)
        //        {
        //            note += dc.ColumnName + ":" + dt.Rows[0][dc].ToString() + ";";                  
        //        }


        //        string sql1 = "insert into TableUpdateLog(KeySysNo,CreateTime,UpdateTable,Note)values(@KeySysNo,GetDate(),@UpdateTable,@Note)";
        //        SqlParameter[] sp2 = { new SqlParameter("@KeySysNo",SysNo),new SqlParameter("@UpdateTable",table),new SqlParameter("@Note",note)};
        //        SqlHelper.ExecuteNonQuery(connection, sql1, sp2);
        //    }
        //    catch(Exception e) {
        //        SendErrorEmail("表：" + table +"主键值："+SysNo+"，错误信息："+ e.Message);
        //        //"发邮件";
        //    }
        //}
        //public static void SendErrorEmail(string msg)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(Path.GetFullPath(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "App_Data/EmailSMSConfig/Mis.xml")));
        //    XmlNode node = null;
        //    string address = xmlDoc.SelectSingleNode("//Mis//MisServiceError//Organ//Email//Address").InnerText;
        //    #region 邮件
        //    string emailStr = address;
        //    string[] emailArr = emailStr.Split(',');
        //    for (int i = 0; i < emailArr.Length; i++)
        //    {
        //        Email_InternalEntity emailModel = new Email_InternalEntity();
        //        emailModel.AlreadyRepeatCount = 0;
        //        emailModel.CreateTime = DateTime.Now;
        //        emailModel.MaxRepeat = 3;
        //        emailModel.OrganSysNo = AppConfig.DefaultOrgan;
        //        emailModel.MailAddress = emailArr[i];
        //        emailModel.MailBody = msg;
        //        emailModel.MailSubject = "DAC层记录log失败";
        //        emailModel.Priority = (int)AppEnum.EmailPriority.High;
        //        emailModel.Status = (int)AppEnum.EmailStatus.WaitSend;
        //        new Email_InternalDac().Add(emailModel);
        //    }
        //    #endregion

        //}

    }
}