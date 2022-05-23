using AutoMapper;

using PersistenceServices.Interfaces;

namespace PersistenceServices.Implementataions
{

    public class DataStoreRepository<TDataModel, TDomainModel> : IDataStoreRepository<TDataModel, TDomainModel> where TDataModel : IDocument
    {
        private readonly IMapper mapper;

        public DataStoreRepository(IMapper mapper)
        {
            valueCollection = new List<TDataModel>();
            this.mapper = mapper;
        }

        private List<TDataModel> valueCollection { get; set; }

        private TDataModel Add(TDataModel data)
        {
            if (Get(data.Id) != null)
            {
                throw new InvalidOperationException("conflict in Id Key");
            }
            if (string.IsNullOrEmpty(data.Id))
            {
                data.Id = Guid.NewGuid().ToString();
            }
            valueCollection.Add(data);
            return data;
        }
        public TDomainModel Add(TDomainModel model)
        {
            TDataModel data = mapper.Map<TDataModel>(model);
            return mapper.Map<TDomainModel>(Add(data));

        }

        public int Count()
        {
            return valueCollection.Count();
        }


        private TDataModel Get(string id)
        {
            return valueCollection.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TDomainModel> FindAsModels(Func<TDataModel, bool> exp)
        {
            IEnumerable<TDataModel> enumerable = valueCollection.Where(exp);
            return this.mapper.Map<List<TDomainModel>>(enumerable);
        }

        public TDomainModel GetModel(string id) => mapper.Map<TDomainModel>(Get(id));

        public TDomainModel UpdateModel(TDomainModel model)
        {
            var dataModel = mapper.Map<TDataModel>(model);
            Update(dataModel);
            return model;
        }
        private void Update(TDataModel data)
        {
            var item = valueCollection.RemoveAll(x => x.Id == data.Id);
            valueCollection.Add(data);
        }
    }
}