using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvents.Models
{
    public class FriendManager
    {
        IMobileServiceTable<Friend> friendTable;
        MobileServiceClient client;

        public FriendManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);

            friendTable = client.GetTable<Friend>();
        }

        public async Task<Friend> GetUserWhere(Expression<Func<Friend, bool>> linq)
        {
            try
            {
                List<Friend> newUser = await friendTable.Where(linq).Take(1).ToListAsync();
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


        public async Task<Friend> SaveGetUserAsync(Friend frienz)
        {
            if (frienz.ID == null)
            {
                await friendTable.InsertAsync(frienz);
            }
            else
            {
                await friendTable.UpdateAsync(frienz);
            }

            try
            {
                List<Friend> newUser = await friendTable.Where(userSelect => userSelect.friendpetname == frienz.friendpetname).ToListAsync();
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

        public async Task<List<Friend>> ListUserWhere(Expression<Func<Friend, bool>> linq)
        {
            try
            {
                return new List<Friend>
                (
                    await friendTable.Where(linq).ToListAsync()
                );
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
