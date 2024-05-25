using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edex.Model
{


    public static class cSTORESETTING
    {

        //اعدادات المخازن
        #region WH_SETTING
        public static int ID { get; set; }
        //إنشاء قيد تكاليف المبيعات
        public static int chkSTC_CreateCostRegistation { get; set; }
        //نوع التكلفة
        public static int STC_STORINGSETTING_COSTTYPE { get; set; }
        //نوع المتوسط
        public static int STC_STORINGSETTING_AVERAGETYPE { get; set; }
        //تسعيرة الاصناف
        public static int STC_STORINGSETTING_ITEMPRICES { get; set; }
        public static int STC_DECIMALNUMRICEFORCOSTDIGITS { get; set; }


        //حدود الأصناف
        public static int STC_STORINGSETTING_MAXITEMSQTY { get; set; }
        //طريقة ترحيل المخزون
        public static int STC_TRANSMETHOD_STORINGSETTING { get; set; }
        //طريقة عمولةالأصناف
        public static int STC_STORINGSETTING_METHODITEMS { get; set; }
        //حد الطلب
        public static int STC_StoringSetting_MaxOrderQty { get; set; }
        //الوان الصنف
        public static int STC_ColorsItem { get; set; }
        //تكرار الصنف في التحويل
        public static int STC_STORESETTING_REPITEMITRANS { get; set; }
        //إظهار التكلفة في التحويل
        public static int chkSTC_ShowCostInTrans { get; set; }
        //اظهار الكمية المتوفرة في التحويل
        public static int chkSTC_ShowQuantityInTransfer { get; set; }
        //اعتماد سعر التحويل تكلفة للاستلام المخزني
        public static int chkSTC_PriceTransAssCostRsiveStoring { get; set; }
        //السماح بتعديل الكمية المستلمة في الاستلام المخزني
        public static int chkSTC_AllowReceivedQtyInStockreceipt { get; set; }
        //السماح بتعديلالمخزن  في الاستلام المخزني
        public static int chkSTC_AllowmodificationStoreInInBail { get; set; }
        //فحص المخزن المطلوب منه في التحويل المخزني
        public static int chkSTC_CheckingStoreInWarehouseTransfer { get; set; }
        //اظهار الشد في التحوي المخزني
        public static int chkSTC_ShowTensionInTrans { get; set; }

        //تعدد المخازن في أوامر التوريد
        public static int chkSTC_MultipleStoresInsupplyOrders { get; set; }

        //تعدد المخازن في أوامر الصرف
        public static int chkSTC_MultipleStoresInExchangeOrders { get; set; }
        //استخدام السندات المعلقة
        public static int chkSTC_UseofoutstandingBonds { get; set; }
        //اعتماد سعر الوحدة الاصغر على سعر الوحدة الأكبر في بيانات الاصناف
        public static int chkSTC_AdoptingSmallerUnitPrice { get; set; }
        //استخدام الصنف البديل
        public static int chkSTC_UseReplacementItem { get; set; }
        //استخدام وزن الوحدة
        public static int chkSTC_UseUitWeight { get; set; }
        //اظهار البيان على مستوى الصنف
        public static int chkSTC_ShowStatementatItemLevel { get; set; }
        //استخدام الوزن من أصغر وحدة
        public static int chkSTC_UseWeightSmallestUnit { get; set; }
        //استخدام البيان محلي وأجنبي
        public static int chkSTC_UseStatementLocalAndforeign { get; set; }
        //استخدام الألوان للصنف
        public static int chkSTC_UseColorItemes { get; set; }
        //استخدام الأصناف الخدمية في الصرف المخزني
        public static int chkSTC_UseServiceItemsInOut { get; set; }

        //إعتماد الطلبات والأوامر أليا
        public static int chkSTC_ApprovalRequestsAutomatically { get; set; }
        //استخدام تجميع الأصناف
        public static int chkSTC_UseItemGrouping { get; set; }
        //الأصناف المركبة
        public static int STC_CompoundItems { get; set; }
        //استخدام المقاسات
        public static int chkSTC_UseSizesing { get; set; }

        //استخدام رقم الدفعه
        public static int chkSTC_UseBachNo { get; set; }
        //chkSTC_UseExpirationDate
        public static int chkSTC_UseExpirationDate { get; set; }
        //اظهار مواصفات الصنف
        public static int chkSTC_ShowItemDescription { get; set; }
        //استخدام الأرقام التسلسلية
        public static int chkSTC_UseSerilNumber { get; set; }
        //استخدام مكونات الأصناف
        public static int chkSTC_UseItemComponents { get; set; }

        public static int chkSTC_ShowItemPackage { get; set; }


        //استخدام باركود واحد للصنف
        public static int chkSTC_UseOneBarcodToItem { get; set; }
        //تكوين الباركود لأصغر وحدة
        public static int chkConfigureBarcodesSmallestUnit { get; set; }
        //تعدد المراكز في أوامر التوريد
        public static int STC_COSTCENTEROPTIONSIN_IN { get; set; }
        //تعدد المراكز في أوامر الصرف
        public static int STC_COSTCENTEROPTIONSIN_OUT { get; set; }
        //تعدد ا لمراكز في التسوية
        public static int STC_MultiCostCenterInInventory { get; set; }
        //استخدام المراكز في التحويلات
        public static int STC_UseCostCenterInTrans { get; set; }
        //أوامر التوريد المخزني
        public static int STC_STORE_SERILIN_IN { get; set; }

        //تسلسل العملية
        public static int STC_STORE_SERIALOPRATION { get; set; }

        //التحويل المخزني
        public static int STC_STORE_SERIALTRANS { get; set; }

        //الطلبات المخزنية
        public static int STC_STORE_SERIALORDER { get; set; }
        //الجرد المخزني

        public static int STC_STORE_SERIALINVENTORY { get; set; }

        //استخدام المشاريع في أوامر التوريد
        public static int STC_STORE_PROJECTIN_IN { get; set; }

        //استخدام المشاريع في أوامر الصرف
        public static int STC_STORE_PROJECTIN_OUT { get; set; }
        //استخدام المشاريع في المشتريات
        public static int STC_STORE_PROJECTIN_PURCHAS { get; set; }

        //استخدام المشاريع في التحويلات
        public static int STC_STORE_PROJECTIN_TRANS { get; set; }
        //ادخال رقم المرجع اجباري
        public static int chkSTC_MandatoryEnterReferenceNumber { get; set; }
        //ادخال البيان اجباري
        public static int chkSTC_DescriptionEntryRequired { get; set; }
        //تكرار الصنف في التوريد ا لمخزني
        public static int chkSTC_ItemDuplicationInInBail { get; set; }
        //تكرار الصنف في الجرد اليدوي
       public static int chkSTC_ItemDuplicationInTalking { get; set; }

        //تكرار الصنف في الصرف المخزني
        public static int STC_STORE_REPEATITEMSINOUT { get; set; }

        //   تسلسل العملية
        public static int STC_STORE_SERIALOPRATIONINOUT { get; set; }

        //اضهار التاريخ
        public static int STC_STORINGSETTING_SHOWDATE { get; set; }


        //الارقام العشرية
        public static int STC_DecimalNumriceDigts { get; set; }
        //الارقام العشرية للتكاليف
        public static int STC_DecimalNumriceForCostDigits { get; set; }
        //الارقام العشرية للكمية
        public static int STC_QtyDigits { get; set; }

        //خانات المادة للمواد الوزنية    
        public static int STC_ItemBarcodeWeightDigits { get; set; }
        //خانات السعر للمواد الوزنية    
        public static int STC_PriceBarcodeWeightDigits { get; set; }
         //.....................................................................
        #endregion
        
    }

    public static class cPURCHASESETTING
    {

        //اعدادات المخازن
        #region PURCHASE_SETTING
        public static int PUR_DigitResourceNumber { get; set; }

        public static int PUR_JOINWITHSTATEMENT { get; set; }
        public static int PUR_COSTCENTEROPTION { get; set; }
        public static int PUR_SERIALOPRATION { get; set; }
        public static int chkPUR_FirstDiscountOnSublier { get; set; }
        public static int PUR_SERIALPROFITSALEPRICE { get; set; }
        public static int PUR_SERIALRETURN { get; set; }
        public static int PUR_SERIALSUBLIER { get; set; }
        public static int PUR_SERIALINVOICE { get; set; }
        public static int PUR_SERIALORDER { get; set; }
        public static int PUR_SERIALREQUEST { get; set; }
        public static int PUR_SETTING_REPEATENUMREFRENCE { get; set; }
        public static int PUR_DecimalNumrice { get; set; }
        public static int PUR_SETTING_SHOWDATE { get; set; }
        public static int PUR_TYPENUMRICESUBLIER { get; set; }
        public static int PUR_SETTING_METHODEDESCOUNT { get; set; }
        public static int PUR_WrningUseInvoice { get; set; }
        
        public static int chkPUR_UsePurchaseCheck { get; set; }
        public static int chkPUR_UsePKSPurchaseRequisition { get; set; }

        public static int chkPUR_BringTaxReturnPreviousYears { get; set; }
        public static int chkPUR_ShowDomesticAndForeignData { get; set; }
        public static int chkPUR_AutomaticApprovalRequestsAndOrders { get; set; }
        public static int chkPUR_UseRefundRequest { get; set; }
        public static int chkPUR_Showitemunits { get; set; }
        public static int chkPUR_AlertifCostLessCurrentPurchasePrice { get; set; }
        public static int chkPUR_DownloadDefaultunitswhenAddingItem { get; set; }
        public static int chkPUR_ModifyQuotationwhenModifyingOrder { get; set; }
        public static int chkPUR_UsePrivateContracts { get; set; }
        public static int chkPUR_FetchDefaultSellingPrice { get; set; }
        public static int chkPUR_AllowCancelQtyExternal { get; set; }
        public static int chkPUR_ExternalPurchasetAlertfrompermissionsupply { get; set; }
        public static int chkPUR_UseTaxonFreeQty { get; set; }
        public static int chkPUR_ShowstatementItem { get; set; }
        public static int chkPUR_UseTaxBurden { get; set; }
        public static int chkPUR_UseoutStandingBonds { get; set; }
        public static int chkPUR_ShowItemSpecifications { get; set; }
        public static int chkPUR_UseAccountforTaxpayerCustoms { get; set; }
        public static int chkPUR_UseTaxInclusivePrice { get; set; }
        public static int chkPUR_ModifyExistingQuotation { get; set; }
        public static int chkPUR_EnterSalepriceInpurchase { get; set; }
        public static int chkPUR_NewInvoiceForLastYear { get; set; }
        public static int chkPUR_UseServiceItems { get; set; }
        public static int chkPUR_UseDiscountEveryItems { get; set; }
        public static int chkPUR_UseFreeQty { get; set; }
        public static int chkPUR_ShowAlertItemNotExist { get; set; }
        public static int chkPUR_Payinstallmentsmanually { get; set; }
        public static int chkPUR_UseMultipleStores { get; set; }
        #endregion




    }


    public static class cACCOUNTSSETTING
    {

        #region ACC_SETTING
        //الارقام العشرية
        public static int ACC_DecimalNumbers { get; set; }
        //طول رقم الحساب
        public static int  ACC_AccountDigits { get; set; }
        //نوع رقم الحساب
        public static int ACC_TypeAccountID { get; set; }

        //رتبة الحساب الفرعي
        public static int ACC_SubAccountRank { get; set; }

        //ادخال رقم المرجع احباري
        public static int chkACC_EnterRefrenceNoMondatry { get; set; }

        //رقم المحصل في سند القبض اجباري
        public static int chkACC_CollectorReceiptRequired { get; set; }

        //ادخال نوع السند اجباري
        public static int chkACC_BondTypeMandatory { get; set; }

        //ربط الحسابات بالمراكز
        public static int chkACC_LinkingAccountsWithCenters { get; set; }
        //ربط الحسابات بالمشاريع
        public static int chkACC_LinkingAccountsWithprojects { get; set; }
        //ربط مراكز التكلفة بالمشاريع
        public static int chkACC_LinkingCenterWithprojects { get; set; }

        //السماح بربط اكثر من بنك بحساب واجد
        public static int chkACC_AllowlinkmorebankWithoneaccount { get; set; }

        //السماح بربط اكثر من صندوق بحساب واجد
        public static int chkACC_AllowlinkmoreCashirWithoneaccount { get; set; }
        //استخدام حساب العهد
        public static int ACC_UseCovenantAccount { get; set; }

        //تسلسل العملية
        public static int cmbACC_OperationSequence { get; set; }
        //اظهار التاريخ
        public static int cmbACC_ShowDate { get; set; }
        //تسلسل طلبات قيود اليومية
        public static int cmbACC_SequenceRestrictionRequests { get; set; }

        //تسلسل طلبات سندات الصرف
        public static int cmbACC_SequenceRequestsBonds { get; set; }

        //تسلسل الاشعارات
        public static int cmbACC_NotificationSequence { get; set; }

        //تسلسل سندات الصرف
        public static int cmbACC_SequenceExchangeVouchers { get; set; }

        //تسلسل سندات القبض
        public static int cmbACC_SequenceRreceipts { get; set; }

        //أنواع الحدود 
        public static int cmbACC_TypesLimit { get; set; }
        //نوع القيد
        public static int cmbACC_RestrictionTypes { get; set; }
        //مـلاحـظــــات
        public static int txtACC_Notes { get; set; }



        //استخدام السندات المعلقة
        public static int chkACC_UseOutstandingBonds { get; set; }

        //السماح بادخال قيود تسوية فروق عملة
        public static int chkACC_AllowCurrencyDifferenceRestrictions { get; set; }


        //استخدام القيد المتكرر
        public static int chkACC_UseFrequentConstraint { get; set; }

        //ادخال المقابل المحلي للمبلغ بالاجنبي يدويا
        public static int chkACC_ManuallylocalAmountForeign { get; set; }

        //اظهار عدد الأيام في سند الصرف
        public static int chkACC_ShowDaysinVoucher { get; set; }

        //استخدام قيود التسوية
        public static int chkACC_UseSettlementRestrictions { get; set; }

        #endregion


    }
    public static class cSALSESETTING
    {
        //تثبيت المعلومات
        public static int chkFixedInformation { get; set; }
        //استخدام تعدد المخازن
        public static int chkUseMultipleStores { get; set; }
        //اظهار مواصفات الصنف
        public static int chkShowItemDescription { get; set; }
        //اظهار البيان على مستوى الصنف
        public static int chkShowDescriptionPerItem { get; set; }
        //ادخال رقم المندوب في الفاتورة اجباري
        public static int chkRepCodMandtoryInSL { get; set; }
        //ادخال رقم المنطقة في الفاتورة اجباري
        public static int chkRegionManadtoryEnteredSL { get; set; }
        //ادخال رقم المحصل في الفاتورة اجباري
        public static int chkCollectoreManadtoryEnteredSL { get; set; }
        //ادخال رقم السائق في الفاتورة إجباري
        public static int chkDriverNoManadtoryEnteredSL { get; set; }
        //ادخال رقم المرجع إجباري
        public static int chkManadtoryDetermineReferenceNo { get; set; }
        //ادخال البيان إجباري
        public static int chkEnterDescpriptionManadtory { get; set; }
        //استخدام سندات الاستلام
        public static int chkUseReceibtsVouchers { get; set; }
        //التعامل مع بضاعة الأمانات
        public static int chkUseGoodOnConsignment { get; set; }
        //عرض وحدات الصنف أليا
        public static int chkShowitemunitauto { get; set; }
        //ترحيل ايرادات المبيعات بالصافي
        public static int chkPostNetRevenueSales { get; set; }
        //جلب الكميات المتوفرة فقط من الطلب
        public static int chkFeechonlyquantitiesFoundofrequ { get; set; }
        //استخدام الخصم على مستوى الأصناف
        public static int chkUseDescPerItems { get; set; }
        //استخدام الكميات المجانية
        public static int chkUseFreeQty { get; set; }

        //عدم اظهار نسبة الصنف من الخصم أليا في الفاتورة
        public static int chkNotShowpercentageItemDiscAutomatically { get; set; }
        //عدم اظهار نسبة الصنف من المجاني أليا في الفاتورة
        public static int chkNotShowpercentageItemFreeAutomatically { get; set; }
        //استخدام الخصم في الفترات
        public static int chkUseDescPerPeriode { get; set; }
        //اظهار البيان بالمحلي والأجنبي
        public static int chkViewLocalAndForeignDexcription { get; set; }

        //استخدام الانزال للسعر والكمية
        public static int chkUseInsToPriceAndQty { get; set; }

        //استخدام الدفع في الفواتير
        public static int chkUsePaidInInvoice { get; set; }
        //مردود المبيعات الى نفس مخزن البيع
        public static int chkReturnItemsToWHCODESales { get; set; }

        //استخدام مردود البيع بدون فاتورة بيع
        public static int chkUseReturnWithOutInvoiceNo { get; set; }

        //تثبيت مستوى التسعيره للعميل في المبيعات
        public static int chkFixedLevelPriceForClient { get; set; }

        //تسديد الأقساط يدويا
        public static int chkPaidInstallmentManually { get; set; }

        //جلب الطلبات بدون أسعار
        public static int chkFillSalseOrderWithOutPrice { get; set; }

        //تجاوز كمية طلب العميل في فاتورة المبيعات
        public static int chkExceedQuantitySalesOrderInSalesInvoice { get; set; }

        //عرض الأصناف ذات الكميات المتوفرة
        public static int chkShowAvailableQtyOnly { get; set; }


        //استخدام العقود
        public static int chkUseContract { get; set; }

        //استخدام نظام البوابة
        public static int chkUseThePortalSystem { get; set; }
        //استخدام وزن الوحدة
        public static int chkUseUnitWeight { get; set; }

        //فواتير المبيعات لا تؤثر على المخزون
        public static int chkSaleInvoiceDoesnotAffectWH { get; set; }
        //جميع فواتير  المبيعات لا تؤثر على المخزون
        public static int chkAllInvoiceDoesnotAffectWH { get; set; }
        //إستخدام العقود الخاصة
        public static int chkUseSpecialContract { get; set; }
        //اظهار رقم صنف العميل
        public static int chkShowItemIDOfCustomer { get; set; }
        //استخدام نظام الخرسانة
        public static int chkUseConcreteSystem { get; set; }
        //نوع خصم الصنف

        public static int ItemDiscType { get; set; }
        //نوع خصم المبيعات
        public static int SalesDiscType { get; set; }
        //نوع منح المجاني
        public static int SalesFreeQtyType { get; set; }
        //استخدام منح المجاني حسب الفترات
        public static int chkUseFreeQtyPyPeriod { get; set; }
        //عدم اظهار المجاني في طلب العميل
        public static int chkDoesNotShowFreeQtyInOrder { get; set; }


        //تسلسل العملية
        public static int DocumentGenerations { get; set; }
        //إظهار التاريخ
        public static int DateGenerations { get; set; }

        //الربط مع الاستاذ العام
        public static int LinkWithGL { get; set; }
        //طول رقم العميل
        public static int CustomerIDDigits { get; set; }
        //نوع رقم العميل
        public static int CustomerIDType { get; set; }
        //الأرقام العشرية
        public static int DecimalPoint { get; set; }

        //تسلسل عروض الأسعار
        public static int OrderPriceSerial { get; set; }
        
        //  تسلسل طلبات العملا 
        
        public static int OrderCustomerPriceSerial { get; set; }
        //تسلسل فواتير المبيعات
        public static int SaleInvoiceSerial { get; set; }
        //تسلسل فواتير مردود المبيعات
        public static int SaleInvoiceReturnSerial { get; set; }

        //نوع نظام نقاطي
        public static int TypeOfPointSystem { get; set; }
        //طريقة احتساب النقاط
        public static int MethodCalcPoint { get; set; }
        //أقل عدد نقاط للتحويل الى مبلغ
        public static int LessPointToConvertAmount { get; set; }

        //نسبة تحويل نقاط الى مبلغ
        public static int PercentToConvertAmount { get; set; }
           

    }

}
