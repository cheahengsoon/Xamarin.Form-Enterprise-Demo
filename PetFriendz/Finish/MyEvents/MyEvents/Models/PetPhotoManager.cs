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
    class PetPhotoManager
    {
        IMobileServiceTable<PetPhoto> petphotoTable;
        MobileServiceClient client;

        public PetPhotoManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);

            petphotoTable = client.GetTable<PetPhoto>();
        }

        public async Task<PetPhoto> GetUserWhere(Expression<Func<PetPhoto, bool>> linq)
        {
            try
            {
                List<PetPhoto> newUser = await petphotoTable.Where(linq).Take(1).ToListAsync();
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


        public async Task<PetPhoto> SaveGetUserAsync(PetPhoto petphoto)
        {
            if (petphoto.ID == null)
            {
                await petphotoTable.InsertAsync(petphoto);
            }
            else
            {
                await petphotoTable.UpdateAsync(petphoto);
            }

            try
            {
                List<PetPhoto> newUser = await petphotoTable.Where(userSelect => userSelect.owneremail == petphoto.owneremail).ToListAsync();
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

        public async Task<List<PetPhoto>> ListUserWhere(Expression<Func<PetPhoto, bool>> linq)
        {
            try
            {
                return new List<PetPhoto>
                (
                    await petphotoTable.Where(linq).ToListAsync()
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
