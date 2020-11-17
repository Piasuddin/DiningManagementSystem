import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FeeListComponent } from './components/fee-list/fee-list.component';
import { FeeSettingComponent } from './components/fee-setting/fee-setting.component';
import { FeeCollectionComponent } from './components/fee-collection/fee-collection.component';
import { StudentLedgerComponent } from './components/student-ledger/student-ledger.component';
import { DonorListComponent } from './components/donor-list/donor-list.component';
import { DonationTypeComponent } from './components/donation-type/donation-type.component';
import { DonationHeadComponent } from './components/donation-head/donation-head.component';
import { DonationCollectionComponent } from './components/donation-collection/donation-collection.component';
import { GeneralExpanseComponent } from './components/general-expanse/general-expanse.component';
import { FixedExpenseComponent } from './components/fixed-expense/fixed-expense.component';
import { PurchaseGoodsComponent } from './components/purchase-goods/purchase-goods.component';
import { ProjectExpenseComponent } from './components/project-expense/project-expense.component';
import { VouchersComponent } from './components/vouchers/vouchers.component';
import { ShowExpenseComponent } from './components/show-expense/show-expense.component';
import { ExpenseHeadComponent } from './components/expense-head/expense-head.component';
import { FixedExpenseSettingComponent } from './components/fixed-expense-setting/fixed-expense-setting.component';
import { InternalTransferComponent } from './components/internal-transfer/internal-transfer.component';
import { DonationSettingComponent } from './components/donation-setting/donation-setting.component';
import { ExternalTransferComponent } from './components/external-transfer/external-transfer.component';
import { ExternalPersonComponent } from './components/external-person/external-person.component';
import { BankTransferComponent } from './components/bank-transfer/bank-transfer.component';
import { BankAccountComponent } from './components/bank-account/bank-account.component';
import { AddNewFeeCollectionComponent } from './components/add-new-fee-collection/add-new-fee-collection.component';
import { AddNewFeeComponent } from './components/add-new-fee/add-new-fee.component';
import { AddNewFeeSettingComponent } from './components/add-new-fee-setting/add-new-fee-setting.component';
import { StudentStatementComponent } from './components/student-statement/student-statement.component';
import { AddGeneralExpensesComponent } from './components/add-general-expenses/add-general-expenses.component';
import { AddFixedExpenseComponent } from './components/add-fixed-expense/add-fixed-expense.component';
import { AddPurchaseGoodsComponent } from './components/add-purchase-goods/add-purchase-goods.component';
import { AddProjectExpenseComponent } from './components/add-project-expense/add-project-expense.component';
import { AddVoucherComponent } from './components/add-voucher/add-voucher.component';
import { AddExpenseHeadComponent } from './components/add-expense-head/add-expense-head.component';
import { AddFixedExpenseSettingComponent } from './components/add-fixed-expense-setting/add-fixed-expense-setting.component';
import { ExpenseVoucherComponent } from './components/expense-voucher/expense-voucher.component';
import { AddInternalTransferComponent } from './components/add-internal-transfer/add-internal-transfer.component';
import { AddExternalTransferComponent } from './components/add-external-transfer/add-external-transfer.component';
import { EmployeePayScaleComponent } from './components/employee-pay-scale/employee-pay-scale.component';
import { AddExternalPersonComponent } from './components/add-external-person/add-external-person.component';
import { AddBankTransferComponent } from './components/add-bank-transfer/add-bank-transfer.component';
import { AddBankComponent } from './components/add-bank/add-bank.component';
import { AddNewDonorComponent } from './components/add-new-donor/add-new-donor.component';
import { ExternalTransferDetailsComponent } from './components/external-transfer-details/external-transfer-details.component';
import { DonorDetailsComponent } from './components/donor-details/donor-details.component';
import { FixedExpenseDetailsComponent } from './components/fixed-expense-details/fixed-expense-details.component';
import { InternalTransferDetailsComponent } from './components/internal-transfer-details/internal-transfer-details.component';
import { BankAccountDetailsComponent } from './components/bank-account-details/bank-account-details.component';
import { ExternalPersonDetailsComponent } from './components/external-person-details/external-person-details.component';
import { BankTranferDetailsComponent } from './components/bank-tranfer-details/bank-tranfer-details.component';
import { GeneralExpenseDetailsComponent } from './components/general-expense-details/general-expense-details.component';
import { PurchaseGoodsDetailsComponent } from './components/purchase-goods-details/purchase-goods-details.component';
import { FixedExpenseSettingDetailsComponent } from './components/fixed-expense-setting-details/fixed-expense-setting-details.component';
import { FeeDetailsComponent } from './components/fee-details/fee-details.component';
import { FeeSettingDetailsComponent } from './components/fee-setting-details/fee-setting-details.component';
import { ExpenseHeadDetailsComponent } from './components/expense-head-details/expense-head-details.component';
import { FeeCollectionDetailsComponent } from './components/fee-collection-details/fee-collection-details.component';
import { AddNewDonationSettingComponent } from './components/add-new-donation-setting/add-new-donation-setting.component';
import { AddDonationTypeComponent } from './components/add-donation-type/add-donation-type.component';
import { AddDonationHeadComponent } from './components/add-donation-head/add-donation-head.component';
import { AddNewDonationCollectionComponent } from './components/add-new-donation-collection/add-new-donation-collection.component';
import { MobileBankingTransferComponent } from './components/mobile-banking-transfer/mobile-banking-transfer.component';
import { DonationCandidateComponent } from './components/donation-candidate/donation-candidate.component';
import { AddDonationCandidateComponent } from './components/add-donation-candidate/add-donation-candidate.component';
import { DonationCandidateDetailsComponent } from './components/donation-candidate-details/donation-candidate-details.component';
import { AddSwDonationHeadComponent } from './components/add-sw-donation-head/add-sw-donation-head.component';
import { SwDonationHeadComponent } from './components/sw-donation-head/sw-donation-head.component';
import { AddSwDonationSettingComponent } from './components/add-sw-donation-setting/add-sw-donation-setting.component';
import { SwDonationSettingComponent } from './components/sw-donation-setting/sw-donation-setting.component';
import { AddSwDonationCollectionComponent } from './components/add-sw-donation-collection/add-sw-donation-collection.component';
import { SwDonationCollectionComponent } from './components/sw-donation-collection/sw-donation-collection.component';
import { AddSwDonationAllocationComponent } from './components/add-sw-donation-allocation/add-sw-donation-allocation.component';
import { SwDonationAllocationComponent } from './components/sw-donation-allocation/sw-donation-allocation.component';
import { AddSwDonationDistributionComponent } from './components/add-sw-donation-distribution/add-sw-donation-distribution.component';
import { SwDonationDistributionComponent } from './components/sw-donation-distribution/sw-donation-distribution.component';
import { AddDiningMealComponent } from './components/add-dining-meal/add-dining-meal.component';
import { DiningMealComponent } from './components/dining-meal/dining-meal.component';
import { DiningMealDetailsComponent } from './components/dining-meal-details/dining-meal-details.component';
import { AddDiningExpenseComponent } from './components/add-dining-expense/add-dining-expense.component';
import { DiningExpenseComponent } from './components/dining-expense/dining-expense.component';
import { DiningExpenseDetailsComponent } from './components/dining-expense-details/dining-expense-details.component';
import { AddDiningBillComponent } from './components/add-dining-bill/add-dining-bill.component';
import { DiningBillComponent } from './components/dining-bill/dining-bill.component';
import { DiningBillDetailsComponent } from './components/dining-bill-details/dining-bill-details.component';
import { AddDiningBillCollectionComponent } from './components/add-dining-bill-collection/add-dining-bill-collection.component';
import { DiningBillCollectionComponent } from './components/dining-bill-collection/dining-bill-collection.component';
import { DiningBillCollectionDetailsComponent } from './components/dining-bill-collection-details/dining-bill-collection-details.component';
import { AddDiningStockManageComponent } from './components/add-dining-stock-manage/add-dining-stock-manage.component';
import { DiningStockManageComponent } from './components/dining-stock-manage/dining-stock-manage.component';
import { DiningStockManageDetailsComponent } from './components/dining-stock-manage-details/dining-stock-manage-details.component';
import { AddDiningStockProductComponent } from './components/add-dining-stock-product/add-dining-stock-product.component';
import { DiningStockProductComponent } from './components/dining-stock-product/dining-stock-product.component';
import { DiningStockProductDetailsComponent } from './components/dining-stock-product-details/dining-stock-product-details.component';
import { AddDiningMealManageComponent } from './components/add-dining-meal-manage/add-dining-meal-manage.component';
import { DiningMealManageComponent } from './components/dining-meal-manage/dining-meal-manage.component';
import { DiningBoarderDetailsComponent } from './components/dining-boarder-details/dining-boarder-details.component';
import { AddDiningBoarderComponent } from './components/add-dining-boarder/add-dining-boarder.component';
import { DiningBoarderComponent } from './components/dining-boarder/dining-boarder.component';
import { DiningStockAdjustmentComponent } from './components/dining-stock-adjustment/dining-stock-adjustment.component';
import { AddDiningStockAdjustmentComponent } from './components/add-dining-stock-adjustment/add-dining-stock-adjustment.component';
import { DiningStockAdjustmentDetailsComponent } from './components/dining-stock-adjustment-details/dining-stock-adjustment-details.component';
import { DiningBoarderStatementComponent } from './components/dining-boarder-statement/dining-boarder-statement.component';
import { DiningBoarderBillDetailsComponent } from './components/dining-boarder-bill-details/dining-boarder-bill-details.component';
import { AddInitialValueComponent } from './components/add-initial-value/add-initial-value.component';
import { InitialValueComponent } from './components/initial-value/initial-value.component';
import { JournalComponent } from './components/journal/journal.component';
import { InitialValueDetailsComponent } from './components/initial-value-details/initial-value-details.component';
import { PersonalLedgerComponent } from './components/personal-ledger/personal-ledger.component';
import { CashBookComponent } from './components/cash-book/cash-book.component';
import { PurchasedProductComponent } from './components/purchased-product/purchased-product.component';
import { AddPurchasedProductComponent } from './components/add-purchased-product/add-purchased-product.component';
import { PurchasedProductDetailsComponent } from './components/purchased-product-details/purchased-product-details.component';
import { RouteGuradWithAccessRight } from '../merp-common/services/route-guard-with-access-right.service';
import { EmployeeSalaryPaymentComponent } from './components/employee-salary-payment/employee-salary-payment.component';
import { AddEmployeeSalaryAllowancePaymentComponent } from './components/add-employee-salary-allowance-payment/add-employee-salary-allowance-payment.component';
import { EmployeeSalaryPaymentDetailsComponent } from './components/employee-salary-payment-details/employee-salary-payment-details.component';
import { FeeDeductionDetailsComponent } from './components/fee-deduction-details/fee-deduction-details.component';
import { FeeDeductionComponent } from './components/fee-deduction/fee-deduction.component';
import { AddFeeDeductionComponent } from './components/add-fee-deduction/add-fee-deduction.component';
import { FeeAdditionDetailsComponent } from './components/fee-addition-details/fee-addition-details.component';
import { FeeAdditionComponent } from './components/fee-addition/fee-addition.component';
import { AddFeeAdditionComponent } from './components/add-fee-addition/add-fee-addition.component';
import { MobileBankingAccountComponent } from './components/mobile-banking-account/mobile-banking-account.component';
import { AddMobileBankingAccountComponent } from './components/add-mobile-banking-account/add-mobile-banking-account.component';
import { MobileBankingAccountDetailsComponent } from './components/mobile-banking-account-details/mobile-banking-account-details.component';
import { AddMobileBankingTransferComponent } from './components/add-mobile-banking-transfer/add-mobile-banking-transfer.component';
import { ExpenseVoucherDetailsComponent } from './components/expense-voucher-details/expense-voucher-details.component';
import { DonationCollectionDetailsComponent } from './components/donation-collection-details/donation-collection-details.component';
import { DiningMealManageDetailsComponent } from './components/dining-meal-manage-details/dining-meal-manage-details.component';
import { MobileBankingTransferDetailsComponent } from './components/mobile-banking-transfer-details/mobile-banking-transfer-details.component';
import { SupplierCompanyComponent } from './components/supplier-company/supplier-company.component';
import { AddSupplierCompanyComponent } from './components/add-supplier-company/add-supplier-company.component';
import { SupplierCompanyDetailsComponent } from './components/supplier-company-details/supplier-company-details.component';
import { DonationSettingDetailsComponent } from './components/donation-setting-details/donation-setting-details.component';
import { DonationHeadDetailsComponent } from './components/donation-head-details/donation-head-details.component';
import { SwDonationAllocationDetailsComponent } from './components/sw-donation-allocation-details/sw-donation-allocation-details.component';
import { SwDonationHeadDetailsComponent } from './components/sw-donation-head-details/sw-donation-head-details.component';
import { SwDonationSettingDetailsComponent } from './components/sw-donation-setting-details/sw-donation-setting-details.component';
import { SwDonationDistributionDetailsComponent } from './components/sw-donation-distribution-details/sw-donation-distribution-details.component';
import { SwDonationCollectionDetailsComponent } from './components/sw-donation-collection-details/sw-donation-collection-details.component';
import { GeneralStockProductComponent } from './components/general-stock-product/general-stock-product.component';
import { AddGeneralStockProductComponent } from './components/add-general-stock-product/add-general-stock-product.component';
import { GeneralStockProductDetailsComponent } from './components/general-stock-product-details/general-stock-product-details.component';
import { AddDiningMealAttendanceComponent } from './components/add-dining-meal-attendance/add-dining-meal-attendance.component';
import { DiningMealAttendanceComponent } from './components/dining-meal-attendance/dining-meal-attendance.component';
import { DiningMealAttendanceDetailsComponent } from './components/dining-meal-attendance-details/dining-meal-attendance-details.component';
import { ExternalTransferStatementComponent } from './components/external-transfer-statement/external-transfer-statement.component';
import { DiningExpenseStatementComponent } from './components/dining-expense-statement/dining-expense-statement.component';
import { FeeSettingNotApplicableForWhomComponent } from './components/fee-setting-not-applicable-for-whom/fee-setting-not-applicable-for-whom.component';

