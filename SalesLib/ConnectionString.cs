using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SalesLib
{
    public static class ConnectionString
    {
        private static string _server;
        private static string _db;
        private static string _user;
        private static string _password;

        public static string Init (string path)
        {
            using var file = new StreamReader(path);
            var temp = file.ReadLine();
            var config = temp.Split('|');
            _server = config[0];
            _db = config[1];
            _user = config[2];
            _password = config[3];

            return $"Server={_server};Database={_db};Uid={_user};Pwd={_password};";
        }

    }
}
