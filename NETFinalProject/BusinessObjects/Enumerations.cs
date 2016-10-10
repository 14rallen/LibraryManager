namespace BusinessObjects
{
    public enum Active
    {
        active,
        inactive,
        both
    }

    public enum BookSearchType
    {
        isbn,
        title,
        author,
        all,
        deleted
    }

    public enum BookMode
    {
        add,
        edit,
        delete,
        restore
    }

    public enum AuthorMode
    {
        add,
        edit
    }

    public enum PublisherMode
    {
        add,
        edit
    }
}