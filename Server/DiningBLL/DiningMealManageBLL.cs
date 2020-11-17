using CommonBLL;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.Repository;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MERP.FinanceBLL
{
    public class DiningMealManageBLL
    {
        private readonly IRepository<DiningMealManage> diningMealManageRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<DiningMeal> diningMealRepository;
        private readonly DiningBoarderBLL diningBoarderBLL;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<DiningBoarderExternal> diningBoarderExternalRepository;
        private readonly IRepository<Employee> employeeRepository;

        public DiningMealManageBLL(IRepository<DiningMealManage> diningMealManageRepository,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository, IRepository<User> userRepository,
            IRepository<DiningMeal> diningMealRepository, DiningBoarderBLL diningBoarderBLL, IRepository<Student> studentRepository,
            IRepository<DiningBoarderExternal> diningBoarderExternalRepository, IRepository<Employee> employeeRepository)
        {
            this.diningMealManageRepository = diningMealManageRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.userRepository = userRepository;
            this.diningMealRepository = diningMealRepository;
            this.diningBoarderBLL = diningBoarderBLL;
            this.studentRepository = studentRepository;
            this.diningBoarderExternalRepository = diningBoarderExternalRepository;
            this.employeeRepository = employeeRepository;
        }
        public async Task<ResponseMessage> CreateDiningMealManage(DiningMealManageViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningMealManage>
                    (await diningMealManageRepository.GetAsync(), token);
                DiningMealManage diningMealManage = new DiningMealManage();
                diningMealManage.Id = codeAndId.Id + 1;
                diningMealManage.InstituteId = token.InstituteID;
                diningMealManage.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                diningMealManage.DiningMealManageNo = codeAndId.No;
                diningMealManage.Meals = model.Meals;
                diningMealManage.Boarders = model.Boarders;
                diningMealManage.FromDate = model.FromDate;
                diningMealManage.ToDate = model.ToDate;
                diningMealManage.CreatedBy = token.UserID;
                diningMealManage.CreatedDate = DateTime.UtcNow;
                diningMealManage.Status = (byte)Status.Active;

                DiningMealManage result = await diningMealManageRepository.AddAsync(diningMealManage);

                if (result != null)
                {
                    response.ResponseObj = result;
                    response.Message = "Dining Meal Manage Created Successfully With ID: " + result.DiningMealManageNo;
                    response.StatusCode = (byte)StatusCode.Success;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Meal Manage! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningMealManage(DiningMealManageViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningMealManage diningMealManage = await diningMealManageRepository.GetAsync((long)model.Id);
                if (diningMealManage != null)
                {
                    diningMealManage.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                    diningMealManage.Meals = model.Meals;
                    diningMealManage.Boarders = model.Boarders;
                    diningMealManage.FromDate = model.FromDate;
                    diningMealManage.ToDate = model.ToDate;
                    diningMealManage.UpdatedBy = token.UserID;
                    diningMealManage.UpdatedDate = DateTime.Now;

                    DiningMealManage result = await diningMealManageRepository.UpdateAsync(diningMealManage);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Meal Manage Updated Successfully With ID: " + result.DiningMealManageNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.DiningMealManageNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Meal Manage! Please try again...";
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
                    DiningMealManage diningMealManage = await diningMealManageRepository.GetAsync(id);
                    if (diningMealManage != null)
                    {
                        await diningMealManageRepository.DeleteAsync(diningMealManage);
                        response.Message = "Dining Meal Manage Deleted Succesfuly with id: " + diningMealManage.DiningMealManageNo;
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMealManage;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Meal Manage";
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
        public async Task<ResponseMessage> SearchDiningMealManage(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningMealManage diningMealManage = await diningMealManageRepository.GetAsync(id);
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
                    List<DiningBill> diningBills = await DataController.GetDataList<DiningBill>(token);
                    if (diningBills.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBills;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Meal Manage";
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
        public async Task<ResponseMessage> searchDiningMealManageForTable(RequestMessage request, ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                InstituteInfoViewModel model = JsonConvert.DeserializeObject<InstituteInfoViewModel>(request.Content);
                List<DiningMealManage> diningMealManages = await DataController.GetDataList<DiningMealManage>(token, model);
                List<DiningMealManageTableData> diningMealManageViewModels = new List<DiningMealManageTableData>();

                if (diningMealManages.Count > 0)
                {
                    foreach (DiningMealManage diningMealManage in diningMealManages)
                    {
                        DiningMealManageTableData diningMealManageViewModel = new DiningMealManageTableData
                        {
                            Id = diningMealManage.Id,
                            DiningMealManageNo = diningMealManage.DiningMealManageNo,
                            FromDate = diningMealManage.FromDate,
                            ToDate = diningMealManage.ToDate,
                            FromDateString = Formater.FormatDateddMMyyyy(diningMealManage.FromDate),
                            ToDateString = Formater.FormatDateddMMyyyy(diningMealManage.ToDate),
                            InstituteId = diningMealManage.InstituteId,
                            InstituteName = diningMealManage.InstituteId > 0 ? instituteRepository.Get(diningMealManage.InstituteId).InstituteName : null,
                            CampusName = diningMealManage.CampusId > 0 ? campusRepository.Get(diningMealManage.CampusId).CampusName : null,
                            CampusId = diningMealManage.CampusId,
                            Status = diningMealManage.Status,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningMealManage.Status),
                        };
                        List<long> boarders = JsonConvert.DeserializeObject<List<long>>(diningMealManage.Boarders);
                        diningMealManageViewModel.TotalBoarder = boarders.Count;
                        List<long> Meals = JsonConvert.DeserializeObject<List<long>>(diningMealManage.Meals);
                        string MealName = "";
                        foreach (var meal in Meals)
                        {
                            string name = diningMealRepository.Get(meal).MealName;
                            MealName += name + ", ";
                        }
                        diningMealManageViewModel.TakenMeals = MealName.Substring(0, MealName.Length - 2);
                        diningMealManageViewModels.Add(diningMealManageViewModel);
                    }
                    if (diningMealManageViewModels != null)
                    {
                        response.ResponseObj = diningMealManageViewModels;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.NotFound;
                        response.Message = "Not found any Dining Meal Manage";
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
                    DiningMealManage diningMealManage = await diningMealManageRepository.GetAsync(id);

                    if (diningMealManage != null && DataController.IsReturnable(diningMealManage, token))
                    {
                        DiningMealManageTableData diningMealManageViewModel = new DiningMealManageTableData
                        {
                            Id = diningMealManage.Id,
                            DiningMealManageNo = diningMealManage.DiningMealManageNo,
                            FromDateString = Formater.FormatDateddMMyyyy(diningMealManage.FromDate),
                            ToDateString = Formater.FormatDateddMMyyyy(diningMealManage.ToDate),
                            CreatedByName = userRepository.Get(diningMealManage.CreatedBy).FullName,
                            CreatedDateString = Formater.FormatDateddMMyyyy(diningMealManage.CreatedDate),
                            UpdatedByName = diningMealManage.UpdatedBy > 0 ?
                                userRepository.Get(diningMealManage.UpdatedBy).FullName : null,
                            UpdatedDateString = diningMealManage.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningMealManage.UpdatedDate) : null,
                            Status = diningMealManage.Status,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningMealManage.Status),
                        };
                        List<long> boarders = JsonConvert.DeserializeObject<List<long>>(diningMealManage.Boarders);
                        diningMealManageViewModel.TotalBoarder = boarders.Count;
                        List<long> Meals = JsonConvert.DeserializeObject<List<long>>(diningMealManage.Meals);
                        string MealName = "";
                        foreach (var meal in Meals)
                        {
                            string name = diningMealRepository.Get(meal).MealName;
                            MealName += name + ", ";
                        }
                        diningMealManageViewModel.TakenMeals = MealName.Substring(0, MealName.Length - 2);
                        List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                        foreach (long boarder in boarders)
                        {
                            RequestMessage request = new RequestMessage
                            {
                                Content = boarder.ToString()
                            };
                            var boarderDetails = await diningBoarderBLL.SearchDetails(request, response, token);
                            diningBoarderDetailsViewModels.Add((DiningBoarderDetailsViewModel)boarderDetails.ResponseObj);
                        }
                        diningMealManageViewModel.DiningBoarderDetails = diningBoarderDetailsViewModels;
                        response.ResponseObj = diningMealManageViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Meal Manage";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningMealManageAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                List<DiningBoarder> diningBoarders = await DataController.GetActiveDataListAsync<DiningBoarder>(token);
                List<DiningBoarderDetailsViewModel> DiningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                foreach (var diningBoarder in diningBoarders)
                {
                    DiningBoarderDetailsViewModel diningBoarderViewModel = new DiningBoarderDetailsViewModel()
                    {
                        Id = diningBoarder.Id,
                        BoarderNo = diningBoarder.DiningBoarderNo,
                        DiningBoarderId = diningBoarder.DiningBoarderId,
                        BoarderName = diningBoarder.BoarderTypeId == (byte)BoarderType.Student ?
                            studentRepository.Get(diningBoarder.DiningBoarderId).StudentName :
                            diningBoarder.BoarderTypeId == (byte)BoarderType.Outer ?
                            diningBoarderExternalRepository.Get(diningBoarder.DiningBoarderId).Name :
                            employeeRepository.Get(diningBoarder.DiningBoarderId).EmployeeName,

                        BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId)
                    };
                    DiningBoarderDetailsViewModels.Add(diningBoarderViewModel);
                }
                List<DiningMeal> diningMeals = await DataController.GetActiveDataListAsync<DiningMeal>(token);
                response.ResponseObj = new { diningBoarders = DiningBoarderDetailsViewModels, diningMeals };
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
    }
}
