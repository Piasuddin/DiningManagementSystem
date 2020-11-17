using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.Repository;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;

namespace MERP.FinanceBLL
{
    public class DiningMealAttendanceBLL
    {
        private readonly IRepository<DiningMealAttendance> diningMealAttendanceRepository;
        private readonly IRepository<DiningBoarder> diningBoarderRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<DiningMeal> diningMealRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly IRepository<DiningBoarderExternal> diningBoarderExternalRepository;
        private readonly IRepository<User> userRepository;
        private readonly DiningBoarderBLL diningBoarderBLL;
        private readonly IRepository<DiningStockAdjustment> diningStockAdjustmentRepository;
        private readonly IRepository<DiningMealManage> diningMealManageRepository;

        public DiningMealAttendanceBLL(IRepository<DiningMealAttendance> diningMealAttendanceRepository,
            IRepository<DiningBoarder> diningBoarderRepository, IRepository<Employee> employeeRepository,
            IRepository<Student> studentRepository, IRepository<DiningMeal> diningMealRepository,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository,
            IRepository<DiningBoarderExternal> diningBoarderExternalRepository, IRepository<User> userRepository,
            DiningBoarderBLL diningBoarderBLL, IRepository<DiningStockAdjustment> diningStockAdjustmentRepository,
            IRepository<DiningMealManage> diningMealManageRepository)
        {
            this.diningMealAttendanceRepository = diningMealAttendanceRepository;
            this.diningBoarderRepository = diningBoarderRepository;
            this.employeeRepository = employeeRepository;
            this.studentRepository = studentRepository;
            this.diningMealRepository = diningMealRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.diningBoarderExternalRepository = diningBoarderExternalRepository;
            this.userRepository = userRepository;
            this.diningBoarderBLL = diningBoarderBLL;
            this.diningStockAdjustmentRepository = diningStockAdjustmentRepository;
            this.diningMealManageRepository = diningMealManageRepository;
        }
        public async Task<ResponseMessage> CreateDiningMealManage(DiningMealAttendanceViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                if (model != null)
                {
                    CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningMealAttendance>
                        (await diningMealAttendanceRepository.GetAsync(), token);
                    DiningMealAttendance diningMealManage = new DiningMealAttendance
                    {
                        Id = codeAndId.Id + 1,
                        InstituteId = token.InstituteID,
                        CampusId = model.CampusId == null ? token.CampusID : (long)model.CampusId,
                        DiningMealAttendanceNo = codeAndId.No,
                        MealDate = model.MealDate,
                        BoarderWithTakenMeal = model.BoarderWithTakenMeal,
                        CreatedBy = token.UserID,
                        CreatedDate = DateTime.UtcNow,
                        Status = (byte)DiningMealStatus.Taken
                    };
                    DiningMealAttendance result = await diningMealAttendanceRepository.AddAsync(diningMealManage);

                    if (result != null)
                    {
                        response.Message = "Dining meal attendance created successfully.";
                        response.ResponseObj = result;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed to Create dining meal attendance! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> CreateMealAttendanceByMealManage(ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DateTime date = DateTime.UtcNow;

                DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(
                    x => x.AdjustmentForMonth.Month == date.Month && x.AdjustmentForMonth.Year == date.Year);
                if (diningStockAdjustment == null)
                {
                    List<DiningMealAttendance> diningMealAttendances = await DataController.GetDataList<DiningMealAttendance>(token);
                    DateTime lastAttendanceDate = diningMealAttendances.OrderBy(x => x.MealDate).LastOrDefault().MealDate;
                    List<DiningMeal> diningMeals = await diningMealRepository.FilterListAsync(x =>
                        x.InstituteId == token.InstituteID && x.CampusId == token.CampusID
                        && x.Status == (byte)Status.Active);
                    List<DiningMealAttendance> addedDiningMealAttendances = new List<DiningMealAttendance>();

                    while (lastAttendanceDate.Date < date.Date)
                    {
                        lastAttendanceDate = lastAttendanceDate.AddDays(1);
                        List<DiningBoarder> diningBoarders = await diningBoarderRepository.FilterListAsync(x =>
                            x.InstituteId == token.InstituteID && x.CampusId == token.CampusID
                            && x.EnrollmentDate.Date <= lastAttendanceDate.Date && x.Status == (byte)Status.Active);
                        CodeAndId codeAndId = null;
                        if (addedDiningMealAttendances.Count > 0)
                        {
                            codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningMealAttendance>
                               (addedDiningMealAttendances, token);
                        }
                        else
                        {
                            codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningMealAttendance>
                            (await diningMealAttendanceRepository.GetAsync(), token);
                        }
                        
                        DiningMealAttendance diningMealAttendance = new DiningMealAttendance
                        {
                            Id = codeAndId.Id + 1,
                            InstituteId = token.InstituteID,
                            CampusId = token.CampusID,
                            DiningMealAttendanceNo = codeAndId.No,
                            MealDate = lastAttendanceDate,
                            BoarderWithTakenMeal = await GenerateBoarderTakenMeal(diningBoarders, diningMeals, lastAttendanceDate),
                            CreatedBy = token.UserID,
                            CreatedDate = DateTime.UtcNow,
                            Status = (byte)DiningMealStatus.Taken
                        };
                        addedDiningMealAttendances.Add(diningMealAttendance);
                    }

                    var result = await diningMealAttendanceRepository.AddRangeAsync(addedDiningMealAttendances);

                    if (result.Count > 0)
                    {
                        response.Message = "Dining meal attendance created successfully.";
                        response.ResponseObj = result;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed to Create dining meal attendance! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningMealManage(DiningMealAttendanceViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                if (model != null)
                {
                    DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.GetAsync((long)model.Id);
                    if (diningMealManage != null)
                    {
                        diningMealManage.BoarderWithTakenMeal = model.BoarderWithTakenMeal;
                        diningMealManage.UpdatedBy = token.UserID;
                        diningMealManage.UpdatedDate = DateTime.UtcNow;

                        DiningMealAttendance result = await diningMealAttendanceRepository.UpdateAsync(diningMealManage);

                        if (result != null)
                        {
                            response.Message = "Dining meal attendance updated successfully.";
                            response.ResponseObj = result;
                            response.StatusCode = (byte)StatusCode.Success;
                        }
                    }
                    else
                    {
                        response.Message = "Not found any meal attendance.";
                        response.StatusCode = (byte)StatusCode.Failed;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed to Update dining meal attendance! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningMeal(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.GetAsync(id);
                    if (diningMealManage != null && DataController.IsReturnable(diningMealManage, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMealManage;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningMealAttendance> diningMealManages = await DataController.GetDataList<DiningMealAttendance>(token);
                    if (diningMealManages.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMealManages;
                    }
                    else
                    {
                        response.Message = "Not found any dining meal attendance";
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
        public async Task<ResponseMessage> SearchDiningMealManageForTable(ResponseMessage response,
            RequestMessage requestObject, UserAuthorization token)
        {
            try
            {
                var obj = new { year = 0, month = 0, isNext = false };
                dynamic key = JsonConvert.DeserializeObject(requestObject.Content, obj.GetType());

                List<DiningMealAttendance> allData = await DataController.GetDataList<DiningMealAttendance>(token);
                if (allData.Count > 0)
                {
                    List<DiningMeal> diningMeals = await DataController.GetDataList<DiningMeal>(token);
                    var data = allData.Where(x => x.MealDate.Month == key.month && x.MealDate.Year == key.year).ToList();

                    var months = allData.Where(x => x.MealDate.Year == key.year).Select(y => y.MealDate.Month).Distinct()
                        .OrderBy(z => z).ToList();
                    var years = allData.Select(x => x.MealDate.Year).Distinct().OrderBy(y => y).ToList();
                    if (data.Count == 0 && key.isNext)
                    {
                        data = allData.Where(x => x.MealDate.Month == months.FirstOrDefault() && x.MealDate.Year == key.year).ToList();
                    }
                    else if (data.Count == 0)
                    {
                        data = allData.Where(x => x.MealDate.Month == months.LastOrDefault() && x.MealDate.Year == key.year).ToList();
                    }

                    var diningMealManages = data;

                    List<DiningMealManageDetailsViewModel> diningMealManageDetailsViewModels = new List<DiningMealManageDetailsViewModel>();
                    foreach (DiningMealAttendance diningMealManage in diningMealManages)
                    {
                        DiningMealManageDetailsViewModel diningMealManageDetailsViewModel = new DiningMealManageDetailsViewModel
                        {
                            Id = diningMealManage.Id,
                            MealDate = diningMealManage.MealDate,
                            Status = diningMealManage.Status,
                            MealDateString = Formater.FormatDateddMMyyyy(diningMealManage.MealDate),
                            InstituteId = diningMealManage.InstituteId,
                            InstituteName = diningMealManage.InstituteId > 0 ? instituteRepository.Get(diningMealManage.InstituteId).InstituteName : null,
                            CampusName = diningMealManage.CampusId > 0 ? campusRepository.Get(diningMealManage.CampusId).CampusName : null,
                            CampusId = diningMealManage.CampusId,
                            StatusName = Formater.GetEnumValueDescription<DiningMealStatus>(diningMealManage.Status)
                        };

                        var individualBorderRecord = JsonConvert.DeserializeObject<List<BoarderWithTakenMealVM>>(diningMealManage.BoarderWithTakenMeal);

                        int count = 0;
                        for (int i = 0; i < individualBorderRecord.Count(); i++)
                        {
                            if (HasTakenMeal(individualBorderRecord[i], diningMeals))
                            {
                                count++;
                            };
                        }
                        diningMealManageDetailsViewModel.TotalBorder = count;

                        foreach (DiningMeal diningMeal in diningMeals)
                        {
                            diningMealManageDetailsViewModel.TakenMeals.Add(GetMealCount(individualBorderRecord, diningMeal));
                        }
                        diningMealManageDetailsViewModel.TakenMeals = diningMealManageDetailsViewModel.TakenMeals.OrderBy(x => x.MealSequence).ToList();
                        diningMealManageDetailsViewModels.Add(diningMealManageDetailsViewModel);
                    }
                    diningMeals = diningMeals.OrderBy(x => x.MealSequence).ToList();
                    response.ResponseObj = new
                    {
                        diningMealManage = diningMealManageDetailsViewModels.OrderBy(x => x.MealDate).ToList(),
                        diningMeals,
                        others = new { years = years, months = months }
                    };
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Not found any data";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningMealManage(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.GetAsync(id);
                    if (diningMealManage != null)
                    {
                        await diningMealAttendanceRepository.DeleteAsync(diningMealManage);
                        response.Message = "Dining meal attendance deleted succesfully.";
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMealManage;
                    }
                    else
                    {
                        response.Message = "Not found any dining meal attendance.";
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
        public async Task<ResponseMessage> SearchDiningMealAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                var data = new { date = DateTime.UtcNow, campusId = 0 };
                dynamic searchKey = JsonConvert.DeserializeObject(requestMessage.Content, data.GetType());
                DateTime date = searchKey.date;
                long id = (long)searchKey.campusId;
                if (date != null)
                {
                    if (!(date.Date > DateTime.UtcNow.Date))
                    {
                        DiningStockAdjustment diningStockAdjustment = await diningStockAdjustmentRepository.FilterOneAsync(
                            x => x.AdjustmentForMonth.Month == date.Month && x.AdjustmentForMonth.Year == date.Year);
                        if (diningStockAdjustment == null)
                        {
                            DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.FilterOneAsync(x =>
                                x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                && x.MealDate.Date == date.Date);
                            if (diningMealManage == null)
                            {

                                List<DiningMealAttendance> diningMealAttendances = await DataController.GetDataList<DiningMealAttendance>(token);

                                DiningMealAttendance prevDayMealManage = await diningMealAttendanceRepository.FilterOneAsync(x =>
                                    x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                    && x.MealDate.Date == date.AddDays(-1).Date);

                                DateTime lastAttendanceDate = diningMealAttendances.OrderBy(x => x.MealDate).Last().MealDate;

                                if (diningMealAttendances.Count == 0 || (prevDayMealManage == null && lastAttendanceDate.Month != date.Month)
                                || prevDayMealManage != null)
                                {


                                    List<DiningBoarder> diningBoarders = await diningBoarderRepository.FilterListAsync(x =>
                                        x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID)
                                        && x.EnrollmentDate <= date.Date && x.Status == (byte)Status.Active);
                                    diningBoarders = diningBoarders.OrderBy(x => x.BoarderTypeId).ToList();
                                    List<DiningMeal> diningMeals = await diningMealRepository.FilterListAsync(x =>
                                        x.InstituteId == token.InstituteID && (id > 0 ? x.CampusId == id : x.CampusId == token.CampusID));
                                    List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                                    List<DiningMealDetailsViewModel> diningMealViewModels = GetDiningMealViewModels(diningMeals);

                                    byte prevTypeId = 0;
                                    int rowId = 1;
                                    foreach (DiningBoarder diningBoarder in diningBoarders)
                                    {
                                        if (prevTypeId != diningBoarder.BoarderTypeId)
                                        {
                                            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                                            {
                                                IsGrouping = true,
                                                BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId)
                                            };
                                            diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                                        }
                                        DiningBoarderDetailsViewModel diningBoarderViewModel = new DiningBoarderDetailsViewModel()
                                        {
                                            BoarderNo = diningBoarder.DiningBoarderNo,
                                            BoarderTypeId = diningBoarder.BoarderTypeId,
                                            DiningBoarderId = diningBoarder.DiningBoarderId,
                                            BoarderName = diningBoarder.BoarderTypeId == (byte)BoarderType.Student ?
                                                studentRepository.Get(diningBoarder.DiningBoarderId).StudentName :
                                                diningBoarder.BoarderTypeId == (byte)BoarderType.Outer ?
                                                diningBoarderExternalRepository.Get(diningBoarder.DiningBoarderId).Name :
                                                employeeRepository.Get(diningBoarder.DiningBoarderId).EmployeeName,
                                            Id = diningBoarder.Id,
                                            RowId = rowId,
                                            BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId),
                                            DiningMeals = diningMealViewModels.OrderBy(x => x.MealSequence).ToList()
                                        };
                                        rowId++;
                                        prevTypeId = diningBoarder.BoarderTypeId;
                                        diningBoarderDetailsViewModels.Add(diningBoarderViewModel);
                                    }
                                    DiningMealManageAddViewModel diningMealManageAddViewModel = new DiningMealManageAddViewModel
                                    {
                                        DiningBoarders = diningBoarderDetailsViewModels,
                                        MealDateString = Formater.FormatDateddMMyyyy(date),
                                        MealDate = date,
                                        DiningMeals = diningMeals.OrderBy(x => x.MealSequence).ToList()
                                    };
                                    if (diningMealManageAddViewModel != null)
                                    {
                                        response.ResponseObj = diningMealManageAddViewModel;
                                        response.StatusCode = (byte)StatusCode.Success;
                                    }
                                }
                                else
                                {
                                    response.Message = "Please first complete the attendence on " +
                                        Formater.FormatDateddMMyyyy(date.AddDays(-1));
                                    response.StatusCode = (byte)StatusCode.Failed;
                                }
                            }
                            else
                            {
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = "You have already added meal on " + Formater.FormatDateddMMyyyy(date);
                            }
                        }
                        else
                        {
                            response.StatusCode = (byte)StatusCode.Failed;
                            response.Message = "The accountants of " + Formater.GetMonthName(date) + " has already been closed." +
                                "You can't add more attendance in " + Formater.GetMonthName(date) + ". Try for next month.";
                        }
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.Failed;
                        response.Message = "You can't add advance meal attendence";
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Date can't be null";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningMealEditData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(requestMessage.Content));
                DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.FilterOneAsync(x => x.Id == id);
                if (diningMealManage != null && DataController.IsReturnable(diningMealManage, token))
                {
                    List<DiningMeal> diningMeals = await diningMealRepository.FilterListAsync(x => token.CampusID > 0 ?
                    (x.InstituteId == token.InstituteID && x.CampusId == token.CampusID) : (x.InstituteId == token.InstituteID));
                    List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                    var individualBorderRecord = JsonConvert.DeserializeObject<List<BoarderWithTakenMealVM>>(diningMealManage.BoarderWithTakenMeal);
                    List<DiningBoarder> diningBoarders = new List<DiningBoarder>();
                    List<DiningMealDetailsViewModel> diningMealViewModels = GetDiningMealViewModels(diningMeals);
                    foreach (var data in individualBorderRecord)
                    {
                        diningBoarders.Add(await diningBoarderRepository.GetAsync(data.BoarderId));
                    }
                    List<DiningBoarder> boarders = await diningBoarderRepository.FilterListAsync(x => x.EnrollmentDate <= diningMealManage.MealDate
                        && !diningBoarders.Select(x2 => x2.Id).Contains(x.Id) && x.InstituteId == token.InstituteID && x.CampusId == token.CampusID
                        && x.Status == (byte)Status.Active);
                    if (boarders.Count > 0)
                        diningBoarders.AddRange(boarders);
                    diningBoarders = diningBoarders.OrderBy(x => x.BoarderTypeId).ToList();
                    byte prevTypeId = 0;
                    int rowId = 1;
                    foreach (var diningBoarder in diningBoarders)
                    {
                        if (prevTypeId != diningBoarder.BoarderTypeId)
                        {
                            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                            {
                                IsGrouping = true,
                                BoarderTypeName = Enum.GetName(typeof(BoarderType), diningBoarder.BoarderTypeId)
                            };
                            diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                        }
                        var mealRecord = individualBorderRecord.Find(x => x.BoarderId == diningBoarder.Id);
                        DiningBoarderDetailsViewModel diningBoarderViewModel = new DiningBoarderDetailsViewModel()
                        {
                            RowId = rowId,
                            BoarderNo = diningBoarder.DiningBoarderNo,
                            BoarderTypeId = diningBoarder.BoarderTypeId,
                            DiningBoarderId = diningBoarder.DiningBoarderId,
                            BoarderName = diningBoarder.BoarderTypeId == (byte)BoarderType.Student ?
                                studentRepository.Get(diningBoarder.DiningBoarderId).StudentName :
                                diningBoarder.BoarderTypeId == (byte)BoarderType.Outer ?
                                diningBoarderExternalRepository.Get(diningBoarder.DiningBoarderId).Name :
                                employeeRepository.Get(diningBoarder.DiningBoarderId).EmployeeName,
                            Id = diningBoarder.Id,
                            DiningMeals = mealRecord == null ? diningMealViewModels.OrderBy(x => x.MealSequence).ToList() :
                                GetBorderMealObject(mealRecord, diningMeals).OrderBy(x => x.MealSequence).ToList(),
                            BoarderTypeName = Enum.GetName(typeof(BoarderType), diningBoarder.BoarderTypeId),
                        };
                        rowId++;
                        prevTypeId = diningBoarder.BoarderTypeId;
                        diningBoarderDetailsViewModels.Add(diningBoarderViewModel);
                    }
                    DiningMealManageAddViewModel diningMealManageAddViewModel = new DiningMealManageAddViewModel
                    {
                        DiningBoarders = diningBoarderDetailsViewModels,
                        MealDateString = Formater.FormatDateddMMyyyy(diningMealManage.MealDate),
                        MealDate = diningMealManage.MealDate,
                        DiningMeals = diningMeals.OrderBy(x => x.MealSequence).ToList()
                    };
                    if (diningMealManageAddViewModel != null)
                    {
                        response.ResponseObj = diningMealManageAddViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Not found any meal attendance data";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public TakenMeals GetMealCount(List<BoarderWithTakenMealVM> meals, DiningMeal diningMeal)
        {
            TakenMeals mealCountVM = null;
            var keys = Enum.GetValues(typeof(DiningMealNumber));
            foreach (var key in keys)
            {
                if (diningMeal.DiningMealNumberId == (int)key)
                {
                    List<decimal> data = meals.Where(x => (decimal)x[key.ToString()] > 0).Select(y => (decimal)y[key.ToString()] / diningMeal.MealSize).ToList();
                    mealCountVM = new TakenMeals
                    {
                        TakenMealCount = data.Sum(x => x),
                        DiningMealNumberId = diningMeal.DiningMealNumberId,
                        MealName = diningMeal.MealName
                    };
                    break;
                }
            }
            if (mealCountVM == null)
            {
                mealCountVM = new TakenMeals();
                mealCountVM.DiningMealNumberId = diningMeal.DiningMealNumberId;
                mealCountVM.TakenMealCount = 0;
            }
            return mealCountVM;
        }
        public List<DiningMealDetailsViewModel> GetBorderMealObject(BoarderWithTakenMealVM mealCountVM, List<DiningMeal> diningMeals)
        {
            List<DiningMealDetailsViewModel> list = new List<DiningMealDetailsViewModel>();
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
                    var returnClass = new DiningMealDetailsViewModel();
                    foreach (var key1 in keys)
                    {
                        if (key1.ToString() == key.Name)
                        {
                            returnClass.DiningMealNumberId = (byte)(int)key1;
                            break;
                        }
                    }
                    DiningMeal diningMeal = diningMeals.Find(e => e.DiningMealNumberId == returnClass.DiningMealNumberId);
                    returnClass.Id = diningMeal.Id;
                    returnClass.MealName = diningMeal.MealName;
                    returnClass.MealNo = diningMeal.DiningMealNo;
                    returnClass.MealSize = diningMeal.MealSize;
                    returnClass.MealTime = diningMeal.MealTime;
                    returnClass.MealKey = diningMeal.MealKey;
                    returnClass.MealSequence = diningMeal.MealSequence;
                    returnClass.DiningMealNumberString = Enum.GetName(typeof(DiningMealNumber), diningMeal.DiningMealNumberId);
                    returnClass.MealNumber = (int)(value / diningMeal.MealSize);
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
                                    var returnClass = new DiningMealDetailsViewModel();
                                    returnClass.Id = diningMeal.Id;
                                    returnClass.MealName = diningMeal.MealName;
                                    returnClass.MealNo = diningMeal.DiningMealNo;
                                    returnClass.MealSize = diningMeal.MealSize;
                                    returnClass.MealTime = diningMeal.MealTime;
                                    returnClass.MealKey = diningMeal.MealKey;
                                    returnClass.MealSequence = diningMeal.MealSequence;
                                    returnClass.DiningMealNumberString = Enum.GetName(typeof(DiningMealNumber), diningMeal.DiningMealNumberId);
                                    returnClass.DiningMealNumberId = (byte)(int)key1;
                                    returnClass.MealNumber = 0;
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
                result = (decimal)boarderWithTakenMealVM[Enum.GetName(typeof(DiningMealNumber), diningMeal.DiningMealNumberId)] > 0;
                if (result)
                    break;
            }
            return result;
        }
        public async Task<ResponseMessage> SearchDetails(RequestMessage requestObject,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = JsonConvert.DeserializeObject<long>(requestObject.Content);
                if (id > 0)
                {
                    DiningMealAttendance diningMealManage = await diningMealAttendanceRepository.GetAsync(id);
                    List<DiningMeal> diningMeals = await DataController.GetDataList<DiningMeal>(token);
                    if (diningMealManage != null && DataController.IsReturnable(diningMealManage, token))
                    {
                        List<BoarderWithTakenMealVM> takenMeals = JsonConvert.DeserializeObject<List<BoarderWithTakenMealVM>>(diningMealManage.BoarderWithTakenMeal);
                        DiningMealManageDetailsViewModel diningMealDetailsViewModel = new DiningMealManageDetailsViewModel
                        {
                            DiningMealManageNo = diningMealManage.DiningMealAttendanceNo,
                            MealDateString = Formater.FormatDateddMMyyyy(diningMealManage.MealDate),
                            TotalBorder = takenMeals.Count(),
                            CreatedByName = userRepository.Get(diningMealManage.CreatedBy).FullName,
                            CreatedDateString = diningMealManage.CreatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningMealManage.CreatedDate) : null,
                            UpdatedByName = diningMealManage.UpdatedBy != null ?
                                userRepository.Get(diningMealManage.UpdatedBy).FullName : null,
                            UpdatedDateString = diningMealManage.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningMealManage.UpdatedDate) : null,
                            StatusName = Enum.GetName(typeof(DiningMealStatus), diningMealManage.Status)
                        };
                        decimal totalMeal = 0;
                        List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                        foreach (BoarderWithTakenMealVM takenMeal in takenMeals)
                        {
                            if (HasTakenMeal(takenMeal, diningMeals))
                            {
                                DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(takenMeal.BoarderId);
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
                                }
                                diningBoarderDetailsViewModel.DiningMeals = GetBorderMealObject(takenMeal, diningMeals);
                                diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                            }
                        };
                        diningMealDetailsViewModel.BoarderDetails = diningBoarderDetailsViewModels;
                        diningMealDetailsViewModel.TotalMeal = totalMeal;
                        response.ResponseObj = diningMealDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Meal Taken!";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public List<DiningMealDetailsViewModel> GetDiningMealViewModels(List<DiningMeal> diningMeals)
        {
            List<DiningMealDetailsViewModel> diningMealViewModels = new List<DiningMealDetailsViewModel>();
            foreach (DiningMeal diningMeal in diningMeals)
            {
                DiningMealDetailsViewModel diningMealViewModel = new DiningMealDetailsViewModel
                {
                    Id = diningMeal.Id,
                    MealName = diningMeal.MealName,
                    MealNo = diningMeal.DiningMealNo,
                    MealSize = diningMeal.MealSize,
                    MealTime = diningMeal.MealTime,
                    DiningMealNumberString = Formater.GetEnumValueDescription<DiningMealNumber>(diningMeal.DiningMealNumberId),
                    DiningMealNumberId = diningMeal.DiningMealNumberId,
                    MealNumber = 0,
                    MealKey = diningMeal.MealKey,
                    MealSequence = diningMeal.MealSequence
                };
                diningMealViewModels.Add(diningMealViewModel);
            }
            return diningMealViewModels;
        }
        public async Task<string> GenerateBoarderTakenMeal(List<DiningBoarder> diningBoarders, List<DiningMeal> diningMeals, 
            DateTime mealDate)
        {
            List<DiningMealManage> diningMealManages = await diningMealManageRepository.FilterListAsync(x =>
               x.FromDate.Date <= mealDate.Date && (x.ToDate == null || x.ToDate >= mealDate.Date));
            IDictionary<long, List<long>> boarders = new Dictionary<long, List<long>>();
            IDictionary<long, List<long>> meals = new Dictionary<long, List<long>>();
            for (int i = 0; i < diningMealManages.Count; i++)
            {
                boarders.Add(i, JsonConvert.DeserializeObject<List<long>>(diningMealManages[i].Boarders));
                meals.Add(i, JsonConvert.DeserializeObject<List<long>>(diningMealManages[i].Meals));
            }
            List<IDictionary<string, object>> objects = new List<IDictionary<string, object>>();
            foreach (DiningBoarder diningBoarder in diningBoarders)
            {
                List<int> foundedIndexes = new List<int>();
                for (int j = 0; j < boarders.Count; j++)
                {
                    var boardersId = boarders[j];
                    if (boardersId.Contains(diningBoarder.Id))
                    {
                        foundedIndexes.Add(j);
                    }
                }
                var obj = new ExpandoObject() as IDictionary<string, object>;
                obj.Add("boarderId", diningBoarder.Id);
                foreach (DiningMeal diningMeal in diningMeals)
                {
                    decimal mealSize = diningMeal.MealSize;
                    for(int k = 0; k < foundedIndexes.Count; k++)
                    {
                        List<long> mealsId = meals[foundedIndexes[k]];
                        if (mealsId.Contains(diningMeal.Id)) 
                        {
                            mealSize = 0;
                            break;
                        }
                    }
                    var keys = Enum.GetValues(typeof(DiningMealNumber));
                    foreach(var key in keys)
                    {
                        if((int)key == diningMeal.DiningMealNumberId)
                        {
                            obj.Add(key.ToString(), mealSize);
                            break;
                        }
                    }
                }
                objects.Add(obj);
            }
            return JsonConvert.SerializeObject(objects);
        }
    }
}
