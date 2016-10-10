using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using BusinessObjects;

namespace BusinessLogic
{
    public static class SecurityManager
    {
        public static bool ValidateExistingUser(string username, string password)
        {
            bool valid = false;
            try
            {
                if (1 == CustomerAccessor.FindUserByUsernameAndPassword(username, password.HashSha256()))
                {
                    valid = true;
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

            return valid;
        }

        public static bool ValidateNewUser(string username, string newPassword)
        {
            if (1 == CustomerAccessor.FindUserByUsernameAndPassword(username, "password"))
            {
                CustomerAccessor.SetPasswordForUsername(username, "password", newPassword.HashSha256());
            }
            else
            {
                throw new ApplicationException("New user could not be created.");
            }

            return ValidateExistingUser(username, newPassword);
        }

        public static AccessToken LogInUser(Customer customer)
        {
            AccessToken accessToken = new AccessToken();
            accessToken.User = customer;

            var roles = CustomerAccessor.RetrieveRolesByUserID(customer.CustomerID);
            accessToken.Roles = roles;

            return accessToken;
        }


        public static bool IsUserInRole(this AccessToken token, string role)
        {
            bool isRole = false;

            foreach(Role r in token.Roles)
            {
                if(r.RoleID == role)
                {
                    isRole = true;
                }
            }

            return isRole;
        }

        public static bool IsUserInRoles(this AccessToken accessToken, string[] roles)
        {
            bool isRole = false;

            if(accessToken != null)
            {
                foreach (Role r in accessToken.Roles)
                {
                    foreach (string s in roles)
                    {
                        if (r.RoleID == s)
                        {
                            isRole = true;
                            break;
                        }
                    }
                }
            }

            return isRole;
        }
    }
}
