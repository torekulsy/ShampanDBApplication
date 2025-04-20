using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using PFViewModel.DTOs;
using DataTable = System.Data.DataTable;
namespace PFServer.Library
{
    public class DBSQLConnection
    {
        //private string ConnectionString = "";
        public DBSQLConnection()
        {

        }


        static int tt = 0;
        public SqlConnection GetConnectionNoPooling(SysDBInfoVMTemp connTemp = null)
        {
            string ConnectionString = "";
            if (connTemp != null)
            {
                SysDBInfoVM.SysdataSource = connTemp.SysdataSource;
                SysDBInfoVM.SysPassword = connTemp.SysPassword;
                SysDBInfoVM.SysUserName = connTemp.SysUserName;
                DatabaseInfoVM.DatabaseName = connTemp.SysDatabaseName;
            }

            if (SysDBInfoVM.IsWindowsAuthentication)
            {
                ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";trusted_Connection=True;Initial Catalog=" + DatabaseInfoVM.DatabaseName + ";connect Timeout=600; pooling=no;";
            }
            else
            {
                ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";Initial Catalog=" + DatabaseInfoVM.DatabaseName + ";user id=" + SysDBInfoVM.SysUserName
                  + ";password=" + SysDBInfoVM.SysPassword + ";connect Timeout=600; pooling=no;";
            }
            SqlConnection conn = new SqlConnection(ConnectionString);


            return conn;
        }

        public SqlConnection GetConnection(SysDBInfoVMTemp connTemp = null)
        {
            string ConnectionString = "";
            if (connTemp != null)
            {
                //SysDBInfoVM.SysdataSource = connTemp.SysdataSource;
                //SysDBInfoVM.SysPassword = connTemp.SysPassword;
                //SysDBInfoVM.SysUserName = connTemp.SysUserName;
                //DatabaseInfoVM.DatabaseName = connTemp.SysDatabaseName;

                if (SysDBInfoVM.IsWindowsAuthentication)
                {
                    ConnectionString = "Data Source=" + connTemp.SysdataSource + ";trusted_Connection=True;Initial Catalog="
                        + connTemp.SysDatabaseName + ";connect Timeout=600; pooling=no;";
                }
                else
                {
                    ConnectionString = "Data Source=" + connTemp.SysdataSource + ";Initial Catalog=" + connTemp.SysDatabaseName
                        + ";user id=" + connTemp.SysUserName
                      + ";password=" + connTemp.SysPassword + ";connect Timeout=600; pooling=no;";
                }

            }
            else
            {
                if (SysDBInfoVM.IsWindowsAuthentication)
                {
                    ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";trusted_Connection=True;Initial Catalog="
                                       + DatabaseInfoVM.DatabaseName + ";connect Timeout=600; pooling=no;";
                }
                else
                {
                    ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";Initial Catalog=" + DatabaseInfoVM.DatabaseName + ";user id=" + SysDBInfoVM.SysUserName
                                       + ";password=" + SysDBInfoVM.SysPassword + ";connect Timeout=600; pooling=no;";
                }
            }


            //if (SysDBInfoVM.IsWindowsAuthentication)
            //{
            //    ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";trusted_Connection=True;Initial Catalog=" 
            //        + DatabaseInfoVM.DatabaseName + ";connect Timeout=600; pooling=no;";
            //}
            //else
            //{
            //    ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";Initial Catalog=" + DatabaseInfoVM.DatabaseName + ";user id=" + SysDBInfoVM.SysUserName
            //      + ";password=" + SysDBInfoVM.SysPassword + ";connect Timeout=600; pooling=no;";
            //}
            SqlConnection conn = new SqlConnection(ConnectionString);


            return conn;
        }

        public SqlConnection GetConnectionSys(SysDBInfoVMTemp connTemp = null)
        {
            string ConnectionString = "";
            if (connTemp != null)
            {
                //SysDBInfoVM.SysdataSource = connTemp.SysdataSource;
                //SysDBInfoVM.SysPassword = connTemp.SysPassword;
                //SysDBInfoVM.SysUserName = connTemp.SysUserName;
            }

            ConnectionString = "Data Source=" + SysDBInfoVM.SysdataSource + ";" +
                                    "Initial Catalog=master;" +
                                    "user id=" + SysDBInfoVM.SysUserName + ";" +
                                    "password=" + SysDBInfoVM.SysPassword + ";" +
                                    "connect Timeout=60;";

            SqlConnection conn = new SqlConnection(ConnectionString);


            return conn;
        }
    }

}
