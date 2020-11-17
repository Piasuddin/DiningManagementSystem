using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using MERP.EntityFramworkDAL.Repository;
using System.Transactions;

namespace MERP.FinanceBLL
{
    public class DiningBillBLL
    {
        private readonly IRepository<DiningBill> diningBillRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<DiningMealAttendance> diningMealAttendanceRepository;
        private readonly IRepository<DiningMeal> diningMealRepository;
        private readonly IRepository<DiningExpense> diningExpenseRepository;
        private readonly IRepository<DiningStockManage> diningStockManageRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<DiningBoarderExternal> diningBoarderExternalRepository;
        private readonly IRepository<DiningStockAdjustment> diningStockAdjustmentRepository;
        private readonly IRepository<DiningStockAdjustmentDetails> diningStockAdjustmentRepositoryDetails;
        private readonly IRepository<DiningBillDetails> diningBillDetailsRepository;
        private readonly IRepository<StockProduct> stockProductRepository;
        private readonly IRepository<DiningBillCollection> diningBillCollectionRepository;
        private readonly DiningBoarderBLL diningBoarderBLL;
        private readonly IRepository<DiningBoarder> diningBoarderRepository;

        public DiningBillBLL(IRepository<DiningBill> diningBillRepository, IRepository<User> userRepository,
            IRepository<DiningMealAttendance> diningMealAttendanceRepository, DiningBoarderBLL diningBoarderBLL,
            IRepository<DiningBillCollection> diningBillCollectionRepository, IRepository<DiningMeal>
            diningMealRepository, IRepository<DiningExpense> diningExpenseRepository, IRepository<DiningBoarder>
            diningBoarderRepository, IRepository<DiningStockManage> diningStockManageRepository,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository,
            IRepository<Employee> employeeRepository, IRepository<Student> studentRepository,
            IRepository<DiningBoarderExternal> diningBoarderExternalRepository, IRepository<DiningStockAdjustment> diningStockAdjustmentRepository,
            IRepository<DiningStockAdjustmentDetails> diningStockAdjustmentRepositoryDetails,
            IRepository<DiningBillDetails> diningBillDetailsRepository, IRepository<StockProduct> stockProductRepository)
        {
            this.diningBillRepository = diningBillRepository;
            this.userRepository = userRepository;
            this.diningMealAttendanceRepository = diningMealAttendanceRepository;
            this.diningMealRepository = diningMealRepository;
            this.diningExpenseRepository = diningExpenseRepository;
            this.diningStockManageRepository = diningStockManageRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.employeeRepository = employeeRepository;
            this.studentRepository = studentRepository;
            this.diningBoarderExternalRepository = diningBoarderExternalRepository;
            this.diningStockAdjustmentRepository = diningStockAdjustmentRepository;
            this.diningStockAdjustmentRepositoryDetails = diningStockAdjustmentRepositoryDetails;
            this.diningBillDetailsRepository = diningBillDetailsRepository;
            this.stockProductRepository = stockProductRepository;
            this.diningBillCollectionRepository = diningBillCollectionRepository;
            this.diningBoarderBLL = diningBoarderBLL;
            this.diningBoarderRepository = diningBoarderRepository;
        }
        public async Task<ResponseMessage> CreateDiningBill(ResponseMessage response, UserAuthorization token, RequestMessage requestMessage)
        {
            try
            {
                long adjustmentId = JsonConvert.DeserializeObject<long>(requestMessage.Content);

                DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.GetAsync(adjustmentId);
                if (diningStockAdjustment != null && DataController.IsReturnable(diningStockAdjustment, token))
                {
                    List<DiningMealAttendance> diningMealManages = await diningMealAttendanceRepository.FilterListAsync(x =>
                       x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                       && x.MealDate.Month == diningStockAdjustment.AdjustmentForMonth.Month
                       && x.MealDate.Year == diningStockAdjustment.AdjustmentForMonth.Year && x.Status == (byte)DiningMealStatus.Taken);
                    IEnumerable<DiningMeal> diningMeals = await diningMealRepository.FilterListAsync(x =>
                        x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                        && x.Status == (byte)Status.Active);
                    List<BoarderWithTakenMealVM> boarderWithTakenMealVMs = new List<BoarderWithTakenMealVM>();

                    foreach (DiningMealAttendance diningMealManage in diningMealManages)
                    {
                        diningMealManage.Status = (byte)DiningMealStatus.Locked;
                        List<BoarderWithTakenMealVM> data = JsonConvert
                            .DeserializeObject<List<BoarderWithTakenMealVM>>(diningMealManage.BoarderWithTakenMeal);
                        boarderWithTakenMealVMs.AddRange(data);
                    }
                    List<MealCountVM> mealCountVMs = new List<MealCountVM>();
                    var groupBoarderWithTakenMealVMs = boarderWithTakenMealVMs.GroupBy(x => x.BoarderId);

                    foreach (IEnumerable<BoarderWithTakenMealVM> item in groupBoarderWithTakenMealVMs)
                    {
                        var borderMealRecord = item.ToList();
                        foreach (DiningMeal diningMeal in diningMeals)
                        {
                            mealCountVMs.Add(GetMealCount(borderMealRecord, diningMeal));
                        }
                    }
                    decimal totalMeal = mealCountVMs.Sum(x => x.TotalMeal);

                    List<DiningExpense> diningExpenses = await diningExpenseRepository.FilterListAsync(x =>
                        x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                        && x.ExpenseDate.Month == diningStockAdjustment.AdjustmentForMonth.Month
                        && x.ExpenseDate.Year == diningStockAdjustment.AdjustmentForMonth.Year
                        && x.Status == (byte)ExpenseVoucherStatus.Approved);

                    List<DiningStockAdjustmentDetails> diningStockAdjustmentDetails = await diningStockAdjustmentRepositoryDetails
                        .FilterListAsync(x => x.DiningStockAdjustmentId == adjustmentId);

                    List<DiningStockAdjustment> allData = await diningStockAdjustmentRepository.FilterListAsync(x =>
                            x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                            && x.AdjustmentForMonth.Month < diningStockAdjustment.AdjustmentForMonth.Month
                            && x.AdjustmentForMonth.Year <= diningStockAdjustment.AdjustmentForMonth.Year);
                    List<DiningStockAdjustmentDetails> prevMonthAdjustments = new List<DiningStockAdjustmentDetails>();
                    DiningStockAdjustment prevAdjust = allData.OrderBy(x => x.AdjustmentForMonth).LastOrDefault();
                    if (prevAdjust != null)
                    {
                        prevMonthAdjustments = await diningStockAdjustmentRepositoryDetails
                            .FilterListAsync(x => x.DiningStockAdjustmentId == prevAdjust.Id);
                    }
                    decimal totalCost = (diningExpenses.Sum(x => x.Amount) + prevMonthAdjustments.Sum(x => x.Price))
                        - diningStockAdjustmentDetails.Sum(x => x.Price);
                    decimal mealRate = totalCost / totalMeal;

                    CodeAndId codeAndId = await IdGenerator
                        .GenerateBaseClassIdAsync<DiningBill>(await diningBillRepository.GetAsync(), token);

                    DiningBill diningBill = new DiningBill()
                    {
                        Id = codeAndId.Id + 1,
                        BillMonth = diningStockAdjustment.AdjustmentForMonth,
                        DiningBillNo = codeAndId.No,
                        CampusId = token.CampusID,
                        InstituteId = token.InstituteID,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = token.UserID,
                        MealRate = mealRate,
                        Status = (byte)Status.Active
                    };
                    List<DiningBillDetails> diningBillDetails = new List<DiningBillDetails>();

                    var mealCountsByBoarder = mealCountVMs.GroupBy(x => x.BoarderId);
                    long id = await IdGenerator.GenerateChildClassIdAsync<DiningBillDetails>(await diningBillDetailsRepository.GetAsync());

                    foreach (IEnumerable<MealCountVM> boarderMeals in mealCountsByBoarder)
                    {
                        DiningBillDetails diningBillDetail = GetBorderMealCost(boarderMeals.ToList(), mealRate, id + 1, diningBill.Id);
                        diningBillDetails.Add(diningBillDetail);
                        id++;
                    }
                    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await diningMealAttendanceRepository.UpdateRangeAsync(diningMealManages);
                        diningStockAdjustment.Status = (byte)DiningStockAdjustmentStatus.Running;
                        await diningStockAdjustmentRepository.UpdateAsync(diningStockAdjustment);
                        DiningBill result = await diningBillRepository.AddAsync(diningBill);

                        if (result != null)
                        {
                            await diningBillDetailsRepository.AddRangeAsync(diningBillDetails);
                            transaction.Complete();
                            response.ResponseObj = result;
                            response.Message = $"Stock adjustment useable Successfully for month " +
                                $"{Formater.GetMonthName(diningStockAdjustment.AdjustmentForMonth)}.";
                            response.StatusCode = (byte)StatusCode.Success;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Bill! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningBill(DiningBillViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBill diningBill = await diningBillRepository.GetAsync((long)model.Id);
                if (diningBill != null)
                {
                    //diningBill.BillMonth = model.BillMonth;
                    //diningBill.BoarderId = model.BoarderId;
                    //diningBill.MealRate = model.MealRate;
                    //diningBill.TotalMeal = model.TotalMeal;
                    //diningBill.TotalBill = model.TotalBill;
                    //diningBill.UpdatedBy = token.UserID;
                    //diningBill.UpdatedDate = DateTime.Now;

                    //DiningBill result = await diningBillRepository.UpdateAsync(diningBill);

                    //if (result != null)
                    //{
                    //    response.ResponseObj = result;
                    //    response.Message = "Dining Bill Created Successfully With ID: " + result.DiningBillNo;
                    //    response.StatusCode = (byte)StatusCode.Success;
                    //}
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.DiningBillNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Bill! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningBill(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBill diningBill = await diningBillRepository.GetAsync(id);
                    if (diningBill != null)
                    {
                        if (await DeletePreventer.CheckDeletableWithObjectAndPropertyName<DiningBill>(diningBill))
                        {
                            await diningBillRepository.DeleteAsync(diningBill);
                            response.Message = "Dining Bill Deleted Succesfuly with id: " + diningBill.DiningBillNo;
                            response.StatusCode = (byte)StatusCode.Success;
                            response.ResponseObj = diningBill;
                        }
                        else
                        {
                            response.Message = "You can't delete this record. This record is referenced to another table.";
                            response.StatusCode = (byte)StatusCode.Failed;
                        }

                    }
                    else
                    {
                        response.Message = "Not found any Dining Bill";
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
        public async Task<ResponseMessage> searchDiningBillForTable(ResponseMessage response, UserAuthorization token)
        {
            try
            {
                var bills = await DataController.GetDataList<DiningBill>(token).ConfigureAwait(false);
                List<DiningBillDetailsViewModel> diningBillViewModels = new List<DiningBillDetailsViewModel>();

                if (bills.Count() > 0)
                {
                    foreach (var diningBill in bills)
                    {
                        List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                            x.DiningBillId == diningBill.Id);
                        DiningBillDetailsViewModel diningBillViewModel = new DiningBillDetailsViewModel
                        {
                            Id = diningBill.Id,
                            BillMonth = diningBill.BillMonth,
                            TotalMeal = diningBillDetails.Sum(x => x.TotalMeal),
                            TotalBill = diningBillDetails.Sum(x => x.TotalBill),
                            BillForMonthName = Formater.GetMonthName(diningBill.BillMonth)
                            + " - " + diningBill.BillMonth.Year,
                            InstituteId = diningBill.InstituteId,
                            InstituteName = diningBill.InstituteId > 0 ? instituteRepository.Get(diningBill.InstituteId).InstituteName : null,
                            CampusName = diningBill.CampusId > 0 ? campusRepository.Get(diningBill.CampusId).CampusName : null,
                            CampusId = diningBill.CampusId,
                            Status = diningBill.Status,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningBill.Status),
                            IsDeletable = await DeletePreventer.CheckDeletableWithObjectAndPropertyName(diningBill)
                        };
                        diningBillViewModel.MealRate = diningBillViewModel.TotalBill / diningBillViewModel.TotalMeal;
                        diningBillViewModels.Add(diningBillViewModel);
                    }
                    response.ResponseObj = diningBillViewModels;
                    response.StatusCode = (byte)StatusCode.Success;
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Bill";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBill(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBill diningBill = await diningBillRepository.GetAsync(id);
                    if (diningBill != null && DataController.IsReturnable(diningBill, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBill;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningBill> diningBills = await DataController.GetDataList<DiningBill>(token);
                    if (diningBills.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBills;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Bill";
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
                DateTime date = Convert.ToDateTime(JsonConvert.DeserializeObject<DateTime>(requestObject.Content));
                DiningBill diningBill = await diningBillRepository.FilterOneAsync(x =>
                    x.BillMonth.Year == date.Year && x.BillMonth.Month == date.Month);
                if (diningBill != null)
                {
                    List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                        x.DiningBillId == diningBill.Id);
                    DiningBillDetailsForDetailsViewModel diningBillDetailsForDetailsViewModel = new DiningBillDetailsForDetailsViewModel
                    {
                        MealRate = diningBill.MealRate,
                        TotalMeal = diningBillDetails.Sum(x => x.TotalMeal),
                        TotalBill = diningBillDetails.Sum(x => x.TotalBill),
                        BillForMonthName = Formater.GetMonthName(diningBill.BillMonth)
                            + " - " + diningBill.BillMonth.Year,
                        CreatedByName = userRepository.Get(diningBill.CreatedBy).FullName,
                        CreatedDateString = diningBill.CreatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBill.CreatedDate) : null,
                        UpdatedByName = diningBill.UpdatedBy != null ?
                                userRepository.Get(diningBill.UpdatedBy).FullName : null,
                        UpdatedDateString = diningBill.UpdatedDate != null ?
                            Formater.FormatDateddMMyyyy(diningBill.UpdatedDate) : null,
                        StatusName = Formater.GetEnumValueDescription<AccountTransectionType>(diningBill.Status)
                    };
                    List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                    foreach (DiningBillDetails diningBillDetail in diningBillDetails)
                    {
                        DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(diningBillDetail.BoarderId);
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel();
                        if (diningBoarder.BoarderTypeId == (byte)BoarderType.Employee)
                        {
                            diningBoarderDetailsViewModel = await diningBoarderBLL.GetEmployeeDetails(await employeeRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        }
                        else if (diningBoarder.BoarderTypeId == (byte)BoarderType.Outer)
                        {
                            diningBoarderDetailsViewModel = await diningBoarderBLL.GetExternalBoarderDetails(await diningBoarderExternalRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        }
                        else
                        {
                            diningBoarderDetailsViewModel = await diningBoarderBLL.GetStudentDetails(await studentRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        };
                        diningBoarderDetailsViewModel.TotalMeal = diningBillDetail.TotalMeal;
                        diningBoarderDetailsViewModel.TotalBill = diningBillDetail.TotalBill;
                        diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                    };

                    diningBillDetailsForDetailsViewModel.BoarderDetails = diningBoarderDetailsViewModels;

                    response.ResponseObj = diningBillDetailsForDetailsViewModel;
                    response.StatusCode = (byte)StatusCode.Success;
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Bill";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public MealCountVM GetMealCount(List<BoarderWithTakenMealVM> meals, DiningMeal diningMeal)
        {
            MealCountVM mealCountVM = new MealCountVM();
            var keys = Enum.GetValues(typeof(DiningMealNumber));
            foreach (var key in keys)
            {
                if (diningMeal.DiningMealNumberId == (int)key)
                {
                    List<decimal> data = meals.Where(x => (decimal)x[key.ToString()] > 0).Select(y => (decimal)y[key.ToString()]).ToList();
                    mealCountVM = new MealCountVM
                    {
                        BoarderId = meals.FirstOrDefault().BoarderId,
                        TotalMeal = data.Sum(x => x),
                        DiningMealNumberId = diningMeal.DiningMealNumberId
                    };
                    break;
                }
            }
            return mealCountVM;
        }
        public DiningBillDetails GetBorderMealCost(List<MealCountVM> mealCountVMs, decimal mealRate, long id, long diningBilId)
        {
            decimal totalMeal = mealCountVMs.Sum(x => x.TotalMeal);

            DiningBillDetails diningBillDetails = new DiningBillDetails
            {
                Id = id,
                BoarderId = mealCountVMs.First().BoarderId,
                DiningBillId = diningBilId,
                TotalMeal = totalMeal,
                TotalBill = mealRate * totalMeal,
                MealAccounts = GetCountByMealJosn(mealCountVMs)
            };
            return diningBillDetails;
        }
        public string GetCountByMealJosn(List<MealCountVM> mealCountVMs)
        {
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            returnClass.Add("boarderId", mealCountVMs.First().BoarderId);
            var keys = Enum.GetValues(typeof(DiningMealNumber));

            foreach (var key in keys)
            {
                foreach (MealCountVM mealCountVM in mealCountVMs)
                {
                    if (mealCountVM.DiningMealNumberId == (int)key && mealCountVM.TotalMeal > 0)
                    {
                        returnClass.Add(key.ToString(), mealCountVM.TotalMeal);
                    }
                }
            }
            return JsonConvert.SerializeObject(returnClass);
        }
        public async Task<ResponseMessage> GetDiningBoarderStatement(DiningBoarderStatementSearchVM model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBoarder diningBoarder = await diningBoarderRepository.FilterOneAsync(x =>
                x.DiningBoarderNo == model.BoarderNo);
                if (diningBoarder != null && DataController.IsReturnable(diningBoarder, token))
                {
                    DiningBill diningBill = await diningBillRepository.FilterOneAsync(x =>
                        x.InstituteId == diningBoarder.InstituteId && x.CampusId == diningBoarder.CampusId &&
                        x.BillMonth > model.FormDate && x.BillMonth < model.ToDate);
                    if (diningBill != null)
                    {
                        List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                            x.BoarderId == diningBoarder.Id && x.DiningBillId == diningBill.Id);
                        List<DiningBillCollection> diningBillCollections = await diningBillCollectionRepository.FilterListAsync(x =>
                        x.BoarderId == diningBoarder.Id && x.CollectionDate > model.FormDate && x.CollectionDate < model.ToDate);
                        if (diningBillDetails.Count > 0)
                        {
                            DiningBoarderStatementVM diningBoarderStatementVM = new DiningBoarderStatementVM();
                            List<Statement> statements = new List<Statement>();
                            foreach (DiningBillDetails diningBillDetail in diningBillDetails)
                            {
                                Statement statement = new Statement
                                {
                                    Date = Formater.FormatDateddMMyyyy(diningBill.CreatedDate),
                                    Payable = diningBillDetail.TotalBill,
                                    PersonId = diningBillDetail.BoarderId,
                                    Paid = 0,
                                    Instruments = diningBill.DiningBillNo,
                                    Particulars = "Payable as per dining bill for month " + Formater.GetMonthName(diningBill.BillMonth),
                                };
                                statements.Add(statement);
                            }
                            foreach (DiningBillCollection diningBillCollection in diningBillCollections)
                            {
                                Statement statement = new Statement
                                {
                                    Date = Formater.FormatDateddMMyyyy(diningBillCollection.CollectionDate),
                                    Payable = 0,
                                    PersonId = diningBillCollection.BoarderId,
                                    Paid = diningBillCollection.Amount,
                                    Instruments = diningBillCollection.DiningBillCollectionNo,
                                    Particulars = "Collection as per details",
                                };
                                statements.Add(statement);
                            }
                            RequestMessage requestMessage = new RequestMessage();
                            requestMessage.Content = JsonConvert.SerializeObject(diningBoarder.Id);
                            await diningBoarderBLL.SearchDetails(requestMessage, response, token);
                            diningBoarderStatementVM.BoarderDetails = (DiningBoarderDetailsViewModel)response.ResponseObj;
                            if (statements.Count > 0)
                            {
                                List<Statement> result = new List<Statement>();
                                foreach (var item in RunningTotal(statements))
                                {
                                    result.Add(item);
                                }
                                diningBoarderStatementVM.Statements = result;
                                diningBoarderStatementVM.TotalPaid = result.Sum(x => x.Paid);
                                diningBoarderStatementVM.TotalPayable = result.Sum(x => x.Payable);
                                diningBoarderStatementVM.TotalBalance = diningBoarderStatementVM.TotalPayable - diningBoarderStatementVM.TotalPaid;
                                response.ResponseObj = diningBoarderStatementVM;
                                response.StatusCode = (byte)StatusCode.Success;
                            }
                        }
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Not found any boarder with id " + model.BoarderNo;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> GetDiningExpenseStatement(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBill diningBill1 = await diningBillRepository.GetAsync(JsonConvert.DeserializeObject<long>(requestMessage.Content));
                DateTime billOfMonth = diningBill1.BillMonth;
                List<DiningExpense> diningExpenses = await diningExpenseRepository.FilterListAsync(
                     x => x.InstituteId == diningBill1.InstituteId && x.CampusId == diningBill1.CampusId
                     && x.ExpenseDate.Month == billOfMonth.Month && x.ExpenseDate.Year == billOfMonth.Year
                     && x.Status == (byte)ExpenseVoucherStatus.Approved);
                DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(x =>
                    x.AdjustmentForMonth.Month == diningBill1.BillMonth.Month && x.AdjustmentForMonth.Year == diningBill1.BillMonth.Year
                    && x.InstituteId == diningBill1.InstituteId && x.CampusId == diningBill1.CampusId);

                List<DiningStockAdjustmentDetails> diningStockAdjustmentDetails = await diningStockAdjustmentRepositoryDetails
                    .FilterListAsync(x => x.DiningStockAdjustmentId == diningStockAdjustment.Id);

                List<DiningStockAdjustment> allData = await diningStockAdjustmentRepository.FilterListAsync(x =>
                            x.InstituteId == diningStockAdjustment.InstituteId && x.CampusId == diningStockAdjustment.CampusId
                            && x.AdjustmentForMonth.Month < diningStockAdjustment.AdjustmentForMonth.Month
                            && x.AdjustmentForMonth.Year <= diningStockAdjustment.AdjustmentForMonth.Year);
                List<DiningStockAdjustmentDetails> prevMonthAdjustments = new List<DiningStockAdjustmentDetails>();
                DiningStockAdjustment prevAdjust = allData.OrderBy(x => x.AdjustmentForMonth).LastOrDefault();
                if (prevAdjust != null)
                {
                    prevMonthAdjustments = await diningStockAdjustmentRepositoryDetails
                        .FilterListAsync(x => x.DiningStockAdjustmentId == prevAdjust.Id);
                }
                var stockProductExpense = diningExpenses.FindAll(x => x.IsStockProduct).GroupBy(x => x.ProductName);
                var nonStockProductExpense = diningExpenses.FindAll(x => !x.IsStockProduct);
                if (diningExpenses.Count > 0 || prevMonthAdjustments.Count > 0)
                {
                    List<DiningExpenseStatementViewModel> statementViewModels = new List<DiningExpenseStatementViewModel>();
                    foreach (var diningExpense in nonStockProductExpense)
                    {
                        DiningExpenseStatementViewModel diningExpenseStatementViewModel = new DiningExpenseStatementViewModel
                        {
                            Date = diningExpense.ExpenseDate,
                            DateString = Formater.FormatDateddMMyyyy(diningExpense.ExpenseDate),
                            ProductName = diningExpense.ProductName,
                            Amount = diningExpense.Amount,
                            Qty = diningExpense.Qty,
                            Rate = diningExpense.Rate,
                            IsStockProduct = false,
                            TransactionId = diningExpense.DiningExpenseNo,
                            Unit = diningExpense.Unit,
                            ProductTypeName = "Non-Stock Product",
                        };
                        statementViewModels.Add(diningExpenseStatementViewModel);
                    }
                    foreach (var diningExpense in stockProductExpense)
                    {
                        StockProduct stockProduct = await stockProductRepository.GetAsync(Convert.ToInt64(diningExpense.First().ProductName));
                        decimal amount = diningExpense.Sum(x => x.Amount);
                        decimal qty = diningExpense.Sum(x => x.Qty);
                        DiningExpenseStatementViewModel diningExpenseStatementViewModel = new DiningExpenseStatementViewModel
                        {
                            Date = diningExpense.First().ExpenseDate,
                            DateString = Formater.FormatDateddMMyyyy(diningExpense.First().ExpenseDate),
                            ProductName = stockProduct.ProductName,
                            Amount = amount,
                            Qty = qty,
                            IsStockProduct = true,
                            TransactionId = diningExpense.First().DiningExpenseNo,
                            Unit = diningExpense.First().Unit,
                            ProductTypeName = "Stock Product",
                        };
                        diningExpenseStatementViewModel.Rate = diningExpenseStatementViewModel.Amount / diningExpenseStatementViewModel.Qty;
                        if (prevMonthAdjustments.Count == 0)
                        {
                            var adjustProduct = diningStockAdjustmentDetails.Find(x => x.ProductId == stockProduct.Id);
                            if (adjustProduct != null)
                            {
                                diningExpenseStatementViewModel.Qty = qty - adjustProduct.RemainingQty;
                                diningExpenseStatementViewModel.Amount = amount - adjustProduct.Price;
                                diningExpenseStatementViewModel.Rate = diningExpenseStatementViewModel.Amount / diningExpenseStatementViewModel.Qty;
                            }

                        }
                        statementViewModels.Add(diningExpenseStatementViewModel);
                    }
                    foreach (var item in prevMonthAdjustments)
                    {
                        var stockProduct = await stockProductRepository.GetAsync(item.ProductId);
                        var stockProductForThisMonth = diningStockAdjustmentDetails.Find(x => x.ProductId == item.ProductId);
                        DiningExpenseStatementViewModel diningExpenseStatementViewModel = new DiningExpenseStatementViewModel
                        {
                            Date = diningStockAdjustment.AdjustmentForMonth,
                            DateString = Formater.FormatDateddMMyyyy(diningStockAdjustment.AdjustmentForMonth),
                            ProductName = stockProduct.ProductName,
                            Qty = item.RemainingQty - (stockProductForThisMonth != null ? stockProductForThisMonth.RemainingQty : 0),
                            IsStockProduct = true,
                            TransactionId = diningStockAdjustment.DiningStockAdjustmentNo,
                            Unit = stockProduct.Unit,
                            ProductTypeName = "Stock Product",
                            Amount = item.Price - (stockProductForThisMonth != null ? stockProductForThisMonth.Price : 0)
                        };
                        diningExpenseStatementViewModel.Rate = diningExpenseStatementViewModel.Amount / diningExpenseStatementViewModel.Qty;
                        statementViewModels.Add(diningExpenseStatementViewModel);
                    }
                    response.ResponseObj = statementViewModels.OrderBy(x => x.IsStockProduct).ThenBy(y => y.Date).ToList();
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Not found any expenses";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> GetDiningBoarderBillDetails(SearchDiningBoarderBillDetails model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(model.Id);
                DateTime date = DateTime.Parse(model.Month);
                if (diningBoarder != null && DataController.IsReturnable(diningBoarder, token))
                {
                    DiningBill diningBill = await diningBillRepository.FilterOneAsync(x =>
                    x.InstituteId == diningBoarder.InstituteId && x.CampusId == diningBoarder.CampusId
                    && x.BillMonth.Month == date.Month);

                    if (diningBill != null)
                    {
                        List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                            x.DiningBillId == diningBill.Id);

                        List<DiningMeal> diningMeals = await diningMealRepository.FilterListAsync(x =>
                          x.InstituteId == diningBoarder.InstituteId && x.CampusId == diningBoarder.CampusId
                          && x.Status == (byte)Status.Active);

                        DiningBoarderBillDetailsVM diningBoarderBillDetailsVM = new DiningBoarderBillDetailsVM()
                        {
                            TotalMealInMonth = diningBillDetails.Sum(x => x.TotalMeal),
                            TotalExpenseInMonth = diningBillDetails.Sum(x => x.TotalBill),
                            BillOfMonth = Formater.GetMonthName(date) + " " + date.Year,
                            Id = diningBoarder.DiningBoarderNo,
                            DiningMeal = diningMeals.OrderBy(x => x.MealSequence).ToList()
                        };
                        if (diningBoarder.BoarderTypeId == (byte)BoarderType.Employee)
                        {
                            Employee employee = await employeeRepository.GetAsync(diningBoarder.DiningBoarderId);
                            diningBoarderBillDetailsVM.BoarderName = employee.EmployeeName;
                        }
                        else if (diningBoarder.BoarderTypeId == (byte)BoarderType.Outer)
                        {
                            DiningBoarderExternal diningBoarderExternal = await diningBoarderExternalRepository.GetAsync(diningBoarder.DiningBoarderId);
                            diningBoarderBillDetailsVM.BoarderName = diningBoarderExternal.Name;
                        }
                        else
                        {
                            Student student = await studentRepository.GetAsync(diningBoarder.DiningBoarderId);
                            diningBoarderBillDetailsVM.BoarderName = student.StudentName;
                        };
                        List<DiningMealAttendance> diningMealManages = await diningMealAttendanceRepository.FilterListAsync(x =>
                        x.InstituteId == diningBoarder.InstituteId && x.CampusId == diningBoarder.CampusId
                        && x.MealDate.Month == date.Month);

                        foreach (DiningMealAttendance diningMealManage in diningMealManages)
                        {
                            var individualBorderRecord = JsonConvert
                                .DeserializeObject<List<BoarderWithTakenMealVM>>(diningMealManage.BoarderWithTakenMeal);
                            MealWithDate mealWithDate = new MealWithDate();
                            mealWithDate.MealDate = Formater.FormatDateddMMyyyy(diningMealManage.MealDate) + " - " +
                                Formater.GetShortMonthName(diningMealManage.MealDate);
                            var data = individualBorderRecord.Where(x => x.BoarderId == diningBoarder.Id).FirstOrDefault();
                            if (data != null && HasTakenMeal(data, diningMeals))
                            {
                                mealWithDate.TakenMeals.AddRange(GetBorderMealObject(data, diningMeals, true));
                                mealWithDate.TotalMeal = mealWithDate.TakenMeals.Sum(x => x.MealSize);
                                diningBoarderBillDetailsVM.MealWithDates.Add(mealWithDate);
                            }
                        }
                        DiningBillDetails diningBillDetails1 = diningBillDetails.FirstOrDefault(x => x.BoarderId == diningBoarder.Id);
                        if (diningBillDetails1 != null)
                        {
                            var data = JsonConvert
                                .DeserializeObject<BoarderWithTakenMealVM>(diningBillDetails1.MealAccounts);
                            diningBoarderBillDetailsVM.Summery.AddRange(GetBorderMealObject(data, diningMeals, true));
                            diningBoarderBillDetailsVM.SummeryTotal = diningBoarderBillDetailsVM.Summery.Sum(x => x.MealSize);
                            diningBoarderBillDetailsVM.BoarderTakenMealInMonth = diningBillDetails1.TotalMeal;
                            diningBoarderBillDetailsVM.BoarderBillInMonth = diningBillDetails1.TotalBill;
                            diningBoarderBillDetailsVM.MealRateInMonth = diningBill.MealRate;
                        }
                        response.ResponseObj = diningBoarderBillDetailsVM;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Not found any boarder";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public IEnumerable<Statement> RunningTotal(List<Statement> diningBoarderStatements)
        {
            decimal runningTotal = 0;
            foreach (var diningBoarderStatement in diningBoarderStatements)
            {
                runningTotal = runningTotal + Convert.ToDecimal((diningBoarderStatement.Payable - diningBoarderStatement.Paid));
                diningBoarderStatement.Balance = runningTotal;
                yield return diningBoarderStatement;
            }
        }
        public List<TakenMeal> GetBorderMealObject(BoarderWithTakenMealVM mealCountVM, List<DiningMeal> diningMeals, bool isWithMealValue)
        {
            List<TakenMeal> list = new List<TakenMeal>();
            var keys = Enum.GetValues(typeof(DiningMealNumber));

            foreach (var key in mealCountVM.GetType().GetProperties())
            {
                if (key.GetIndexParameters().Length > 0)
                {
                    continue;
                }
                decimal value = Convert.ToDecimal(key.GetValue(mealCountVM, null));
                if (value > 0)
                {
                    if (key.Name == "BoarderId")
                    {
                        continue;
                    }
                    var returnClass = new TakenMeal();
                    foreach (var key1 in keys)
                    {
                        if (key1.ToString() == key.Name)
                        {
                            returnClass.DiningMealNumberId = (byte)(int)key1;
                            break;
                        }
                    }
                    DiningMeal diningMeal = diningMeals.Find(e => e.DiningMealNumberId == returnClass.DiningMealNumberId);
                    returnClass.MealSize = isWithMealValue ? value : (int)(value / diningMeal.MealSize);
                    returnClass.MealSequence = diningMeal.MealSequence;
                    list.Add(returnClass);
                }
                else
                {
                    foreach (var key1 in keys)
                    {
                        if (key1.ToString() == key.Name)
                        {
                            foreach (var diningMeal in diningMeals)
                            {
                                if (diningMeal.DiningMealNumberId == (byte)(int)key1)
                                {
                                    var returnClass = new TakenMeal();
                                    returnClass.MealSize = 0;
                                    list.Add(returnClass);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return list.OrderBy(x => x.MealSequence).ToList();
        }
        public bool HasTakenMeal(BoarderWithTakenMealVM boarderWithTakenMealVM, List<DiningMeal> diningMeals)
        {
            bool result = false;
            foreach (DiningMeal diningMeal in diningMeals)
            {
                result = (decimal)boarderWithTakenMealVM[Formater.GetEnumValueDescription<DiningMealNumber>(diningMeal.DiningMealNumberId)] > 0;
                if (result)
                    break;
            }
            return result;
        }
    }
}
