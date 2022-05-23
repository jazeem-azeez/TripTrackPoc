
using PersistenceServices.Interfaces;

using Shared.DomainModels;

namespace PersistenceServices.PersistenceModels
{
    public class TripLogDocument : TripLogDOM, IDocument
    {
        public string CountryCode { get; set; }
    }
}
