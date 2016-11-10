using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace MvcDWZ.Base
{
    public class PagerData
    {
        private PagerData()
        {
            Where = string.Empty;
            SearchWhere = string.Empty;
            Collection = new SqlCommand().Parameters;
        }

        public static PagerData GetInstance()
        {
            return new PagerData();
        }

        #region 属性
        [Description("当前页数")]
        private int _CurrentPageIndex;
        public int CurrentPageIndex
        {
            get
            {
                return this._CurrentPageIndex;
            }
            set
            {
                this._CurrentPageIndex = value;
            }
        }

        [Description("总页数")]
        private int _PageCount;
        public int PageCount
        {
            get
            {
                return this._PageCount;
            }
            set
            {
                this._PageCount = value;
            }
        }

        [Description("每页显示数")]
        private int _PageSize;
        public int PageSize
        {
            get
            {
                return this._PageSize;
            }
            set
            {
                this._PageSize = value;
            }
        }

        [Description("总记录数")]
        private int _RecordCount;
        public int RecordCount
        {
            get
            {
                return this._RecordCount;
            }
            set
            {
                this._RecordCount = value;
            }
        }

        [Description("表名")]
        private string _Table;
        public string Table
        {
            get
            {
                return this._Table;
            }
            set
            {
                this._Table = value;
            }
        }

        [Description("显示字段,用','分割")]
        private string _Field;
        public string Field
        {
            get
            {
                return this._Field;
            }
            set
            {
                this._Field = value;
            }
        }

        [Description("默认的查询条件")]
        private string _Where;
        public string Where
        {
            get
            {
                return this._Where;
            }
            set
            {
                this._Where = value;
            }
        }
        [Description("SqlParameterCollection参数集合")]
        public SqlParameterCollection Collection
        {
            get;
            set;
        }

        [Description("变化的查询条件")]
        private string _SearchWhere;
        public string SearchWhere
        {
            get
            {
                return this._SearchWhere;
            }
            set
            {
                this._SearchWhere = value;
            }
        }

        [Description("排序方式")]
        private string _Order;
        public string Order
        {
            get
            {
                return this._Order;
            }
            set
            {
                this._Order = value;
            }
        }

        [Description("分组方式")]
        private string _Group;
        public string Group
        {
            get
            {
                return this._Group;
            }
            set
            {
                this._Group = value;
            }
        }

        [Description("数据库链接")]
        private string _Conn;
        public string Conn
        {
            get
            {
                return this._Conn;
            }
            set
            {
                this._Conn = value;
            }
        }

        #endregion

        #region 公共方法

        [Description("返回下一页的数据")]
        public DataSet NextPage()
        {
            _CurrentPageIndex = this.CurrentPageIndex;
            if (_CurrentPageIndex < _PageCount)
            {
                _CurrentPageIndex++;
            }
            else
            {
                _CurrentPageIndex = 1;
            }
            return GetPage(_CurrentPageIndex);
        }

        [Description("返回上一页的数据")]
        public DataSet PrevPage()
        {
            _CurrentPageIndex = this.CurrentPageIndex;
            if (_CurrentPageIndex > 1)
            {
                _CurrentPageIndex--;
            }
            else
            {
                _CurrentPageIndex = _PageCount;
            }
            return GetPage(_CurrentPageIndex);
        }

        [Description("返回第一页的数据")]
        public DataSet FirstPage()
        {
            return GetPage(1);
        }

        [Description("返回最后一页的数据")]
        public DataSet LastPage()
        {
            return GetPage(this.PageCount);
        }

        [Description("返回指定页的数据")]
        public DataSet GetPage(int PageNo)
        {
            string sWhere = "";
            try
            {
                //SqlConnection SqlConn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]);


                if (this.Where.Trim() == "")
                {
                    if (this.SearchWhere.Trim() != "")
                    {
                        sWhere = "WHERE " + this.SearchWhere;
                    }
                }
                else
                {
                    if (this.SearchWhere.Trim() != "")
                    {
                        sWhere = this.Where + " AND (" + this.SearchWhere + ")";
                    }
                    else
                    {
                        sWhere = this.Where;
                    }
                }
                string sqlText = "select * from (select " + Field + ",ROW_NUMBER()Over(" + Order + ")as RowNumber from " + Table + "  " + sWhere +
                    Group +
                    ")as RowNumberTableSource where RowNumber between @pageStart and @pageEnd";
                string sqlCount = "select count(RowNumber)as [RowCount] from (select 1 as RowNumber from " + Table + "  " + sWhere +
                       Group +
                    ")as RowNumberTableSource ";
                string sql = sqlText + ";" + sqlCount;
                int pageStart = (PageNo - 1) * PageSize + 1;
                int pageEnd = PageNo * PageSize;
                using (SqlConnection conn = new SqlConnection(Conn))
                {
                    if (conn.State != ConnectionState.Open)
                        conn.Open();
                    if (Collection == null)
                        Collection = new SqlCommand().Parameters;
                    Collection.Add(new SqlParameter("@pageStart", pageStart));
                    Collection.Add(new SqlParameter("@pageEnd", pageEnd));
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 120;
                    if (Collection != null && Collection.Count > 0)
                    {
                        SqlParameter[] temp = new SqlParameter[Collection.Count];
                        for (int i = 0; i < Collection.Count; i++)
                        {
                            temp[i] = (SqlParameter)((ICloneable)Collection[i]).Clone();
                        }
                        cmd.Parameters.AddRange(temp);
                    }

                    SqlDataAdapter sqlDA = new SqlDataAdapter();
                    sqlDA.SelectCommand = cmd;
                    DataSet dataSet = new DataSet();
                    sqlDA.Fill(dataSet, "Anonymous");
                    return dataSet;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        #endregion
    }
}