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

namespace MERP.FinanceBLL
{
    public class DiningBillCollectionBLL
    {
        private readonly IRepository<DiningBoarder> diningBoarderRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<Student> studentRepository;
        private readonly IRepository<EmployeeBasicInfo> employeeBasicInfoRepository;
        private readonly IRepository<EmployeeAddress> employeeAddressRepository;
        private readonly IRepository<StudentBasicInfo> studentBasicInfoRepository;
        private readonly IRepository<StudentAddress> studentAddressRepository;
        private readonly IRepository<DistrictPostOffice> districtPostOfficeRepository;
        private readonly IRepository<Designation> employeeDesignationRepository;
        private readonly IRepository<DistrictUpazila> districtUpazilasRepository;
        private readonly IRepository<District> districtsRepository;
        private readonly IRepository<ClassName> classNameRepository;
        private readonly IRepository<DiningBillCollection> diningBillCollectionRepository;
        private readonly IRepository<BankAccount> bankAccountRepository;
        private readonly IRepository<MobileBankingAccount> mobileBankingAccountRepository;
        private readonly IRepository<DiningBill> diningBillRepository;
        private readonly BankTransferBLL bankTransferBLL;
        private readonly MobileBankTransferBLL mobileBankTransferBLL;
        private readonly IRepository<DiningBillCollectionDetails> diningBillCollectionDetailsRepository;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly IRepository<DiningBillDetails> diningBillDetailsRepository;
        private readonly IRepository<ExternalPerson> externalpersonRepository;

