using Demo.Azure;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Models
{
    class UsersManager
    {

        IMobileServiceTable<User> usersTable;
        MobileServiceClient client;

        public UsersManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);

            usersTable = client.GetTable<User>();
        }
        public async Task<User> GetUserWhere(Expression<Func<User, bool>> linq)
        {
            try
            {
                List<User> newUser = await usersTable.Where(linq).Take(1).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

            return null;
        }

        public async Task<User> SaveGetUserAsync(User user)
        {
            if (user.ID == null)
            {
                await usersTable.InsertAsync(user);
            }
            else
            {
                await usersTable.UpdateAsync(user);
            }

            try
            {
                List<User> newUser = await usersTable.Where(userSelect => userSelect.email == user.email).ToListAsync();
                return newUser.First();
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"INVALID {0}", msioe.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"ERROR {0}", e.Message);
            }

            return null;
        }
    }
}
