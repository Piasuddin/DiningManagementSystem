using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using MERP.EntityFramworkDAL.Repository;

namespace MERP.FinanceBLL
{
    public class DiningMealBLL
    {
        private readonly IRepository<DiningMeal> diningMealRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;

        public DiningMealBLL(IRepository<DiningMeal> diningMealRepository, IRepository<User> userRepository,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository)
        {
            this.diningMealRepository = diningMealRepository;
            this.userRepository = userRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
        }
        public async Task<ResponseMessage> CreateDiningMeal(DiningMealViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningMeal>
                    (await diningMealRepository.GetAsync(), token);
                List<DiningMeal> diningMeals = await DataController.GetDataList<DiningMeal>(token);
                byte diningMealNumberId = 0;
                var keyValue = Enum.GetValues(typeof(DiningMealNumber));

                foreach (var key in keyValue)
                {
                    DiningMeal item = diningMeals.Where(x => x.DiningMealNumberId == (int)key).FirstOrDefault();
                    if (item == null)
                    {
                        diningMealNumberId = (byte)((int)key);
                        break;
                    }
                }
                DiningMeal diningMeal = new DiningMeal();
                diningMeal.Id = codeAndId.Id + 1;
                diningMeal.InstituteId = token.InstituteID;
                diningMeal.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                diningMeal.DiningMealNo = codeAndId.No;
                diningMeal.DiningMealNumberId = diningMealNumberId;
                diningMeal.MealName = model.MealName;
                diningMeal.MealSize = model.MealSize;
                diningMeal.MealTime = model.MealTime;
                diningMeal.MealKey = model.MealKey;
                diningMeal.MealSequence = model.MealSequence;
                diningMeal.CreatedBy = token.UserID;
                diningMeal.CreatedDate = DateTime.UtcNow;
                diningMeal.Status = (byte)Status.Active;

                DiningMeal result = await diningMealRepository.AddAsync(diningMeal);

                if (result != null)
                {
                    response.ResponseObj = result;
                    response.Message = "Dining Meal Created Successfully With ID: " + result.DiningMealNo;
                    response.StatusCode = (byte)StatusCode.Success;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Meal! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningMeal(DiningMealViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningMeal diningMeal = await diningMealRepository.GetAsync((long)model.Id);
                if (diningMeal != null)
                {
                    diningMeal.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                    diningMeal.MealName = model.MealName;
                    diningMeal.MealSize = model.MealSize;
                    diningMeal.MealTime = model.MealTime;
                    diningMeal.MealSequence = model.MealSequence;
                    diningMeal.MealKey = model.MealKey;
                    diningMeal.UpdatedBy = token.UserID;
                    diningMeal.UpdatedDate = DateTime.UtcNow;

                    DiningMeal result = await diningMealRepository.UpdateAsync(diningMeal);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Meal updated Successfully With ID: " + result.DiningMealNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.MealNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Meal! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningMeal(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningMeal diningMeal = await diningMealRepository.GetAsync(id);
                    if (diningMeal != null)
                    {
                        await diningMealRepository.DeleteAsync(diningMeal);
                        response.Message = "Dining Meal Deleted Succesfully with id: " + diningMeal.DiningMealNo;
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMeal;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Meal";
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
        public async Task<ResponseMessage> searchDiningMealForTable(ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                List<DiningMeal> diningMeals = await DataController.GetDataList<DiningMeal>(token);
                List<DiningMealDetailsViewModel> diningMealDetailsViewModels = new List<DiningMealDetailsViewModel>();

                if (diningMeals.Count > 0)
                {
                    foreach (DiningMeal diningMeal in diningMeals)
                    {
                        DiningMealDetailsViewModel diningMealDetailsViewModel = new DiningMealDetailsViewModel
                        {
                            Id = diningMeal.Id,
                            MealNo = diningMeal.DiningMealNo,
                            MealName = diningMeal.MealName,
                            MealSize = diningMeal.MealSize,
                            MealTime = diningMeal.MealTime,
                            InstituteId = diningMeal.InstituteId,
                            InstituteName = diningMeal.InstituteId > 0 ? instituteRepository.Get(diningMeal.InstituteId).InstituteName : null,
                            CampusName = diningMeal.CampusId > 0 ? campusRepository.Get(diningMeal.CampusId).CampusName : null,
                            CampusId = diningMeal.CampusId,
                            Status = diningMeal.Status,
                            MealSequence = diningMeal.MealSequence,
                            MealKey = diningMeal.MealKey,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningMeal.Status)
                        };
                        diningMealDetailsViewModels.Add(diningMealDetailsViewModel);
                    }
                    response.ResponseObj = diningMealDetailsViewModels;
                    response.StatusCode = (byte)StatusCode.Success;
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Meal";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
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
                    DiningMeal diningMeal = await diningMealRepository.GetAsync(id);
                    if (diningMeal != null && DataController.IsReturnable(diningMeal, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMeal;
                    }

                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningMeal> diningMeals = await DataController.GetDataList<DiningMeal>(token);
                    if (diningMeals.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningMeals;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Meal";
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
                    DiningMeal diningMeal = await diningMealRepository.GetAsync(id);

                    if (diningMeal != null && DataController.IsReturnable(diningMeal, token))
                    {
                        DiningMealDetailsViewModel diningMealDetailsViewModel = new DiningMealDetailsViewModel
                        {
                            CampusId = diningMeal.CampusId,
                            InstituteId = diningMeal.InstituteId,
                            MealNo = diningMeal.DiningMealNo,
                            MealName = diningMeal.MealName,
                            MealSize = diningMeal.MealSize,
                            MealTime = diningMeal.MealTime,
                            CreatedByName = userRepository.Get(diningMeal.CreatedBy).FullName,
                            CreatedDateString = diningMeal.CreatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningMeal.CreatedDate) : null,
                            UpdatedByName = diningMeal.UpdatedBy != null ?
                                userRepository.Get(diningMeal.UpdatedBy).FullName : null,
                            UpdatedDateString = diningMeal.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningMeal.UpdatedDate) : null,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningMeal.Status)
                        };
                        response.ResponseObj = diningMealDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Meal";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
    }
}
