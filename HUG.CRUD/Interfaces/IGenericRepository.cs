using HUG.CRUD.Base;
using HUG.CRUD.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HUG.CRUD.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        //normal
        T GetById(int id);
        ICollection<T> GetAll();
        ICollection<T> Search(string field, string keyWords);
        bool IsExists(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool SoftDelete(T entity);
        bool Save();

        //async func
        Task<T> GetById_Async(int id);
        Task<ICollection<T>> GetAll_Async();
        Task<bool> IsExists_Async(int id);
        Task<bool> Create_Async(T entity);
        Task<bool> Update_Async(T entity);
        Task<bool> Delete_Async(T entity);
        Task<bool> Save_Async();

    }
}
