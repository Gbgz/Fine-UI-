using FineUICore.QuickStart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FineUICore.EmptyProject.RazorForms
{
    public class BaseModel : PageModel
    {
        #region IsPostBack

        /// <summary>
        /// 是否页面回发
        /// </summary>
        public bool IsPostBack
        {
            get
            {
                return FineUICore.PageContext.IsFineUIAjaxPostBack();
            }
        }

        #endregion

        #region RegisterStartupScript

        /// <summary>
        /// 注册客户端脚本
        /// </summary>
        /// <param name="scripts"></param>
        public void RegisterStartupScript(string scripts)
        {
            FineUICore.PageContext.RegisterStartupScript(scripts);
        }

        #endregion

        #region ViewBag

        private DynamicViewData _viewBag;

        /// <summary>
        /// Add ViewBag to PageModel
        /// https://forums.asp.net/t/2128012.aspx?Razor+Pages+ViewBag+has+gone+
        /// https://github.com/aspnet/Mvc/issues/6754
        /// </summary>
        public dynamic ViewBag
        {
            get
            {
                if (_viewBag == null)
                {
                    _viewBag = new DynamicViewData(ViewData);
                }
                return _viewBag;
            }
        } 
        #endregion

        #region ShowNotify

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        public virtual void ShowNotify(string message)
        {
            ShowNotify(message, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon)
        {
            ShowNotify(message, messageIcon, Target.Top);
        }

        /// <summary>
        /// 显示通知对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageIcon"></param>
        /// <param name="target"></param>
        public virtual void ShowNotify(string message, MessageBoxIcon messageIcon, Target target)
        {
            Notify n = new Notify();
            n.Target = target;
            n.Message = message;
            n.MessageBoxIcon = messageIcon;
            n.PositionX = Position.Center;
            n.PositionY = Position.Top;
            n.DisplayMilliseconds = 3000;
            n.ShowHeader = false;

            n.Show();
        }

        #endregion

        private MovieContext _db;

        /// <summary>
        /// 每个请求共享一个数据库连接实例
        /// </summary>
        protected MovieContext DB
        {
            get
            {
                if (_db == null)
                {
                    _db = BaseModel.GetDbConnection();
                }
                return _db;
            }
        }

        /// <summary>
        /// 获取数据库连接实例（静态方法）
        /// </summary>
        /// <returns></returns>
        public static MovieContext GetDbConnection()
        {
            return FineUICore.PageContext.GetRequestService<MovieContext>();
        }


    }
}