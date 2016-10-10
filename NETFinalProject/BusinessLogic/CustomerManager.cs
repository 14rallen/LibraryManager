using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class CustomerManager
    {
        public Customer GetCustomerByUserName(string username)
        {
            try
            {
                Customer customer = CustomerAccessor.FetchCustomerByUserName(username);

                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Customer GetCustomerByID(int customerID)
        {
            try
            {
                Customer customer = CustomerAccessor.FetchCustomerByID(customerID);

                if (customer != null)
                {
                    return customer;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddCustomer(Customer customer)
        {
            customer.Password = customer.Password.HashSha256();

            try
            {
                if (CustomerAccessor.InsertCustomer(customer))
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw new ApplicationException("User could not be added");
            }

            return false;
        }

        public bool EditCustomer(Customer customer)
        {
            bool edited = false;
            customer.Active = true;

            try
            {
                if (CustomerAccessor.UpdateCustomer(customer))
                {
                    edited = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("User could not be edited: " + ex.Message);
            }

            return edited;
        }

        public bool IsUserStaff(string username)
        {
            bool isStaff = false;

            try
            {
                isStaff = CustomerAccessor.IsCustomerStaff(username);
            }
            catch (Exception)
            {
                throw;
            }

            return isStaff;
        }

        public bool CheckUserName(string username)
        {
            try
            {
                if (CustomerAccessor.CheckUserName(username) == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }
    }
}
