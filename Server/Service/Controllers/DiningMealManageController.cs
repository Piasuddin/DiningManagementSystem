using System;
using System.Threading.Tasks;
using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningMealManageController : ControllerBase
    {
        private readonly DiningMealManageBLL diningMealManageBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningMealManageController(TokenExtracter tokenExtracter, DiningMealManageBLL diningMealManageBLL)
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
                MERP.EntityFramworkDAL.ViewModels.DiningMealManageViewModel diningMealManageViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealManageViewModel>(requestObject.Content);
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
        [Route("search")]
        public async Task<ResponseMessage> Search(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDiningMealManage(requestObject, response, token);
                response.Message = result.Message;
                response.ResponseObj = result.ResponseObj;
                response.StatusCode = result.StatusCode;
            }
            catch (Exception e)
            {
                response.StatusCode = (byte)CommonBLL.StatusCode.Exception;
                response.Message = "Something going wrong! Please try again...";
            }
            return response;
        }
        [HttpPost]
        [Route("update")]
        public async Task<ResponseMessage> Update(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningMealManageViewModel diningMealManageViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealManageViewModel>(requestObject.Content);
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
                var result = await diningMealManageBLL.searchDiningMealManageForTable(requestObject, response, token);
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
        public async Task<ResponseMessage> SearchDiningMealManageAddData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningMealManageBLL.SearchDiningMealManageAddData(requestObject, response, token);
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