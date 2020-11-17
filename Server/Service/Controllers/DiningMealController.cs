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
    public class DiningMealController : ControllerBase
    {
        private readonly DiningMealBLL diningMealBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningMealController(TokenExtracter tokenExtracter, DiningMealBLL diningMealBLL, IHostingEnvironment hostingEnvironment)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningMealBLL = diningMealBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningMealViewModel diningMealViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealViewModel>(requestObject.Content);
                var result = await diningMealBLL.CreateDiningMeal(diningMealViewModel, response, token);
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
                MERP.EntityFramworkDAL.ViewModels.DiningMealViewModel diningMealViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningMealViewModel>(requestObject.Content);
                var result = await diningMealBLL.UpdateDiningMeal(diningMealViewModel, response, token);
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
                var result = await diningMealBLL.DeleteDiningMeal(requestObject, response, token);
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
                var result = await diningMealBLL.SearchDiningMeal(requestObject, response, token);
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
                var result = await diningMealBLL.searchDiningMealForTable(response, token);
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
                var result = await diningMealBLL.SearchDetails(requestObject, response, token);
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