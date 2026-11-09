namespace CY_BM
{
    public enum TicketStatus
    {
        Waiting = 1,
        Answered = 2,
        Closed = 3,
    }
    public enum GuaranteeType
    {
        Guarantee = 1,
        Repairs = 2,

    }
    public enum UserStatus
    {
        Active = 0,
        Blocked = 1,
        Deactive = 2,
        UnConfirmed = 3,
    }

    public enum textEditorState
    {
        oldEditor = 0,
        newEditor = 1,
    }
    //public enum UserType
    //{
    //    Customer = 1,


    //    SysAdmin = 8,
    //    SalesManager = 6,
    //    WarehouseKeeper = 5,
    //    Reporter = 4,
    //}

    //public enum inSupplyStaus
    //{
    //        { id: 1, status: "در حال تامین" },
    //{ id: 2, status: "در حال ارسال" },
    //{ id: 3, status: "تحویل داده شده" },
    //}

    public enum UserType
    {
        /// <summary>
        /// تعیین نقش نشده 
        /// </summary>
        NewUser = -1,


        AllType = 0,


        /// <summary>
        /// ادمین اصلی
        /// </summary>
        SysAdmin = 8,

        /// <summary>
        /// بازرس
        /// </summary>
        Reporter = 4,

        /// <summary>
        /// کارشناس گروه
        /// </summary>
        GroupExpert = 10,

        /// <summary>
        /// کارشناس خرید
        /// </summary>
        PurchasingExpert = 11,

        /// <summary>
        /// مدیر گروه
        /// </summary>
        GroupManager = 12,

        /// <summary>
        /// مدیر واحد
        /// </summary>
        DepartmentManager = 13,

        /// <summary>
        /// مدیر خرید
        /// </summary>
        PurchasingManager = 14,

        /// <summary>
        /// مدیر کل
        /// </summary>
        GeneralManager = 15,

        /// <summary>
        /// ادمین مشتری
        /// </summary>
        CustomerAdmin = 16
    }
    public enum ProductStatus
    {
        InStock = 10,
        Enquiry = 20,

        /// <summary>
        /// !!!
        /// </summary>
        Inactive = 1,
        Active = 2,
        OutOfStock = 3,
        Discontinued = 4
    }

    public enum OrderItemStatus
    {
        Pending = 0,
        Active = 1,
        Deactive = 2,
    }

    public enum OrderStatus
    {
        /// <summary>
        /// در حال ثبت سفارش
        /// </summary>
        Pending = 10,



        /// <summary>
        /// در حال استعلام گیری
        /// </summary>
        Inquiring = 15,

        /// <summary>
        /// در انتظار تایید مشتری
        /// </summary>
        AwaitingCustomerApproval = 20,

        /// <summary>
        /// درصف خرید
        /// </summary>
        InOrderToSupply = 25,

        /// <summary>
        /// در حال تامین
        /// </summary>
        InSupply = 30,

        /// <summary>
        /// خاتمه یافته
        /// </summary>
        Finished = 35,

        Delivered = 40,
        Canceled = -1,


        /// <summary>
        /// !!!!
        /// </summary>
        InBasket = 0,
        Draft = 7,
        InBOM = 8,
        IsSent = 9
    }
    public enum OrderType
    {
        Parts=1,
        PCB=2,
        // استعلام اولیه
        Inquiry=3,
    }

    public enum Currency
    {
        Yuan = 1,
        Rial=2,
        Dollor=3,
    }
    public enum PCBFileType
    {
        Gerber = 1,
        StackLayer = 2,
        MI = 3,
        PickAndPlace = 4,
        BOM = 5,

    }

    public enum NotificationStatus
    {
        Sent = 1,
        Seen = 2
    }
    public enum OrderMessageType
    {
        ClientSide = 1,
        MasterSide = 2,
    }
    public enum InspectionLab
    {
        Iran = 1,
        China = 2
    }

    //public enum Ordermode
    //{
    //    Normal = 0,
    //    Pcb = 1,


    //    /// <summary>
    //    /// !!!!
    //    /// </summary>
    //    Inspection = 2,
    //    Bom = 3
    //}

    public enum ProductProperty
    {
        manufacturer = 1,
        picture = 2,
        category = 3,
    }
}
