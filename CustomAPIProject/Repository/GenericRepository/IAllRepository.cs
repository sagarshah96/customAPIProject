using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomAPIProject.Repository.GenericRepository
{
    interface IAllRepository<T> where T : class
    {
        IEnumerable<T> GetModel();
        T GetModelById(int ModelId);
        void InsertModel(T model);
        void UpdateModel(T model);
        void DeleteModel(T ModelId);
        void Save();
    }
}
