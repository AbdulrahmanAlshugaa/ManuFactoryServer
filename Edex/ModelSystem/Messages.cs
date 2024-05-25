using DevExpress.XtraEditors;
using Edex.Model.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Edex.ModelSystem
{
    public static class Messages
    {
       /// <summary>
       /// // These static properties represent the messages displayed to the user in different scenarios
       /// </summary>
        #region Declare  
        public static string msgNoPermissionToUseScreen { get; set; } // message for when the user lacks permission to use a screen
        public static string msgNoPermissionToChange { get; set; } // message for when the user lacks permission to make changes
        public static string msgSelectRecordToDeleteIt { get; set; } // message prompt for when the user selects a record to delete
        public static string msgDeleteComplete { get; set; } // message for when a record has been successfully deleted
        public static string msgDeleteQuestion { get; set; } // message prompt for asking the user's confirmation to delete a record
        public static string msgSaveComplete { get; set; } // message for when a record has been successfully saved
        public static string msgSaveQuestion { get; set; } // message prompt for asking the user's confirmation to save a record
        public static string msgWrongUserName { get; set; } // message for when the entered username is incorrect
        public static string msgWrongPassword { get; set; } // message for when the entered password is incorrect
        public static string msgNotActiveUser { get; set; }// Message displayed when the user is not active
        public static string msgShouldCompleteData { get; set; }  // Message displayed when data is incomplete
        public static string msgShouldSaveDataThenMoveToOtherRecord { get; set; }  // Message displayed when data should be saved before moving to another record
        public static string msgNoRecordFoundWithThisNumber { get; set; }   // Message displayed when no record is found with a given number
        public static string msgNoPermissionToAddNewRecord { get; set; }// Message displayed when the user does not have permission to add a new record
        public static string msgNoPermissionToDeleteRecord { get; set; }  // Message displayed when the user does not have permission to delete a record
        public static string msgNoPermissionToUpdateRecord { get; set; }// Message displayed when the user does not have permission to update a record
        public static string msgNoPermissionToViewRecord { get; set; } // Message displayed when the user does not have permission to view a record
        public static string msgNoRecordsWereFound { get; set; }    // Message displayed when no records are found
        public static string msgExecutedBy { get; set; } // Message displayed to indicate who executed a particular action
        public static string msgEditedBy { get; set; }   // Message displayed with the name of the user the record was edited by
        public static string msgThisRecordIsRelatedWithOtherTables { get; set; }// Message displayed when a record is related to other tables
        public static string msgThisNameIsExist { get; set; }// Message displayed when a name already exists
        public static string msgDoTransfer { get; set; } // Message displayed when a transfer  be executed
        public static string msgDoCancelTransfer { get; set; } // Message displayed when a transfer  be cancelled
        public static string msgDuplicatedAccount { get; set; }// Message displayed when an account already exists
        public static string msgConfirmDelete { get; set; }// Message displayed to confirm deletion of a record
        public static string msgVoucherTransfer { get; set; } // Message displayed when a voucher transfer   be executed
        public static string msgCancelVoucherTransfer { get; set; }// Message displayed when a voucher transfer   be cancelled.
        public static string msgYouShouldSaveDataBeforePrinting { get; set; }  //Message that indicates you should save data before printing.
        public static string msgYouShouldSaveDataBeforeTry { get; set; }  //Message that indicates you should save data before Try .
        public static string msgNoPermissionToPrintReport { get; set; } // Message that indicates the user has no permission to print the report.
        public static string msgNoPermissionToExportReport { get; set; }//Message that indicates the user has no permission to export the report.
        public static string msgAddPrintersToTheReport { get; set; } // Message that indicates the user should add printers to the report.
        public static string msgDayaAllowedToEditExpired { get; set; }  //Message that indicates the user can only edit expired data on the current day.
        public static string msgYouCanNotEnterSameNumberTwice { get; set; } //Message that indicates the user cannot enter the same number twice.
        public static string msgAccountClosedHasExpired { get; set; }//Message that indicates the user's account is closed and has expired.
        public static string msgConfirmUpdate { get; set; }// Message that confirms an update.
        public static string msgInputIsRequired { get; set; }// Message that indicates the input is required.
        public static string msgThereIsErrorInput { get; set; }// Message that indicates there is an error with the input.
        public static string msgThereIsNoRecordInput { get; set; }// Message that indicates there is no record for the input.
        public static string msgInputIsGreaterThanZero { get; set; }  // Message that indicates the input is greater than zero.
        // Title properties
        public static string TitleInfo { get; set; } // holds the title for information messages
        public static string TitleWorning { get; set; } // holds the title for warning messages
        public static string TitleConfirm { get; set; } // holds the title for confirmation messages
        public static string TitleError { get; set; } // holds the title for error messages
        // Message properties
        public static string msgInputShouldBeNumber { get; set; } // Message indicating that the input should be a number.
        public static string msgNotAllowedPercentDiscount { get; set; }//Message indicating that a percentage discount is not allowed.
        public static string msgNoFoundSizeForItem { get; set; }// Message indicating that no size was found for the item.
        public static string msgNoFoundThisBarCode { get; set; }// Message indicating that no barcode was found for the item.
        public static string msgNoFoundThisAccountID { get; set; } //Message indicating that no account ID was found for the item.
        public static string msgErrorSave { get; set; }// Message indicating that an error occurred while saving data.
        public static string msgThereIsNotPrinterSelected { get; set; }//Message indicating that no printer has been selected.
        public static string msgEnterRegistrationNo { get; set; } //Message indicating that a registration number must be entered.
        public static string msgWorningSaveDuplicateRegistrationNo { get; set; } // Warning Message indicating that the registration number already exists.
        public static string msgWorningThisUnitIsStop { get; set; }// Warning message indicating that the unit is stopped
        public static string msgNoFoundThisItem { get; set; }  // Message indicating that the item was not found.
        public static string msgCanNotChoseSameAccount { get; set; }// Message indicating that the same account cannot be chosen.
        public static string msgWorningSaveDuplicateBarcode { get; set; }//Warning message indicating that the barcode being saved is a duplicate.
        public static string msgCanNotChoseSameStore { get; set; }  // Message indicating that the same store cannot be chosen.
        public static string msgInputEmil { get; set; }//Message indicating that an email be entered.
        public static string msgNumStounOutNumStonLosGreterThenNumStonin { get; set; }
        public static string msgNumStounOutGreterThanNumStonin { get; set; }
        public static string msgWeightStounOutGreterThanWeightStonin { get; set; }
        public static string msgWeightStounOutWeightStonLosGreterThenWeightStonin { get; set; }
        public static string msgMustEnterTypeCurrency { get; set; }
        public static string msgMustEnterTransPrice { get; set; }
        public static string msgAccountNotHaveMachine { get; set; }
        public static string msgAccountIsStope { get; set; }
        public static string msgAccountMaxLimit { get; set; }
        public static string msgAccountMaxLimitSaveOrNot { get; set; }
        public static string msgQtyisNotAvilable { get; set; }
        public static string msgNotFoundAnyQtyInStore { get; set; }
        public static string msgNotSelectShowInDetilsOrder { get; set; }
        //
        public static string msgDontRepetTheOrderinMoreCommend { get; set; }
        public static string msgTheQTyinOrderisExceed { get; set; }
        public static string msgTheDateGreaterCurrntDate { get; set; }
        public static string msgTheOrderAlreadyExists { get; set; }
        public static string msgTheProcessIsNotUpdateBecuseIsPosted { get; set; }

        #endregion
        #region Function
        /// <summary>
        /// // This method initializes the Messages To  properties.
        /// </summary>
        /// <param name="Language"></param>
        /// 
        public static void initialization(iLanguage Language)
        {
            if (Language == iLanguage.Arabic)
            {
                msgMustEnterTypeCurrency = "يجب ادخال نوع العملة";
                msgMustEnterTransPrice = "يجب ادخال سعر الصرف";
                msgNoPermissionToUseScreen = " لا يوجد لديك صلاحية الدخول الى الشاشة  ";
                msgWorningSaveDuplicateBarcode = "لا يمكن الحفظ لانه رقم الباركود موجود مسبقا";
                msgCanNotChoseSameAccount = "لايمكن اختيار نفس الحساب";
                msgNoFoundThisItem = "لايوجد هذه الصنف ";
                msgWorningSaveDuplicateRegistrationNo = "لايمكن الحفظ رقم القيد موجد مسبقا ";
                msgEnterRegistrationNo = "يجب ادخال رقم القيد";
                msgThereIsNotPrinterSelected = "لايوجد طابعة تم تحديده من شاشة تحديد الطابعات";
                msgErrorSave = "لم يتم حفظ البيانات لقد حدث خطاء اثناء الحفظ ";
                msgNoPermissionToChange = "لايوجد لديك صلاحية تغير";
                msgNoFoundThisAccountID = "لا يوجد رقم هذه الحساب";
                msgNoFoundThisBarCode = "لايوجد هذا الباركود";
                msgNoFoundSizeForItem = "الصنف ليس لديه الوحدة المدخله";
                msgNotAllowedPercentDiscount = "نسبة الخصم التي ادخلتها غير مسموح لك";
                msgThereIsNoRecordInput = "لايمكن الحفظ .. لايوجد سجلات مدخله";
                msgThereIsErrorInput = "هناك اخطاء في إدخال البيانات";
                msgInputIsGreaterThanZero = "الرقم يجب ان يكون أكبر من الصفر";
                msgInputIsRequired = "لايمكن ان يكون الحقل فارغ";
                msgAccountClosedHasExpired = "تم إغلاق الحساب - لقد انتهت الفتره المجانيه";
                msgSelectRecordToDeleteIt = "الرجاء تحديد سجل ليتم حذفه";
                msgDeleteComplete = "لقد تم حذف السجل بنجاح";
                msgDeleteQuestion = "هل تريد حذف السجل ؟";
                msgSaveComplete = "تم حفظ السجل بنجاح";
                msgSaveQuestion = "هل تريد حفظ السجل ؟";
                msgWrongUserName = "هذا الرمز غير موجود ، أعد المحاولة";
                msgWrongPassword = "كلمة السر خاطئة ، أعد المحاولة";
                msgNotActiveUser = "هذا المستخدم غير مفعل،الرجاء الإتصال بمدير الصلاحيات لإرجاع الصلاحيات الخاصة بك";
                msgShouldCompleteData = "يجب عليك إكمال البيانات قبل عملية الحفظ";
                msgShouldSaveDataThenMoveToOtherRecord = "يجب عليك حفظ البيانات المدخلة أو إلغاؤها قبل الإنتقال إلى السجل التالي";
                msgNoRecordFoundWithThisNumber = "لا يوجد سجل بهذا الرقم ، للوصول للسجل المطلوب الرجاء الاستعانة بأيقونة البحث";
                msgNoPermissionToAddNewRecord = "لا توجد لديك صلاحية لإضافة سجل جديد" + " " + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية الإضافة";
                msgNoPermissionToDeleteRecord = "لا توجد لديك صلاحية لحذف سجل" + " " + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية الحذف";
                msgNoPermissionToUpdateRecord = "لا توجد لديك صلاحية لتعديل السجل" + " " + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية التعديل";
                msgNoPermissionToViewRecord = "لا توجد لديك صلاحية لتصفح السجلات" + " " + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية التصفح";
                msgNoRecordsWereFound = "لا يوجد سجلات لهذا الاستعلام";
                msgExecutedBy = "تم إنشاء هذا السجل بوساطة ";
                msgEditedBy = "وتم تعديل هذا السجل بوساطة ";
                msgThisRecordIsRelatedWithOtherTables = "لا يمكنك حذف هذا السجل ، لأنه مرتبط بعمليات داخل جداول أخرى";
                msgThisNameIsExist = "لقد تم إدخال هذا الاسم من قبل ، الرجاء إدخال اسم آخر";
                msgDoTransfer = "تم ترحيل السند بنجاح";
                msgDoCancelTransfer = "تم الغاء ترحيل السند بنجاح";
                msgDuplicatedAccount = "لا يمكن ان يتكرر رقم حساب اكثر من مره في القيد المحاسبي الواحد";
                msgConfirmDelete = "هل تريد بالفعل حذف السـند";
                msgVoucherTransfer = "هل تريد بالفعل ترحيل السـند";
                msgCancelVoucherTransfer = "هل تريد بالفعل إلغاء ترحيل السـند";
                msgYouShouldSaveDataBeforePrinting = "يجب حفظ البيانات قبل عملية الطباعة";
                msgYouShouldSaveDataBeforeTry = "يجب حفظ البيانات قبل اجراء هذه العملية";
                msgNoPermissionToPrintReport = "لا توجد لديك صلاحية للطباعة" + "" + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية الطباعة";
                msgNoPermissionToExportReport = "لا توجد لديك صلاحية للتصـدير" + "" + "الرجاء مراجعة مسؤول صلاحيات البرنامج لإعطائك صلاحية التصدير";
                msgAddPrintersToTheReport = "لم يتم تحديد أي طابعة لهذا التقرير ، الرجاء تحديد الطابعة المطلوبة من شاشة تحديد طابعات التقارير";
                msgDayaAllowedToEditExpired = "انتهت عدد الايام المسموحة للتعديل";
                msgConfirmUpdate = "هل تريد بالفعل التعديل السـند";
                TitleInfo = "معلومة - أومكس المحاسبي";
                TitleWorning = "رسالة تنبيه - أومكس المحاسبي  ";
                TitleConfirm = "رسالة تأكيد - أومكس المحاسبي  ";
                TitleError = "رسالة خطأ - أومكس المحاسبي  ";
                msgInputShouldBeNumber = " !! يجب ان يكون المدخل رقم وليس نص";
                msgWorningThisUnitIsStop = "هذه الوحدة تم ايقافها ولايمكن استخدامها ";
                msgCanNotChoseSameStore = " !! لا يمكن ان يتم اختيار نفس المخزن  ";
                msgInputEmil = "الإيميل غير صحيح";
                msgNumStounOutNumStonLosGreterThenNumStonin = "لا يمكن أن يكون عدد الاحجار المرجعة + عدد الاحجار الفاقدة أكبر من المستلمة  ";
               msgNumStounOutGreterThanNumStonin= "لا يمكن أن يكون عدد الاحجار المرجعة أكبر من المستلمة  ";
               msgWeightStounOutGreterThanWeightStonin = "لا يمكن ان يكون وزن الاحجار المرجعة أكبر من وزن الاحجار المستلمه ";
                msgNoPermissionToUseScreen = "No Permation  ";
                msgWeightStounOutWeightStonLosGreterThenWeightStonin = "لا يمكن ان يكون وزن الاحجار المرجعة + وزن الاحجار الفاقد اكبر من الاحجار المستلمة";
                msgNumStounLostGreterThanNumStonin = "لا يمكن ان يكون عدد الاحجار الفاقد أكبر من عدد الاحجار المستلمة ";
                msgWeightStounLostGreterThanWeightStonin = "لا يمكن ان يكون وزن الاحجار الفاقد اكبر من وزن الاحجار المستلمة";
                msgAccountNotHaveMachine = "لا يوجد مكينة مرتبطة بهذا الحساب ";
                msgAccountIsStope = "هذا الحساب تم إيقافة ولا يمكن إستخدامة ! ";
                msgAccountMaxLimit = "لا يمكن تجاوز الحد الأعلى المحدد للحساب :";
                msgAccountMaxLimitSaveOrNot = "الحساب تجاوز الحد الاعلى المحدد له ... هل تريد متابعة الحفظ؟";
                msgDontRepetTheOrderinMoreCommend = " الطلبية التي قمت بإختيارها تمتلك أمر سابق .. لذلك لايمكن عمل أمر أخر لنفس الطلبية";
                msgQtyisNotAvilable = "الكمية المدخلة غير متوفرة في المخزن.. الرجاء اعادة ادخال كمية متوفرة,, لان الكمية المتوفرة هي :";
                msgNotFoundAnyQtyInStore = "لا يتوفر اي كمية للصنف الحالي في المستودع ";
                msgNotSelectShowInDetilsOrder = " الصنف لا يمتلك خاصية الظهور في تفاصيل الطلبيات  لذلك لا يمكن تفعيل الخيار ";
                msgTheQTyinOrderisExceed = "الكميات المحدد  للصنف في الأمرالحالي تتعدى الكمية المتبقية للصنف في المخزن ";

                msgTheDateGreaterCurrntDate = "  التاريخ المدخل اكبر من التاريخ الحالي .. الرجاء ادخال تاريخ صحيح ";
                msgTheOrderAlreadyExists = "الرجاء الانتباه رقم الطلبية المدخل لدية امر سابق ";
                msgTheProcessIsNotUpdateBecuseIsPosted = "لا يمكن تعديل  حالة الترحيل للعملية  من حالة اعلى الى حالة أدنى علما بأن الترتيب كالتالي : 1-معلق 2-غير مرحل 3- مرحل ";

            }
            else
            {

                msgAccountNotHaveMachine = "There is no machine associated with this account";
                msgMustEnterTypeCurrency = "Must Enter Type Currency";
                msgMustEnterTransPrice = "Must Enter Type Price ";
                msgWorningSaveDuplicateBarcode = "You can not save there is Duplicate Barcode";
                msgCanNotChoseSameAccount = "you Can not Chose Same Account";
                msgNoFoundThisItem = "this item is not found";
                msgWorningThisUnitIsStop = "This Unit Is Stop You Can not Use It";
                msgWorningSaveDuplicateRegistrationNo = "Can Not Savr Registration No is already Existing";
                msgEnterRegistrationNo = "Enter Registration No";
                msgThereIsNotPrinterSelected = "There Is Not Printer Selected From Selector Printers From";
                msgErrorSave = "There is Error in Save Date";
                msgNoPermissionToChange = "you Do not have Permission to Change";
                msgYouShouldSaveDataBeforeTry = "You must save the data before performing this operation.";
                msgNoFoundThisAccountID = "This Account ID  No Found";
                msgNoFoundThisBarCode = "This BarCode No Found";
                msgNoFoundSizeForItem = "This Size No Found For Item";
                msgNotAllowedPercentDiscount = "The discount rate you entered is not allowed";
                msgThereIsNoRecordInput = "Can not save .. No records entry";
                msgThereIsErrorInput = "There Is Error Input";
                msgInputIsGreaterThanZero = "The number should be greater than zero";
                msgInputIsRequired = "Field can not be empty";
                msgAccountClosedHasExpired = "Account closed - The free period has expired";
                msgSelectRecordToDeleteIt = "Select Record To Delete It";
                msgDeleteComplete = "Complete Deleting Record";
                msgDeleteQuestion = "Are You Sure You Want Delete This Record ?";
                msgSaveComplete = "Complete Saving Record";
                msgSaveQuestion = "Are You Sure You Want Save This Record ?";
                msgWrongUserName = "Wrong code , Try Again";
                msgWrongPassword = "Wrong Password , Try Again";
                msgNotActiveUser = "This User Is Not Active,Please Contact To The Administrator To Return Your Access";
                msgShouldCompleteData = "You Should Complete The Data Before Saving It";
                msgShouldSaveDataThenMoveToOtherRecord = "You Should Save Or Delete The Data Before Move To Other Record";
                msgNoRecordFoundWithThisNumber = "No Record Found With This Number" + "" + " To Search For The Correct Number Please Make Search";
                msgNoPermissionToAddNewRecord = "Permission Denied , You Can Not Add New Record " + "" + "Please Contact With An Admin To Give You Add Permission";
                msgNoPermissionToDeleteRecord = "Permission Denied , You Can Not Delete The Record " + "" + "Please Contact With An Admin To Give You Delete Permission";
                msgNoPermissionToUpdateRecord = "Permission Denied , You Can Not Update The Record " + "" + "Please Contact With An Admin To Give You Update Permission";
                msgNoPermissionToViewRecord = "Permission Denied , You Can Not View Records " + "" + "Please Contact With An Admin To Give You View Permission";
                msgYouCanNotEnterSameNumberTwice = "You Can Not Enter The Same Number Twice";
                msgNoRecordsWereFound = "No Records Were Found To View It To Report";
                msgThisRecordIsRelatedWithOtherTables = "You Can Not Delete This Record , Because Its Related With Other Tables";
                msgThisNameIsExist = "This Name Was Entered Before, Please Type Another Name";
                msgDoTransfer = "The Voucher Posted Successfuly";
                msgDoCancelTransfer = "The Voucher UnPosted Successfuly";
                msgDuplicatedAccount = "The Account ID  cant duplicated more than one time in the same voucher";
                msgConfirmDelete = "Are you sure you want to delete";
                msgVoucherTransfer = "Are you Sure";
                msgCancelVoucherTransfer = "Are you Sure";
                msgYouShouldSaveDataBeforePrinting = "You Should Save Data Before Printing";
                msgNoPermissionToPrintReport = "Permission Denied , You Can Not Print The Report " + "" + "Please Contact With An Admin To Give You Print Permission";
                msgNoPermissionToExportReport = "Permission Denied , You Can Not Export The Report " + "" + "Please Contact With An Admin To Give You Export Permission";
                msgAddPrintersToTheReport = "There Is No Printer For This Report, Please Select Printer For The Report From Selected Reports Printers Screen";
                msgDayaAllowedToEditExpired = "The Daya Allowed To Edit has been Expired";
                msgConfirmUpdate = "Are you sure you want to Update";
                msgInputShouldBeNumber = "input Should Be Number Not text !!";
                TitleInfo = "Info - Omex Accounting";
                TitleWorning = "Worning - Omex Accounting";
                TitleConfirm = "Confirm - Omex Accounting";
                TitleError = "Error - Omex Accounting";
                msgCanNotChoseSameStore = " !! Can Not Chose Same Store  ";
                msgInputEmil = "Email is Error";
                msgNumStounOutNumStonLosGreterThenNumStonin = "The number of stones returned + the number of lost stones cannot be greater than the number of stones received ";
                msgNumStounOutGreterThanNumStonin = "The number of stones returned cannot be greater than the number received";
                msgWeightStounOutGreterThanWeightStonin = "The weight of the stones returned cannot be greater than the weight of the stones received";
                msgWeightStounOutWeightStonLosGreterThenWeightStonin = "The weight of the returned stones + the weight of the lost stones cannot be greater than the stones received";
                msgNumStounLostGreterThanNumStonin = "The number of stones lost cannot be greater than the number of stones received";
                msgWeightStounLostGreterThanWeightStonin = "The weight of the lost stones cannot be greater than the weight of the stones received";
                msgAccountIsStope = "This Account is Stope, Can't Use ! ";
                msgAccountMaxLimit = "The maximum limit specified for the account cannot be exceeded:";
                msgAccountMaxLimitSaveOrNot = "The account has exceeded the maximum limit... Do you want to continue saving?";
                msgDontRepetTheOrderinMoreCommend = "The order you have selected has a previous Command";
                msgQtyisNotAvilable = "The quantity entered is not available in stock. Please re-enter an available quantity.";
                msgNotFoundAnyQtyInStore = "There are no quantities for the current item in stock";

                msgNotSelectShowInDetilsOrder = " The item does not have the feature to appear in order details, so the option cannot be activated..";
                msgTheQTyinOrderisExceed = "The quantities specified for the item in the current order exceed the remaining quantity of the item in stock ";
                msgTheDateGreaterCurrntDate = "  The entered date is greater than the current date. Please enter a valid date ";
                msgTheOrderAlreadyExists = "Please note that the order number entered has a previous order.";
                msgTheProcessIsNotUpdateBecuseIsPosted = "It is not possible to modify the migration status of the process from a higher state to a lower state, noting that the order is as follows: 1-Pending 2-Not Posted 3-Posted";
            }
        }
        /// <summary>
        ///  // This method displays a message box with an exclamation icon
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        public static void MsgExclamationk(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// // This method displays a message box with an asterisk icon
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        public static void MsgAsterisk(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        /// <summary>
        /// // This method displays a message box with an Hand icon
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        public static void MsgHand(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        /// <summary>
        /// // This method displays a message box with an Error icon
       /// </summary>
       /// <param name="Title"></param>
       /// <param name="Content"></param>
        public static void MsgError(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        /// <summary>
        /// // This method displays a message box with an Stop icon
      /// </summary>
      /// <param name="Title"></param>
      /// <param name="Content"></param>
        public static void MsgStop(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
        /// <summary>
        /// // This method displays a message box with an Information icon
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        public static void MsgInfo(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
       /// <summary>
       ///  This method displays a message box with an asterisk None
       /// </summary>
       /// <param name="Title"></param>
       /// <param name="Content"></param>
        public static void MsgNone(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.None);
        }
       /// <summary>
       /// This method displays a message box with an Warning icon
       /// </summary>
       /// <param name="Title"></param>
       /// <param name="Content"></param>
        public static void MsgWarning(string Title, string Content)
        {
            XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        /********************************YesNO***********************************************/
        /// <summary>
     /// This method displays a message box with an Question icon,Yes or No
     /// </summary>
     /// <param name="Title"></param>
     /// <param name="Content"></param>
     /// <returns></returns>
        public static bool MsgQuestionYesNo(string Title, string Content)
        {

            DialogResult result = XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }
        /// <summary>
        /// This method displays a message box with an Warning icon,Yes or No
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static bool MsgWarningYesNo(string Title, string Content)
        {
            DialogResult result = XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }
        /// <summary>
        /// /// This method displays a message box with an Stop icon,Yes or No
       /// </summary>
       /// <param name="Title"></param>
       /// <param name="Content"></param>
       /// <returns></returns>
        public static bool MsgStopYesNo(string Title, string Content)
        {
            DialogResult result = XtraMessageBox.Show(Content.Trim(), Title.Trim(), MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
            if (result == DialogResult.Yes)
                return true;
            else
                return false;
        }
        #endregion








        public static string msgNumStounLostGreterThanNumStonin { get; set; }

        public static string msgWeightStounLostGreterThanWeightStonin { get; set; }
    }
}
