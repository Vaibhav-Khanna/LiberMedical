using libermedical.Models;

namespace libermedical.Managers
{
    public class DocumentsManager : DataManager<Document>, IDocumentsManager
    {
        public DocumentsManager(IRestService<Document> service) : base(service)
        {

        }
    }

    public interface IDocumentsManager : IDataManager<Document>
    {

    }
}