using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class AuthorManager
    {
        public List<Author> GetAuthorList(Active group)
        {
            try
            {
                List<Author> authorList = AuthorAccessor.FetchAuthorList(group);

                if (authorList.Count > 0)
                {
                    return authorList;
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

        public Author GetAuthorByID(int authorID)
        {
            try
            {
                Author author = AuthorAccessor.FetchAuthorByID(authorID);
                return author;
            }
            catch (Exception)
            {
                throw new ApplicationException("Author not found");
            }
        }

        public bool AddNewAuthor(Author author)
        {
            try
            {
                if (AuthorAccessor.InsertAuthor(author))
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

        public bool EditAuthor(Author author)
        {
            try
            {
                if (AuthorAccessor.UpdateAuthor(author))
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

        public bool DeleteAuthor(Author author, bool toRestore)
        {
            try
            {
                if (AuthorAccessor.InactivateAuthor(author, toRestore))
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
