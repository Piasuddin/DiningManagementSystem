using System;
using System.Threading.Tasks;
using MERP.EntityFramworkDAL.Models;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CommonBLL;

namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningMealAttendanceController : ControllerBase
    {
        private readonly DiningMealAttendanceBLL diningMealManageBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningMealAttendanceController(TokenExtracter tokenExtracter, DiningMealAttendanceBLL diningMealManageBLL)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningMealManageBLL = diningMealManageBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel diningMealManageViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel>(requestObject.Content);
                var result = await diningMealManageBLL.CreateDiningMealManage(diningMealManageViewModel, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        [HttpPost]
        [Route("createByMealManage")]
        public async Task<ResponseMessage> CreateByMealManage(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel diningMealManageViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel>(requestObject.Content);
                var result = await diningMealManageBLL.CreateMealAttendanceByMealManage(response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        //[HttpPost]
        //[Route("search")]
        //public async Task<ResponseMessage> Search(RequestMessage requestObject)
        //{
        //    ResponseMessage response = new ResponseMessage();
        //    try
        //    {
        //        
        //        token = await tokenExtracter.ExtractTokenAsync(User);
        //        var result = await donationAllocationBLL.SearchDonationAllocation(requestObject, response);
        //        response.Message = result.Message;
        //        response.ResponseObj = result.ResponseObj;
        //        response.StatusCode = result.StatusCode;
        //    }
        //    catch (Exception e)
        //    {
        //        response.StatusCode = (byte)CommonBLL.StatusCode.Exception;
        //        response.Message = "Something going wrong! Please try again...";
        //    }
        //    return response;
        //}
        [HttpPost]
        [Route("update")]
        public async Task<ResponseMessage> Update(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel diningMealManageViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealAttendanceViewModel>(requestObject.Content);
                var result = await diningMealManageBLL.UpdateDiningMealManage(diningMealManageViewModel, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        [HttpPost]
        [Route("searchTableData")]
        public async Task<ResponseMessage> searchForTable(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDiningMealManageForTable(response, requestObject, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        [HttpPost]
        [Route("delete")]
        public async Task<ResponseMessage> Delete(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.DeleteDiningMealManage(requestObject, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        [HttpPost]
        [Route("searchDetails")]
        public async Task<ResponseMessage> SearchDetails(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDetails(requestObject, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        [HttpPost]
        [Route("addData")]
        public async Task<ResponseMessage> SearchDiningMealAddData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDiningMealAddData(requestObject, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
        //}
        [HttpPost]
        [Route("editData")]
        public async Task<ResponseMessage> SearchDiningMealEditData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDiningMealEditData(requestObject, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response = await ExceptionLogger.LogExceptionAsync(e, token, response);
            }
            return response;
        }
    }
}