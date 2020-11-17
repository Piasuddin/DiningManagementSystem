using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MERP.EntityFramworkDAL.Models;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CommonBLL;

namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningStockManageController : ControllerBase
    {
        private readonly DiningStockManageBLL diningStockManageBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningStockManageController(TokenExtracter tokenExtracter, DiningStockManageBLL diningStockManageBLL, IHostingEnvironment hostingEnvironment)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningStockManageBLL = diningStockManageBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                List<MERP.EntityFramworkDAL.ViewModels.DiningStockManageAddDataViewModel> diningStockManageViewModel =
                    JsonConvert.DeserializeObject<List<MERP.EntityFramworkDAL.ViewModels.DiningStockManageAddDataViewModel>>(requestObject.Content);
                var result = await diningStockManageBLL.CreateDiningStockManage(diningStockManageViewModel, response, token);
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
        //[Route("update")]
        //public async Task<ResponseMessage> Update(RequestMessage requestObject)
        //{
        //    ResponseMessage response = new ResponseMessage();
        //    try
        //    {
        //        token = await tokenExtracter.ExtractTokenAsync(User);
        //        MERP.EntityFramworkDAL.ViewModels.DiningStockManageViewModel diningStockManageViewModel =
        //            JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningStockManageViewModel>(requestObject.Content);
        //        var result = await diningStockManageBLL.UpdateDiningStockManage(diningStockManageViewModel, response, token);
        //        response.Message = result.Message;
        //        response.ResponseObj = result.ResponseObj;
        //        response.StatusCode = result.StatusCode;
        //    }            catch (Exception e)
        //    {
        //       response = await ExceptionLogger.LogExceptionAsync(e, token, response);
        //    }
        //    return response;
        //}
        [HttpPost]
        [Route("delete")]
        public async Task<ResponseMessage> Delete(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningStockManageBLL.DeleteDiningStockManage(requestObject, response, token);
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
                var result = await diningStockManageBLL.SearchDiningStockManage(requestObject, response, token);
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
                var result = await diningStockManageBLL.searchDiningStockManageForTable(response, token);
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
                var result = await diningStockManageBLL.SearchDetails(requestObject, response, token);
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
        public async Task<ResponseMessage> SearchDiningStockManageAddData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningStockManageBLL.SearchDiningStockManageAddData(requestObject, response, token);
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