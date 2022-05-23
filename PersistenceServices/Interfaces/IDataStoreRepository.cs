
using Shared.DomainModels;

namespace PersistenceServices.Interfaces
{
    public interface IDataStoreRepository<TDatamodel, TDomainModel> where TDatamodel : IDocument
    {
        TDomainModel Add(TDomainModel data);
        int Count();
        IEnumerable<TDomainModel> FindAsModels(Func<TDatamodel, bool> exp);
        TDomainModel GetModel(string id);
        TDomainModel UpdateModel(TDomainModel data);
    }
}