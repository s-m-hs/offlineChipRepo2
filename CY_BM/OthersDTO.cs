using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class listPro
    {
        public string? PartNumber { get; set; }
        public string? Manufacturer { get; set; }
        public int? Quantity { get; set; }
        public string? creatorCommentA { get; set; }

    }

    public class GIDTO
    {
        public Guid? GID { get; set; }
        public Guid? ID { get; set; }
        public string? Str { get; set; }
    }
   
    public class RegisterModel
    {
        public required string Un { get; set; }
        public required string Pw { get; set; }
        public required string Name { get; set; }
    }

    public class LoginModel
    {
        public required string Un { get; set; }
        public required string Pw { get; set; }
    }

    public class PageDTO
    {
        public string Cat { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 0;
    }

    public class PageSubCatDTO
    {
        public Guid CatId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 0;
    }

    public class PageDTOprodWithNull
    {
        public ProductProperty Property { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 0;
    }

    public class PagedProdCatDTO
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
    }

    public class SearchSubjectDTO
    {
        public string SearchStr { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 10;
    }

    public class PagedOrdersDto
    {
        public OrderStatus OrderStatus { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }


    public class PageResultDTO<T> where T : class
    {
        public string? PageTitle { get; set; }
        public List<T> ItemList { get; set; }
        public int AllCount { get; set; }
    }

    public class UpdatePcbDTO
    {
        public Guid OrderId { get; set; }
        public string? StatusText { get; set; }
        public OrderStatus Status { get; set; }
    }

    public class OutFileModel
    {
        public Guid? ID { get; set; }
        public string? Adress { get; set; }
    }
    public class UserSelect
    {
        public Guid? id { get; set; }
        public string userName { get; set; }
        public string role { get; set; }
        public string vahedText { get; set; }
        public string group { get; set; }
        public Guid? vahedId { get; set; }
        public Guid? gruopId { get; set; }
    }
    public class UserSelectB
    {
        public Guid? id { get; set; }
        public string? userName { get; set; }
        public string? Access { get; set; }
        public Guid? AccessID { get; set; }
        public string? Group { get; set; }
        public Guid? GroupID { get; set; }
        public string? Vahed { get; set; }
        public Guid? VahedID { get; set; }

        public UserType? UserType { get; set; }
        public UserStatus? UserStatus { get; set; }
    }
}