        public DiningBillCollectionBLL(IRepository<DiningBoarder> diningBoarderRepository, IRepository<User> userRepository, IRepository<Employee> employeeRepository,
            IRepository<Student> studentRepository, IRepository<EmployeeBasicInfo> employeeBasicInfoRepository, IRepository<EmployeeAddress> employeeAddressRepository,
            IRepository<StudentBasicInfo> studentBasicInfoRepository, IRepository<StudentAddress> studentAddressRepository, IRepository<DistrictPostOffice> districtPostOfficeRepository,
            IRepository<Designation> employeeDesignationRepository, IRepository<DistrictUpazila> districtUpazilasRepository, IRepository<District> districtsRepository,
            IRepository<ClassName> classNameRepository, IRepository<DiningBillCollection> diningBillCollectionRepository, IRepository<BankAccount> bankAccountRepository,
            IRepository<MobileBankingAccount> mobileBankingAccountRepository, IRepository<DiningBill> diningBillRepository, BankTransferBLL bankTransferBLL,
            MobileBankTransferBLL mobileBankTransferBLL, IRepository<DiningBillCollectionDetails> diningBillCollectionDetailsRepository,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository,
            IRepository<DiningBillDetails> diningBillDetailsRepository, IRepository<ExternalPerson> externalpersonRepository)
        {
            this.employeeRepository = employeeRepository;
            this.studentRepository = studentRepository;
            this.employeeBasicInfoRepository = employeeBasicInfoRepository;
            this.employeeAddressRepository = employeeAddressRepository;
            this.studentBasicInfoRepository = studentBasicInfoRepository;
            this.studentAddressRepository = studentAddressRepository;
            this.districtPostOfficeRepository = districtPostOfficeRepository;
            this.employeeDesignationRepository = employeeDesignationRepository;
            this.districtUpazilasRepository = districtUpazilasRepository;
            this.districtsRepository = districtsRepository;
            this.classNameRepository = classNameRepository;
            this.diningBillCollectionRepository = diningBillCollectionRepository;
            this.bankAccountRepository = bankAccountRepository;
            this.mobileBankingAccountRepository = mobileBankingAccountRepository;
            this.diningBillRepository = diningBillRepository;
            this.bankTransferBLL = bankTransferBLL;
            this.mobileBankTransferBLL = mobileBankTransferBLL;
            this.diningBillCollectionDetailsRepository = diningBillCollectionDetailsRepository;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.diningBillDetailsRepository = diningBillDetailsRepository;
            this.externalpersonRepository = externalpersonRepository;
            this.userRepository = userRepository;
            this.diningBoarderRepository = diningBoarderRepository;
        }
        public async Task<ResponseMessage> CreateDiningBillCollection(DiningBillCollectionViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                if (model.Amount > 0)
                {
                    CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningBillCollection>
                        (await diningBillCollectionRepository.GetAsync(), token);
                    List<DiningBillCollectionDetails> diningBillCollectionDetails = new List<DiningBillCollectionDetails>();
                    InstruementViewModel instruementViewModel = model.Instrument == null ? null :
                        JsonConvert.DeserializeObject<InstruementViewModel>(model.Instrument);

                    DiningBillCollection diningBillCollection = new DiningBillCollection();
                    diningBillCollection.Id = codeAndId.Id + 1;
                    diningBillCollection.InstituteId = token.InstituteID;
                    diningBillCollection.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                    diningBillCollection.DiningBillCollectionNo = codeAndId.No;
                    diningBillCollection.BoarderId = model.BoarderId;
                    diningBillCollection.PaymentModeId = model.PaymentModeId;
                    diningBillCollection.Note = instruementViewModel == null ? model.Note : instruementViewModel.Note;
                    diningBillCollection.Amount = model.Amount;
                    diningBillCollection.Instrument = model.Instrument;
                    diningBillCollection.CollectionDate = model.CollectionDate;
                    diningBillCollection.CapturedBy = token.UserID;
                    diningBillCollection.CapturedDate = DateTime.UtcNow;

                    if (model.PaymentModeId == (byte)CollectionMode.Cash)
                    {
                        diningBillCollection.Status = (byte)TransferStatus.Approved;
                    }
                    else
                    {
                        diningBillCollection.Status = (byte)TransferStatus.Requested;
                        diningBillCollection.RequestedBy = token.UserID;
                        diningBillCollection.RequestedDate = DateTime.UtcNow;
                    }
                    DiningBillCollection result = await diningBillCollectionRepository.AddAsync(diningBillCollection);
                    if (result != null)
                    {
                        long DiningBillCollectionDetailsId = await IdGenerator.GenerateChildClassIdAsync<DiningBillCollectionDetails>
                                (await diningBillCollectionDetailsRepository.GetAsync());
                        foreach (DiningBillCollectionDetails item in model.DiningBillCollectionDetails)
                        {
                            if (item.Amount > 0)
                            {
                                DiningBillCollectionDetails collectionDetails = new DiningBillCollectionDetails()
                                {
                                    Id = DiningBillCollectionDetailsId + 1,
                                    Amount = item.Amount,
                                    DiningBillCollectionId = result.Id,
                                    DiningBillId = item.DiningBillId,
                                    Status = (byte)Status.Active
                                };
                                diningBillCollectionDetails.Add(collectionDetails);
                                DiningBillCollectionDetailsId++;
                            }
                        }
                        var collectionDetailsResult = await diningBillCollectionDetailsRepository
                            .AddRangeAsync(diningBillCollectionDetails);
                        if (collectionDetailsResult.Count > 0)
                        {
                            if (model.PaymentModeId == (byte)CollectionMode.AccountPayeeCheque)
                            {
                                BankTransferViewModel bankTransfer = new BankTransferViewModel();

                                bankTransfer.BankAccountId = (long)instruementViewModel.BankAccountId;
                                bankTransfer.Deposit = model.Amount;
                                bankTransfer.RequestBy = token.UserID;
                                bankTransfer.TransferFromForm = (byte)TransferFromForm.DonationCollection;
                                bankTransfer.TransectionDate = (DateTime)instruementViewModel.TransferDate;
                                bankTransfer.Status = (byte)TransferStatus.Requested;
                                bankTransfer.Note = "This transaction if form dining bill collection with id-" + result.DiningBillCollectionNo;

                                response = await bankTransferBLL.CreateBankTransfer(bankTransfer, response, token);
                                if (response.ResponseObj != null)
                                {
                                    response.StatusCode = (byte)StatusCode.Success;
                                    response.Message = "Dining bill Collection successfully.";
                                }
                                else
                                {
                                    await diningBillCollectionRepository.DeleteAsync(result);
                                    response.StatusCode = (byte)StatusCode.Failed;
                                    response.Message = "Failed to donate.";
                                }
                            }
                            else if (model.PaymentModeId == (byte)CollectionMode.DepositeToBank)
                            {
                                BankTransferViewModel bankTransfer = new BankTransferViewModel();

                                bankTransfer.BankAccountId = (long)instruementViewModel.BankAccountId;
                                bankTransfer.Deposit = model.Amount;
                                bankTransfer.RequestBy = token.UserID;
                                bankTransfer.TransectionDate = (DateTime)instruementViewModel.DepositDate;
                                bankTransfer.TransectionNo = instruementViewModel.TransectionNo;
                                bankTransfer.TransferFromForm = (byte)TransferFromForm.DonationCollection;
                                bankTransfer.Status = (byte)TransferStatus.Requested;
                                bankTransfer.Note = "This transaction if form dining bill collection with id-" + result.DiningBillCollectionNo;

                                response = await bankTransferBLL.CreateBankTransfer(bankTransfer, response, token);
                                if (response.ResponseObj != null)
                                {
                                    response.StatusCode = (byte)StatusCode.Success;
                                    response.Message = "Dining bill collection successfully.";
                                }
                                else
                                {
                                    await diningBillCollectionRepository.DeleteAsync(result);
                                    response.StatusCode = (byte)StatusCode.Failed;
                                    response.Message = "Failed to donate.";
                                }
                            }
                            else if (model.PaymentModeId == (byte)CollectionMode.MobileBanking)
                            {
                                MobileBankTransferViewModel mobileBankTransfer = new MobileBankTransferViewModel();

                                mobileBankTransfer.MobileBankingAccId = (long)instruementViewModel.MobileBankingAccId;
                                mobileBankTransfer.TransectionNo = instruementViewModel.TransectionNo;
                                mobileBankTransfer.Deposit = model.Amount;
                                mobileBankTransfer.RequestBy = token.UserID;
                                mobileBankTransfer.TransectionDate = (DateTime)instruementViewModel.TransferDate;
                                mobileBankTransfer.TransferFrom = (byte)TransferFromForm.DonationCollection;
                                mobileBankTransfer.Status = (byte)TransferStatus.Requested;
                                mobileBankTransfer.Note = "This transaction if form dining bill collection with id-" + result.DiningBillCollectionNo;

                                response = await mobileBankTransferBLL.CreateMobileBankingTransfer(mobileBankTransfer, response, token);
                                if (response.ResponseObj != null)
                                {
                                    response.StatusCode = (byte)StatusCode.Success;
                                    response.Message = "Dining Bill Collection successfully.";
                                }
                                else
                                {
                                    await diningBillCollectionRepository.DeleteAsync(result);
                                    response.StatusCode = (byte)StatusCode.Failed;
                                    response.Message = "Failed to donate.";
                                }
                            }
                            else
                            {
                                response.StatusCode = (byte)StatusCode.Success;
                                response.Message = "Dining Bill Collection successfully.";
                            }
                        }

                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.Failed;
                    response.Message = "Dining Bill Collection save failed.";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Bill Collection! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningBillCollection(DiningBillCollectionViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBillCollection diningBillCollection = await diningBillCollectionRepository.GetAsync((long)model.Id);
                if (diningBillCollection != null)
                {
                    diningBillCollection.BoarderId = model.BoarderId;
                    diningBillCollection.PaymentModeId = model.PaymentModeId;
                    diningBillCollection.Note = model.Note;
                    diningBillCollection.Amount = model.Amount;
                    diningBillCollection.Instrument = model.Instrument;
                    diningBillCollection.UpdatedBy = token.UserID;
                    diningBillCollection.UpdatedDate = DateTime.Now;

                    DiningBillCollection result = await diningBillCollectionRepository.UpdateAsync(diningBillCollection);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Bill Collection Created Successfully With ID: " + result.DiningBillCollectionNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.DiningBillCollectionNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Bill Collection! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningBillCollection(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBillCollection diningBillCollection = await diningBillCollectionRepository.GetAsync(id);
                    if (diningBillCollection != null)
                    {
                        await diningBillCollectionRepository.DeleteAsync(diningBillCollection);
                        List<DiningBillCollectionDetails> lstDiningBillCollectionDetails = await diningBillCollectionDetailsRepository.FilterListAsync(x => x.DiningBillCollectionId == id);
                        if (lstDiningBillCollectionDetails.Count > 0)
                        {
                            await diningBillCollectionDetailsRepository.DeleteRangeAsync(lstDiningBillCollectionDetails);
                        }
                        response.Message = "Dining Bill Collection Deleted Succesfully with id: " + diningBillCollection.DiningBillCollectionNo;
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBillCollection;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Bill Collection";
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
        public async Task<ResponseMessage> searchDiningBillCollectionForTable(ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                List<DiningBillCollection> diningBillCollections = await DataController.GetDataList<DiningBillCollection>(token);
                List<DiningBillCollectionTableData> diningBillCollectionTableDatas = new List<DiningBillCollectionTableData>();

                if (diningBillCollections.Count > 0)
                {
                    foreach (DiningBillCollection diningBillCollection in diningBillCollections)
                    {
                        DiningBillCollectionTableData diningBillCollectionTableData = new DiningBillCollectionTableData
                        {
                            Id = diningBillCollection.Id,
                            DiningBillCollectionNo = diningBillCollection.DiningBillCollectionNo,
                            BoarderId = diningBillCollection.BoarderId,
                            PaymentModeId = diningBillCollection.PaymentModeId,
                            Note = diningBillCollection.Note,
                            Amount = diningBillCollection.Amount,
                            Instrument = diningBillCollection.Instrument,
                            CollectionDate = diningBillCollection.CollectionDate,
                            CollectionDateString = Formater.FormatDateddMMyyyy(diningBillCollection.CollectionDate),
                            InstituteId = diningBillCollection.InstituteId,
                            InstituteName = diningBillCollection.InstituteId > 0 ? instituteRepository.Get(diningBillCollection.InstituteId).InstituteName : null,
                            CampusName = diningBillCollection.CampusId > 0 ? campusRepository.Get(diningBillCollection.CampusId).CampusName : null,
                            CampusId = diningBillCollection.CampusId,
                            Status = diningBillCollection.Status,
                            StatusName = Formater.GetEnumValueDescription<TransferStatus>(diningBillCollection.Status),
                        };
                        DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(diningBillCollection.BoarderId);
                        diningBillCollectionTableData.BoarderName = diningBoarder.BoarderTypeId == (byte)BoarderType.Employee ?
                                employeeRepository.Get(diningBoarder.DiningBoarderId).EmployeeName :
                                studentRepository.Get(diningBoarder.DiningBoarderId).StudentName;

                        diningBillCollectionTableDatas.Add(diningBillCollectionTableData);
                    }
                    if (diningBillCollections != null)
                    {
                        response.ResponseObj = diningBillCollectionTableDatas;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                    else
                    {
                        response.StatusCode = (byte)StatusCode.NotFound;
                        response.Message = "Not found any Dining Bill Collection";
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBillCollection(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBillCollection diningBillCollection = await diningBillCollectionRepository.GetAsync(id);
                    if (diningBillCollection != null && DataController.IsReturnable(diningBillCollection, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBillCollection;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningBillCollection> diningBillCollections = await DataController.GetDataList<DiningBillCollection>(token);
                    if (diningBillCollections.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBillCollections;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Bill Collection";
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
                    DiningBillCollection diningBillCollection = await diningBillCollectionRepository.GetAsync(id);

                    if (diningBillCollection != null && DataController.IsReturnable(diningBillCollection, token))
                    {
                        DiningBillCollectionDetailsViewModel diningBillCollectionDetailsViewModel = new DiningBillCollectionDetailsViewModel
                        {
                            Id = diningBillCollection.Id,
                            DiningBillCollectionNo = diningBillCollection.DiningBillCollectionNo,
                            BoarderId = diningBillCollection.BoarderId,
                            PaymentModeName = Formater.GetEnumValueDescription<PaymentMode>(diningBillCollection.PaymentModeId),
                            BoarderNo = diningBoarderRepository.Get(diningBillCollection.BoarderId).DiningBoarderNo,
                            Note = diningBillCollection.Note,
                            Amount = diningBillCollection.Amount,
                            CollectionDateString = Formater.FormatDateddMMyyyy(diningBillCollection.CollectionDate),
                            Status = diningBillCollection.Status,
                            CapturedByName = userRepository.Get(diningBillCollection.CapturedBy).FullName,
                            CapturedDateString = Formater.FormatDateddMMyyyy(diningBillCollection.CapturedDate),
                            RequestedByName = diningBillCollection.RejectedBy > 0 ? userRepository.Get(diningBillCollection.RequestedBy).FullName : null,
                            RequestedDateString = diningBillCollection.RequestedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBillCollection.RequestedDate) : null,
                            UpdatedByName = diningBillCollection.UpdatedBy != null ?
                                userRepository.Get(diningBillCollection.UpdatedBy).FullName : null,
                            UpdatedDateString = diningBillCollection.UpdatedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBillCollection.UpdatedDate) : null,
                            ForwardedByName = diningBillCollection.ForwardedBy != null ?
                                userRepository.Get(diningBillCollection.ForwardedBy).FullName : null,
                            ForwardedDateString = diningBillCollection.ForwardedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBillCollection.ForwardedDate) : null,
                            ApprovedByName = diningBillCollection.ApprovedBy != null ?
                                userRepository.Get(diningBillCollection.ApprovedBy).FullName : null,
                            ApprovedDateString = diningBillCollection.ApprovedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBillCollection.ApprovedDate) : null,
                            RejectedByName = diningBillCollection.RejectedBy != null ?
                                userRepository.Get(diningBillCollection.RejectedBy).FullName : null,
                            RejectedDateString = diningBillCollection.RejectedDate != null ?
                                Formater.FormatDateddMMyyyy(diningBillCollection.RejectedDate) : null,
                            StatusName = Formater.GetEnumValueDescription<TransferStatus>(diningBillCollection.Status)
                        };
                        if (diningBillCollection.Instrument != null)
                        {
                            InstruementViewModel instrument = JsonConvert.DeserializeObject<InstruementViewModel>(diningBillCollection.Instrument);
                            BankAccount bankAccount = await bankAccountRepository.GetAsync(instrument.BankAccountId);
                            MobileBankingAccount mobileBankingAccount = await mobileBankingAccountRepository.GetAsync(instrument.MobileBankingAccId);
                            InstruementDetailsViewModel instruementDetailsViewModel = new InstruementDetailsViewModel()
                            {
                                AccountNo = instrument.AccountNo,
                                BankName = instrument.BankName,
                                Branch = instrument.Branch,
                                ChequeDateString = Formater.FormatDateddMMyyyy(instrument.ChequeDate),
                                ChequeNo = instrument.ChequeNo,
                                DepositDateString = Formater.FormatDateddMMyyyy(instrument.DepositDate),
                                ExternalName = instrument.ExternalId > 0 ? externalpersonRepository.Get(instrument.ExternalId).ExternalPersonName : null,
                                TransectionNo = instrument.TransectionNo,
                                Note = instrument.Note,
                                TransferDateString = Formater.FormatDateddMMyyyy(instrument.TransferDate),
                                MobileAccountName = instrument.MobileBankingAccId > 0 ? Formater.GetEnumValueDescription<MobileBankingAccType>(mobileBankingAccount.MobileBankingAccType) + " - " +
                                    Formater.GetEnumValueDescription<MobileBankingType>(mobileBankingAccount.MobileBankingType) + " - " + mobileBankingAccount.MobileBankingMobileNo : null,
                                AccountName = instrument.BankAccountId > 0 ? bankAccount.AccountName + " - " + bankAccount.AccountNo + " - " + bankAccount.BankName + " - " + bankAccount.BranchName : null,
                            };
                            diningBillCollectionDetailsViewModel.InstrumentDetails = instruementDetailsViewModel;
                        }
                        List<DiningBillCollectionDetails> billCollectionDetails = await diningBillCollectionDetailsRepository
                            .FilterListAsync(x => x.DiningBillCollectionId == diningBillCollection.Id);
                        List<DiningBillDetailsViewModel> diningBillDetailsViewModels = new List<DiningBillDetailsViewModel>();
                        foreach (DiningBillCollectionDetails item in billCollectionDetails)
                        {
                            decimal total = 0;
                            DiningBill diningBill = await diningBillRepository.GetAsync(item.DiningBillId);
                            List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                                x.DiningBillId == item.DiningBillId);
                            //DiningBillDetails diningBillDetail = diningBillDetails.FirstOrDefault(x => x.BoarderId == item.b)
                            List<DiningBillCollection> collections = await diningBillCollectionRepository
                                .FilterListAsync(x => x.Status == (byte)TransferStatus.Approved);
                            foreach (var col in collections)
                            {

                                List<DiningBillCollectionDetails> details = await diningBillCollectionDetailsRepository
                                    .FilterListAsync(x => x.DiningBillCollectionId == col.Id);
                                foreach (var d in details)
                                {
                                    if (d.DiningBillId == item.DiningBillId && col.BoarderId == diningBillCollection.BoarderId)
                                    {
                                        total += d.Amount;
                                    }
                                };
                            }
                            diningBillCollectionDetailsViewModel.TotalPaidAmount = total;
                            DiningBillDetailsViewModel diningBillDetailsViewModel = new DiningBillDetailsViewModel
                            {
                                DiningBillNo = diningBill.DiningBillNo,
                                BillMonth = diningBill.BillMonth,
                                BoarderId = diningBillCollection.BoarderId,
                                MealRate = diningBill.MealRate,
                                TotalMeal = diningBillDetails.Sum(x => x.TotalMeal),
                                TotalBill = diningBillDetails.Sum(x => x.TotalBill),
                                BillForMonthName = Formater.GetMonthName(diningBill.BillMonth)
                                + " - " + diningBill.BillMonth.Year
                            };
                            diningBillDetailsViewModel.Amount = item.Amount;
                            diningBillDetailsViewModels.Add(diningBillDetailsViewModel);
                        };
                        diningBillCollectionDetailsViewModel.DiningBillDetails = diningBillDetailsViewModels;
                        response.ResponseObj = diningBillCollectionDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Bill Collection";
                }
            }

            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBillCollectionAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = JsonConvert.DeserializeObject<long>(requestMessage.Content);

                DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(id);
                if (diningBoarder != null && DataController.IsReturnable(diningBoarder, token))
                {
                    DiningBillCollectionAddViewModel diningBillCollectionAddViewModel = new DiningBillCollectionAddViewModel();
                    var details = await GetBorderDetails(diningBoarder);
                    diningBillCollectionAddViewModel.DiningBorderDetails = details;

                    List<Object> CollectionModeList = new List<object>();
                    diningBillCollectionAddViewModel.BankAccounts = await DataController.GetActiveDataListAsync<BankAccount>(token);
                    var keys = Enum.GetValues(typeof(CollectionMode));
                    foreach (var key in keys)
                    {
                        Object listItem = new { name = key.ToString(), id = (int)key };
                        CollectionModeList.Add(listItem);
                    }
                    diningBillCollectionAddViewModel.CollectionMode = CollectionModeList;

                    List<MobileBankingAccount> mobileBankingAccounts = await DataController.GetActiveDataListAsync<MobileBankingAccount>(token);
                    List<MobileBankingAccountDetailsViewModel> mobileBankingAccountDetailsViewModels = new List<MobileBankingAccountDetailsViewModel>();

                    if (mobileBankingAccounts.Count > 0)
                    {
                        foreach (MobileBankingAccount mobileBankingAccount in mobileBankingAccounts)
                        {
                            MobileBankingAccountDetailsViewModel mobileBankingAccountDetailsViewModel = new MobileBankingAccountDetailsViewModel
                            {
                                Id = mobileBankingAccount.Id,
                                MobileBankingAccTypeName = Formater.GetEnumValueDescription<MobileBankingAccType>(mobileBankingAccount.MobileBankingAccType),
                                MobileBankingTypeName = Formater.GetEnumValueDescription<MobileBankingType>(mobileBankingAccount.MobileBankingType),
                                MobileBankingMobileNo = mobileBankingAccount.MobileBankingMobileNo
                            };
                            mobileBankingAccountDetailsViewModels.Add(mobileBankingAccountDetailsViewModel);
                        }
                    }
                    diningBillCollectionAddViewModel.MobileBankingAccounts = mobileBankingAccountDetailsViewModels;
                    List<DiningBillDetails> diningBillDetails = await diningBillDetailsRepository.FilterListAsync(x =>
                        x.BoarderId == diningBoarder.Id);
                    List<DiningBill> diningBills = await diningBillRepository.FilterListAsync(x =>
                       diningBillDetails.Select(y => y.DiningBillId).Contains(x.Id));
                    foreach (DiningBill diningBill in diningBills)
                    {
                        List<DiningBillCollectionDetails> diningBillCollectionDetails = await diningBillCollectionDetailsRepository
                            .FilterListAsync(x => x.DiningBillId == diningBill.Id);
                        decimal payableAmount = diningBillDetails.FindAll(x => x.DiningBillId == diningBill.Id).Sum(x => x.TotalBill);
                        decimal totalPaidAmount = 0;
                        decimal dueAmount = 0;
                        if (diningBillCollectionDetails.Count > 0)
                        {
                            totalPaidAmount = diningBillCollectionDetails.Sum(x => x.Amount);
                        }
                        dueAmount = payableAmount - totalPaidAmount;
                        DiningBillPayableViewModel diningBillPayableViewModel = new DiningBillPayableViewModel()
                        {
                            DiningBillId = diningBill.Id,
                            MonthName = diningBill.BillMonth.ToString("MMMM") + ' ' + diningBill.BillMonth.Year,
                            Payable = dueAmount,
                            PayingAmount = dueAmount
                        };
                        diningBillCollectionAddViewModel.DiningBillPayable.Add(diningBillPayableViewModel);
                    }
                    response.ResponseObj = diningBillCollectionAddViewModel;
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found! The boarder may not active.";
                }
            }
            catch (Exception e)
            {

            }
            return response;
        }
        public async Task<DiningBoarderDetailsViewModel> GetBorderDetails(DiningBoarder diningBoarder)
        {
            Employee employee;
            Student students;
            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
            {
                BoarderNo = diningBoarder.DiningBoarderNo,
                Id = diningBoarder.Id,
                CampusId = diningBoarder.CampusId,
                CampusName = campusRepository.Get(diningBoarder.CampusId).CampusName,
                BoarderTypeId = diningBoarder.BoarderTypeId,
                BoarderTypeName = Enum.GetName(typeof(BoarderType), diningBoarder.BoarderTypeId),
                EnrollmentDate = diningBoarder.EnrollmentDate,
                DiningBoarderId = diningBoarder.DiningBoarderId
            };
            if (diningBoarder.DiningBoarderId != null)
            {
                if (diningBoarder.BoarderTypeId == (byte)BoarderType.Employee)
                {
                    employee = await employeeRepository.GetAsync(diningBoarder.DiningBoarderId);
                    diningBoarderDetailsViewModel.BoarderName = employee.EmployeeName;
                    diningBoarderDetailsViewModel.FathersName = employeeBasicInfoRepository.GetAsync().Result
                        .Where(x => x.EmployeeId == employee.Id).Select(x => x.FathersName).FirstOrDefault();
                    diningBoarderDetailsViewModel.DesignationName = employeeDesignationRepository.Get(employee.DesignationId).DesignationName;
                    diningBoarderDetailsViewModel.Mobile = employee.MobileNo;
                    EmployeeAddress employeeAddress = await employeeAddressRepository.FilterOneAsync(x => x.EmployeeId == employee.Id);
                    EmployeeAddressViewModel employeeAddressViewModel = new EmployeeAddressViewModel
                    {
                        PresentVillageHouse = employeeAddress.PresentVillageHouse,
                        PresentRoadBlockSector = employeeAddress.PresentRoadBlockSector,
                        PresentPostOffice = districtPostOfficeRepository.Get(employeeAddress.PresentPostOffice).PostOfficeName,
                        PresentPostCode = employeeAddress.PresentPostCode,
                        PresentUpazila = districtUpazilasRepository.Get(employeeAddress.PresentUpazila).UpazilaName,
                        PresentDistrict = districtsRepository.Get(employeeAddress.PresentDistrict).DistrictName,
                        PermanentVillageHouse = employeeAddress.PermanentVillageHouse,
                        PermanentDistrict = districtsRepository.Get(employeeAddress.PermanentDistrict).DistrictName,
                        PermanentRoadBlockSector = employeeAddress.PermanentRoadBlockSector,
                        PermanentUpazila = districtUpazilasRepository.Get(employeeAddress.PermanentUpazila).UpazilaName,
                        PermanentPostCode = employeeAddress.PermanentPostCode,
                        PermanentPostOffice = districtPostOfficeRepository.Get(employeeAddress.PermanentPostOffice).PostOfficeName
                    };

                    diningBoarderDetailsViewModel.Address = $"{employeeAddressViewModel.PresentDistrict}, " +
                        $"{employeeAddressViewModel.PresentUpazila}, {employeeAddressViewModel.PresentPostOffice} - " +
                        $"{employeeAddressViewModel.PresentPostCode} {employeeAddressViewModel.PresentVillageHouse} - " +
                        $"{employeeAddressViewModel.PresentRoadBlockSector}";
                }
                else
                {
                    students = await studentRepository.GetAsync(diningBoarder.DiningBoarderId);
                    diningBoarderDetailsViewModel.BoarderName = students.StudentName;
                    diningBoarderDetailsViewModel.FathersName = studentBasicInfoRepository.GetAsync().Result
                        .Where(x => x.Id == students.Id).Select(x => x.FatherName).FirstOrDefault();
                    diningBoarderDetailsViewModel.ClassName = classNameRepository.Get(students.ClassId).Name;
                    diningBoarderDetailsViewModel.Mobile = students.MobileNo;
                    StudentAddress studentAddress = await studentAddressRepository.FilterOneAsync(x => x.StudentId == students.Id);
                    StudentAddressDetailsViewModel studentAddressDetailsViewModel = new StudentAddressDetailsViewModel
                    {
                        PermanentDistrictName = districtsRepository.Get(studentAddress.PermanentDistrict).DistrictName,
                        PermanentPostCode = studentAddress.PermanentPostCode,
                        PermanentPostOfficeName = districtPostOfficeRepository.Get(studentAddress.PermanentPostOffice).PostOfficeName,
                        PermanentUpazilaName = districtUpazilasRepository.Get(studentAddress.PermanentUpazila).UpazilaName,
                        PermanentRoadBlockSector = studentAddress.PermanentRoadBlockSector,
                        PermanentVillageHouse = studentAddress.PermanentVillageHouse,
                        PresentDistrictName = districtsRepository.Get(studentAddress.PresentDistrict).DistrictName,
                        PresentPostCode = studentAddress.PresentPostCode,
                        PresentPostOfficeName = districtPostOfficeRepository.Get(studentAddress.PresentPostOffice).PostOfficeName,
                        PresentRoadBlockSector = studentAddress.PresentRoadBlockSector,
                        PresentUpazilaName = districtUpazilasRepository.Get(studentAddress.PresentUpazila).UpazilaName,
                        PresentVillageHouse = studentAddress.PresentVillageHouse
                    };
                    diningBoarderDetailsViewModel.Address = $"{studentAddressDetailsViewModel.PresentDistrictName}, " +
                        $"{studentAddressDetailsViewModel.PresentUpazilaName}, {studentAddressDetailsViewModel.PresentPostOfficeName} - " +
                        $"{studentAddressDetailsViewModel.PresentPostCode} {studentAddressDetailsViewModel.PresentVillageHouse} - " +
                        $"{studentAddressDetailsViewModel.PresentRoadBlockSector}";
                }
            }
            else
            {
                //external boarder details code
            }
            return diningBoarderDetailsViewModel;
        }
    }
}
