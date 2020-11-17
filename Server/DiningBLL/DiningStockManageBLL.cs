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
    public class DiningStockManageBLL
    {
        private readonly IRepository<DiningStockManage> diningStockManageRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<StockProduct> diningStockProductRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;

        public DiningStockManageBLL(IRepository<DiningStockManage> diningStockManageRepository, IRepository<User> userRepository,
            IRepository<StockProduct> diningStockProductRepository, IRepository<Institute> instituteRepository,
            IRepository<Campus> campusRepository)
        {
            this.diningStockManageRepository = diningStockManageRepository;
            this.userRepository = userRepository;
            this.diningStockProductRepository = diningStockProductRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
        }
        public async Task<ResponseMessage> CreateDiningStockManage(List<DiningStockManageAddDataViewModel> models,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                if (models.Count > 0)
                {
                    long id = Convert.ToInt64(await IdGenerator.GenerateChildClassIdAsync<DiningStockManage>
                        (await diningStockManageRepository.GetAsync()));
                    List<DiningStockManage> toUpdate = new List<DiningStockManage>();
                    List<DiningStockManage> toAdd = new List<DiningStockManage>();
                    foreach (DiningStockManageAddDataViewModel model in models)
                    {
                        DiningStockManage diningStockManage = await diningStockManageRepository.GetAsync((long)model.Id);

                        if (diningStockManage != null && DataController.IsReturnable(diningStockManage, token) && model.InStock > 0)
                        {
                            decimal prevPrice = diningStockManage.Price;
                            diningStockManage.StockQty = model.UsedQyt;
                            diningStockManage.Price = model.UsedQyt * diningStockManage.Rate;
                            diningStockManage.Status = (byte)DiningStockAdjustmentStatus.Closed;
                            toUpdate.Add(diningStockManage);

                            decimal leftMoney = prevPrice - diningStockManage.Price;

                            DiningStockManage nextStockManage = await diningStockManageRepository.FilterOneAsync(x =>
                            x.InstituteId == token.InstituteID && x.CampusId == token.CampusID && x.ProductId ==
                            diningStockManage.ProductId && x.StockForMonth.Month == diningStockManage.StockForMonth.AddMonths(1).Month);
                            if (nextStockManage != null)
                            {
                                nextStockManage.StockQty = nextStockManage.StockQty + model.InStock;
                                nextStockManage.Price = nextStockManage.Price + leftMoney;
                                nextStockManage.Rate = nextStockManage.Price / nextStockManage.StockQty;
                                toUpdate.Add(diningStockManage);
                            }
                            else
                            {
                                DiningStockManage newDiningStock = new DiningStockManage()
                                {
                                    Id = id + 1,
                                    CampusId = token.CampusID,
                                    InstituteId = token.InstituteID,
                                    CreatedBy = token.UserID,
                                    CreatedDate = DateTime.UtcNow,
                                    Price = leftMoney,
                                    StockQty = model.InStock,
                                    ProductId = diningStockManage.ProductId,
                                    Rate = leftMoney / model.InStock,
                                    StockForMonth = DateTime.UtcNow,
                                    DiningStockManageNo = Convert.ToString(new Random().Next(10000000, 99999999)),
                                    Status = (byte)DiningStockAdjustmentStatus.Running
                                };
                                toAdd.Add(newDiningStock);
                            }
                        }
                        else
                        {
                            diningStockManage.Status = (byte)DiningStockAdjustmentStatus.Closed;
                            toUpdate.Add(diningStockManage);
                        }
                    }
                    using (TransactionScope transaction = new TransactionScope())
                    {
                        List<DiningStockManage> result = new List<DiningStockManage>();
                        if (toUpdate.Count > 0)
                        {
                            result = await diningStockManageRepository.UpdateRangeAsync(toUpdate);
                        }
                        if (toAdd.Count > 0)
                        {
                            result = await diningStockManageRepository.AddRangeAsync(toAdd);
                        }
                        if (result.Count > 0)
                        {
                            transaction.Complete();
                            response.ResponseObj = result;
                            response.Message = "Dining Stock Manage Created Successfully";
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
        public async Task<ResponseMessage> UpdateDiningStockManage(DiningStockManageViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningStockManage diningStockManage = await diningStockManageRepository.GetAsync((long)model.Id);
                if (diningStockManage != null)
                {
                    diningStockManage.StockForMonth = model.StockForMonth;
                    diningStockManage.ProductId = model.StockProductId;
                    diningStockManage.Price = model.Price;
                    diningStockManage.Rate = model.Rate;
                    diningStockManage.StockQty = model.StockQty;
                    diningStockManage.UpdatedBy = token.UserID;
                    diningStockManage.UpdatedDate = DateTime.UtcNow;

                    DiningStockManage result = await diningStockManageRepository.UpdateAsync(diningStockManage);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Stock Manage Created Successfully With ID: " + result.DiningStockManageNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.StockNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Stock Manage! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningStockManage(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningStockManage diningStockManage = await diningStockManageRepository.GetAsync(id);
                    if (diningStockManage != null)
                    {
                        await diningStockManageRepository.DeleteAsync(diningStockManage);
                        response.Message = "Dining Stock Manage Deleted Succesfully with id: " + diningStockManage.DiningStockManageNo;
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningStockManage;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Stock Manage";
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
        public async Task<ResponseMessage> searchDiningStockManageForTable(ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                List<DiningStockManage> diningStockManages = await DataController.GetDataList<DiningStockManage>(token);
                List<DiningStockManageDetailsViewModel> diningStockManageDetailsViewModels = new List<DiningStockManageDetailsViewModel>();

                if (diningStockManages.Count > 0)
                {
                    foreach (DiningStockManage diningStockManage in diningStockManages)
                    {
                        DiningStockManageDetailsViewModel diningStockManageDetailsViewModel = new DiningStockManageDetailsViewModel
                        {
                            Id = diningStockManage.Id,
                            InstituteId = diningStockManage.InstituteId,
                            CampusId = diningStockManage.CampusId,
                            StockNo = diningStockManage.DiningStockManageNo,
                            StockForMonth = diningStockManage.StockForMonth,
                            StockProductId = diningStockManage.ProductId,
                            Price = diningStockManage.Price,
                            Rate = diningStockManage.Rate,
                            StockQty = diningStockManage.StockQty,
                            InstituteName = diningStockManage.InstituteId > 0 ? instituteRepository.Get(diningStockManage.InstituteId).InstituteName : null,
                            CampusName = diningStockManage.CampusId > 0 ? campusRepository.Get(diningStockManage.CampusId).CampusName : null,
                            Status = diningStockManage.Status,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningStockManage.Status)
                        };
                        diningStockManageDetailsViewModels.Add(diningStockManageDetailsViewModel);
                    }
                    if (diningStockManages != null)
                    {
                        response.ResponseObj = diningStockManageDetailsViewModels;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.NotFound;
                        response.Message = "Not found any Dining Stock Manage";
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningStockManage(RequestMessage request,
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
                    if (diningStockManages.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningStockManages;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Stock Manage";
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
                    DiningStockManage diningStockManage = await diningStockManageRepository.GetAsync(id);

                    if (diningStockManage != null && DataController.IsReturnable(diningStockManage, token))
                    {
                        DiningStockManageDetailsViewModel diningStockManageDetailsViewModel = new DiningStockManageDetailsViewModel
                        {
                            Id = diningStockManage.Id,
                            InstituteId = diningStockManage.InstituteId,
                            CampusId = diningStockManage.CampusId,
                            StockNo = diningStockManage.DiningStockManageNo,
                            StockForMonth = diningStockManage.StockForMonth,
                            StockProductId = diningStockManage.ProductId,
                            Price = diningStockManage.Price,
                            Rate = diningStockManage.Rate,
                            StockQty = diningStockManage.StockQty,
                            CreatedByName = userRepository.Get(diningStockManage.CreatedBy).FullName,
                            CreatedDateString = diningStockManage.CreatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningStockManage.CreatedDate) : null,
                            UpdatedByName = diningStockManage.UpdatedBy != null ?
                                userRepository.Get(diningStockManage.UpdatedBy).FullName : null,
                            UpdatedDateString = diningStockManage.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningStockManage.UpdatedDate) : null,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningStockManage.Status)
                        };
                        response.ResponseObj = diningStockManageDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Stock Manage";
                }
            }

            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningStockManageAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                string stringDate = JsonConvert.DeserializeObject<string>(requestMessage.Content);
                DateTime date = DateTime.Parse(stringDate);
                if (date.Month == DateTime.UtcNow.Month)
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "You can't close current month's accouts.";
                }
                else
                {
                    List<DiningStockManage> diningStockManages = await diningStockManageRepository.FilterListAsync(x =>
                    x.InstituteId == token.InstituteID && x.CampusId == token.CampusID && x.StockForMonth.Month == date.Month);
                    DiningStockManage previousMonthRecord = await diningStockManageRepository.FilterOneAsync(x =>
                    x.InstituteId == token.InstituteID && x.CampusId == token.CampusID && x.StockForMonth.Month
                    == date.AddMonths(-1).Month);
                    if (previousMonthRecord != null && previousMonthRecord.Status != (byte)DiningStockAdjustmentStatus.Closed)
                    {
                        response.StatusCode = (byte)StatusCode.Failed;
                        response.Message = $"The accounts of {Formater.GetMonthName(date.AddMonths(-1))} is not closed." +
                            $"Please close {Formater.GetMonthName(date.AddMonths(-1))}'s accounts before closing " +
                            $"{Formater.GetMonthName(date)}'s accounts.";
                    }
                    else
                    {
                        if (diningStockManages.Count > 0)
                        {
                            if (diningStockManages.FirstOrDefault().Status == (byte)DiningStockAdjustmentStatus.Closed)
                            {
                                response.StatusCode = (byte)StatusCode.Failed;
                                response.Message = $"The accounts of {Formater.GetMonthName(date)} has already been closed";
                            }
                            else
                            {
                                List<DiningStockManageAddDataViewModel> diningStockManageAddDatas = new List<DiningStockManageAddDataViewModel>();

                                foreach (DiningStockManage diningStockManage in diningStockManages)
                                {
                                    DiningStockManageAddDataViewModel diningStockManageAddData = new DiningStockManageAddDataViewModel
                                    {
                                        ProductName = diningStockProductRepository.Get(diningStockManage.ProductId).ProductName,
                                        StockQty = diningStockManage.StockQty,
                                        Id = diningStockManage.Id,
                                        UsedQyt = diningStockManage.StockQty,
                                        StockProductId = diningStockManage.ProductId
                                    };
                                    diningStockManageAddDatas.Add(diningStockManageAddData);
                                }
                                if (diningStockManageAddDatas.Count > 0)
                                {
                                    response.ResponseObj = diningStockManageAddDatas;
                                    response.StatusCode = (byte)StatusCode.Success;
                                }
                            }
                        }
                        else
                        {
                            response.StatusCode = (byte)StatusCode.Failed;
                            response.Message = $"Not found any record of month {Formater.GetMonthName(date)}";
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return response;
        }
    }
}
