using System;
using AutoMapper;
using DAL.BAL;
using DTO;

namespace WebAPI.Mapper
{
    public class AllMappersProfile : Profile
    {
        public AllMappersProfile()
        {
            BookDTOMapping();
            CustomerDTOMapping();
            RentsDTOMapping();
        }

        void BookDTOMapping()
        {
            // Mit dieser Methode kann man die "Regeln" des mappens festlegen
            // in die <> Klammern kommen: <TSource, TDestionation>
            CreateMap<Book, DTOBook>()
                .ForMember(
                    x => x.NameOfAuthor,
                    opt =>
                        opt.MapFrom(src =>
                            String.Format("{0} {1}",
                                src.Author.Firstname,
                                src.Author.Surname))
                );
        }

        void CustomerDTOMapping()
        {
            CreateMap<Customer, DTOCustomer>()
                .ForMember(
                    x => x.History,
                    opt => opt.MapFrom(y => y.Rents)
                ).ReverseMap();
        }

        void RentsDTOMapping()
        {
            CreateMap<Rent, DTORent>().ReverseMap();
        }
    }
}
