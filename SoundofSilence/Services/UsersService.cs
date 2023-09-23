using Data;
using Entities;
using SoundofSilence.IServices;

namespace SoundofSilence.Services
{
    public class UsersService : BaseContextService, IUsersService
    {
        public UsersService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public int InsertUsers(Users users)
        {
            _serviceContext.Users.Add(users);
            _serviceContext.SaveChanges();
            return users.Id_user;
        }
    }
}
