using Data;
using Entity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public interface IUserApiKeyService
    {
        UserApiKey CreateUserApiKey(UserApiKey UserApiKey, string userId);
        List<UserApiKey> GetUserApiKeys(string userId);
        UserApiKey GetUserApiKey(int Id, string userId);
        void UpdateUserApiKey(UserApiKey UserApiKey, string userId);
        void DeleteUserApiKey(UserApiKey UserApiKey, string userId);
    }

    public class UserApiKeyService : IUserApiKeyService
    {
        private ReadLaterDataContext _ReadLaterDataContext;
        private readonly ILogger _logger;

        public UserApiKeyService(ReadLaterDataContext readLaterDataContext, ILoggerFactory loggerFactory) 
        {
            _logger = loggerFactory.CreateLogger(typeof(UserApiKeyService));
            _ReadLaterDataContext = readLaterDataContext;            
        }

        public UserApiKey CreateUserApiKey(UserApiKey UserApiKey, string userId)
        {
            UserApiKey.UserId = userId;
            _ReadLaterDataContext.Add(UserApiKey);
            _ReadLaterDataContext.SaveChanges();
            return UserApiKey;
        }

        public void UpdateUserApiKey(UserApiKey UserApiKey, string userId)
        {
            UserApiKey.UserId = userId;
            var foundUserApiKey = _ReadLaterDataContext.UserApiKey.Where(u => u.ID == UserApiKey.ID && u.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
            if (foundUserApiKey is null)
            {
                _logger.LogError("Unable to match userApiKey with id {UserApiKeyId} with userId {userId}", UserApiKey.ID, userId);
                return;
            }

            _ReadLaterDataContext.Update(UserApiKey);
            _ReadLaterDataContext.SaveChanges();
        }

        public List<UserApiKey> GetUserApiKeys(string userId)
        {
            return _ReadLaterDataContext.UserApiKey.Where(u => u.UserId.ToLower() == userId.ToLower()).ToList();
        }

        public UserApiKey GetUserApiKey(int Id, string userId)
        {
            return _ReadLaterDataContext.UserApiKey.Where(u => u.ID == Id && u.UserId.ToLower() == userId.ToLower()).FirstOrDefault();
        }

        public void DeleteUserApiKey(UserApiKey UserApiKey, string userId)
        {
            var foundUserApiKey = _ReadLaterDataContext.UserApiKey
                .Where(u => u.ID == UserApiKey.ID && u.UserId.ToLower() == userId.ToLower())
                .FirstOrDefault();
            if (foundUserApiKey != null)
            {
                _ReadLaterDataContext.UserApiKey.Remove(foundUserApiKey);
            }
            else
            {
                _logger.LogError("Unable to match userApiKey with id {UserApiKeyId} with userId {userId}", UserApiKey.ID, userId);
                return;
            }
            _ReadLaterDataContext.SaveChanges();
        }
    }
}
