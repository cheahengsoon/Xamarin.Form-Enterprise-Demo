using MyEvents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MyEvents.Views
{
    public partial class ReportAbuse : ContentPage
    {
        private string email;
        private string ownername;
        private string text;

        ReportManager manager;

        public ReportAbuse(string text)
        {
            InitializeComponent();
            this.text = text;
            txtowner.Text = text;
            manager = new ReportManager();
        }

        async Task AddReport(Report report)
        {
            Report userResponse = await manager.SaveGetUserAsync(report);
            // Application.Current.Properties["user"] = userResponse;
        }

        async void OnSubmit_Clicked(object sender, EventArgs e)
        {
            string description = this.txtDescription.Text;
            string owner = this.txtowner.Text;


            if (!string.IsNullOrEmpty(description))
            {


                var report = new Report
                {
                    ReportDescription = description,
                    ReportUser = owner


                };
                await AddReport(report);

                await DisplayAlert("Success", "Thank You Reported!", "Cancel");
            }



        }
    }
}
