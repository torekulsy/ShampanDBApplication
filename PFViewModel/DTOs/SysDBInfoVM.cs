using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PFViewModel.DTOs
{
    public class SysDBInfoVM
    {
        public static string SysDatabaseName = "SymphonyVATSys";
        public static string SysUserName = "sa";// { get; set; }
        public static string SysPassword = "S123456_";// { get; set; }
        public static string SysdataSource = ".";// { get; set; }

        public static bool IsWindowsAuthentication = false;// { get; set; }

    }
}
