using Microsoft.Extensions.Logging;
using SKSGroupF.SKS.Package.DataAccess.Entities.Models;
using SKSGroupF.SKS.Package.DataAccess.Interfaces;
using SKSGroupF.SKS.Package.DataAccess.Interfaces.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SKSGroupF.SKS.Package.DataAccess.Sql
{
    public class SqlWebhookRepository : IWebhookRepository
    {
        private ISqlDbContext context;
        private readonly ILogger logger;

        public SqlWebhookRepository(ISqlDbContext context, ILogger<SqlWebhookRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public long? Create(DALWebhookResponse response)
        {
            string errorMsg;
           
            try
            {
                //Insert Parcel and return db ID
                logger.LogInformation("Inserting subscirption into database.");
                var ent = context.DbWebhooks.Add(response);
                logger.LogInformation("Subscription inserted with ID + " + ent.Entity.Id + ".");
                SaveChanges();
                return ent.Entity.Id;
            }
            catch (Exception ex)
            {
                errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg, ex);
                throw new DALDataException(nameof(SqlParcelRepository), nameof(Create), errorMsg, ex);
            }
        }

        public void Delete(long? id)
        {
            logger.LogInformation("Removing existing webhook from database.");
            var webhook = context.DbWebhooks.Find(id);
            if (webhook == null)
            {
                string errorMsg = "Webhook with specified Id could not be found in database.";
                logger.LogError(errorMsg);
                throw new DALDataNotFoundException(nameof(SqlWebhookRepository), nameof(Delete), errorMsg);
            }
            context.DbWebhooks.Remove(webhook);
            SaveChanges();
        }

        public void Clear()
        {
            logger.LogInformation("Clearing all webhooks from database.");

            try
            {
                foreach (var rec in context.DbWebhooks)
                    context.DbWebhooks.Remove(rec);
                SaveChanges();
            }
            catch (Exception ex)
            {
                string errorMsg = "An error occurred when removing webhooks from database.";
                logger.LogError(errorMsg);
                throw new DALSqlContextException(nameof(SqlWebhookRepository), nameof(Clear), errorMsg, ex);
            }
        }

        public DALWebhookResponses GetAll()
        {
            string errorMsg;

            try
            {
                logger.LogInformation("Trying to select all webhooks from database.");
                var tmp = new DALWebhookResponses();
                foreach (var i in context.DbWebhooks)
                {
                    tmp.Add(i);
                }
                return tmp;
            }
            catch (Exception ex)
            {
                errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg, ex);
                throw new DALDataException(nameof(SqlParcelRepository), nameof(Create), errorMsg, ex);
            }
        }

        public DALWebhookResponses GetAllWithTrackingId(string trackingId)
        {
            string errorMsg;

            try
            {
                logger.LogInformation("Trying to select all webhooks for trackingId from database.");
                var tmp = new DALWebhookResponses();
                foreach (var i in context.DbWebhooks.Where(a => a.TrackingId == trackingId))
                {
                    tmp.Add(i);
                }
                return tmp;
            }
            catch (Exception ex)
            {
                errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg, ex);
                throw new DALDataException(nameof(SqlParcelRepository), nameof(Create), errorMsg, ex);
            }
        }

        public DALWebhookResponse GetById(long? id)
        {
            string errorMsg;

            try
            {
                logger.LogInformation("Trying to select webhook with specified id from database.");

                return context.DbWebhooks.Single(a => a.Id == id);
            }
            catch (Exception ex)
            {
                errorMsg = "There has been an error with the database";
                logger.LogError(errorMsg, ex);
                throw new DALDataException(nameof(SqlParcelRepository), nameof(Create), errorMsg, ex);
            }
        }

        private int SaveChanges()
        {
            logger.LogInformation("Saving changes to database.");
            return context.SaveChangesToDb();
        }
    }
}
