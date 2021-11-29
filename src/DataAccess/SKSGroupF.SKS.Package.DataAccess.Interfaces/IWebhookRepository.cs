using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Interfaces
{
    public interface IWebhookRepository
    {
        long? Create(DALWebhookResponse response);

        void Delete(long? id);

        void Clear();

        DALWebhookResponses GetAll();

        DALWebhookResponses GetAllWithTrackingId(string trackingId);

        DALWebhookResponse GetById(long? id);
    }
}
