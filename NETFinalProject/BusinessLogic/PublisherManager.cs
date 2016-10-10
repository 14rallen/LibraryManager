using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class PublisherManager
    {
        public List<Publisher> GetPublisherList(Active group)
        {
            try
            {
                var publisherList = PublisherAccessor.FetchPublisherList(group);

                if (publisherList.Count > 0)
                {
                    return publisherList;
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

        public Publisher GetPublisherByID(string publisherID)
        {
            try
            {
                Publisher publisher = PublisherAccessor.FetchPublisherByID(publisherID);
                return publisher;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddNewPublisher(Publisher publisher)
        {
            try
            {
                if (PublisherAccessor.InsertPublisher(publisher))
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

        public bool EditPublisher(Publisher publisher)
        {
            try
            {
                if (PublisherAccessor.UpdatePublisher(publisher))
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

        public bool DeletePublisher(Publisher publisher, bool toRestore)
        {
            try
            {
                if (PublisherAccessor.InactivatePublisher(publisher, toRestore))
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
