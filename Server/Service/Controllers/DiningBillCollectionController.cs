using System;
using MERP.EntityFramworkDAL.Models;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CommonBLL;

namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningBillCollectionController : ControllerBase
    {
        private readonly DiningBillCollectionBLL diningBillCollectionBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningBillCollectionController(TokenExtracter tokenExtracter, DiningBillCollectionBLL diningBillCollectionBLL, IHostingEnvironment hostingEnvironment)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningBillCollectionBLL = diningBillCollectionBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningBillCollectionViewModel diningBillCollectionViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningBillCollectionViewModel>(requestObject.Content);
                var result = await diningBillCollectionBLL.CreateDiningBillCollection(diningBillCollectionViewModel, response, token);
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
                MERP.EntityFramworkDAL.ViewModels.DiningBillCollectionViewModel diningBillCollectionViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningBillCollectionViewModel>(requestObject.Content);
                var result = await diningBillCollectionBLL.UpdateDiningBillCollection(diningBillCollectionViewModel, response, token);
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
                var result = await diningBillCollectionBLL.DeleteDiningBillCollection(requestObject, response, token);
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
                var result = await diningBillCollectionBLL.SearchDiningBillCollection(requestObject, response, token);
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
                var result = await diningBillCollectionBLL.searchDiningBillCollectionForTable(response, token);
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
                var result = await diningBillCollectionBLL.SearchDetails(requestObject, response, token);
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
        public async Task<ResponseMessage> SearchDiningBillCollectionAddData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningBillCollectionBLL.SearchDiningBillCollectionAddData(requestObject, response, token);
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