using FineUICore.EmptyProject.RazorForms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FineUICore.QuickStart
{
    public partial class MovieEditModel : BaseModel
    {
        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await DB.Movies.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return Content("��Ч������");
            }

            return Page();
        }

        protected async Task btnSaveClose_ClickAsync(object sender, EventArgs e)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            DB.Attach(Movie).State = EntityState.Modified;

            try
            {
                await DB.SaveChangesAsync();
                //Alert.Show("�޸ĳɹ���");
                Alert.Show("�޸ĳɹ���", string.Empty, MessageBoxIcon.Success, ActiveWindow.GetHidePostBackReference());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DB.Movies.Any(e => e.ID == Movie.ID))
                {
                    Alert.Show("ָ���ĵ�Ӱ�����ڣ�" + Movie.Title);
                }
            }
        }
    }
}