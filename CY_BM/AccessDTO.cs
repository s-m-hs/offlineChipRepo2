using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CY_BM
{
    public class AccessDTO
    {
        public virtual Guid? ID { get; set; }


        public required string Text { get; set; }


        /// <summary>
        ///  ایجاد درخواست
        /// </summary>
        public bool AllowCreateOrdere { get; set; } = false;

        /// <summary>
        ///به قیمت قطاعات و قیمت کل سفارش 
        /// </summary>
        public bool AllowSeePriceOrder_OrderParts { get; set; } = false;

        /// <summary>
        ///درخواست استعلام اولیه 
        /// </summary>
        public bool AllowRequestInitialinquiry { get; set; } = false;


        /// <summary>
        ///دسترسی دعوت به گفتگو 
        /// </summary>
        public bool AllowInviteToChat { get; set; } = false;

        /// <summary>
        ///دسترسی به مشاهده سفارشات 
        /// </summary>
        public bool AllowSeeOrders { get; set; } = false;


        /// <summary>
        ///بارگذاری استعلام سفارشات 
        /// </summary>
        public bool AllowUploudInitialinquiry { get; set; } = false;

        /// <summary>
        ///دسترسی به سفارشات ارسال شده جهت اسعلام گیری 
        /// </summary>
        public bool AllowSentOrdersForInquiry { get; set; } = false;

        /// <summary>
        /// تعیین وضعیت سفارشات جاری(تحویل شده، در حال ارسال و یا در حال تامین)
        /// </summary>
        public bool AllowSetCurrentOrdersStatusA { get; set; } = false;

        /// <summary>
        ///تغییر وضعیت سفارش از حالت در "انتظار تایید مشتری" به "در حال تامین 
        /// </summary>
        public bool AllowSetCurrentOrdersStatusB { get; set; } = false;

        /// <summary>
        ///دسترسی به سفارشات تایید شده مشتری جهت تامین 
        /// </summary>
        public bool AllowAccessConfirmedOrdersForSupply { get; set; } = false;

        /// <summary>
        ///دسترسی به سفارشات تحویل شده 
        /// </summary>
        public bool AllowAccessDeliveredOrders { get; set; } = false;

        /// <summary>
        ///دسترسی به سفارشات لغو شده 
        /// </summary>
        public bool AllowAccessCanceledOrders { get; set; } = false;

        /// <summary>
        ///دسترسی به تمامی سفارشات 
        /// </summary>
        public bool AllowAccessAllOrders { get; set; } = false;

        /// <summary>
        ///لغو درخواست خرید 
        /// </summary>
        public bool AllowCanclePurchaseRequest { get; set; }

        /// <summary>
        ///بارگذاری و موجودی انبار 
        /// </summary>
        public bool AllowUploudWarehouse { get; set; } = false;

        /// <summary>
        ///دسترسی به پنل بخش خرید 
        /// </summary>
        public bool AllowAccessPurchasePanel { get; set; } = false;

        /// <summary>
        ///دسترسی به پنل حساب های کاربران 
        /// </summary>
        public bool AllowAccessUsersPanel { get; set; } = false;


        //دسترسی به بروز رسانی موجودی انبار

        //دسترسی به پنل دسترسی
    }
}
