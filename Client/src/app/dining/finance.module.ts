import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinanceRoutingModule } from './finance-routing.module';
import { FeeListComponent } from './components/fee-list/fee-list.component';
import { FeeSettingComponent } from './components/fee-setting/fee-setting.component';
import { AddBankComponent } from './components/add-bank/add-bank.component';
import { AddBankTransferComponent } from './components/add-bank-transfer/add-bank-transfer.component';
import { AddDonationHeadComponent } from './components/add-donation-head/add-donation-head.component';
import { AddDonationTypeComponent } from './components/add-donation-type/add-donation-type.component';
import { AddExpenseHeadComponent } from './components/add-expense-head/add-expense-head.component';
import { AddExternalPersonComponent } from './components/add-external-person/add-external-person.component';
import { AddExternalTransferComponent } from './components/add-external-transfer/add-external-transfer.component';
import { AddFixedExpenseComponent } from './components/add-fixed-expense/add-fixed-expense.component';
import { AddFixedExpenseSettingComponent } from './components/add-fixed-expense-setting/add-fixed-expense-setting.component';
import { AddGeneralExpensesComponent } from './components/add-general-expenses/add-general-expenses.component';
import { AddInternalTransferComponent } from './components/add-internal-transfer/add-internal-transfer.component';
import { AddNewDonationCollectionComponent } from './components/add-new-donation-collection/add-new-donation-collection.component';
import { AddNewDonationSettingComponent } from './components/add-new-donation-setting/add-new-donation-setting.component';
import { AddNewDonorComponent } from './components/add-new-donor/add-new-donor.component';
import { AddNewFeeCollectionComponent } from './components/add-new-fee-collection/add-new-fee-collection.component';
import { AddNewFeeComponent } from './components/add-new-fee/add-new-fee.component';
import { AddNewFeeSettingComponent } from './components/add-new-fee-setting/add-new-fee-setting.component';
import { AddProjectExpenseComponent } from './components/add-project-expense/add-project-expense.component';
import { AddPurchaseGoodsComponent } from './components/add-purchase-goods/add-purchase-goods.component';
import { AddVoucherComponent } from './components/add-voucher/add-voucher.component';
import { BankAccountComponent } from './components/bank-account/bank-account.component';
import { BankTransferComponent } from './components/bank-transfer/bank-transfer.component';
import { DonationHeadComponent } from './components/donation-head/donation-head.component';
import { DonationTypeComponent } from './components/donation-type/donation-type.component';
import { DonorListComponent } from './components/donor-list/donor-list.component';
import { ExpenseVoucherComponent } from './components/expense-voucher/expense-voucher.component';
import { ExternalPersonComponent } from './components/external-person/external-person.component';
import { ExternalTransferComponent } from './components/external-transfer/external-transfer.component';
import { FixedExpenseComponent } from './components/fixed-expense/fixed-expense.component';
import { GeneralExpanseComponent } from './components/general-expanse/general-expanse.component';
import { InternalTransferComponent } from './components/internal-transfer/internal-transfer.component';
import { ProjectExpenseComponent } from './components/project-expense/project-expense.component';
import { PurchaseGoodsComponent } from './components/purchase-goods/purchase-goods.component';
import { ShowExpenseComponent } from './components/show-expense/show-expense.component';
import { StudentStatementComponent } from './components/student-statement/student-statement.component';
import { VouchersComponent } from './components/vouchers/vouchers.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FeeCollectionComponent } from './components/fee-collection/fee-collection.component';
import { StudentLedgerComponent } from './components/student-ledger/student-ledger.component';
import { DonationCollectionComponent } from './components/donation-collection/donation-collection.component';
import { ExpenseHeadComponent } from './components/expense-head/expense-head.component';
import { FixedExpenseSettingComponent } from './components/fixed-expense-setting/fixed-expense-setting.component';
import { DonationSettingComponent } from './components/donation-setting/donation-setting.component';
import { MaterialModule } from '../material/material.module';
import { AppCommonModule } from '../components/app-common.module';
import { EmployeePayScaleComponent } from './components/employee-pay-scale/employee-pay-scale.component';
import { ExternalTransferDetailsComponent } from './components/external-transfer-details/external-transfer-details.component';
import { DonorDetailsComponent } from './components/donor-details/donor-details.component';
import { FixedExpenseDetailsComponent } from './components/fixed-expense-details/fixed-expense-details.component';
import { InternalTransferDetailsComponent } from './components/internal-transfer-details/internal-transfer-details.component';
import { BankAccountDetailsComponent } from './components/bank-account-details/bank-account-details.component';
import { ExternalPersonDetailsComponent } from './components/external-person-details/external-person-details.component';
import { FeeDetailsComponent } from './components/fee-details/fee-details.component';
import { FeeSettingDetailsComponent } from './components/fee-setting-details/fee-setting-details.component';
import { BankTranferDetailsComponent } from './components/bank-tranfer-details/bank-tranfer-details.component';
import { GeneralExpenseDetailsComponent } from './components/general-expense-details/general-expense-details.component';
import { PurchaseGoodsDetailsComponent } from './components/purchase-goods-details/purchase-goods-details.component';
import { FixedExpenseSettingDetailsComponent } from './components/fixed-expense-setting-details/fixed-expense-setting-details.component';
import { ExpenseHeadDetailsComponent } from './components/expense-head-details/expense-head-details.component';
import { FeeCollectionDetailsComponent } from './components/fee-collection-details/fee-collection-details.component';
import { MobileBankingTransferComponent } from './components/mobile-banking-transfer/mobile-banking-transfer.component';
import { AddMobileBankingTransferComponent } from './components/add-mobile-banking-transfer/add-mobile-banking-transfer.component';
import { MobileBankingTransferDetailsComponent } from './components/mobile-banking-transfer-details/mobile-banking-transfer-details.component';
import { DonationCandidateComponent } from './components/donation-candidate/donation-candidate.component';
import { AddDonationCandidateComponent } from './components/add-donation-candidate/add-donation-candidate.component';
import { DonationCandidateDetailsComponent } from './components/donation-candidate-details/donation-candidate-details.component';
import { SwDonationHeadComponent } from './components/sw-donation-head/sw-donation-head.component';
import { AddSwDonationHeadComponent } from './components/add-sw-donation-head/add-sw-donation-head.component';
import { AddSwDonationSettingComponent } from './components/add-sw-donation-setting/add-sw-donation-setting.component';
import { SwDonationSettingComponent } from './components/sw-donation-setting/sw-donation-setting.component';
import { AddSwDonationCollectionComponent } from './components/add-sw-donation-collection/add-sw-donation-collection.component';
import { SwDonationCollectionComponent } from './components/sw-donation-collection/sw-donation-collection.component';
import { SwDonationAllocationComponent } from './components/sw-donation-allocation/sw-donation-allocation.component';
import { AddSwDonationAllocationComponent } from './components/add-sw-donation-allocation/add-sw-donation-allocation.component';
import { AddSwDonationDistributionComponent } from './components/add-sw-donation-distribution/add-sw-donation-distribution.component';
import { SwDonationDistributionComponent } from './components/sw-donation-distribution/sw-donation-distribution.component';
import { AddDiningMealComponent } from './components/add-dining-meal/add-dining-meal.component';
import { DiningMealDetailsComponent } from './components/dining-meal-details/dining-meal-details.component';
import { DiningMealComponent } from './components/dining-meal/dining-meal.component';
import { AddDiningExpenseComponent } from './components/add-dining-expense/add-dining-expense.component';
import { DiningExpenseComponent } from './components/dining-expense/dining-expense.component';
import { DiningExpenseDetailsComponent } from './components/dining-expense-details/dining-expense-details.component';
import { DiningBillDetailsComponent } from './components/dining-bill-details/dining-bill-details.component';
import { DiningBillComponent } from './components/dining-bill/dining-bill.component';
import { AddDiningBillComponent } from './components/add-dining-bill/add-dining-bill.component';
import { AddDiningBillCollectionComponent } from './components/add-dining-bill-collection/add-dining-bill-collection.component';
import { DiningBillCollectionComponent } from './components/dining-bill-collection/dining-bill-collection.component';
import { DiningBillCollectionDetailsComponent } from './components/dining-bill-collection-details/dining-bill-collection-details.component';
import { DiningStockManageComponent } from './components/dining-stock-manage/dining-stock-manage.component';
import { AddDiningStockManageComponent } from './components/add-dining-stock-manage/add-dining-stock-manage.component';
import { DiningStockManageDetailsComponent } from './components/dining-stock-manage-details/dining-stock-manage-details.component';
import { DiningStockProductDetailsComponent } from './components/dining-stock-product-details/dining-stock-product-details.component';
import { DiningStockProductComponent } from './components/dining-stock-product/dining-stock-product.component';
import { AddDiningStockProductComponent } from './components/add-dining-stock-product/add-dining-stock-product.component';
import { DiningMealManageComponent } from './components/dining-meal-manage/dining-meal-manage.component';
import { AddDiningMealManageComponent } from './components/add-dining-meal-manage/add-dining-meal-manage.component';
import { AddDiningBoarderComponent } from './components/add-dining-boarder/add-dining-boarder.component';
import { DiningBoarderComponent } from './components/dining-boarder/dining-boarder.component';
import { DiningBoarderDetailsComponent } from './components/dining-boarder-details/dining-boarder-details.component';
import { DiningStockAdjustmentComponent } from './components/dining-stock-adjustment/dining-stock-adjustment.component';
import { AddDiningStockAdjustmentComponent } from './components/add-dining-stock-adjustment/add-dining-stock-adjustment.component';
import { DiningStockAdjustmentDetailsComponent } from './components/dining-stock-adjustment-details/dining-stock-adjustment-details.component';
import { DiningBoarderBillDetailsComponent } from './components/dining-boarder-bill-details/dining-boarder-bill-details.component';
import { DiningBoarderStatementComponent } from './components/dining-boarder-statement/dining-boarder-statement.component';
import { InitialValueComponent } from './components/initial-value/initial-value.component';
import { AddInitialValueComponent } from './components/add-initial-value/add-initial-value.component';
import { InitialValueDetailsComponent } from './components/initial-value-details/initial-value-details.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { PdfPreviewComponent } from '../components/pdf-preview/pdf-preview.component';
import { JournalComponent } from './components/journal/journal.component';
import { PersonalLedgerComponent } from './components/personal-ledger/personal-ledger.component';
import { CashBookComponent } from './components/cash-book/cash-book.component';
import { EndDate } from '../validators/end-date-validator.directive';
import { AddPurchasedProductComponent } from './components/add-purchased-product/add-purchased-product.component';
import { PurchasedProductComponent } from './components/purchased-product/purchased-product.component';
import { PurchasedProductDetailsComponent } from './components/purchased-product-details/purchased-product-details.component';
import { EmployeeSalaryPaymentComponent } from './components/employee-salary-payment/employee-salary-payment.component';
import { AddEmployeeSalaryAllowancePaymentComponent } from './components/add-employee-salary-allowance-payment/add-employee-salary-allowance-payment.component';
import { EmployeeSalaryPaymentDetailsComponent } from './components/employee-salary-payment-details/employee-salary-payment-details.component';
import { MAT_MOMENT_DATE_FORMATS } from '@angular/material-moment-adapter';
import { MomentUtcDateAdapter } from '../merp-common/services/moment-utc-date-adapter.service';
import { FeeDeductionComponent } from './components/fee-deduction/fee-deduction.component';
import { AddFeeDeductionComponent } from './components/add-fee-deduction/add-fee-deduction.component';
import { FeeDeductionDetailsComponent } from './components/fee-deduction-details/fee-deduction-details.component';
import { FeeAdditionComponent } from './components/fee-addition/fee-addition.component';
import { AddFeeAdditionComponent } from './components/add-fee-addition/add-fee-addition.component';
import { FeeAdditionDetailsComponent } from './components/fee-addition-details/fee-addition-details.component';
import { AddMobileBankingAccountComponent } from './components/add-mobile-banking-account/add-mobile-banking-account.component';
import { MobileBankingAccountComponent } from './components/mobile-banking-account/mobile-banking-account.component';
import { MobileBankingAccountDetailsComponent } from './components/mobile-banking-account-details/mobile-banking-account-details.component';
import { ExpenseVoucherDetailsComponent } from './components/expense-voucher-details/expense-voucher-details.component';
import { DonationCollectionDetailsComponent } from './components/donation-collection-details/donation-collection-details.component';
import { DiningMealManageDetailsComponent } from './components/dining-meal-manage-details/dining-meal-manage-details.component';
import { AddSupplierCompanyComponent } from './components/add-supplier-company/add-supplier-company.component';
import { SupplierCompanyComponent } from './components/supplier-company/supplier-company.component';
import { SupplierCompanyDetailsComponent } from './components/supplier-company-details/supplier-company-details.component';
import { DonationSettingDetailsComponent } from './components/donation-setting-details/donation-setting-details.component';
import { DonationHeadDetailsComponent } from './components/donation-head-details/donation-head-details.component';
import { SwDonationAllocationDetailsComponent } from './components/sw-donation-allocation-details/sw-donation-allocation-details.component';
import { SwDonationHeadDetailsComponent } from './components/sw-donation-head-details/sw-donation-head-details.component';
import { SwDonationSettingDetailsComponent } from './components/sw-donation-setting-details/sw-donation-setting-details.component';
import { SwDonationDistributionDetailsComponent } from './components/sw-donation-distribution-details/sw-donation-distribution-details.component';
import { SwDonationCollectionDetailsComponent } from './components/sw-donation-collection-details/sw-donation-collection-details.component';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { GeneralStockProductComponent } from './components/general-stock-product/general-stock-product.component';
import { GeneralStockProductDetailsComponent } from './components/general-stock-product-details/general-stock-product-details.component';
import { AddGeneralStockProductComponent } from './components/add-general-stock-product/add-general-stock-product.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { HttpLoaderFactory } from '../app.module';
import { HttpClient } from '@angular/common/http';
import { DiningMealAttendanceComponent } from './components/dining-meal-attendance/dining-meal-attendance.component';
import { AddDiningMealAttendanceComponent } from './components/add-dining-meal-attendance/add-dining-meal-attendance.component';
import { DiningMealAttendanceDetailsComponent } from './components/dining-meal-attendance-details/dining-meal-attendance-details.component';
import { ExternalTransferStatementComponent } from './components/external-transfer-statement/external-transfer-statement.component';
import { DiningExpenseStatementComponent } from './components/dining-expense-statement/dining-expense-statement.component';
import { FeeSettingNotApplicableForWhomComponent } from './components/fee-setting-not-applicable-for-whom/fee-setting-not-applicable-for-whom.component';


