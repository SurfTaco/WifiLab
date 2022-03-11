using System;
namespace DTO
{
    public class DTOBook
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri CoverPicture { get; set; }
        public string NameOfAuthor { get; set; }
    }
}
