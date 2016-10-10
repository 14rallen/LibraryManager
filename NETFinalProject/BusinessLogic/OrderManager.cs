using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class OrderManager
    {
        public List<Order> GetOrderList()
        {
            try
            {
                var orderList = OrderAccessor.FetchOrderList();

                if (orderList.Count > 0)
                {
                    return orderList;
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

        public List<OrderDesc> GetOrderListDesc()
        {
            try
            {
                var orderList = GetOrderList();

                List<OrderDesc> orderDescs = new List<OrderDesc>();

                CustomerManager customerManager = new CustomerManager();

                foreach(Order order in orderList)
                {
                    OrderDesc orderDesc = new OrderDesc();
                    orderDesc = OrderAccessor.FetchOrderListDesc(order.OrderID);
                    /*
                    orderDesc.OrderID = order.OrderID;
                    orderDesc.StaffID = order.StaffID;
                    orderDesc.DateOrdered = order.DateOrdered;
                    */
                    Customer customer = new Customer();
                    customer = customerManager.GetCustomerByID(order.CustomerID);

                    orderDesc.StaffName = customer.LastName + ", " + customer.FirstName;

                    orderDescs.Add(orderDesc);
                }

                if (orderList.Count > 0)
                {
                    return orderDescs;
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

        public bool AddOrders(int customerID, string isbn, int quantity)
        {
            bool added = false;

            try
            {
                OrderAccessor.InsertOrder(customerID, isbn, quantity);
                added = true;
            }
            catch (Exception)
            {
                throw;
            }

            return added;
        }
    }
}
