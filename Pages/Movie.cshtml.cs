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

            // 新增按钮的客户端事件
            btnNew.OnClientClick = Window1.GetShowReference(Url.Content("~/MovieNew"), "新增");
        }

        protected async Task Grid1_RowCommandAsync(object sender, GridRowCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                // 删除行
                var rowID = Convert.ToInt32(e.RowID);

                var movie = await DB.Movies.FindAsync(rowID);
                if (movie == null)
                {
                    Alert.Show("指定的电影不存在！");
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
