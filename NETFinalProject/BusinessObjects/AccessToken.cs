using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public sealed class AccessToken
    {
        public List<Role> Roles { get; set; }
        public Customer User { get; set; }

        public AccessToken() { }
        public AccessToken(Customer user, List<Role> roles)
        {
            if (user == null || roles == null || roles.Count == 0 || !user.Active)
            {
                throw new ApplicationException("Invalid User");
            }

            User.CustomerID = user.CustomerID;
            User.UserName = user.UserName;
            User.Password = user.Password;
            User.FirstName = user.FirstName;
            User.LastName = user.LastName;
            User.Email = user.Email;
            User.Phone = user.Phone;
            User.Active = user.Active;
            

            Roles = roles;
        }
    }
}
