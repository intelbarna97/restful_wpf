using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_RESTful.entity
{
    public class User
    {
        private int id;

        private string username;

        private string password;

        private int permission;

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public int Id { get => id; set => id = value; }
        public int Permission { get => permission; set => permission = value; }

        public bool isAdmin()
        {
            return permission==1 ? true : false;
        }
    }
}
