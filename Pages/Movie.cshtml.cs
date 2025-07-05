using FineUICore.EmptyProject.RazorForms;
using FineUICore.QuickStart;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FineUICore.EmptyProject.RazorPages
{
    public partial class MovieModel : BaseModel
    {
        public List<Movie> Movies { get; set; }
        //protected FineUICore.Grid Grid1 { get; set; }
        protected async Task Page_LoadAsync(object sender, EventArgs e)
        {
            Grid1.DataSource = await DB.Movies.ToListAsync();
            Grid1.DataBind();

            // ������ť�Ŀͻ����¼�
            btnNew.OnClientClick = Window1.GetShowReference(Url.Content("~/MovieNew"), "����");
        }

        protected async Task Grid1_RowCommandAsync(object sender, GridRowCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                // ɾ����
                var rowID = Convert.ToInt32(e.RowID);

                var movie = await DB.Movies.FindAsync(rowID);
                if (movie == null)
                {
                    Alert.Show("ָ���ĵ�Ӱ�����ڣ�");
                    return;
                }

                DB.Movies.Remove(movie);
                await DB.SaveChangesAsync();

                await LoadDataAsync();
            }
        }
        private async Task LoadDataAsync()
        {
            Grid1.DataSource = await DB.Movies.ToListAsync();
            Grid1.DataBind();
        }
    }
}
