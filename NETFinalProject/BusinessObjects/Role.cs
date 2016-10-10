using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Role
    {
        public string RoleID { get; set; }
        public string Description { get; set; }

        public Role(string roleID, string description)
        {
            this.RoleID = roleID;
            this.Description = description;
        }

        public Role()
        {
        }

        public override string ToString()
        {
            return RoleID;
        }
    }
}
