using FineUICore.EmptyProject.RazorForms;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FineUICore.QuickStart
{
    public partial class MovieNewModel : BaseModel
    {
        [BindProperty]
        public Movie Movie { get; set; }

        public void OnGet()
        {

        }

        protected async Task btnSaveClose_ClickAsync(object sender, EventArgs e)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            DB.Movies.Add(Movie);
            await DB.SaveChangesAsync();

            Alert.Show("±£´æ³É¹¦£¡");
        }

    }
}