@NgModule({
  declarations: [
    FeeListComponent,
    FeeSettingComponent,
    AddBankComponent,
    FeeCollectionComponent,
    StudentLedgerComponent,
    DonationCollectionComponent,
    ExpenseHeadComponent,
    FixedExpenseSettingComponent,
    DonationSettingComponent,
    AddBankTransferComponent,
    AddDonationHeadComponent,
    AddDonationTypeComponent,
    AddExpenseHeadComponent,
    AddExternalPersonComponent,
    AddExternalTransferComponent,
    AddFixedExpenseComponent,
    AddFixedExpenseSettingComponent,
    AddGeneralExpensesComponent,
    AddInternalTransferComponent,
    AddNewDonationCollectionComponent,
    AddNewDonationSettingComponent, 
    AddNewDonorComponent,
    AddNewFeeCollectionComponent,
    AddNewFeeComponent,
    AddNewFeeSettingComponent,
    AddProjectExpenseComponent,
    AddPurchaseGoodsComponent,
    AddVoucherComponent,
    BankAccountComponent,
    BankTransferComponent,
    DonationHeadComponent,
    DonationTypeComponent,
    DonorListComponent,
    ExpenseVoucherComponent,
    ExternalPersonComponent,
    ExternalTransferComponent,
    FixedExpenseComponent,
    GeneralExpanseComponent,
    InternalTransferComponent,
    ProjectExpenseComponent,
    PurchaseGoodsComponent,
    ShowExpenseComponent,
    StudentStatementComponent,
    VouchersComponent,
    EmployeePayScaleComponent,
    ExternalTransferDetailsComponent,
    DonorDetailsComponent,
    FixedExpenseDetailsComponent,
    InternalTransferDetailsComponent,
    BankAccountDetailsComponent,
    ExternalPersonDetailsComponent,
    FeeDetailsComponent,
    FeeSettingDetailsComponent,
    BankTranferDetailsComponent,
    GeneralExpenseDetailsComponent,
    PurchaseGoodsDetailsComponent,
    FixedExpenseSettingDetailsComponent,
    ExpenseHeadDetailsComponent,
    FeeCollectionDetailsComponent,
    MobileBankingTransferComponent,
    AddMobileBankingTransferComponent,
    MobileBankingTransferDetailsComponent,
    DonationCandidateComponent,
    AddDonationCandidateComponent,
    DonationCandidateDetailsComponent,
    SwDonationHeadComponent,
    AddSwDonationHeadComponent,
    AddSwDonationSettingComponent,
    SwDonationSettingComponent,
    AddSwDonationCollectionComponent,
    SwDonationCollectionComponent,
    SwDonationAllocationComponent,
    AddSwDonationAllocationComponent,
    AddSwDonationDistributionComponent,
    SwDonationDistributionComponent,
    AddDiningMealComponent,
    DiningMealDetailsComponent,
    DiningMealComponent,
    DiningMealManageComponent,
    AddDiningMealManageComponent,
    DiningMealComponent,
    AddDiningExpenseComponent,
    DiningExpenseComponent,
    DiningExpenseDetailsComponent,
    DiningBillDetailsComponent,
    DiningBillComponent,
    AddDiningBillComponent,
    AddDiningBillCollectionComponent,
    DiningBillCollectionComponent,
    DiningBillCollectionDetailsComponent,
    DiningStockManageComponent,
    AddDiningStockManageComponent,
    DiningStockManageDetailsComponent,
    DiningStockProductDetailsComponent,
    DiningStockProductComponent,
    AddDiningStockProductComponent,
    AddDiningBoarderComponent,
    DiningBoarderComponent,
    DiningBoarderDetailsComponent,
    DiningStockAdjustmentComponent,
    AddDiningStockAdjustmentComponent,
    DiningStockAdjustmentDetailsComponent,
    DiningBoarderBillDetailsComponent,
    DiningBoarderStatementComponent,
    InitialValueComponent,
    AddInitialValueComponent,
    InitialValueDetailsComponent,
    JournalComponent,
    PersonalLedgerComponent,
    CashBookComponent,
    EndDate,
    AddPurchasedProductComponent,
    PurchasedProductComponent,
    PurchasedProductDetailsComponent,    
    EmployeeSalaryPaymentComponent,
    AddEmployeeSalaryAllowancePaymentComponent,
    EmployeeSalaryPaymentDetailsComponent,
    FeeDeductionComponent,
    AddFeeDeductionComponent,
    FeeDeductionDetailsComponent,
    FeeAdditionComponent,
    AddFeeAdditionComponent,
    FeeAdditionDetailsComponent,
    AddMobileBankingAccountComponent,
    MobileBankingAccountComponent,
    MobileBankingAccountDetailsComponent,
    ExpenseVoucherDetailsComponent,
    DonationCollectionDetailsComponent,
    DiningMealManageDetailsComponent,
    AddSupplierCompanyComponent,
    SupplierCompanyComponent,
    SupplierCompanyDetailsComponent,
    DonationSettingDetailsComponent,
    DonationHeadDetailsComponent,
    SwDonationAllocationDetailsComponent,
    SwDonationHeadDetailsComponent,
    SwDonationSettingDetailsComponent,
    SwDonationDistributionDetailsComponent,
    SwDonationCollectionDetailsComponent,
    GeneralStockProductComponent,
    GeneralStockProductDetailsComponent,
    AddGeneralStockProductComponent,
    DiningMealAttendanceComponent,
    AddDiningMealAttendanceComponent,
    DiningMealAttendanceDetailsComponent,
    ExternalTransferStatementComponent,
    DiningExpenseStatementComponent,
    FeeSettingNotApplicableForWhomComponent,
  ],
  imports: [
    AppCommonModule,
    CommonModule,
    FinanceRoutingModule,
    FormsModule,
    MaterialModule,
    ReactiveFormsModule,
    PdfViewerModule,
    TranslateModule.forRoot({
      loader: {
          provide: TranslateLoader,
          useFactory: HttpLoaderFactory,
          deps: [HttpClient]
      },
      defaultLanguage: 'en'
  })
  ],
  entryComponents:[
    PdfPreviewComponent
  ],
  providers:[
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
    { provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS },
    { provide: DateAdapter, useClass: MomentUtcDateAdapter }
  ]
})
export class FinanceModule { }
