using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using MERP.EntityFramworkDAL.Repository;

namespace MERP.FinanceBLL
{
    public class DiningStockAdjustmentBLL
    {
        private readonly IRepository<DiningStockManage> diningStockManageRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<StockProduct> diningStockProductRepository;
        private readonly IRepository<DiningStockAdjustment> diningStockAdjustmentRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly DiningBillBLL diningBillBLL;
        private readonly IRepository<DiningMealAttendance> diningMealAttendanceRepository;
        private readonly IRepository<DiningExpense> diningExpenseRepository;
        private readonly ExpenseVoucherBLL expenseVoucherBLL;
        private readonly IRepository<DiningStockAdjustmentDetails> diningStockAdjustmentDetailsRepository;

        public DiningStockAdjustmentBLL(IRepository<DiningStockManage> diningStockManageRepository,
            IRepository<User> userRepository, IRepository<StockProduct> diningStockProductRepository,
            IRepository<DiningStockAdjustment> diningStockAdjustmentRepository, IRepository<Institute>
            instituteRepository, IRepository<Campus> campusRepository, DiningBillBLL diningBillBLL,
            IRepository<DiningMealAttendance> diningMealAttendanceRepository, IRepository<DiningExpense> diningExpenseRepository,
            ExpenseVoucherBLL expenseVoucherBLL, IRepository<DiningStockAdjustmentDetails> diningStockAdjustmentDetailsRepository)
        {
            this.diningStockManageRepository = diningStockManageRepository;
            this.userRepository = userRepository;
            this.diningStockProductRepository = diningStockProductRepository;
            this.diningStockAdjustmentRepository = diningStockAdjustmentRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.diningBillBLL = diningBillBLL;
            this.diningMealAttendanceRepository = diningMealAttendanceRepository;
            this.diningExpenseRepository = diningExpenseRepository;
            this.expenseVoucherBLL = expenseVoucherBLL;
            this.diningStockAdjustmentDetailsRepository = diningStockAdjustmentDetailsRepository;
        }
        public async Task<ResponseMessage> CreateDiningStockAdjustment(List<DiningStockAdjustmentAddDataViewModel> models,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                if (models.Count > 0)
                {
                    List<DiningStockAdjustmentDetails> toAddAdjustmentDetails = new List<DiningStockAdjustmentDetails>();

                    DiningStockAdjustment toUpdateAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(x =>
                         x.Status == (byte)DiningStockAdjustmentStatus.Running && x.CampusId == models.First().CampusId
                         && x.InstituteId == models.First().InstituteId);
                    CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningStockAdjustment>
                                    (await diningStockAdjustmentRepository.GetAsync(), token);

                    DiningStockAdjustment toAddAdjustment = new DiningStockAdjustment
                    {
                        Id = codeAndId.Id + 1,
                        AdjustmentForMonth = models.First().AdjustmentForMonth,
                        CampusId = (long)models.First().CampusId,
                        InstituteId = (long)models.First().InstituteId,
                        CreatedBy = token.UserID,
                        CreatedDate = DateTime.UtcNow,
                        DiningStockAdjustmentNo = codeAndId.No,
                        Status = (byte)DiningStockAdjustmentStatus.Created
                    };

                    long adjustmentDetailsId = await IdGenerator.GenerateChildClassIdAsync<DiningStockAdjustmentDetails>
                                    (await diningStockAdjustmentDetailsRepository.GetAsync());

                    foreach (DiningStockAdjustmentAddDataViewModel model in models)
                    {
                        if (model.InStock > 0)
                        {
                            DiningStockAdjustmentDetails diningStockAdjustmentDetails = new DiningStockAdjustmentDetails()
                            {
                                Id = adjustmentDetailsId + 1,
                                DiningStockAdjustmentId = toAddAdjustment.Id,
                                ProductId = model.ProductId,
                                Price = model.InStock * model.Rate,
                                Rate = model.Rate,
                                RemainingQty = model.InStock,
                            };
                            adjustmentDetailsId++;
                            toAddAdjustmentDetails.Add(diningStockAdjustmentDetails);
                        }
                    }
                    using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        DiningStockAdjustment result = new DiningStockAdjustment();
                        if (toUpdateAdjustment != null)
                        {
                            toUpdateAdjustment.Status = (byte)DiningStockAdjustmentStatus.Closed;
                            result = await diningStockAdjustmentRepository.UpdateAsync(toUpdateAdjustment);
                        }
                        result = await diningStockAdjustmentRepository.AddAsync(toAddAdjustment);

                        if (result != null)
                        {
                            await diningStockAdjustmentDetailsRepository.AddRangeAsync(toAddAdjustmentDetails);
                            transaction.Complete();
                            response.ResponseObj = result;
                            response.Message = "Dining Stock Adjustment Created Successfully";
                            response.StatusCode = (byte)StatusCode.Success;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Stock Manage! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningStockAdjustment(List<DiningStockAdjustmentAddDataViewModel> models,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository
                    .GetAsync(models.First().DiningStockAdjustmentId);
                if (diningStockAdjustment != null)
                {
                    List<DiningStockAdjustmentDetails> addedData = new List<DiningStockAdjustmentDetails>();
                    List<DiningStockAdjustmentDetails> updatedData = new List<DiningStockAdjustmentDetails>();
                    List<DiningStockAdjustmentDetails> deletedData = new List<DiningStockAdjustmentDetails>();
                    long adjustmentDetailsId = await IdGenerator.GenerateChildClassIdAsync<DiningStockAdjustmentDetails>
                                         (await diningStockAdjustmentDetailsRepository.GetAsync());
                    foreach (DiningStockAdjustmentAddDataViewModel model in models)
                    {
                        if (model.InStock > 0)
                        {
                            if (model.Id > 0)
                            {
                                DiningStockAdjustmentDetails adjustmentDetails = await diningStockAdjustmentDetailsRepository.GetAsync(model.Id);
                                if (adjustmentDetails != null)
                                {
                                    adjustmentDetails.Price = model.InStock * model.Rate;
                                    adjustmentDetails.RemainingQty = model.InStock;
                                    updatedData.Add(adjustmentDetails);
                                }
                            }
                            else
                            {
                                DiningStockAdjustmentDetails diningStockAdjustmentDetails = new DiningStockAdjustmentDetails()
                                {
                                    Id = adjustmentDetailsId + 1,
                                    DiningStockAdjustmentId = diningStockAdjustment.Id,
                                    ProductId = model.ProductId,
                                    Price = model.InStock * model.Rate,
                                    Rate = model.Rate,
                                    RemainingQty = model.InStock,
                                };
                                adjustmentDetailsId++;
                                addedData.Add(diningStockAdjustmentDetails);
                            }
                        }
                        else
                        {
                            if (model.Id > 0)
                            {
                                DiningStockAdjustmentDetails adjustmentDetails = await diningStockAdjustmentDetailsRepository.GetAsync(model.Id);
                                if (adjustmentDetails != null)
                                {
                                    deletedData.Add(adjustmentDetails);
                                }
                            }
                        }
                    }
                    await diningStockAdjustmentDetailsRepository.AddRangeAsync(addedData);
                    await diningStockAdjustmentDetailsRepository.UpdateRangeAsync(updatedData);
                    await diningStockAdjustmentDetailsRepository.DeleteRangeAsync(deletedData);

                    response.Message = "Dining Stock Adjustment Updated Successfully With ID: " + diningStockAdjustment
                        .DiningStockAdjustmentNo;
                    response.StatusCode = (byte)StatusCode.Success;

                }
                else
                {
                    response.Message = "Not found any record to update";
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Stock Adjustment! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningStockAdjustment(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.GetAsync(id);
                    if (diningStockAdjustment != null && DataController.IsReturnable(diningStockAdjustment, token))
                    {
                        if (diningStockAdjustment.Status == (byte)DiningStockAdjustmentStatus.Created)
                        {
                            List<DiningStockAdjustmentDetails> diningStockAdjustmentDetails = await diningStockAdjustmentDetailsRepository
                                .FilterListAsync(x => x.DiningStockAdjustmentId == diningStockAdjustment.Id);
                            await diningStockAdjustmentRepository.DeleteAsync(diningStockAdjustment);
                            await diningStockAdjustmentDetailsRepository.DeleteRangeAsync(diningStockAdjustmentDetails);
                            response.Message = "Dining Stock Adjustment Deleted Succesfully with id: " + diningStockAdjustment.DiningStockAdjustmentNo;
                            response.StatusCode = (byte)StatusCode.Success;
                            response.ResponseObj = diningStockAdjustment;
                        }
                        else
                        {
                            response.Message = "You can't delete this record.";
                            response.StatusCode = (byte)StatusCode.Failed;
                        }
                    }
                    else
                    {
                        response.Message = "Not found any Dining Stock Adjustment";
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> searchDiningStockAdjustmentForTable(ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                List<DiningStockAdjustment> data = await DataController.GetDataList<DiningStockAdjustment>(token);
                List<DiningStockAdjustmentTableDataViewModel> diningStockAdjustmentTableDataViewModels = new List<DiningStockAdjustmentTableDataViewModel>();

                if (data.Any())
                {
                    foreach (var diningStockAdjustment in data)
                    {
                        var details = await diningStockAdjustmentDetailsRepository.FilterListAsync(x => x.DiningStockAdjustmentId ==
                            diningStockAdjustment.Id);
                        DiningStockAdjustmentTableDataViewModel diningStockAdjustmentTableDataViewModel = new DiningStockAdjustmentTableDataViewModel
                        {
                            Id = diningStockAdjustment.Id,
                            Month = Formater.GetMonthName(diningStockAdjustment.AdjustmentForMonth)
                                + " - " + diningStockAdjustment.AdjustmentForMonth.Year,
                            AdjustmentNo = diningStockAdjustment.DiningStockAdjustmentNo,
                            NoOfAdjustmentProduct = details.Count(),
                            Status = diningStockAdjustment.Status,
                            AdjustmentForMonth = diningStockAdjustment.AdjustmentForMonth,
                            InstituteId = diningStockAdjustment.InstituteId,
                            InstituteName = instituteRepository.Get(diningStockAdjustment.InstituteId).InstituteName,
                            CampusName = campusRepository.Get(diningStockAdjustment.CampusId).CampusName,
                            CampusId = diningStockAdjustment.CampusId,
                            StatusName = Formater.GetEnumValueDescription<DiningStockAdjustmentStatus>(diningStockAdjustment.Status)
                        };
                        diningStockAdjustmentTableDataViewModels.Add(diningStockAdjustmentTableDataViewModel);
                    }
                    if (diningStockAdjustmentTableDataViewModels.Count > 0)
                    {
                        response.ResponseObj = diningStockAdjustmentTableDataViewModels;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.NotFound;
                        response.Message = "Not found any Dining Stock Adjustment";
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningStockAdjustment(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningStockManage diningStockManage = await diningStockManageRepository.GetAsync(id);
                    if (diningStockManage != null && DataController.IsReturnable(diningStockManage, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningStockManage;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningStockManage> diningStockManages = await DataController.GetDataList<DiningStockManage>(token);
                    if (diningStockManages != null)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningStockManages;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Stock Adjustment";
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDetails(RequestMessage requestObject,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = JsonConvert.DeserializeObject<long>(requestObject.Content);
                if (id > 0)
                {
                    DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.GetAsync(id);

                    if (diningStockAdjustment != null)
                    {
                        List<DiningStockAdjustmentDetailsViewModel> diningStockAdjustmentDetails = new List<DiningStockAdjustmentDetailsViewModel>();

                        List<DiningStockAdjustmentDetails> diningStockAdjustmentDetails1 = await diningStockAdjustmentDetailsRepository
                            .FilterListAsync(x => x.DiningStockAdjustmentId == diningStockAdjustment.Id);

                        List<DiningStockAdjustment> allData = await diningStockAdjustmentRepository.FilterListAsync(x =>
                            x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                            && x.AdjustmentForMonth.Month < diningStockAdjustment.AdjustmentForMonth.Month
                            && x.AdjustmentForMonth.Year <= diningStockAdjustment.AdjustmentForMonth.Year);
                        List<DiningStockAdjustmentDetails> prevMonthAdjustments = new List<DiningStockAdjustmentDetails>();
                        DiningStockAdjustment prevAdjust = allData.OrderBy(x => x.AdjustmentForMonth).LastOrDefault();
                        if (prevAdjust != null)
                        {
                            prevMonthAdjustments = await diningStockAdjustmentDetailsRepository
                                .FilterListAsync(x => x.DiningStockAdjustmentId == prevAdjust.Id);
                        }

                        List<DiningExpense> allStockProductExpenses = await diningExpenseRepository.FilterListAsync(x =>
                                x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                                && x.ExpenseDate.Month == diningStockAdjustment.AdjustmentForMonth.Month
                                && x.ExpenseDate.Year == diningStockAdjustment.AdjustmentForMonth.Year
                                && x.IsStockProduct == true && x.Status == (byte)ExpenseVoucherStatus.Approved);
                        var x2 = allStockProductExpenses.Sum(x => x.Amount);
                        var diningExpenses = allStockProductExpenses.GroupBy(x => x.ProductName).ToList();
                        foreach (var diningExpense in diningExpenses)
                        {
                            StockProduct diningStockProduct = await diningStockProductRepository
                                .GetAsync(Convert.ToInt64(diningExpense.FirstOrDefault().ProductName));
                            DiningStockAdjustmentDetails currentMonthAdjustProcut = diningStockAdjustmentDetails1.FirstOrDefault(x =>
                                x.ProductId == diningStockProduct.Id);

                            if (currentMonthAdjustProcut != null)
                            {
                                var prevProduct = prevMonthAdjustments.FirstOrDefault(x => x.ProductId == diningStockProduct.Id);

                                decimal diningExpenseAmount = diningExpense.Sum(x => x.Amount);
                                decimal diningExpenseQty = diningExpense.Sum(x => x.Qty);

                                decimal rate = Convert.ToDecimal(string.Format("{0:0.00}",
                                    Convert.ToDecimal(string.Format("{0:0.00}", diningExpenseAmount))
                                    / Convert.ToDecimal(string.Format("{0:0.000}", diningExpenseQty))));

                                if (prevProduct != null)
                                {
                                    rate = (diningExpenseAmount + prevProduct.Price) / (diningExpenseQty + prevProduct.RemainingQty);
                                    prevMonthAdjustments.Remove(prevProduct);
                                }

                                DiningStockAdjustmentDetailsViewModel diningStockAdjustmentDetailsViewModel = new DiningStockAdjustmentDetailsViewModel
                                {
                                    AdjustmentNo = diningStockAdjustment.DiningStockAdjustmentNo,
                                    Month = Formater.GetMonthName(diningStockAdjustment.AdjustmentForMonth)
                                        + " - " + diningStockAdjustment.AdjustmentForMonth.Year,
                                    ProductName = diningStockProduct.ProductName,
                                    RemainingQty = currentMonthAdjustProcut != null ? currentMonthAdjustProcut.RemainingQty : 0,
                                    Rate = rate,
                                    CreatedByName = userRepository.Get(diningStockAdjustment.CreatedBy).FullName,
                                    CreatedDateString = Formater.FormatDateddMMyyyy(diningStockAdjustment.CreatedDate),
                                    StatusName = Formater.GetEnumValueDescription<DiningStockAdjustmentStatus>(diningStockAdjustment.Status),
                                    Unit = diningStockProduct.Unit,
                                    UsedQty = (diningExpense.Sum(x => x.Qty) + (prevProduct != null ? prevProduct.RemainingQty : (decimal)0))
                                    - (currentMonthAdjustProcut != null ? currentMonthAdjustProcut.RemainingQty : 0)
                                };
                                diningStockAdjustmentDetailsViewModel.Price = diningStockAdjustmentDetailsViewModel.RemainingQty * rate;
                                diningStockAdjustmentDetailsViewModel.UsedItemPrice = diningStockAdjustmentDetailsViewModel.UsedQty * rate;
                                diningStockAdjustmentDetails.Add(diningStockAdjustmentDetailsViewModel);
                            }
                        }
                        foreach (var details in prevMonthAdjustments)
                        {
                            StockProduct diningStockProduct = await diningStockProductRepository.GetAsync(details.ProductId);

                            DiningStockAdjustmentDetails currentMonthAdjustProcut = diningStockAdjustmentDetails1.FirstOrDefault(x =>
                                x.ProductId == diningStockProduct.Id);

                            if (currentMonthAdjustProcut != null)
                            {
                                decimal rate = details.Rate;
                                DiningStockAdjustmentDetailsViewModel diningStockAdjustmentDetailsViewModel = new DiningStockAdjustmentDetailsViewModel
                                {
                                    AdjustmentNo = diningStockAdjustment.DiningStockAdjustmentNo,
                                    Month = Formater.GetMonthName(diningStockAdjustment.AdjustmentForMonth)
                                        + " - " + diningStockAdjustment.AdjustmentForMonth.Year,
                                    ProductName = diningStockProduct.ProductName,
                                    RemainingQty = currentMonthAdjustProcut.RemainingQty,
                                    Rate = rate,
                                    CreatedByName = userRepository.Get(diningStockAdjustment.CreatedBy).FullName,
                                    CreatedDateString = Formater.FormatDateddMMyyyy(diningStockAdjustment.CreatedDate),
                                    StatusName = Formater.GetEnumValueDescription<DiningStockAdjustmentStatus>(diningStockAdjustment.Status),
                                    Unit = diningStockProduct.Unit,
                                    UsedQty = details.RemainingQty - (currentMonthAdjustProcut != null ? currentMonthAdjustProcut.RemainingQty : 0)
                                };
                                diningStockAdjustmentDetailsViewModel.Price = diningStockAdjustmentDetailsViewModel.RemainingQty * rate;
                                diningStockAdjustmentDetailsViewModel.UsedItemPrice = diningStockAdjustmentDetailsViewModel.UsedQty * rate;
                                diningStockAdjustmentDetails.Add(diningStockAdjustmentDetailsViewModel);
                            }
                        }
                        //var u = diningStockAdjustmentDetails.Sum(x => x.UsedItemPrice);
                        //var p = diningStockAdjustmentDetails.Sum(x => x.Price);
                        //var total = u + p;
                        if (diningStockAdjustmentDetails.Count > 0)
                        {
                            response.ResponseObj = diningStockAdjustmentDetails;
                            response.StatusCode = (byte)StatusCode.Success;
                        }
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Stock Adjustment";
                }
            }

            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningStockAdjustmentAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                var data = new { date = DateTime.UtcNow, campusId = 0 };
                dynamic searchKey = JsonConvert.DeserializeObject(requestMessage.Content, data.GetType());
                DateTime date = searchKey.date;
                long id = (long)searchKey.campusId;
                if (date.Month > DateTime.UtcNow.Month)
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "You can not adjust stock of advance month!";
                }
                else
                {
                    DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(x =>
                         x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                         && x.AdjustmentForMonth.Date.Month == date.Date.Month && x.AdjustmentForMonth.Year == date.Year
                         && x.Status == (byte)DiningStockAdjustmentStatus.Created);
                    if (diningStockAdjustment != null)
                    {
                        response.StatusCode = (byte)StatusCode.Failed;
                        response.Message = $"Stock adjustment for {Formater.GetMonthName(date)} has already been created." +
                            $" You cant now update that record!";
                    }
                    else
                    {
                        List<DiningMealAttendance> allTakenDiningMealAttendances = await diningMealAttendanceRepository.FilterListAsync(
                            x => x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                            && x.Status == (byte)DiningMealStatus.Taken);
                        if (allTakenDiningMealAttendances.Count > 0)
                        {
                            List<DiningMealAttendance> prvDiningMealAttendances = allTakenDiningMealAttendances.Where(
                            x => x.MealDate.Date.Month < date.Date.Month && x.MealDate.Year <= date.Year).ToList();
                            if (prvDiningMealAttendances.Count > 0)
                            {
                                DateTime lastMonthAttendance = prvDiningMealAttendances.OrderBy(x => x.MealDate)
                                    .ToList().LastOrDefault().MealDate;
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = $"The accounts of {Formater.GetMonthName(lastMonthAttendance)} is not closed." +
                                    $"Please close {Formater.GetMonthName(lastMonthAttendance)}'s accounts before closing " +
                                    $"{Formater.GetMonthName(date)}'s accounts.";
                            }
                            else
                            {
                                List<DiningMealAttendance> diningMealAttendances = allTakenDiningMealAttendances.Where(
                                    x => x.MealDate.Month == date.Date.Month && x.MealDate.Date.Year == date.Date.Year).ToList();
                                if (diningMealAttendances.Count > 0)
                                {
                                    List<DiningExpense> diningExpenses = await diningExpenseRepository.FilterListAsync(x =>
                                           x.ExpenseDate.Date.Month == date.Date.Month && x.ExpenseDate.Date.Year == date.Date.Year && x.VoucherId != null
                                           && x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID));
                                    //if (diningExpenses.Count > 0)
                                    //{
                                    List<DiningExpense> unCompleteDiningExpenses = diningExpenses.Where(x =>
                                        x.Status == (byte)ExpenseVoucherStatus.Submitted
                                        && x.Status == (byte)ExpenseVoucherStatus.Audited).ToList();
                                    if (unCompleteDiningExpenses.Count == 0)
                                    {
                                        DiningStockAdjustment prevDiningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(x =>
                                          x.AdjustmentForMonth.Date.Month < date.Date.Month && x.AdjustmentForMonth.Year <= date.Year
                                          && x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                          && x.Status == (byte)DiningStockAdjustmentStatus.Running);
                                        List<DiningStockAdjustmentAddDataViewModel> diningStockAdjustmentsAddData = await GetDiningStockAdjustmentAsync(
                                            diningExpenses.Where(x => x.IsStockProduct == true && x.Status == (byte)ExpenseVoucherStatus.Approved)
                                            .ToList(), date, prevDiningStockAdjustment, diningStockAdjustment);

                                        if (diningStockAdjustmentsAddData.Count > 0)
                                        {
                                            response.ResponseObj = diningStockAdjustmentsAddData;
                                            response.StatusCode = (byte)StatusCode.Success;
                                        }
                                        else
                                        {
                                            response.StatusCode = (byte)StatusCode.Failed;
                                            response.Message = $"You don't have any stock product to adjust.";
                                        }
                                    }
                                    else
                                    {
                                        int count = unCompleteDiningExpenses.Count;
                                        List<byte> vs = unCompleteDiningExpenses.Select(x => x.Status).Distinct().ToList();
                                        response.StatusCode = (byte)StatusCode.Failed;
                                        string ms = "";
                                        int i = 0;
                                        foreach (var s in vs)
                                        {
                                            if (i > 0)
                                            {
                                                ms += " and " + Formater.GetEnumValueDescription<ExpenseVoucherStatus>(s).ToLower();
                                            }
                                            else
                                            {
                                                ms += Formater.GetEnumValueDescription<ExpenseVoucherStatus>(s).ToLower();
                                            }
                                            i++;
                                        }
                                        response.Message = $"You have {count} expense with {ms}. Please reject or approved these expense first.";
                                    }
                                    //}
                                    //else
                                    //{
                                    //    response.StatusCode = (byte)StatusCode.Failed;
                                    //    response.Message = $"Not found any record of month {Formater.GetMonthName(date)}";
                                    //}
                                }
                                else
                                {
                                    List<DiningMealAttendance> lockedDiningMealAttendances = await diningMealAttendanceRepository.FilterListAsync(
                                        x => x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                        && x.Status == (byte)DiningMealStatus.Locked
                                        && x.MealDate.Date.Month == date.Date.Month && x.MealDate.Date.Year == date.Date.Year);
                                    if (lockedDiningMealAttendances.Count > 0)
                                    {
                                        response.StatusCode = (byte)StatusCode.Failed;
                                        response.Message = $"The accounts of {Formater.GetMonthName(date)} has already been closed";
                                    }
                                    else
                                    {
                                        response.StatusCode = (byte)StatusCode.Failed;
                                        response.Message = $"There is nothing to adjust for month {Formater.GetMonthName(date)}";
                                    }
                                }
                            }
                        }
                        else
                        {
                            List<DiningMealAttendance> lockedDiningMealAttendances = await diningMealAttendanceRepository.FilterListAsync(
                                x => x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                && x.Status == (byte)DiningMealStatus.Locked
                                && x.MealDate.Date.Month == date.Date.Month && x.MealDate.Date.Year == date.Date.Year);
                            if (lockedDiningMealAttendances.Count > 0)
                            {
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = $"The accounts of {Formater.GetMonthName(date)} has already been closed";
                            }
                            else
                            {
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = $"There is nothing to adjust for month {Formater.GetMonthName(date)}";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningStockAdjustmentUpdateData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = JsonConvert.DeserializeObject<long>(requestMessage.Content);
                if (id > 0)
                {
                    DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.GetAsync(id);
                    if (diningStockAdjustment == null)
                    {
                        response.StatusCode = (byte)StatusCode.Failed;
                        response.Message = "Not found any dining stock adjustment";
                    }
                    else
                    {
                        if (diningStockAdjustment.Status == (byte)DiningStockAdjustmentStatus.Created)
                        {
                            List<DiningExpense> diningExpenses = await diningExpenseRepository.FilterListAsync(x =>
                                   x.ExpenseDate.Date.Month == diningStockAdjustment.AdjustmentForMonth.Date.Month
                                   && x.ExpenseDate.Date.Year == diningStockAdjustment.AdjustmentForMonth.Date.Year && x.VoucherId != null
                                   && x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID));

                            DiningStockAdjustment prevDiningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(x =>
                              x.AdjustmentForMonth.Date.Month < diningStockAdjustment.AdjustmentForMonth.Date.Month
                              && x.AdjustmentForMonth.Year <= diningStockAdjustment.AdjustmentForMonth.Year
                              && x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                              && x.Status == (byte)DiningStockAdjustmentStatus.Running);

                            List<DiningStockAdjustmentAddDataViewModel> diningStockAdjustmentsAddData = await GetDiningStockAdjustmentAsync(
                                diningExpenses.Where(x => x.IsStockProduct == true && x.Status == (byte)ExpenseVoucherStatus.Approved)
                                .ToList(), diningStockAdjustment.AdjustmentForMonth, prevDiningStockAdjustment, diningStockAdjustment);

                            if (diningStockAdjustmentsAddData.Count > 0)
                            {
                                response.ResponseObj = diningStockAdjustmentsAddData;
                                response.StatusCode = (byte)StatusCode.Success;
                            }
                            else
                            {
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = $"You don't have any stock product to adjust.";
                            }
                        }
                        else
                        {
                            response.StatusCode = (byte)StatusCode.Failed;
                            response.Message = $"You can update data only when it is in created state";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<List<DiningStockAdjustmentAddDataViewModel>> GetDiningStockAdjustmentAsync(List<DiningExpense> diningExpenses,
            DateTime stockForMonth, DiningStockAdjustment previousAdjustment, DiningStockAdjustment editDataOfDiningStockAdjustment)
        {
            List<DiningStockAdjustmentAddDataViewModel> diningStockAdjustmentAddDatas = new List<DiningStockAdjustmentAddDataViewModel>();
            var expenses = diningExpenses.GroupBy(x => x.ProductName).ToList();
            List<DiningStockAdjustmentDetails> adjustmentDetails = new List<DiningStockAdjustmentDetails>();
            List<DiningStockAdjustmentDetails> editDataOfAdjustmentDetails = new List<DiningStockAdjustmentDetails>();
            if (editDataOfDiningStockAdjustment != null)
            {
                editDataOfAdjustmentDetails = await diningStockAdjustmentDetailsRepository.FilterListAsync(x =>
                    x.DiningStockAdjustmentId == editDataOfDiningStockAdjustment.Id);
            }
            if (previousAdjustment != null)
            {
                adjustmentDetails = await diningStockAdjustmentDetailsRepository.FilterListAsync(x =>
                    x.DiningStockAdjustmentId == previousAdjustment.Id);
            }
            foreach (var expense in expenses)
            {
                var diningStockAdjustment = adjustmentDetails.FirstOrDefault(x => x.ProductId == Convert.ToInt64(expense.First()
                    .ProductName));
                var editDataStockProduct = editDataOfAdjustmentDetails.FirstOrDefault(x => x.ProductId == Convert.ToInt64(expense.First()
                    .ProductName));
                if (diningStockAdjustment != null)
                    adjustmentDetails.Remove(diningStockAdjustment);
                decimal totalPrice = expense.Sum(x => x.Qty * x.Rate) + (diningStockAdjustment != null ? diningStockAdjustment.Price : 0);
                decimal totalQty = Convert.ToDecimal(string.Format("{0:0.000}", expense.Sum(x => x.Qty)
                    + (diningStockAdjustment != null ? diningStockAdjustment.RemainingQty : 0)));
                StockProduct diningStockProduct = await diningStockProductRepository.GetAsync(Convert.ToInt64(expense.First().ProductName));
                DiningStockAdjustmentAddDataViewModel diningStockAdjustmentAddData = new DiningStockAdjustmentAddDataViewModel
                {
                    Id = editDataStockProduct != null ? editDataStockProduct.Id : 0,
                    InstituteId = expense.First().InstituteId,
                    CampusId = expense.First().CampusId,
                    ProductName = diningStockProduct.ProductName,
                    RemainingQty = editDataStockProduct != null ? editDataStockProduct.RemainingQty : totalQty,
                    UsedQyt = totalQty - (editDataStockProduct != null ? editDataStockProduct.RemainingQty : 0),
                    Price = totalPrice,
                    TotalQty = totalQty,
                    Rate = totalPrice / totalQty,
                    InStock = editDataStockProduct != null ? editDataStockProduct.RemainingQty : 0,
                    ProductId = diningStockProduct.Id,
                    Unit = diningStockProduct.Unit,
                    AdjustmentForMonth = stockForMonth,
                    DiningStockAdjustmentId = editDataOfDiningStockAdjustment != null ? editDataOfDiningStockAdjustment.Id : 0
                };
                diningStockAdjustmentAddDatas.Add(diningStockAdjustmentAddData);
            }
            foreach (var adjustment in adjustmentDetails)
            {
                StockProduct diningStockProduct = await diningStockProductRepository.GetAsync(adjustment.ProductId);
                var editDataStockProduct = editDataOfAdjustmentDetails.FirstOrDefault(x => x.ProductId == adjustment.ProductId);
                DiningStockAdjustmentAddDataViewModel diningStockAdjustmentAddData = new DiningStockAdjustmentAddDataViewModel
                {
                    Id = editDataStockProduct != null ? editDataStockProduct.Id : 0,
                    InstituteId = previousAdjustment.InstituteId,
                    CampusId = previousAdjustment.CampusId,
                    ProductName = diningStockProduct.ProductName,
                    TotalQty = adjustment.RemainingQty,
                    RemainingQty = editDataStockProduct != null ? editDataStockProduct.RemainingQty : adjustment.RemainingQty,
                    UsedQyt = adjustment.RemainingQty - (editDataStockProduct != null ? editDataStockProduct.RemainingQty : 0),
                    Price = adjustment.Price,
                    Rate = adjustment.Rate,
                    ProductId = diningStockProduct.Id,
                    Unit = diningStockProduct.Unit,
                    AdjustmentForMonth = stockForMonth,
                    DiningStockAdjustmentId = editDataOfDiningStockAdjustment != null ? editDataOfDiningStockAdjustment.Id : 0
                };
                diningStockAdjustmentAddDatas.Add(diningStockAdjustmentAddData);
            }
            return diningStockAdjustmentAddDatas;
        }
        //public async Task AddDiningStockManage(List<DiningExpense> diningExpenses, UserAuthorization token, ExpenseVoucher expenseVoucher)
        //{
        //    try
        //    {
        //        List<DiningStockManage> diningStockManages = new List<DiningStockManage>();
        //        List<DiningStockManage> diningStockManagesForUpdate = new List<DiningStockManage>();
        //        foreach (DiningExpense diningExpense in diningExpenses)
        //        {
        //            if (diningExpense.IsStockProduct)
        //            {
        //                DiningStockManage existsStockManage = await diningStockManageRepository.FilterOneAsync(x =>
        //                x.ProductId == Convert.ToInt64(diningExpense.ProductName)
        //                && x.StockForMonth.Month == diningExpense.ExpenseDate.Month
        //                && x.StockForMonth.Year == diningExpense.ExpenseDate.Year);

        //                if (existsStockManage != null)
        //                {
        //                    decimal stockQty = existsStockManage.StockQty + diningExpense.Qty;
        //                    decimal price = existsStockManage.Price + diningExpense.Amount;
        //                    decimal rate = price / stockQty;
        //                    existsStockManage.StockQty = stockQty;
        //                    existsStockManage.Price = price;
        //                    existsStockManage.Rate = rate;
        //                    existsStockManage.UpdatedBy = token.UserID;
        //                    existsStockManage.UpdatedDate = DateTime.UtcNow;
        //                    diningStockManagesForUpdate.Add(existsStockManage);
        //                }
        //                else
        //                {
        //                    CodeAndId codeAndId = null;
        //                    if (diningStockManages.Count > 0)
        //                    {
        //                        codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningStockManage>
        //                            (diningStockManages, token);
        //                    }
        //                    else
        //                    {
        //                        codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningStockManage>
        //                            (await diningStockManageRepository.GetAsync(), token);
        //                    }
        //                    DiningStockManage diningStockManage = new DiningStockManage()
        //                    {
        //                        Id = codeAndId.Id + 1,
        //                        ProductId = Convert.ToInt64(diningExpense.ProductName),
        //                        StockForMonth = diningExpense.ExpenseDate,
        //                        DiningStockManageNo = codeAndId.No,
        //                        Status = (byte)DiningStockManageStatus.Running,
        //                        Price = diningExpense.Rate * diningExpense.Qty,
        //                        Rate = diningExpense.Rate,
        //                        StockQty = diningExpense.Qty,
        //                        CreatedBy = token.UserID,
        //                        CreatedDate = DateTime.UtcNow,
        //                        CampusId = expenseVoucher.CampusId,
        //                        InstituteId = expenseVoucher.InstituteId
        //                    };
        //                    diningStockManages.Add(diningStockManage);
        //                }
        //            }
        //        }
        //        if (diningStockManages.Count > 0)
        //            await diningStockManageRepository.AddRangeAsync(diningStockManages);
        //        if (diningStockManagesForUpdate.Count > 0)
        //            await diningStockManageRepository.UpdateRangeAsync(diningStockManagesForUpdate);
        //    }
        //    catch (Exception e)
        //    {
        //        await ExceptionLogger.LogExceptionAsync(e, token, new ResponseMessage());
        //    }
        //}
    }
}
