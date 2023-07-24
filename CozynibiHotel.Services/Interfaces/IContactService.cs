using CozynibiHotel.Core.Dto;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Interfaces
{
    public interface IContactService
    {
        IEnumerable<ContactDto> GetContacts();
        IEnumerable<ContactDto> SearchContacts(string field, string keyWords);
        ContactDto GetContact(int contactId);
        ResponseModel CreateContact(ContactDto contactCreate);
        ResponseModel UpdateContact(int contactId, ContactDto updatedContact);
        ResponseModel UpdateContact(int contactId, bool isDelete);
        ResponseModel UpdateContactStatus(int contactId, bool status);
        ResponseModel DeleteContact(int contactCategoryId);

    }
}