const routes: Routes = [
  { path: '', component: FeeListComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 162 } },
  { path: "feeSetting", component: FeeSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 162 } },
  { path: "feeCollection", component: FeeCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 159 } },
  { path: "feeCollectionDetails/:id", component: FeeCollectionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 160 } },
  { path: "addNewFee", component: AddNewFeeComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 106 } },
  { path: "addNewFee/:id", component: AddNewFeeComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 106 } },
  { path: "addNewFeeSetting", component: AddNewFeeSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 108 } },
  { path: "addNewFeeSetting/:id", component: AddNewFeeSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 108 } },
  { path: "DetailsFeeSetting/:id", component: FeeSettingDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 164 } },
  { path: "AddBankAccount", component: AddBankComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 80 } },
  { path: "AddBankAccount/:id", component: AddBankComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 80 } },
  { path: "AddBankTransfer", component: AddBankTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 81 } },
  { path: "AddBankTransfer/:id", component: AddBankTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 81 } },
  { path: "addDiningBill", component: AddDiningBillComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 82 } },
  { path: "addDiningBill/:id", component: AddDiningBillComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 82 } },
  { path: "addDiningBillCollection", component: AddDiningBillCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 83 } },
  { path: "addDiningBillCollection/:id", component: AddDiningBillCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 83 } },
  { path: "addDiningBoarder", component: AddDiningBoarderComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 84 } },
  { path: "addDiningBoarder/:id", component: AddDiningBoarderComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 84 } },
  { path: "addDiningExpense", component: AddDiningExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 85 } },
  { path: "addDiningExpense/:id", component: AddDiningExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 85 } },
  { path: "addDiningMeal", component: AddDiningMealComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 86 } },
  { path: "addDiningMeal/:id", component: AddDiningMealComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 86 } },
  { path: "addDiningMealManage", component: AddDiningMealManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 87 } },
  { path: "addDiningMealManage/:id", component: AddDiningMealManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 87 } },
  { path: "addDiningMealAttendance", component: AddDiningMealAttendanceComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 268 } },
  { path: "addDiningMealAttendance/:id", component: AddDiningMealAttendanceComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 268 } },
  { path: "addDiningStockAdjustment", component: AddDiningStockAdjustmentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 88 } },
  { path: "addDiningStockAdjustment/:id", component: AddDiningStockAdjustmentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 88 } },
  { path: "addDiningStockManage", component: AddDiningStockManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 89 } },
  { path: "addDiningStockManage/:id", component: AddDiningStockManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 89 } },
  { path: "addDiningStockProduct", component: AddDiningStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 90 } },
  { path: "addDiningStockProduct/:id", component: AddDiningStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 90 } },
  { path: "addDonationCandidate", component: AddDonationCandidateComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 91 } },
  { path: "addDonationCandidate/:id", component: AddDonationCandidateComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 91 } },
  { path: "addDonationCollection", component: AddNewDonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 103 } },
  { path: "addDonationCollection/:id", component: AddNewDonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 103 } },
  { path: "addDonationHead", component: AddDonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 92 } },
  { path: "addDonationHead/:id", component: AddDonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 92 } },
  { path: "addDonationSetting", component: AddNewDonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 104 } },
  { path: "addDonationSetting/:id", component: AddNewDonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 104 } },
  { path: "addDonationType", component: AddDonationTypeComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 93 } },
  { path: "addDonationType/:id", component: AddDonationTypeComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 93 } },
  { path: "addDonor", component: AddNewDonorComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 105 } },
  { path: "addDonor/:id", component: AddNewDonorComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 105 } },
  { path: "addExpanseHead", component: AddExpenseHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 94 } },
  { path: "addExpanseHead/:id", component: AddExpenseHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 94 } },
  { path: "addExpenseItem/:id", component: ExpenseVoucherComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 154 } },
  { path: "AddExternalPerson", component: AddExternalPersonComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 95 } },
  { path: "AddExternalPerson/:id", component: AddExternalPersonComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 95 } },
  { path: "addExternalTransfer", component: AddExternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 96 } },
  { path: "addExternalTransfer/:id", component: AddExternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 96 } },
  { path: "addFixedExpanse", component: AddFixedExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 97 } },
  { path: "addFixedExpanse/:id", component: AddFixedExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 97 } },
  { path: "addFixedExpanseSetting", component: AddFixedExpenseSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 98 } },
  { path: "addFixedExpanseSetting/:id", component: AddFixedExpenseSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 98 } },
  { path: "addGeneralExpanse", component: AddGeneralExpensesComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 99 } },
  { path: "addGeneralExpanse/:id", component: AddGeneralExpensesComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 99 } },
  { path: "addInitialValue", component: AddInitialValueComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 100 } },
  { path: "addInitialValue/:id", component: AddInitialValueComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 100 } },
  { path: "addInternalTransfer", component: AddInternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 101 } },
  { path: "addInternalTransfer/:id", component: AddInternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 101 } },
  { path: "addNewFeeCollection", component: AddNewFeeCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 107 } },
  { path: "addNewFeeCollection/:id", component: AddNewFeeCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 107 } },
  { path: "addProjectExpanse", component: AddProjectExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 109 } },
  { path: "addPurchasedProduct", component: AddPurchasedProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 240 } },
  { path: "addPurchasedProduct/:id", component: AddPurchasedProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 240 } },
  { path: "addPurchaseGoods", component: AddPurchaseGoodsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 110 } },
  { path: "addPurchaseGoods/:id", component: AddPurchaseGoodsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 110 } },
  { path: "addSwDonationAllocation", component: AddSwDonationAllocationComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 112 } },
  { path: "addSwDonationAllocation/:id", component: AddSwDonationAllocationComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 112 } },
  { path: "addSwDonationCollection", component: AddSwDonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 113 } },
  { path: "addSwDonationCollection/:id", component: AddSwDonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 113 } },
  { path: "addSwDonationDistribution", component: AddSwDonationDistributionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 114 } },
  { path: "addSwDonationDistribution/:id", component: AddSwDonationDistributionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 114 } },
  { path: "addSwDonationHead", component: AddSwDonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 115 } },
  { path: "addSwDonationHead/:id", component: AddSwDonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 115 } },
  { path: "addSwDonationSetting", component: AddSwDonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 116 } },
  { path: "addSwDonationSetting/:id", component: AddSwDonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 116 } },
  { path: "bankAccount", component: BankAccountComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 118 } },
  { path: "bankAccountDetails/:id", component: BankAccountDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 119 } },
  { path: "bankTransfer", component: BankTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 121 } },
  { path: "bankTransferDetails/:id", component: BankTranferDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 120 } },
  { path: "cashBook", component: CashBookComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 122 } },
  { path: "createVoucher", component: AddVoucherComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 117 } },
  { path: "createVoucher/:id", component: AddVoucherComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 117 } },
  { path: "diningBill", component: DiningBillComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 124 } },
  { path: "diningBillCollection", component: DiningBillCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 125 } },
  { path: "diningBillCollectionDetails/:id", component: DiningBillCollectionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 126 } },
  { path: "diningBillDetails/:id", component: DiningBillDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 127 } },
  { path: "diningBoarder", component: DiningBoarderComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 128 } },
  { path: "diningBoarderBillDetails", component: DiningBoarderBillDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 129 } },
  { path: "diningBoarderDetails/:id", component: DiningBoarderDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 130 } },
  { path: "diningBoarderStatement", component: DiningBoarderStatementComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 131 } },
  { path: "diningExpense", component: DiningExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 132 } },
  { path: "diningExpenseDetails/:id", component: DiningExpenseDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 133 } },
  { path: "diningMeal", component: DiningMealComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 134 } },
  { path: "diningMealDetails/:id", component: DiningMealDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 135 } },
  { path: "diningMealManage", component: DiningMealManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 136 } },
  { path: "diningMealAttendance", component: DiningMealAttendanceComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 123 } },
  { path: "diningStockAdjustment", component: DiningStockAdjustmentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 137 } },
  { path: "diningStockAdjustmentDetails/:id", component: DiningStockAdjustmentDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 138 } },
  { path: "diningStockManage", component: DiningStockManageComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 139 } },
  { path: "diningStockManageDetails/:id", component: DiningStockManageDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 140 } },
  { path: "diningStockProduct", component: DiningStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 141 } },
  { path: "diningStockProductDetails/:id", component: DiningStockProductDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 142 } },
  { path: "donationCandidate", component: DonationCandidateComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 143 } },
  { path: "donationCandidateDetails/:id", component: DonationCandidateDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 144 } },
  { path: "donationCollection", component: DonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 145 } },
  { path: "donationHead", component: DonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 146 } },
  { path: "donationSetting", component: DonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 147 } },
  { path: "donationType", component: DonationTypeComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 148 } },
  { path: "donor", component: DonorListComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 150 } },
  { path: "donorDetails/:id", component: DonorDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 149 } },
  { path: "expanseHead", component: ExpenseHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 152 } },
  { path: "expanseHeadDetails/:id", component: ExpenseHeadDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 153 } },
  { path: "externalPerson", component: ExternalPersonComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 155 } },
  { path: "externalPersonDetails/:id", component: ExternalPersonDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 156 } },
  { path: "externalTransfer", component: ExternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 157 } },
  { path: "externalTransferDetails/:id", component: ExternalTransferDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 158 } },
  { path: "fixedExpanse", component: FixedExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 165 } },
  { path: "fixedExpanseDetails/:id", component: FixedExpenseDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 166 } },
  { path: "fixedExpanseSetting", component: FixedExpenseSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 167 } },
  { path: "fixedExpanseSettingDetails/:id", component: FixedExpenseSettingDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 168 } },
  { path: "generalExpanse", component: GeneralExpanseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 169 } },
  { path: "generalExpanseDetails/:id", component: GeneralExpenseDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 170 } },
  { path: "initialValue", component: InitialValueComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 171 } },
  { path: "initialValueDetails/:id", component: InitialValueDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 172 } },
  { path: "internalTransfer", component: InternalTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 173 } },
  { path: "internalTransferDetails/:id", component: InternalTransferDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 174 } },
  { path: "journal", component: JournalComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 175 } },
  { path: "mobileBankTransfer", component: MobileBankingTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 176 } },
  { path: "personalLedger", component: PersonalLedgerComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 178 } },
  { path: "projectExpanse", component: ProjectExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 179 } },
  { path: "purchasedProduct", component: PurchasedProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 241 } },
  { path: "purchasedProductDetails/:id", component: PurchasedProductDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 242 } },
  { path: "purchaseGoods", component: PurchaseGoodsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 180 } },
  { path: "purchaseGoodsDetails/:id", component: PurchaseGoodsDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 181 } },
  { path: "SalaryPayScale", component: EmployeePayScaleComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 151 } },
  { path: "showExpanses", component: ShowExpenseComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 182 } },
  { path: "studentLedger", component: StudentLedgerComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 183 } },
  { path: "studentStatement", component: StudentStatementComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 184 } },
  { path: "swDonationAllocation", component: SwDonationAllocationComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 185 } },
  { path: "swDonationCollection", component: SwDonationCollectionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 186 } },
  { path: "swDonationDistribution", component: SwDonationDistributionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 187 } },
  { path: "swDonationHead", component: SwDonationHeadComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 188 } },
  { path: "swDonationSetting", component: SwDonationSettingComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 189 } },
  { path: "voucher", component: VouchersComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 190 } },
  { path: "voucherDetails/:id", component: ExpenseVoucherDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 190 } },
  { path: "feeDetails/:id", component: FeeDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 161 } },
  { path: "salaryPayment", component: EmployeeSalaryPaymentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 229 } },
  { path: "addSalaryPayment/:id", component: AddEmployeeSalaryAllowancePaymentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 201 } },
  { path: "salaryPaymentDetails/:id", component: EmployeeSalaryPaymentDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 230 } },
  { path: "addSalaryPayment", component: AddEmployeeSalaryAllowancePaymentComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 201 } },
  { path: "feeDeductionDetails/:id", component: FeeDeductionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 250 } },
  { path: "feeDeduction", component: FeeDeductionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 248 } },
  { path: "addFeeDeduction", component: AddFeeDeductionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 249 } },
  { path: "feeAdditionDetails/:id", component: FeeAdditionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 253 } },
  { path: "feeAddition", component: FeeAdditionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 251 } },
  { path: "mobileBankingAccount", component: MobileBankingAccountComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 254 } },
  { path: "addMobileBankingAccount", component: AddMobileBankingAccountComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 255 } },
  { path: "addMobileBankingAccount/:id", component: AddMobileBankingAccountComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 255 } },
  { path: "mobileBankingAccountDetails/:id", component: MobileBankingAccountDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 256 } },
  { path: "addMobileBankingTransfer", component: AddMobileBankingTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 102 } },
  { path: "addMobileBankingTransfer/:id", component: AddMobileBankingTransferComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 102 } },
  { path: "addFeeAddition", component: AddFeeAdditionComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 251 } },
  { path: "donationCollectionDetails/:id", component: DonationCollectionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 258 } },
  { path: "diningMealManageDetails/:id", component: DiningMealManageDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 269 } },
  { path: "diningMealAttendanceDetails/:id", component: DiningMealAttendanceDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 270 } },
  { path: "mobileBankTransferDetails/:id", component: MobileBankingTransferDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 176 } },
  { path: "supplierCompany", component: SupplierCompanyComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 261 } },
  { path: "addSupplierCompany", component: AddSupplierCompanyComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 260 } },
  { path: "addSupplierCompany/:id", component: AddSupplierCompanyComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 260 } },
  { path: "supplierCompanyDetails/:id", component: SupplierCompanyDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 262 } },
  { path: "donationSettingDetails/:id", component: DonationSettingDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 147 } },
  { path: "donationHeadDetails/:id", component: DonationHeadDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 146 } },
  { path: "swDonationAlocationDetails/:id", component: SwDonationAllocationDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 185 } },
  { path: "swDonationHeadDetails/:id", component: SwDonationHeadDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 188 } },
  { path: "swDonationSettingDetails/:id", component: SwDonationSettingDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 189 } },
  { path: "swDonationDistributionDetails/:id", component: SwDonationDistributionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 187 } },
  { path: "swDonationCollectionDetails/:id", component: SwDonationCollectionDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 186 } },
  { path: "generalStockProduct", component: GeneralStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 266 } },
  { path: "addGeneralStockProduct", component: AddGeneralStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 265 } },
  { path: "addGeneralStockProduct/:id", component: AddGeneralStockProductComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 265 } },
  { path: "generalStockProductDetails/:id", component: GeneralStockProductDetailsComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 267 } },
  { path: "externalTransferStatement", component: ExternalTransferStatementComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 272 } },
  { path: "diningExpenseStatement/:id", component: DiningExpenseStatementComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 273 } },
  { path: "notApplicableForWhom/:id", component: FeeSettingNotApplicableForWhomComponent, canActivate: [RouteGuradWithAccessRight], data: { formId: 274 } }
]


@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [
    RouterModule
  ]
})
export class FinanceRoutingModule { }
