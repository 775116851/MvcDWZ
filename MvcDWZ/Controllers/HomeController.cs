using MvcDWZ.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcDWZ.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        //用户列表
        public ActionResult UserList()
        {
            //用户类型
            DataTable dtUserType = AppEnum.GetEnumKeyName(typeof(AppEnum.UserType));
            ViewData["dtUserType"] = dtUserType;

            int pageIndex = 1;
            int pageCount = 20;
            int totalCount = 0;
            if (Request.Form["pageNum"] != null)
            {
                pageIndex = Convert.ToInt32(Request.Form["pageNum"]);
            }
            if (Request.Form["numPerPage"] != null)
            {
                pageCount = Convert.ToInt32(Request.Form["numPerPage"]);
            }
            //用户列表
            Hashtable ht = new Hashtable();
            string sUserID = Request.Form["sUserID"];
            string sUserType = Request.Form["sUserType"];
            string sDesc = Request.Form["orderDirection"];
            if(!string.IsNullOrEmpty(sUserID))
            {
                ht.Add("UserID", sUserID);
                ViewData["uUserID"] = sUserID;//
            }
            if (!string.IsNullOrEmpty(sUserType) && sUserType != "-9999")
            {
                ht.Add("UserType", sUserType);
                ViewData["uUserType"] = sUserType;//
            }
            if (string.IsNullOrEmpty(sDesc))
            {
                sDesc = "DESC";
            }
            ht.Add("Sort", sDesc);
            DataSet dsUserList = GetUserDs(pageIndex, pageCount, ht);
            totalCount = Convert.ToInt32(dsUserList.Tables[1].Rows[0][0]);
            ViewData["dsUserList"] = dsUserList;

            ViewData["pageIndex"] = pageIndex;
            ViewData["pageCount"] = pageCount;
            ViewData["totalCount"] = totalCount;
            return View();
        }

        //删除及批量删除
        public ActionResult UserDelte(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string[] SysNoList = ids.Split(',');
                string result = string.Empty;
                int errorCount = 0;
                StringBuilder errorMessage = new StringBuilder();
                foreach (string SysNo in SysNoList)
                {
                    result = DeleteUser(SysNo);
                    if (!string.IsNullOrEmpty(result))//失败
                    {
                        errorMessage.Append(result).Append(";");
                    }
                }
                if(errorCount > 0)//存在删除失败
                {
                    return Json(new
                    {
                        statusCode = "300",
                        message = string.Format("共{0}条删除成功，共{1}条删除失败,失败详情:{2}",(SysNoList.Length - errorCount),errorCount,errorMessage),
                        navTabId = "UserManager",
                        callbackType = "",
                        forwardUrl = ""
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    statusCode = "200",
                    message = "删除成功",
                    navTabId = "UserManager",
                    callbackType = "",
                    forwardUrl = ""
                },JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new 
                {
                    statusCode = "300",
                    message = "删除失败",
                    navTabId = "",
                    callbackType = "",
                    forwardUrl = ""
                },JsonRequestBehavior.AllowGet);
            }
        }

        //新增修改用户
        public ActionResult AddUser(string userSysNo)
        {
            //StringBuilder sbReturn = new StringBuilder();
            //sbReturn.Append("[{\"id\":\"1\", \"orgName\":\"技术部\", \"orgNum\":\"1001\"},{\"id\":\"2\", \"orgName\":\"人事部\", \"orgNum\":\"1002\"}]");
            //ViewData["UserTypeList"] = sbReturn.ToString();
            //用户类型
            DataTable dtUserType = AppEnum.GetEnumKeyName(typeof(AppEnum.UserType));
            ViewData["dtUserType"] = dtUserType;
            int uSysNo = -9999;
            if(!string.IsNullOrEmpty(userSysNo))
            {
                uSysNo = Convert.ToInt32(userSysNo);
                string sSQL = " SELECT * FROM DWZUser WHERE SysNo = '" + uSysNo + "' ";
                DataSet dsUserInfo = SqlHelper.ExecuteDataSet(ConfigurationManager.ConnectionStrings["Conn_DWZ"].ToString(), sSQL);
                if(dsUserInfo != null && dsUserInfo.Tables.Count > 0)
                {
                    ViewData["uSysNo"] = uSysNo;
                    return View(dsUserInfo);
                }
            }
            ViewData["uSysNo"] = uSysNo;
            return View();
        }

        //保存用户
        public ActionResult SaveUser(int u_SysNo, string u_UserID, string u_UserName, int u_UserType)
        {
            if(string.IsNullOrEmpty(u_UserID) || string.IsNullOrEmpty(u_UserName) || u_UserType == -9999)
            {
                return Json(new
                {
                    statusCode = "300",
                    message = "数据未填写完整",
                    navTabId = "",
                    callbackType = "",
                    forwardUrl = ""
                }, JsonRequestBehavior.AllowGet);
            }
            string sSQL = string.Empty; ;
            if (u_SysNo == -9999)//新增
            {
                sSQL = " INSERT INTO DWZUser(UserID,UserName,UserType,CreateTime) VALUES('" + u_UserID + "','" + u_UserName + "'," + u_UserType + ",GETDATE()) ";
            }
            else
            {
                sSQL = " UPDATE DWZUser SET UserID = '" + u_UserID + "',UserName = '" + u_UserName + "',UserType = '" + u_UserType + "' WHERE SysNo = '" + u_SysNo + "' ";
            }
            SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Conn_DWZ"].ToString(), sSQL);
            return Json(new
            {
                statusCode = "200",
                message = "\u64cd\u4f5c\u6210\u529f",
                navTabId = "UserManager",
                callbackType = "closeCurrent",
                forwardUrl = ""
            }, JsonRequestBehavior.AllowGet);
        }

        //用户列表
        public DataSet GetUserDs(int pageIndex, int pageSize, Hashtable ht)
        {
            SqlParameterCollection spc = new SqlCommand().Parameters;
            PagerData pd = PagerData.GetInstance();
            SqlParameterCollection sp1 = new SqlCommand().Parameters;
            pd.Table = @" DWZUser u ";
            pd.Field = @" u.*  ";
            pd.Where = " WHERE 1=1 ";
            pd.SearchWhere = " 1=1 ";
            string sOrder = " ORDER BY u.CreateTime  " + ht["Sort"];
            pd.Order = sOrder;// " ORDER BY u.CreateTime DESC ";
            pd.PageSize = pageSize;
            pd.Conn = ConfigurationManager.ConnectionStrings["Conn_DWZ"].ToString();
            pd.CurrentPageIndex = pageIndex;
            if (ht != null && ht.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string key in ht.Keys)
                {
                    #region 搜索条件
                    if (key == "UserID")
                    {
                        sb.Append(" AND u.UserID = @UserID ");
                        spc.Add(new SqlParameter("@UserID", ht[key].ToString()));
                    }
                    else if (key == "UserType")
                    {
                        sb.Append(" AND u.UserType = @UserType ");
                        spc.Add(new SqlParameter("@UserType", ht[key].ToString()));
                    }
                    #endregion
                }
                pd.SearchWhere += sb.ToString();
            }
            pd.Collection = spc;
            DataSet ds = pd.GetPage(pageIndex);
            return ds;
        }

        //删除用户
        public string DeleteUser(string SysNo)
        {
            string result = string.Empty;
            try
            {
                string sSQL = " DELETE FROM DWZUser WHERE SysNo = '" + SysNo + "' ";
                SqlHelper.ExecuteNonQuery(ConfigurationManager.ConnectionStrings["Conn_DWZ"].ToString(), sSQL);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }


        #region 下面是其他分页测试

        public ActionResult TestPage()
        {
            //用户类型
            DataTable dtUserType = AppEnum.GetEnumKeyName(typeof(AppEnum.UserType));
            ViewData["dtUserType"] = dtUserType;

            int pageIndex = 1;
            int pageCount = 2;
            int totalCount = 0;
            //if (Request.Form["pageNum"] != null)
            //{
            //    pageIndex = Convert.ToInt32(Request.Form["pageNum"]);
            //}
            //if (Request.Form["numPerPage"] != null)
            //{
            //    pageCount = Convert.ToInt32(Request.Form["numPerPage"]);
            //}
            //用户列表
            Hashtable ht = new Hashtable();
            string sUserID = Request.Form["sUserID"];
            string sUserType = Request.Form["sUserType"];
            string sDesc = Request.Form["orderDirection"];
            if (!string.IsNullOrEmpty(sUserID))
            {
                ht.Add("UserID", sUserID);
                //ViewData["uUserID"] = sUserID;//
            }
            if (!string.IsNullOrEmpty(sUserType) && sUserType != "-9999")
            {
                ht.Add("UserType", sUserType);
                //ViewData["uUserType"] = sUserType;//
            }
            if (string.IsNullOrEmpty(sDesc))
            {
                sDesc = "DESC";
            }
            ht.Add("Sort", sDesc);
            DataSet dsUserList = GetUserDs(pageIndex, pageCount, ht);
            totalCount = Convert.ToInt32(dsUserList.Tables[1].Rows[0][0]);
            ViewData["dsUserList"] = dsUserList;

            ViewData["pageIndex"] = pageIndex;
            ViewData["pageCount"] = pageCount;
            ViewData["totalCount"] = totalCount;
            return View();
        }

        [HttpPost]
        public ActionResult TestPage(FormCollection form)
        {
            //return TestPage();
            //return View();
            return TestPageK(form);
        }

        [HttpPost]
        public ActionResult TestPageK(FormCollection form)
        {
            //用户类型
            DataTable dtUserType = AppEnum.GetEnumKeyName(typeof(AppEnum.UserType));
            ViewData["dtUserType"] = dtUserType;

            int pageIndex = 1;
            int pageCount = 2;
            int totalCount = 0;
            if (Request.Form["pageNum"] != null)
            {
                pageIndex = Convert.ToInt32(Request.Form["pageNum"]);
            }
            if (Request.Form["numPerPage"] != null)
            {
                pageCount = Convert.ToInt32(Request.Form["numPerPage"]);
            }
            //用户列表
            Hashtable ht = new Hashtable();
            string sUserID = Request.Form["sUserID"];
            string sUserType = Request.Form["sUserType"];
            string sDesc = Request.Form["orderDirection"];
            if (!string.IsNullOrEmpty(sUserID))
            {
                ht.Add("UserID", sUserID);
                ViewData["uUserID"] = sUserID;//
            }
            if (!string.IsNullOrEmpty(sUserType) && sUserType != "-9999")
            {
                ht.Add("UserType", sUserType);
                ViewData["uUserType"] = sUserType;//
            }
            if (string.IsNullOrEmpty(sDesc))
            {
                sDesc = "DESC";
            }
            ht.Add("Sort", sDesc);
            DataSet dsUserList = GetUserDs(pageIndex, pageCount, ht);
            totalCount = Convert.ToInt32(dsUserList.Tables[1].Rows[0][0]);
            ViewData["dsUserList"] = dsUserList;

            ViewData["pageIndex"] = pageIndex;
            ViewData["pageCount"] = pageCount;
            ViewData["totalCount"] = totalCount;
            //return PartialView();
            return View();
        }

        #endregion
        
    }
}
