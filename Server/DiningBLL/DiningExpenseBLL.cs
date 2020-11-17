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
    public class DiningExpenseBLL
    {
        private readonly IRepository<DiningExpense> diningExpenseRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<ExpenseHead> expenseHeadRepository;
        private readonly IRepository<StockProduct> diningStockProductRepository;
        private readonly IRepository<ExpenseVoucher> expenseVoucherRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;

        public DiningExpenseBLL(IRepository<DiningExpense> diningExpenseRepository, IRepository<User> userRepository,
            IRepository<ExpenseHead> expenseHeadRepository, IRepository<StockProduct> diningStockProductRepository,
            IRepository<ExpenseVoucher> expenseVoucherRepository, IRepository<Institute> instituteRepository,
            IRepository<Campus> campusRepository)
        {
            this.diningExpenseRepository = diningExpenseRepository;
            this.userRepository = userRepository;
            this.expenseHeadRepository = expenseHeadRepository;
            this.diningStockProductRepository = diningStockProductRepository;
            this.expenseVoucherRepository = expenseVoucherRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
        }
        public async Task<ResponseMessage> CreateDiningExpense(DiningExpenseViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningExpense>
                    (await diningExpenseRepository.GetAsync(), token);
                DiningExpense diningExpense = new DiningExpense();
                diningExpense.Id = codeAndId.Id + 1;
                diningExpense.InstituteId = token.InstituteID;
                diningExpense.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                diningExpense.DiningExpenseNo = codeAndId.No;
                diningExpense.ExpenseDate = model.ExpenseDate;
                diningExpense.ExpenseHeadId = model.ExpenseHeadId;
                diningExpense.IsStockProduct = model.IsStockProduct;
                diningExpense.ProductName = model.ProductName;
                diningExpense.Qty = model.Qty;
                diningExpense.Unit = model.Unit;
                diningExpense.Rate = model.Rate;
                diningExpense.VoucherId = model.VoucherId;
                diningExpense.Note = model.Note;
                diningExpense.Amount = model.Amount;
                diningExpense.CreatedBy = token.UserID;
                diningExpense.CreatedDate = DateTime.UtcNow;
                diningExpense.Status = (byte)ExpenseVoucherStatus.Created;

                DiningExpense result = await diningExpenseRepository.AddAsync(diningExpense);

                if (result != null)
                {
                    response.ResponseObj = result;
                    response.Message = "Dining Expense Created Successfully With ID: " + result.DiningExpenseNo;
                    response.StatusCode = (byte)StatusCode.Success;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Expense! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningExpense(DiningExpenseViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningExpense diningExpense = await diningExpenseRepository.GetAsync((long)model.Id);
                if (diningExpense != null)
                {
                    diningExpense.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                    diningExpense.ExpenseDate = model.ExpenseDate;
                    diningExpense.ExpenseHeadId = model.ExpenseHeadId;
                    diningExpense.IsStockProduct = model.IsStockProduct;
                    diningExpense.ProductName = model.ProductName;
                    diningExpense.Qty = model.Qty;
                    diningExpense.Unit = model.Unit;
                    diningExpense.Rate = model.Rate;
                    diningExpense.VoucherId = model.VoucherId;
                    diningExpense.Note = model.Note;
                    diningExpense.Amount = model.Amount;
                    diningExpense.UpdatedBy = token.UserID;
                    diningExpense.UpdatedDate = DateTime.UtcNow;

                    DiningExpense result = await diningExpenseRepository.UpdateAsync(diningExpense);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Expense Updated Successfully With ID: " + result.DiningExpenseNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.ExpenseNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Expense! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningExpense(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningExpense diningExpense = await diningExpenseRepository.GetAsync(id);
                    if (diningExpense != null && diningExpense.VoucherId == null)
                    {
                        await diningExpenseRepository.DeleteAsync(diningExpense);
                        response.Message = "Dining Expense Deleted Succesfully with id: " + diningExpense.DiningExpenseNo;
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningExpense;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Expense";
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
        public async Task<ResponseMessage> searchDiningExpenseForTable(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                bool isForVoucher = JsonConvert.DeserializeObject<bool>(requestMessage.Content);
                List<DiningExpense> diningExpenses = await DataController.GetDataList<DiningExpense>(token);
                if (isForVoucher)
                {
                    diningExpenses = diningExpenses.Where(x => x.CreatedBy == token.UserID).ToList();
                }
                List<DiningExpenseDetailsViewModel> diningExpenseDetailsViewModels = new List<DiningExpenseDetailsViewModel>();

                if (diningExpenses != null)
                {
                    foreach (DiningExpense diningExpense in diningExpenses)
                    {
                        DiningExpenseDetailsViewModel diningExpenseDetailsViewModel = new DiningExpenseDetailsViewModel
                        {
                            Id = diningExpense.Id,
                            ExpenseNo = diningExpense.DiningExpenseNo,
                            ExpenseHeadName = expenseHeadRepository.Get(diningExpense.ExpenseHeadId).Name,
                            Amount = diningExpense.Amount,
                            ExpenseDateString = Formater.FormatDateddMMyyyy(diningExpense.ExpenseDate),
                            CreatedByName = userRepository.Get(diningExpense.CreatedBy).FullName,
                            Note = diningExpense.Note,
                            Qty = diningExpense.Qty,
                            Unit = diningExpense.Unit,
                            Rate = diningExpense.Rate,
                            VoucherIdString = diningExpense.VoucherId > 0 ?
                                expenseVoucherRepository.Get(diningExpense.VoucherId).ExpenseVoucherNo : null,
                            InstituteId = diningExpense.InstituteId,
                            InstituteName = diningExpense.InstituteId > 0 ? instituteRepository.Get(diningExpense.InstituteId).InstituteName : null,
                            CampusName = diningExpense.CampusId > 0 ? campusRepository.Get(diningExpense.CampusId).CampusName : null,
                            CampusId = diningExpense.CampusId,
                            Status = diningExpense.Status,
                            StatusName = Formater.GetEnumValueDescription<ExpenseVoucherStatus>(diningExpense.Status),
                            ExpenseDate = diningExpense.ExpenseDate,
                            VoucherId = diningExpense.VoucherId,
                            Responsible = diningExpense.ResponsibleId > 0 ? userRepository.Get(diningExpense.ResponsibleId).FullName :
                            userRepository.Get(diningExpense.CreatedBy).FullName
                        };

                        List<ExpenseHead> expenseHeads = await expenseHeadRepository.FilterListAsync(x =>
                            x.ExpenseTypeId == (byte)ExpenseType.DiningExpense);

                        if (expenseHeads.Count > 0)
                        {
                            ExpenseHead expenseHead = expenseHeads
                                .Where(x => x.Id == diningExpense.ExpenseHeadId).FirstOrDefault();
                            if (expenseHead.ParentId == 0)
                            {
                                diningExpenseDetailsViewModel.ExpenseHeadName = expenseHead.Name;
                            }
                            else
                            {
                                while (expenseHead.ParentId > 0)
                                {
                                    ExpenseHead expenseHeadParent = expenseHeads
                                       .Where(x => x.Id == expenseHead.ParentId).FirstOrDefault();
                                    diningExpenseDetailsViewModel.ExpenseHeadName = expenseHeadParent.Name
                                        + " > " + expenseHead.Name;
                                    expenseHead.ParentId = expenseHeadParent.ParentId;
                                }
                            }
                        }
                        int result = 0;
                        if (int.TryParse(diningExpense.ProductName, out result))
                        {
                            var product = await diningStockProductRepository.GetAsync((long)result);
                            if(product != null)
                                diningExpenseDetailsViewModel.ProductName = Formater.GetStockProductNameWithAttrValue(product);
                            diningExpenseDetailsViewModel.ItemName = diningExpenseDetailsViewModel.ProductName;
                        }
                        else
                        {
                            diningExpenseDetailsViewModel.ProductName = diningExpense.ProductName;
                            diningExpenseDetailsViewModel.ItemName = diningExpense.ProductName;
                        };

                        diningExpenseDetailsViewModels.Add(diningExpenseDetailsViewModel);
                    }
                    if (diningExpenses != null)
                    {
                        response.ResponseObj = diningExpenseDetailsViewModels.OrderBy(x => x.ExpenseDate).ToList();
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.NotFound;
                        response.Message = "Not found any Dining Expense";
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningExpense(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningExpense diningExpense = await diningExpenseRepository.GetAsync(id);
                    if (diningExpense != null && DataController.IsReturnable(diningExpense, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningExpense;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningExpense> diningExpenses = await DataController.GetDataList<DiningExpense>(token);
                    if (diningExpenses != null)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningExpenses;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Expense";
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
                    DiningExpense diningExpense = await diningExpenseRepository.GetAsync(id);


                    if (diningExpense != null && DataController.IsReturnable(diningExpense, token))
                    {
                        long productId;
                        ExpenseVoucher expenseVoucher = await expenseVoucherRepository.GetAsync(diningExpense.VoucherId);
                        DiningExpenseDetailsViewModel diningExpenseDetailsViewModel = new DiningExpenseDetailsViewModel
                        {
                            Id = diningExpense.Id,
                            ExpenseNo = diningExpense.DiningExpenseNo,
                            ExpenseDateString = Formater.FormatDateddMMyyyy(diningExpense.ExpenseDate),
                            ExpenseHeadName = expenseHeadRepository.Get(diningExpense.ExpenseHeadId).Name,
                            IsStockProductStatus = diningExpense.IsStockProduct == true ? "Stock Product" : "Non-Stock Product",
                            ProductName = Int64.TryParse(diningExpense.ProductName, out productId) == true ? diningStockProductRepository.Get(productId).ProductName : diningExpense.ProductName,
                            Qty = diningExpense.Qty,
                            Rate = diningExpense.Rate,
                            VoucherNo = diningExpense.VoucherId != null ? expenseVoucher.ExpenseVoucherNo : null,
                            Note = diningExpense.Note,
                            Amount = diningExpense.Amount,
                            CreatedByName = userRepository.Get(diningExpense.CreatedBy).FullName,
                            CreatedDateString = diningExpense.CreatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningExpense.CreatedDate) : null,
                            UpdatedByName = diningExpense.UpdatedBy != null ?
                                userRepository.Get(diningExpense.UpdatedBy).FullName : null,
                            UpdatedDateString = diningExpense.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningExpense.UpdatedDate) : null,
                            StatusName = Formater.GetEnumValueDescription<ExpenseVoucherStatus>(diningExpense.Status),
                            Responsible = diningExpense.ResponsibleId > 0 ? userRepository.Get(diningExpense.ResponsibleId).FullName :
                            userRepository.Get(diningExpense.CreatedBy).FullName
                        };
                        response.ResponseObj = diningExpenseDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Expense";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningExpenseAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                List<DiningExpense> diningExpenses = await DataController.GetDataList<DiningExpense>(token);
                string[] units = diningExpenses.Where(x => x.Unit != null).Select(x => x.Unit).Distinct().ToArray();
                List<ExpenseHead> expenseHeads = await expenseHeadRepository.FilterListAsync(x =>
                    x.ExpenseTypeId == (byte)ExpenseType.DiningExpense && x.InstituteId == token.InstituteID
                    && x.CampusId == token.CampusID && x.Status == (byte)Status.Active);
                List<StockProduct> diningStockProducts = await diningStockProductRepository.FilterListAsync(x =>
                    x.StockProductType == (byte)StockProductType.DiningStockProduct &&
                    x.InstituteId == token.InstituteID && x.CampusId == token.CampusID && x.Status == (byte)Status.Active);
                if (expenseHeads.Count > 0)
                {
                    foreach (ExpenseHead expenseHead in expenseHeads)
                    {
                        while (expenseHead.ParentId > 0)
                        {
                            expenseHead.Name = expenseHeads
                                .Where(x => x.Id == expenseHead.ParentId).FirstOrDefault().Name + " > " + expenseHead.Name;
                            expenseHead.ParentId = expenseHeads
                                .Where(x => x.Id == expenseHead.ParentId).FirstOrDefault().ParentId;
                        }
                    }
                    response.ResponseObj = new { expenseHeads = expenseHeads, diningStockProducts = diningStockProducts, units = units };
                }
            }
            catch (Exception e)
            {

            }
            return response;
        }
    }
}
