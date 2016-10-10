using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class PageDetails
    {
        private int perpage = 1;
        private int currentPage = 1;
        private int maxPages = 1;
        private int count = 1;
        public int PerPage { get; set; }
        //public int CurrentPage
        //{
        //    get
        //    {
        //        return currentPage;
        //    }
        //    set
        //    {
        //        if (value > maxPages)
        //        {
        //            currentPage = maxPages;
        //        }
        //        else if (value < 1)
        //        {
        //            currentPage = 1;
        //        }
        //        else
        //        {
        //            currentPage = value;
        //        }
        //    }
        //}

        public int CurrentPage { get; set; }
        public int Count { get; set; }
        public int MaxPages
        {
            get
            {
                if (Count % PerPage != 0)
                {
                    maxPages = (Count / PerPage) + 1;
                }
                else
                {
                    maxPages = (Count / PerPage);
                }

                return maxPages;
            }
            private set
            {
                if (Count % PerPage != 0)
                {
                    maxPages = (Count / PerPage) + 1;
                }
                else
                {
                    maxPages = (Count / PerPage);
                }
            }
        }
    }
}
