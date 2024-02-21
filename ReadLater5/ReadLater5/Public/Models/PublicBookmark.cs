using System;

namespace Entity
{
    public class PublicBookmark
    {
        public int ID { get; set; }

        public string URL { get; set; }

        public string ShortDescription { get; set; }

        public string Category { get; set; }

        public DateTime CreateDate { get; set; }

        public Bookmark GetFullBookmark()
        {
            return new Bookmark()
            {
                ID = ID,
                URL = URL,
                ShortDescription = ShortDescription,
                CreateDate = CreateDate,
                Category = new Category()
                {
                    Name = Category
                }
            };
        }
    }
}
