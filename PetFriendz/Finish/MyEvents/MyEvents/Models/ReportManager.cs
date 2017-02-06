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
    public class ReportManager
    {
        IMobileServiceTable<Report> reportTable;
        MobileServiceClient client;

        public ReportManager()
        {
            client = new MobileServiceClient(Constants.ApplicationURL);

            reportTable = client.GetTable<Report>();
        }
        public async Task<Report> GetUserWhere(Expression<Func<Report, bool>> linq)
        {
            try
            {
                List<Report> newUser = await reportTable.Where(linq).Take(1).ToListAsync();
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

        public async Task<Report> SaveGetUserAsync(Report report)
        {
            if (report.ID == null)
            {
                await reportTable.InsertAsync(report);
            }
            else
            {
                await reportTable.UpdateAsync(report);
            }

            try
            {
                List<Report> newUser = await reportTable.Where(userSelect => userSelect.ReportUser == report.ReportUser).ToListAsync();
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

        public async Task<List<Report>> ListUserWhere(Expression<Func<Report, bool>> linq)
        {
            try
            {
                return new List<Report>
                (
                    await reportTable.Where(linq).ToListAsync()
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