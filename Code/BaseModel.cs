using FineUICore.QuickStart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        protected IQueryable<T> SortAndPage<T>(IQueryable<T> q, Grid grid)
        {
            return SortAndPage(q, grid.PageIndex, grid.PageSize, grid.RecordCount, grid.SortField, grid.SortDirection);
        }
        // 排序后分页
        protected IQueryable<T> SortAndPage<T>(IQueryable<T> q, int pageIndex, int pageSize, int recordCount, string sortField, string sortDirection)
        {
            //// 对传入的 pageIndex 进行有效性验证//////////////
            int pageCount = recordCount / pageSize;
            if (recordCount % pageSize != 0)
            {
                pageCount++;
            }
            if (pageIndex > pageCount - 1)
            {
                pageIndex = pageCount - 1;
            }
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            ///////////////////////////////////////////////

            return Sort(q, sortField, sortDirection).Skip(pageIndex * pageSize).Take(pageSize);
        }

        protected IQueryable<T> Sort<T>(IQueryable<T> q, string sortField, string sortDirection)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                return q;
            }

            // 获取实体类型
            var entityType = typeof(T);

            // 获取属性信息
            var property = entityType.GetProperty(sortField);
            if (property == null)
            {
                return q;
            }

            // 构建排序表达式
            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            // 根据排序方向调用相应的排序方法
            string methodName = sortDirection?.ToUpper() == "DESC" ? "OrderByDescending" : "OrderBy";

            // 获取排序方法
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(entityType, property.PropertyType);

            // 执行排序
            return (IQueryable<T>)method.Invoke(null, new object[] { q, lambda });
        }
    }
}