using PAsia_Dashboard.Universal.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using PAsia_Dashboard.Areas.Security.Models.BEL;
using PAsia_Dashboard.Areas.Security.DAO;
using System.Data.OracleClient;
using PAsia_Dashboard.Universal.DAL;


namespace PAsia_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        DBConnection dbConn = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        UserLogInDAO userLogInDAO = new UserLogInDAO();
        private readonly AuditTrailDAO _adt = new AuditTrailDAO();
        //[LogInChecker]
        public ActionResult Index()
        {
            if (Session["USER_ID"] != null)
            {
                return View();
                //return RedirectToAction("frmHomeDashboard", "HomeDashboard", new { area = "Dashboard" });
            }
            return RedirectToAction("Login", "Home");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        [HttpGet]
        public string CreateMenu()
        {
            //Session["UserMenu"] = "";
            try
            {
                if (Session["UserMenu"].ToString().Length == 0)
                {
                    string htmlMenu = "";

                    int empId = Convert.ToInt32(Session["USER_ID"].ToString());
                    string roleName = Session["ROLE_NAME"].ToString();
                    //int empID = 5;
                    string MHQry = "SELECT DISTINCT SMH.MH_NAME,SMH.MH_ID,SMH.MH_SEQ,SMH.MH_CSS_CLASS,SMC.RL_ID FROM SA_ROLE_CONF SRC,SA_ROLE SR,SA_MENU_CONF SMC,SA_MENU_HEAD SMH"
                                  + " WHERE SRC.ROLE_ID = SR.ROLE_ID AND SMC.RL_ID = SRC.ROLE_ID AND SMH.MH_ID = SMC.MH_ID AND SRC.USER_ID=" + empId + "  ORDER BY SMH.MH_SEQ ASC";
                    DataTable MHdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), MHQry);
                    List<MenuHead> mhList;
                    mhList = (from DataRow row in MHdt.Rows
                              select new MenuHead
                              {
                                  MH_ID = Convert.ToInt32(row["MH_ID"].ToString()),
                                  MH_NAME = row["MH_NAME"].ToString(),
                                  MH_SEQ = row["MH_SEQ"].ToString(),
                                  RL_ID = Convert.ToInt32(row["RL_ID"].ToString()),
                                  MH_CSS_CLASS = row["MH_CSS_CLASS"].ToString()

                              }).ToList();


                    //if (roleName != "FSM")
                    //{
                    ////    htmlMenu = htmlMenu + " <a href = '/FSM/DashboardNationalPrescription/frmDashboardNationalPrescription' >";
                    ////}
                    ////else
                    ////{
                    //    htmlMenu = htmlMenu + " <li class=''>";
                    //    //htmlMenu = htmlMenu + " <a href = '/Dashboard/HomeDashboard/frmHomeDashboard' >";
                    //    htmlMenu = htmlMenu + " <a href = '/Home/Index' >";
                    //    htmlMenu = htmlMenu + " <i class='fa fa-th'></i> <span>Home</span>";
                    //    //htmlMenu = htmlMenu + " <span class='pull -right-container' > ";
                    //    //htmlMenu = htmlMenu + " <i class='fa fa-angle-left pull-right'></i>";
                    //    //htmlMenu = htmlMenu + " </span>";
                    //    htmlMenu = htmlMenu + " </a></li>";
                    //}
                    foreach (var u in mhList)
                    {
                        htmlMenu = htmlMenu + " <li class='treeview'>";
                        htmlMenu = htmlMenu + " <a href = '#' >";
                        htmlMenu = htmlMenu + " <i class='fa fa-th'></i> <span>" + u.MH_NAME + "</span>";
                        htmlMenu = htmlMenu + " <span class='pull-right-container' >";
                        htmlMenu = htmlMenu + " <i class='fa fa-angle-left pull-right'></i>";
                        htmlMenu = htmlMenu + " </span>";
                        htmlMenu = htmlMenu + " </a>";
                        string SMQry = "SELECT DISTINCT SSM.SM_NAME,SSM.SM_ID,SSM.SM_SEQ,SSM.SM_CSS_CLASS,SSM.URL FROM SA_ROLE_CONF SRC,SA_ROLE SR,SA_MENU_CONF SMC,SA_SUB_MENU SSM "
                                  + " WHERE SRC.ROLE_ID = SR.ROLE_ID AND SMC.RL_ID = SRC.ROLE_ID AND SSM.SM_ID = SMC.SM_ID AND SMC.MH_ID=" + u.MH_ID + " AND SMC.RL_ID=" + u.RL_ID + "ORDER BY SSM.SM_SEQ ASC";
                        DataTable SMdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), SMQry);
                        List<SubMenu> smList;
                        smList = (from DataRow row in SMdt.Rows
                                  select new SubMenu
                                  {
                                      SM_ID = Convert.ToInt32(row["SM_ID"].ToString()),
                                      SM_NAME = row["SM_NAME"].ToString(),
                                      SM_SEQ = row["SM_SEQ"].ToString(),
                                      SM_CSS_CLASS = row["SM_CSS_CLASS"].ToString(),
                                      URL = row["URL"].ToString()

                                  }).ToList();
                        if (smList.Any())
                        {
                            htmlMenu = htmlMenu + " <ul class='treeview-menu' >";
                            foreach (var v in smList)
                            {

                                htmlMenu = htmlMenu + " <li class=''><a href = '" + v.URL + "' ><i class='fa fa-circle-o'></i>  " + v.SM_NAME + "</a></li>";
                                //htmlMenu = htmlMenu + " <li><a href = 'index2.html'><i class='fa fa-circle-o'></i> Dashboard v2</a></li>";

                            }
                            htmlMenu = htmlMenu + " </ul>";
                        }
                    }
                    htmlMenu = htmlMenu + " </li>";

                    //htmlMenu = htmlMenu + "<ul class='nav nav-sidebar'>";
                    //htmlMenu = htmlMenu +
                    //"<li class='nav-parent'><a href='/Home/Index'><i class='icon-star' style='color: #66d659;'></i><span>Dashboard</span></a></li>";
                    //"<li><a href='/Home/Index'><i class='icon-star' style='color: #66d659;'></i><span>Dashboard</span></a></li>";

                    //foreach (var u in mhList)
                    // {
                    //   htmlMenu = htmlMenu + "<li class='nav-parent  " + u.Nm + "'>";
                    //htmlMenu = htmlMenu + "<a href='#'><i class='" + u.CssClass + "'></i><span>" + u.Nm + "</span></a>";
                    //   htmlMenu = htmlMenu + "<a href='#'><i class='fa fa-th' style='color: #66d659;'></i><span>" + u.Nm + "</span></a>";
                    //htmlMenu = htmlMenu + "<a href='#'> <img src='/assets/images/square_only_logo.png' height='20px' ; /><span>" + u.Nm + "</span></a>";

                    //var u1 = u;
                    //var sublist = from a in _db.RLConfs
                    //              join b in _db.RLs on a.RL_ID equals b.ID
                    //              join c in _db.MNConfs on a.RL_ID equals c.RL_ID
                    //              join e in _db.SecSMs on c.SM_ID equals e.ID
                    //              where a.USER_ID == empId && c.MH_ID == u1.ID && e.ID == c.SM_ID
                    //              orderby e.Seq
                    //              select new
                    //              {
                    //                  e.Nm,
                    //                  e.ID,
                    //                  e.Seq,
                    //                  e.Url,
                    //                  e.CssClass
                    //              };

                    //if (sublist.Any())
                    //{
                    //   htmlMenu = htmlMenu + "<ul class= 'children collapse'>";

                    //  foreach (var v in sublist)
                    //  {
                    //htmlMenu = htmlMenu + " <li><a href='" + v.Url + "' data-toggle='tooltip' title='"+ v.Nm + "' data-translate='buttons'><i class='" + v.CssClass + "'></i> " + v.Nm + "</a></li>";
                    //     htmlMenu = htmlMenu + " <li><a href='" + v.Url + "' data-toggle='tooltip' title='" + v.Nm +
                    //  "' data-translate='buttons'><i class='" + v.CssClass + "'></i> <span>" + v.Nm +
                    //                       "' data-translate='buttons'><img src='/assets/images/sqr_pharama_logo.png' height='20px' ; /></i> <span>" + v.Nm +
                    //                       "</span ></a></li>";
                    //        }

                    //        htmlMenu = htmlMenu + "</ul>";
                    //    }

                    //    htmlMenu = htmlMenu + "</li>  </li>";
                    //}

                    // htmlMenu = htmlMenu + "</ul>";
                    //*****************************nav bar hover*******************************
                    //htmlMenu = htmlMenu + "<script>$('.nav-sidebar > li').hover(function() {";
                    //htmlMenu = htmlMenu + "clearTimeout(hoverTimeout);";
                    //htmlMenu = htmlMenu + "     $(this).siblings().removeClass('nav-hover');";
                    //htmlMenu = htmlMenu + "     $(this).addClass('nav-hover');";
                    //htmlMenu = htmlMenu + " }, function() {";
                    //htmlMenu = htmlMenu + "    var $self = $(this);";
                    //htmlMenu = htmlMenu + "    hoverTimeout = setTimeout(function() {";
                    //htmlMenu = htmlMenu + "        $self.removeClass('nav-hover');";
                    //htmlMenu = htmlMenu + "    }, 200);";
                    //htmlMenu = htmlMenu + " });";
                    //htmlMenu = htmlMenu + "  $('.nav-sidebar > li .children').hover(function() {";
                    //htmlMenu = htmlMenu + "     clearTimeout(hoverTimeout);";
                    //htmlMenu = htmlMenu + "        $(this).closest('.nav-parent').siblings().removeClass('nav-hover');";
                    //htmlMenu = htmlMenu + "       $(this).closest('.nav-parent').addClass('nav-hover');";
                    //htmlMenu = htmlMenu + "}, function() {";
                    //htmlMenu = htmlMenu + "   var $self = $(this);";
                    //htmlMenu = htmlMenu + "  hoverTimeout = setTimeout(function() {";
                    //htmlMenu = htmlMenu + "      $(this).closest('.nav-parent').removeClass('nav-hover');";
                    //htmlMenu = htmlMenu + "   }, 200);";
                    //htmlMenu = htmlMenu + " });";

                    //htmlMenu = htmlMenu + "var url = window.location.href;";
                    //*****************************active when pageload*******************************
                    //htmlMenu = htmlMenu + "var url = window.location.pathname;";
                    //htmlMenu = htmlMenu + " var res = url.split('/');";
                    //htmlMenu = htmlMenu + " var status = ' ';";
                    //htmlMenu = htmlMenu + " var newURL = window.location.protocol + '_ _' + window.location.host + '_ _' + window.location.pathname;";

                    //htmlMenu = htmlMenu + "varhref=res[1]);";
                    //htmlMenu = htmlMenu + "alert(url);";
                    //htmlMenu = htmlMenu + "  $('li.nav-parent').each(function(){";
                    //htmlMenu = htmlMenu + "    var parent_span_data = $(this).find('a span').html();";

                    //htmlMenu = htmlMenu + "     $(this).find('ul.children li').each(function(){";
                    //htmlMenu = htmlMenu + "         var a_href = $(this).find(' a').attr('href');";
                    //htmlMenu = htmlMenu + "         var child_span_data = $(this).find('a span').html();";
                    //htmlMenu = htmlMenu + "      alert(parent_span_data);";
                    //htmlMenu = htmlMenu + "      alert(child_span_data);";
                    //htmlMenu = htmlMenu + "         if (a_href == url)";
                    //htmlMenu = htmlMenu + "          {";
                    // htmlMenu = htmlMenu + "          alert('yes');";
                    //htmlMenu = htmlMenu + "          $('.'+parent_span_data).addClass('nav-active active');";
                    //htmlMenu = htmlMenu + "          $(this).addClass('active');";
                    //htmlMenu = htmlMenu + "          $(this).attr('id','active');";
                    //htmlMenu = htmlMenu + "          $('.mCSB_container').removeClass('mCS_no_scrollbar');";
                    //htmlMenu = htmlMenu + "         $('.mCSB_scrollTools').show();";
                    //htmlMenu = htmlMenu + "          $(this).parent().css('display','block');";
                    //htmlMenu = htmlMenu + "          $('.sidebar-inner').mCustomScrollbar('scrollTo', $('.sidebar-inner').find('.mCSB_container').find('.active'));";
                    // htmlMenu = htmlMenu + "          $('.sidebar-inner').mCustomScrollbar('scrollTo', '#active');";


                    // htmlMenu = htmlMenu + "          status='f';";
                    //htmlMenu = htmlMenu + "alert(status);";
                    //htmlMenu = htmlMenu + "         return false;";
                    //htmlMenu = htmlMenu + "        }";
                    //htmlMenu = htmlMenu + "     });";
                    //htmlMenu = htmlMenu + "     if(status=='f'){return false;}";
                    //htmlMenu = htmlMenu + "});";
                    //htmlMenu = htmlMenu + "$('.sidebar').mCustomScrollbar('scrollTo', $('.active').position().top);";
                    // htmlMenu = htmlMenu + "$('.sidebar-inner').mCustomScrollbar({ theme: 'minimal' });";

                    //htmlMenu = htmlMenu + "$('.mCSB_container').find('.active').position().top;";
                    //htmlMenu = htmlMenu + " </script>";

                    Session["UserMenu"] = htmlMenu;
                }

                return Session["UserMenu"].ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [HttpPost]
        public JsonResult EventPermission(string smId)
        {
            int empID = Convert.ToInt32(Session["USER_ID"].ToString());
            string roleName = Session["ROLE_NAME"].ToString();
            //int empID = 1;
            int smD = Convert.ToInt32(smId);

            string EventQry = "SELECT SSM.MH_ID,SSM.SM_NAME,SMH.MH_NAME,SMC.SV,SMC.VW,SMC.DL FROM SA_ROLE_CONF SRC,SA_MENU_CONF SMC,SA_MENU_HEAD SMH,SA_SUB_MENU SSM "
                             + "WHERE SRC.ROLE_ID = SMC.RL_ID AND SMC.SM_ID = SSM.SM_ID AND SSM.MH_ID = SMH.MH_ID AND SRC.USER_ID=" + empID + " AND SMC.SM_ID=" + smD;

            DataTable Eventdt = dbHelper.GetDataTable(dbConn.SAConnStrReader("Dashboard"), EventQry);
            List<EventPermission> mhList;
            mhList = (from DataRow row in Eventdt.Rows
                      select new EventPermission
                      {
                          MH_ID = Convert.ToInt32(row["MH_ID"].ToString()),
                          MH_NAME = row["MH_NAME"].ToString(),
                          SM_NAME = row["SM_NAME"].ToString(),
                          SV = row["SV"].ToString(),
                          DL = row["DL"].ToString(),
                          VW = row["VW"].ToString()

                      }).ToList();

            //var data = (from t in _db.RLConfs
            //            from u in _db.MNConfs
            //            from v in _db.SecSMs
            //            from m in _db.SecMHs
            //            where t.RL_ID == u.RL_ID
            //                  && u.SM_ID == v.ID
            //                  && v.MH_ID == m.ID
            //                  && t.USER_ID == empId && u.SM_ID == smD
            //            select new
            //            {
            //                u.Sv,
            //                u.Dl,
            //                v.Nm,
            //                v.MH_ID,
            //                MenuName = m.Nm
            //            });

            //return Json(mhList);
            return Json(new { role_name = roleName, Data = mhList });
        }

        public ActionResult TryLogin(UserLogin model)
        {
            if (model.Username.Length > 0 && model.Password.Length > 0)
            {
                LoginRegistrationDAO loginRegistrationDAO = new LoginRegistrationDAO();
                //string pass = userLogInDAO.Encrypt(model.Password);
                string userName = "";
                string password = "";
                string trID = "";
                string employeeCode = "";
                string roleName = "";
                //string UserQry = "SELECT * FROM SA_USER_LOGIN SUL WHERE UPPER(SUL.USER_NAME)='" + model.Username.ToUpper() + "' AND SUL.PASSWORD='" + model.Password + "' and SUL.STATUS='A' ";
                //using (OracleConnection oracleConnection = new OracleConnection(dbConn.SAConnStrReader("Dashboard")))
                //{
                //    oracleConnection.Open();
                //    using (OracleCommand oracleCommand = new OracleCommand(UserQry, oracleConnection))
                //    {
                //        using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                //        {
                //            if (rdr.Read())
                //            {
                //                userName = rdr["USER_NAME"].ToString();
                //                employeeCode = rdr["EMPLOYEE_CODE"].ToString();
                //                trID = rdr["USER_ID"].ToString();
                //            }
                //        }
                //    }
                //}
                var verifiedUserCredential = loginRegistrationDAO.CheckUserCredential().FirstOrDefault(m => m.USER_NAME.Equals(model.Username) && m.PASSWORD.Equals(model.Password));

                if (verifiedUserCredential != null)
                {
                    userName = verifiedUserCredential.USER_NAME;
                    employeeCode = verifiedUserCredential.EMPLOYEE_CODE;
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(employeeCode))
                {
                    string uQry = "";
                    uQry = "Select sul.USER_ID,sul.USER_NAME,ei.EMPLOYEE_CODE,ei.EMPLOYEE_NAME,sul.CODE,sul.ACCESS_LEVEL,sr.ROLE_NAME from SA_USER_LOGIN sul inner join EMPLOYEE_INFO ei on sul.EMPLOYEE_CODE = ei.EMPLOYEE_CODE" +
                                 " LEFT JOIN SA_ROLE_CONF src ON SUL.USER_ID = SRC.USER_ID LEFT JOIN SA_ROLE sr ON SRC.ROLE_ID=SR.ROLE_ID where sul.EMPLOYEE_CODE='" + employeeCode + "'";
                    var conString = "Dashboard";
                    if (verifiedUserCredential.GROUP_CODE == "02")
                    {
                        uQry = "Select sul.USER_ID,sul.USER_NAME,ei.EMP_CODE EMPLOYEE_CODE,ei.EMPLOYEE_NAME,sul.CODE,sul.ACCESS_LEVEL,sr.ROLE_NAME from SA_USER_LOGIN sul inner join SC_EMPLOYEE ei on sul.EMPLOYEE_CODE = ei.EMP_CODE" +
                                        " LEFT JOIN SA_ROLE_CONF src ON SUL.USER_ID = SRC.USER_ID LEFT JOIN SA_ROLE sr ON SRC.ROLE_ID=SR.ROLE_ID where sul.EMPLOYEE_CODE='" + employeeCode + "'";
                        conString = "DashboardAnimal";
                    }
                    using (OracleConnection oracleConnection = new OracleConnection(dbConn.SAConnStrReader(conString)))
                    {
                        oracleConnection.Open();
                        using (OracleCommand oracleCommand = new OracleCommand(uQry, oracleConnection))
                        {
                            using (OracleDataReader rdr = oracleCommand.ExecuteReader())
                            {
                                if (rdr.HasRows)
                                {
                                    if (rdr.Read())
                                    {
                                        //string a = rdr["USER_ID"].ToString();
                                        Session["USER_ID"] = rdr["USER_ID"].ToString();
                                        Session["USER_NAME"] = rdr["USER_NAME"].ToString();
                                        Session["EMPLOYEE_CODE"] = rdr["EMPLOYEE_CODE"].ToString();
                                        Session["EMPLOYEE_NAME"] = rdr["EMPLOYEE_NAME"].ToString();
                                        Session["CODE"] = rdr["CODE"].ToString();
                                        Session["ACCESS_LEVEL"] = rdr["ACCESS_LEVEL"].ToString();
                                        Session["ROLE_NAME"] = rdr["ROLE_NAME"].ToString();
                                        Session["UserMenu"] = "";
                                        roleName = rdr["ROLE_NAME"].ToString();
                                        // _adt.InsertAudit("frmLogin", "SA_USER_LOGIN", "LogIn", trID);
                                    }
                                }
                            }
                        }
                    }
                    return Json(new { role_name = roleName, Status = "Ok" });
                }
            }
            return Json(new { Status = "Not Ok" });
        }
        public ActionResult LogOut()
        {
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Home", "");
        }
    }
}