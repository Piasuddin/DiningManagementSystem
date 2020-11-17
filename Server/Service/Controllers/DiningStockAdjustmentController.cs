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
    public class DiningStockAdjustmentController : ControllerBase
    {
        private readonly DiningStockAdjustmentBLL diningStockAdjustmentBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningStockAdjustmentController(TokenExtracter tokenExtracter, DiningStockAdjustmentBLL diningStockAdjustmentBLL,
            IHostingEnvironment hostingEnvironment)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningStockAdjustmentBLL = diningStockAdjustmentBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                List<MERP.EntityFramworkDAL.ViewModels.DiningStockAdjustmentAddDataViewModel> diningStockManageViewModel =
                    JsonConvert.DeserializeObject<List<MERP.EntityFramworkDAL.ViewModels.DiningStockAdjustmentAddDataViewModel>>(requestObject.Content);
                var result = await diningStockAdjustmentBLL.CreateDiningStockAdjustment(diningStockManageViewModel, response, token);
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
        [Route("update")]
        public async Task<ResponseMessage> Update(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                List<MERP.EntityFramworkDAL.ViewModels.DiningStockAdjustmentAddDataViewModel> diningStockManageViewModel =
                    JsonConvert.DeserializeObject<List<MERP.EntityFramworkDAL.ViewModels.DiningStockAdjustmentAddDataViewModel>>(requestObject.Content);
                var result = await diningStockAdjustmentBLL.UpdateDiningStockAdjustment(diningStockManageViewModel, response, token);
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
                var result = await diningStockAdjustmentBLL.DeleteDiningStockAdjustment(requestObject, response, token);
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
                var result = await diningStockAdjustmentBLL.SearchDiningStockAdjustment(requestObject, response, token);
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
                var result = await diningStockAdjustmentBLL.searchDiningStockAdjustmentForTable(response, token);
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
                var result = await diningStockAdjustmentBLL.SearchDetails(requestObject, response, token);
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
                var result = await diningStockAdjustmentBLL.SearchDiningStockAdjustmentAddData(requestObject, response, token);
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
        [Route("updateData")]
        public async Task<ResponseMessage> SearchDiningStockManageUpdateData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningStockAdjustmentBLL.SearchDiningStockAdjustmentUpdateData(requestObject, response, token);
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