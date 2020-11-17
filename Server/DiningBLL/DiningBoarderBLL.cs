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
    public class DiningBoarderBLL
    {
        private readonly IRepository<DiningBoarder> diningBoarderRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly DiningBillCollectionBLL diningBillCollectionBLL;
        private readonly IRepository<Institute> instituteRepository;
        private readonly IRepository<Campus> campusRepository;
        private readonly IRepository<DiningBoarderExternal> diningBoarderExternalRepository;
        private readonly AddressFormater addressFormater;
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

        public DiningBoarderBLL(IRepository<DiningBoarder> diningBoarderRepository, IRepository<User> userRepository,
            IRepository<Employee> employeeRepository, IRepository<EmployeeAddress> employeeAddressRepository,
            IRepository<Student> studentRepository, IRepository<EmployeeBasicInfo> employeeBasicInfoRepository,
            IRepository<StudentBasicInfo> studentBasicInfoRepository, IRepository<StudentAddress> studentAddressRepository,
            IRepository<DistrictPostOffice> districtPostOfficeRepository, IRepository<District> districtsRepository,
            IRepository<Designation> employeeDesignationRepository, IRepository<DistrictUpazila> districtUpazilasRepository,
            IRepository<ClassName> classNameRepository, DiningBillCollectionBLL diningBillCollectionBLL,
            IRepository<Institute> instituteRepository, IRepository<Campus> campusRepository, AddressFormater addressFormater,
            IRepository<DiningBoarderExternal> diningBoarderExternalRepository)
        {
            this.diningBoarderRepository = diningBoarderRepository;
            this.userRepository = userRepository;
            this.employeeRepository = employeeRepository;
            this.diningBillCollectionBLL = diningBillCollectionBLL;
            this.instituteRepository = instituteRepository;
            this.campusRepository = campusRepository;
            this.diningBoarderExternalRepository = diningBoarderExternalRepository;
            this.addressFormater = addressFormater;
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
        }
        public async Task<ResponseMessage> CreateDiningBoarder(DiningBoarderViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningBoarder>
                    (await diningBoarderRepository.GetAsync(), token);
                DiningBoarder diningBoarder = new DiningBoarder();
                diningBoarder.Id = codeAndId.Id + 1;
                diningBoarder.InstituteId = token.InstituteID;
                diningBoarder.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                diningBoarder.DiningBoarderNo = codeAndId.No;
                diningBoarder.DiningBoarderId = model.DiningBoarderId;
                diningBoarder.BoarderTypeId = model.BoarderTypeId;
                diningBoarder.EnrollmentDate = model.EnrollmentDate;
                diningBoarder.CreatedBy = token.UserID;
                diningBoarder.CreatedDate = DateTime.UtcNow;
                diningBoarder.Status = (byte)Status.Active;

                DiningBoarder result = await diningBoarderRepository.AddAsync(diningBoarder);

                if (result != null)
                {
                    response.ResponseObj = result;
                    response.Message = "Dining Boarder Created Successfully With ID: " + result.DiningBoarderNo;
                    response.StatusCode = (byte)StatusCode.Success;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Create Dining Boarder! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningBoarder(DiningBoarderViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync((long)model.Id);
                if (diningBoarder != null)
                {
                    diningBoarder.CampusId = model.CampusId != null ? (long)model.CampusId : token.CampusID;
                    diningBoarder.DiningBoarderId = model.DiningBoarderId;
                    diningBoarder.BoarderTypeId = model.BoarderTypeId;
                    diningBoarder.EnrollmentDate = model.EnrollmentDate;
                    diningBoarder.UpdatedBy = token.UserID;
                    diningBoarder.UpdatedDate = DateTime.Now;

                    DiningBoarder result = await diningBoarderRepository.UpdateAsync(diningBoarder);

                    if (result != null)
                    {
                        response.ResponseObj = result;
                        response.Message = "Dining Boarder updated Successfully With ID: " + result.DiningBoarderNo;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.Message = "Not found any data with id: " + model.BoarderNo;
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Dining Boarder! Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> DeleteDiningBoarder(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(id);
                    if (diningBoarder != null)
                    {
                        if (await DeletePreventer.CheckDeletableWithObjectAndPropertyName<DiningBoarder>(diningBoarder, "BoarderId"))
                        {
                            await diningBoarderRepository.DeleteAsync(diningBoarder);
                            response.Message = "Dining Boarder Deleted Succesfuly with id: " + diningBoarder.DiningBoarderNo;
                            response.StatusCode = (byte)StatusCode.Success;
                            response.ResponseObj = diningBoarder;
                        }
                        else
                        {
                            response.Message = "You can't delete this record. This record is referenced to another table.";
                            response.StatusCode = (byte)StatusCode.Failed;
                        }
                    }
                    else
                    {
                        response.Message = "Not found any Dining Boarder";
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
        public async Task<ResponseMessage> searchDiningBoarderForTable(RequestMessage request, ResponseMessage response,
            UserAuthorization token)
        {
            try
            {
                InstituteInfoViewModel model = JsonConvert.DeserializeObject<InstituteInfoViewModel>(request.Content);
                List<DiningBoarder> diningBoarders = await DataController.GetDataList<DiningBoarder>(token, model);
                List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();

                if (diningBoarders.Count > 0)
                {
                    foreach (DiningBoarder diningBoarder in diningBoarders)
                    {
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                        {
                            Id = diningBoarder.Id,
                            BoarderNo = diningBoarder.DiningBoarderNo,
                            BoarderName = diningBoarder.BoarderTypeId == (byte)BoarderType.Student ?
                                studentRepository.Get(diningBoarder.DiningBoarderId).StudentName :
                                diningBoarder.BoarderTypeId == (byte)BoarderType.Outer ?
                                diningBoarderExternalRepository.Get(diningBoarder.DiningBoarderId).Name :
                                employeeRepository.Get(diningBoarder.DiningBoarderId).EmployeeName,
                            BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId),
                            BoarderTypeId = diningBoarder.BoarderTypeId,
                            EnrollmentDate = diningBoarder.EnrollmentDate,
                            EnrollmentDateString = Formater.FormatDateddMMyyyy(diningBoarder.EnrollmentDate),
                            InstituteId = diningBoarder.InstituteId,
                            InstituteName = diningBoarder.InstituteId > 0 ? instituteRepository.Get(diningBoarder.InstituteId).InstituteName : null,
                            CampusName = diningBoarder.CampusId > 0 ? campusRepository.Get(diningBoarder.CampusId).CampusName : null,
                            CampusId = diningBoarder.CampusId,
                            Status = diningBoarder.Status,
                            StatusName = Formater.GetEnumValueDescription<Status>(diningBoarder.Status),
                            DiningBoarderId = diningBoarder.DiningBoarderId,
                            IsDeletable = await DeletePreventer.CheckDeletableWithObjectAndPropertyName(diningBoarder, "BoarderId")
                        };
                        diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                    }
                    if (diningBoarders != null)
                    {
                        diningBoarderDetailsViewModels = diningBoarderDetailsViewModels.OrderBy(x => x.BoarderTypeId).ToList();
                        response.ResponseObj = diningBoarderDetailsViewModels;
                    }
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchBoarderBySearchKey(ResponseMessage response,
            RequestMessage request, UserAuthorization token)
        {
            try
            {
                string key = request.Content.ToLower();
                var studentKeysModel = new SearchKeyViewModel() { Mobile = "MobileNo", Name = "StudentName", No = "DiningBoarderNo" };
                string decision = SearchDecisionMaker.MakeDecision(key, studentKeysModel);
                List<DiningBoarder> diningBoarders = new List<DiningBoarder>();
                List<Student> students = new List<Student>();
                List<Employee> employees = new List<Employee>();
                List<DiningBoarderExternal> diningBoarderExternals = new List<DiningBoarderExternal>();
                if (decision != null)
                {
                    diningBoarders = await DataController.GetActiveDataListAsync<DiningBoarder>(token);
                    if (decision == "DiningBoarderNo")
                    {
                        diningBoarders = diningBoarders.Where(e => e[decision].ToString().ToLower().Contains(key)).ToList();
                        if (diningBoarders.Count > 0)
                        {
                            students = await studentRepository.FilterListAsync(x => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Student).Select(x2 => x2.DiningBoarderId).Contains(x.Id));
                            employees = await employeeRepository.FilterListAsync(x => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Employee).Select(x2 => x2.DiningBoarderId).Contains(x.Id));
                            diningBoarderExternals = await diningBoarderExternalRepository.FilterListAsync(x => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Outer).Select(x2 => x2.DiningBoarderId).Contains(x.Id));
                        }
                    }
                    else
                    {
                        students.AddRange(await studentRepository.FilterListAsync(e => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Student).Select(x2 => x2.DiningBoarderId).Contains(e.Id)
                                && e[decision].ToString().ToLower().Contains(key)));
                        employees.AddRange(await employeeRepository.FilterListAsync(e => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Employee).Select(x2 => x2.DiningBoarderId).Contains(e.Id)
                                && e[decision == "StudentName" ? "EmployeeName" : decision]
                            .ToString().ToLower().Contains(key)));
                        diningBoarderExternals.AddRange(await diningBoarderExternalRepository.FilterListAsync(e => diningBoarders
                                .Where(y => y.BoarderTypeId == (byte)BoarderType.Outer).Select(x2 => x2.DiningBoarderId).Contains(e.Id)
                                && e[decision == "StudentName" ? "Name" : decision].ToString().ToLower().Contains(key)));
                    }
                    List<DiningBoarderDetailsViewModel> diningBoarderDetailsViewModels = new List<DiningBoarderDetailsViewModel>();
                    foreach (var student in students)
                    {
                        DiningBoarder diningBoarder = diningBoarders.FirstOrDefault(x => x.DiningBoarderId == student.Id
                            && x.BoarderTypeId == (byte)BoarderType.Student);
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                        {
                            Id = diningBoarder.Id,
                            CampusId = student.CampusId,
                            CampusName = student.CampusId > 0 ? campusRepository.Get(student.CampusId).CampusName : null,
                            BoarderName = student.StudentName,
                            Mobile = student.MobileNo,
                            BoarderNo = diningBoarder.DiningBoarderNo
                        };
                        diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                    }
                    foreach (var employee in employees)
                    {
                        DiningBoarder diningBoarder = diningBoarders.FirstOrDefault(x => x.DiningBoarderId == employee.Id
                            && x.BoarderTypeId == (byte)BoarderType.Employee);
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                        {
                            Id = diningBoarder.Id,
                            CampusId = employee.CampusId,
                            BoarderName = employee.EmployeeName,
                            CampusName = employee.CampusId > 0 ? campusRepository.Get(employee.CampusId).CampusName : null,
                            Mobile = employee.MobileNo,
                            BoarderNo = diningBoarder.DiningBoarderNo
                        };
                        diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                    }
                    foreach (var diningBoarderExternal in diningBoarderExternals)
                    {
                        DiningBoarder diningBoarder = diningBoarders.FirstOrDefault(x => x.DiningBoarderId == diningBoarderExternal.Id
                            && x.BoarderTypeId == (byte)BoarderType.Outer);
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel
                        {
                            Id = diningBoarder.Id,
                            BoarderName = diningBoarderExternal.Name,
                            Mobile = diningBoarderExternal.MobileNo,
                            BoarderNo = diningBoarder.DiningBoarderNo
                        };
                        diningBoarderDetailsViewModels.Add(diningBoarderDetailsViewModel);
                    }
                    response.ResponseObj = new { diningBoarders = diningBoarderDetailsViewModels };
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBoarder(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(id);
                    if (diningBoarder != null && DataController.IsReturnable(diningBoarder, token))
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBoarder;
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
                        response.StatusCode = (byte)StatusCode.NotFound;
                    }
                }
                else
                {
                    List<DiningBoarder> diningBoarders = await DataController.GetDataList<DiningBoarder>(token);
                    if (diningBoarders.Count > 0)
                    {
                        response.StatusCode = (byte)StatusCode.Success;
                        response.ResponseObj = diningBoarders;
                    }
                    else
                    {
                        response.Message = "Not found any Dining Boarder";
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
                    DiningBoarder diningBoarder = await diningBoarderRepository.GetAsync(id);

                    if (diningBoarder != null && DataController.IsReturnable(diningBoarder, token))
                    {
                        DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel();
                        if (diningBoarder.BoarderTypeId == (byte)BoarderType.Employee)
                        {
                            diningBoarderDetailsViewModel = await GetEmployeeDetails(await employeeRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        }
                        else if (diningBoarder.BoarderTypeId == (byte)BoarderType.Outer)
                        {
                            diningBoarderDetailsViewModel = await GetExternalBoarderDetails(await diningBoarderExternalRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        }
                        else
                        {
                            diningBoarderDetailsViewModel = await GetStudentDetails(await studentRepository.GetAsync(diningBoarder.DiningBoarderId), diningBoarder);
                        }
                        diningBoarderDetailsViewModel.EnrollmentDateString = Formater.FormatDateddMMyyyy(diningBoarder.EnrollmentDate);
                        response.ResponseObj = diningBoarderDetailsViewModel;
                        response.StatusCode = (byte)StatusCode.Success;
                    }
                }
                else
                {
                    response.StatusCode = (byte)StatusCode.NotFound;
                    response.Message = "Not found any Dining Boarder";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBoarderAddData(RequestMessage requestMessage,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                var data = new { searchKey = "", boarderTypeId = 0 };
                dynamic selializeData = JsonConvert.DeserializeObject(requestMessage.Content, data.GetType());
                string searchId = selializeData.searchKey;
                byte boarderType = (byte)selializeData.boarderTypeId;
                bool isExists = true;
                DiningBoarderAddViewModel diningBoarderAddViewModel = new DiningBoarderAddViewModel();
                if (boarderType == (byte)BoarderType.Student)
                {
                    Student students = await studentRepository.FilterOneAsync(x => x.StudentNo == searchId);
                    if (students != null && DataController.IsReturnable(students, token))
                    {
                        if (isExists = await CheckIfAlreadyAdded(students.Id, boarderType, token))
                        {
                            diningBoarderAddViewModel.DiningBoarderDetails = await GetStudentDetails(students, null);
                        }
                    }
                }
                else
                {
                    Employee employees = await employeeRepository.FilterOneAsync(x => x.EmployeeNo == searchId);
                    if (employees != null && DataController.IsReturnable(employees, token))
                    {
                        if (isExists = await CheckIfAlreadyAdded(employees.Id, boarderType, token))
                        {
                            diningBoarderAddViewModel.DiningBoarderDetails = await GetEmployeeDetails(employees, null);
                        }
                    }
                }
                if (diningBoarderAddViewModel.DiningBoarderDetails != null && isExists)
                {
                    response.ResponseObj = diningBoarderAddViewModel;
                }
                else if (diningBoarderAddViewModel.DiningBoarderDetails == null && !isExists)
                {
                    response.Message = "This" + (boarderType == (byte)BoarderType.Student ? " student" : " employee") + " has already been added as dining boarder";
                }
                else
                {
                    response.Message = "Not found any" + (boarderType == (byte)BoarderType.Student ? " student" : " employee") + " with id " + searchId;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        private async Task<bool> CheckIfAlreadyAdded(long diningBoarderId, byte boarderType, UserAuthorization token)
        {
            DiningBoarder diningBoarder = await diningBoarderRepository.FilterOneAsync
                    (x => x.DiningBoarderId == diningBoarderId
                    && x.BoarderTypeId == boarderType && x.InstituteId == token.InstituteID && x.CampusId == token.CampusID);
            return diningBoarder == null;
        }
        public async Task<DiningBoarderDetailsViewModel> GetStudentDetails(Student students, DiningBoarder diningBoarder)
        {
            var studentBasicInfo = await studentBasicInfoRepository.FilterOneAsync(x => x.StudentId == students.Id);
            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel();
            diningBoarderDetailsViewModel.DiningBoarderId = students.Id;
            diningBoarderDetailsViewModel.CampusId = students.CampusId;
            diningBoarderDetailsViewModel.CampusName = campusRepository.Get(students.CampusId).CampusName;
            diningBoarderDetailsViewModel.BoarderTypeId = (byte)BoarderType.Student;
            diningBoarderDetailsViewModel.BoarderName = students.StudentName;
            diningBoarderDetailsViewModel.FathersName = studentBasicInfo.FatherName;
            diningBoarderDetailsViewModel.ClassName = classNameRepository.Get(students.ClassId).Name;
            diningBoarderDetailsViewModel.Mobile = students.MobileNo;
            if (diningBoarder != null)
            {
                diningBoarderDetailsViewModel.BoarderNo = diningBoarder.DiningBoarderNo;
                diningBoarderDetailsViewModel.BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId);
                diningBoarderDetailsViewModel.StatusName = Formater.GetEnumValueDescription<Status>(diningBoarder.Status);
                diningBoarderDetailsViewModel.CreatedByName = userRepository.Get(diningBoarder.CreatedBy).FullName;
                diningBoarderDetailsViewModel.CreatedDateString = Formater.FormatDateddMMyyyy(diningBoarder.CreatedDate);
            }
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
            return diningBoarderDetailsViewModel;
        }
        public async Task<DiningBoarderDetailsViewModel> GetEmployeeDetails(Employee employees, DiningBoarder diningBoarder)
        {
            var employeeBasicInfo = await employeeBasicInfoRepository.FilterOneAsync(x => x.EmployeeId == employees.Id);
            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel();
            diningBoarderDetailsViewModel.DiningBoarderId = employees.Id;
            diningBoarderDetailsViewModel.CampusId = employees.CampusId;
            diningBoarderDetailsViewModel.CampusName = campusRepository.Get(employees.CampusId).CampusName;
            diningBoarderDetailsViewModel.BoarderTypeId = (byte)BoarderType.Employee;
            diningBoarderDetailsViewModel.BoarderName = employees.EmployeeName;
            diningBoarderDetailsViewModel.FathersName = employeeBasicInfo.FathersName;
            diningBoarderDetailsViewModel.DesignationName = employeeDesignationRepository.Get(employees.DesignationId).DesignationName;
            diningBoarderDetailsViewModel.Mobile = employees.MobileNo;
            if (diningBoarder != null)
            {
                diningBoarderDetailsViewModel.BoarderNo = diningBoarder.DiningBoarderNo;
                diningBoarderDetailsViewModel.BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId);
                diningBoarderDetailsViewModel.StatusName = Formater.GetEnumValueDescription<Status>(diningBoarder.Status);
                diningBoarderDetailsViewModel.CreatedByName = userRepository.Get(diningBoarder.CreatedBy).FullName;
                diningBoarderDetailsViewModel.CreatedDateString = Formater.FormatDateddMMyyyy(diningBoarder.CreatedDate);
            }
            EmployeeAddress employeeAddress = await employeeAddressRepository.FilterOneAsync(x => x.EmployeeId == employees.Id);
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
            return diningBoarderDetailsViewModel;
        }
        public async Task<DiningBoarderDetailsViewModel> GetExternalBoarderDetails(DiningBoarderExternal diningBoarderExternal, DiningBoarder diningBoarder)
        {
            DiningBoarderDetailsViewModel diningBoarderDetailsViewModel = new DiningBoarderDetailsViewModel();
            diningBoarderDetailsViewModel.DiningBoarderId = diningBoarderExternal.Id;
            diningBoarderDetailsViewModel.BoarderTypeId = 2;
            diningBoarderDetailsViewModel.BoarderName = diningBoarderExternal.Name;
            diningBoarderDetailsViewModel.FathersName = diningBoarderExternal.FathersName;
            diningBoarderDetailsViewModel.Mobile = diningBoarderExternal.MobileNo;
            if (diningBoarder != null)
            {
                diningBoarderDetailsViewModel.BoarderNo = diningBoarder.DiningBoarderNo;
                diningBoarderDetailsViewModel.BoarderTypeName = Formater.GetEnumValueDescription<BoarderType>(diningBoarder.BoarderTypeId);
                diningBoarderDetailsViewModel.StatusName = Formater.GetEnumValueDescription<Status>(diningBoarder.Status);
                diningBoarderDetailsViewModel.CreatedByName = userRepository.Get(diningBoarder.CreatedBy).FullName;
                diningBoarderDetailsViewModel.CreatedDateString = Formater.FormatDateddMMyyyy(diningBoarder.CreatedDate);
            }
            AddressViewModel address = new AddressViewModel
            {
                District = diningBoarderExternal.PresentDistrict,
                Upazila = diningBoarderExternal.PresentUpazila,
                PostCode = diningBoarderExternal.PresentPostCode,
                PostOffice = diningBoarderExternal.PresentPostOffice,
                RoadBlockSector = diningBoarderExternal.PresentRoadBlockSector,
                VillageHouse = diningBoarderExternal.PresentVillageHouse
            };
            diningBoarderDetailsViewModel.Address = await addressFormater.FormatAddress(address);
            return diningBoarderDetailsViewModel;
        }
    }
}
