using AutoMapper;
using CozynibiHotel.Core.Dto;
using CozynibiHotel.Core.Interfaces;
using CozynibiHotel.Core.Models;
using CozynibiHotel.Services.Interfaces;
using HUG.CRUD.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozynibiHotel.Services.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository,
                            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public ContactDto GetContact(int contactId)
        {
            if (!_contactRepository.IsExists(contactId)) return null;
            var contactMap = _mapper.Map<ContactDto>(_contactRepository.GetById(contactId));
            return contactMap;
        }

        public IEnumerable<ContactDto> GetContacts()
        {
            var contactsMap = _mapper.Map<List<ContactDto>>(_contactRepository.GetAll());
            return contactsMap;
        }
        public ResponseModel CreateContact(ContactDto contactCreate)
        {
            if (contactCreate.CreatedBy == 0) contactCreate.CreatedBy = 1;
            if (contactCreate.UpdatedBy == 0) contactCreate.UpdatedBy = 1;
            contactCreate.CreatedAt = DateTime.Now;
            contactCreate.IsActive = true;
            contactCreate.IsDeleted = false;
            var contacts = _contactRepository.GetAll()
                            .Where(l => l.FullName.Trim().ToLower() == contactCreate.FullName.Trim().ToLower()
                                        && l.Email.Trim().ToLower() == contactCreate.Email.Trim().ToLower()
                                        && l.PhoneNumber == contactCreate.PhoneNumber)
                            .FirstOrDefault();
            if (contacts != null)
            {
                return new ResponseModel(422, "Contact already exists");
            }


            var contactMap = _mapper.Map<Contact>(contactCreate);
            contactMap.CreatedAt = DateTime.Now;


            if (!_contactRepository.Create(contactMap))
            {
                return new ResponseModel(500, "Something went wrong while saving");
            }

            return new ResponseModel(201, "Successfully created");

        }
        public ResponseModel UpdateContact(int contactId, ContactDto updatedContact)
        {
            if (updatedContact.CreatedBy == 0) updatedContact.CreatedBy = 1;
            if (updatedContact.UpdatedBy == 0) updatedContact.UpdatedBy = 1;
            updatedContact.UpdatedAt = DateTime.Now;

            if (!_contactRepository.IsExists(contactId)) return new ResponseModel(404, "Not found");
            var contactMap = _mapper.Map<Contact>(updatedContact);
            if (!_contactRepository.Update(contactMap))
            {
                return new ResponseModel(500, "Something went wrong updating contact");
            }

            return new ResponseModel(204, "");

        }
        public ResponseModel DeleteContact(int contactId)
        {
            if (!_contactRepository.IsExists(contactId)) return new ResponseModel(404, "Not found");
            var contactToDelete = _contactRepository.GetById(contactId);
            if (!_contactRepository.Delete(contactToDelete))
            {
                return new ResponseModel(500, "Something went wrong when deleting contact");
            }
            return new ResponseModel(204, "");
        }

        public IEnumerable<ContactDto> SearchContacts(string field, string keyWords)
        {
            var res = _contactRepository.Search(field, keyWords);
            return res;
        }

        public ResponseModel UpdateContact(int contactId, bool isDelete)
        {
            if (!_contactRepository.SetDelete(contactId, isDelete))
            {
                return new ResponseModel(500, "Something went wrong when updaing isDelete contact");
            }
            return new ResponseModel(204, "");
        }
        public ResponseModel UpdateContactStatus(int contactId, bool status)
        {
            if (!_contactRepository.SetStatus(contactId, status))
            {
                return new ResponseModel(500, "Something went wrong when updaing status contact");
            }
            return new ResponseModel(204, "");
        }
    }
}
