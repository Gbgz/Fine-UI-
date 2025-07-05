using FineUICore.EmptyProject.RazorForms;
using FineUICore.QuickStart;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FineUICore.EmptyProject.RazorPages
{
    public partial class MovieModel : BaseModel
    {
        public List<Movie> Movies { get; set; }
        //protected FineUICore.Grid Grid1 { get; set; }
        protected async Task Page_LoadAsync(object sender, EventArgs e)
        {
            //Grid1.DataSource = await DB.Movies.ToListAsync();
            //Grid1.DataBind();
            await LoadDataAsync();

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
            IQueryable<Movie> q = DB.Movies;

            // ������
            var searchMessage = ttbSearchMessage.Text?.Trim();
            if (!string.IsNullOrEmpty(searchMessage))
            {
                q = q.Where(s => s.Title.Contains(searchMessage));
            }

            // ��ȡ�ܼ�¼�������������֮������ͷ�ҳ֮ǰ��
            Grid1.RecordCount = await q.CountAsync();

            // ���к����ݿ��ҳ
            q = SortAndPage<Movie>(q, Grid1);

            // �󶨱������
            Grid1.DataSource = await q.ToListAsync();
            Grid1.DataBind();
        }
        protected async Task ttbSearchMessage_Trigger1ClickAsync(object sender, EventArgs e)
        {
            // ������������ݣ����������ͼ��
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;

            await LoadDataAsync();
        }

        protected async Task ttbSearchMessage_Trigger2ClickAsync(object sender, EventArgs e)
        {
            // ��ʾ���ͼ��
            ttbSearchMessage.ShowTrigger1 = true;

            await LoadDataAsync();
        }
        protected async Task Grid1_SortAsync(object sender, GridSortEventArgs e)
        {
            await LoadDataAsync();
        }

        protected async Task Grid1_PageIndexChangedAsync(object sender, GridPageEventArgs e)
        {
            await LoadDataAsync();
        }
    }
}
