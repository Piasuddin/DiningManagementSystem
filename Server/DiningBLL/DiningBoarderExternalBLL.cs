using CommonBLL;
using MERP.EntityFramworkDAL.EntityModels;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.Repository;
using MERP.EntityFramworkDAL.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MERP.FinanceBLL
{
    public class DiningBoarderExternalBLL
    {
        private readonly IRepository<DiningBoarderExternal> diningBorderExternalRepository;
        private readonly IRepository<DiningBoarder> diningBoarderRepository;

        public DiningBoarderExternalBLL(IRepository<DiningBoarderExternal> diningBorderExternalRepository, 
            IRepository<DiningBoarder> diningBoarderRepository)
        {
            this.diningBorderExternalRepository = diningBorderExternalRepository;
            this.diningBoarderRepository = diningBoarderRepository;
        }
        public async Task<ResponseMessage> CreateDiningBorderExternal(DiningBorderExternalViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(await IdGenerator.GenerateChildClassIdAsync<DiningBoarderExternal>
                    (await diningBorderExternalRepository.GetAsync()));
                DiningBoarderExternal diningBorderExternal = new DiningBoarderExternal()
                {
                    Id = id + 1,
                    Name = model.Name,
                    AboutBorder = model.AboutBorder,
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    FathersName = model.FathersName,
                    Status = (byte)Status.Active,
                    MobileNo = model.MobileNo,
                    NationalIdNo = model.NationalIdNo,
                    NationalityId = model.NationalityId,
                    PresentDistrict = model.PresentDistrict,
                    PresentPostCode = model.PresentPostCode,
                    PresentPostOffice = model.PresentPostOffice,
                    PresentRoadBlockSector = model.PresentRoadBlockSector,
                    PresentUpazila = model.PresentUpazila,
                    PresentVillageHouse = model.PresentVillageHouse,
                    ReligionId = model.ReligionId
                };
                CodeAndId codeAndId = await IdGenerator.GenerateBaseClassIdAsync<DiningBoarder>
                    (await diningBoarderRepository.GetAsync(), token);
                DiningBoarder diningBoarder = new DiningBoarder
                {
                    Id = codeAndId.Id + 1,
                    DiningBoarderId = diningBorderExternal.Id,
                    BoarderTypeId = model.BoarderTypeId,
                    CampusId = token.CampusID,
                    InstituteId = token.InstituteID,
                    CreatedBy = token.UserID,
                    CreatedDate = DateTime.UtcNow,
                    DiningBoarderNo = codeAndId.No,
                    EnrollmentDate = model.EnrollmentDate,
                    Status = (byte)Status.Active
                };
                using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await diningBoarderRepository.AddAsync(diningBoarder);
                    await diningBorderExternalRepository.AddAsync(diningBorderExternal);
                    transaction.Complete();
                    response.Message = "External dining border created successfull!";
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        public async Task<ResponseMessage> UpdateDiningBorderExternal(DiningBorderExternalViewModel model,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                DiningBoarderExternal diningBorderExternal = await diningBorderExternalRepository.GetAsync(model.Id);
                if (diningBorderExternal != null)
                {
                    diningBorderExternal.AboutBorder = model.AboutBorder;
                    diningBorderExternal.Name = model.Name;
                    diningBorderExternal.DateOfBirth = model.DateOfBirth;
                    diningBorderExternal.Email = model.Email;
                    diningBorderExternal.FathersName = model.FathersName;
                    diningBorderExternal.MobileNo = model.MobileNo;
                    diningBorderExternal.NationalIdNo = model.NationalIdNo;
                    diningBorderExternal.NationalityId = model.NationalityId;
                    diningBorderExternal.PresentDistrict = model.PresentDistrict;
                    diningBorderExternal.PresentPostCode = model.PresentPostCode;
                    diningBorderExternal.PresentPostOffice = model.PresentPostOffice;
                    diningBorderExternal.PresentRoadBlockSector = model.PresentRoadBlockSector;
                    diningBorderExternal.PresentUpazila = model.PresentUpazila;
                    diningBorderExternal.PresentVillageHouse = model.PresentVillageHouse;
                    diningBorderExternal.ReligionId = model.ReligionId;

                    DiningBoarder diningBoarder = await diningBoarderRepository.FilterOneAsync(x => x.DiningBoarderId == diningBorderExternal.Id
                            && x.BoarderTypeId == (byte)BoarderType.Outer && x.CampusId == token.CampusID && x.InstituteId == token.InstituteID);
                    diningBoarder.EnrollmentDate = model.EnrollmentDate; 

                    DiningBoarderExternal result = await diningBorderExternalRepository.UpdateAsync(diningBorderExternal);
                    DiningBoarder diningBoarderResult = await diningBoarderRepository.UpdateAsync(diningBoarder);
                    response.ResponseObj = result;
                    response.Message = "External dining border updated successfully";
                    response.StatusCode = (byte)StatusCode.Success;
                }
                else
                {
                    response.Message = "Not found any data";
                    response.StatusCode = (byte)StatusCode.NotFound;
                }
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
                response.Message = "Failed To Update Examiner Please try again...";
            }
            return response;
        }
        public async Task<ResponseMessage> SearchDiningBorderExternal(RequestMessage request,
            ResponseMessage response, UserAuthorization token)
        {
            try
            {
                long id = Convert.ToInt64(JsonConvert.DeserializeObject<string>(request.Content));
                if (id > 0)
                {
                    DiningBoarderExternal diningBoarderExternal = await diningBorderExternalRepository.GetAsync(id);
                    if (diningBoarderExternal != null)
                    {
                        DiningBoarder diningBoarder = await diningBoarderRepository.FilterOneAsync(x => x.DiningBoarderId == id
                            && x.BoarderTypeId == (byte)BoarderType.Outer);
                        if (DataController.IsReturnable(diningBoarder, token))
                        {
                            DiningBorderExternalViewModel diningBorderExternalViewModel = new DiningBorderExternalViewModel
                            {
                                AboutBorder = diningBoarderExternal.AboutBorder,
                                BoarderTypeId = diningBoarder.BoarderTypeId,
                                DateOfBirth = diningBoarderExternal.DateOfBirth,
                                Email = diningBoarderExternal.Email,
                                Id = diningBoarderExternal.Id,
                                FathersName = diningBoarderExternal.FathersName,
                                MobileNo = diningBoarderExternal.MobileNo,
                                Name = diningBoarderExternal.Name,
                                NationalIdNo = diningBoarderExternal.NationalIdNo,
                                NationalityId = diningBoarderExternal.NationalityId,
                                PresentDistrict = diningBoarderExternal.PresentDistrict,
                                PresentPostCode = diningBoarderExternal.PresentPostCode,
                                PresentPostOffice = diningBoarderExternal.PresentPostOffice,
                                PresentRoadBlockSector = diningBoarderExternal.PresentRoadBlockSector,
                                PresentUpazila = diningBoarderExternal.PresentUpazila,
                                PresentVillageHouse = diningBoarderExternal.PresentVillageHouse,
                                ReligionId = diningBoarderExternal.ReligionId,
                                Status = diningBoarderExternal.Status,
                                EnrollmentDate = diningBoarder.EnrollmentDate
                            };
                            response.StatusCode = (byte)StatusCode.Success;
                            response.ResponseObj = diningBorderExternalViewModel;
                        }
                    }
                    else
                    {
                        response.Message = "Not found any data with id: " + id;
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
    }
}
