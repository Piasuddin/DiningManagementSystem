using System;
using MERP.EntityFramworkDAL.Models;
using MERP.EntityFramworkDAL.ViewModels;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CommonBLL;
namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningBillController : ControllerBase
    {
        private readonly DiningBillBLL diningBillBLL;
        UserAuthorization token; private readonly TokenExtracter tokenExtracter;
        public DiningBillController(TokenExtracter tokenExtracter, DiningBillBLL diningBillBLL)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningBillBLL = diningBillBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                //EntityFramworkDAL.ViewModels.DiningBillViewModel diningBill = JsonConvert.DeserializeObject<DiningBillViewModel>(requestObject.Content);
                var result = await diningBillBLL.CreateDiningBill(response, token, requestObject);
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
        [Route("Update")]
        public async Task<ResponseMessage> Update(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                DiningBillViewModel diningBill = JsonConvert.DeserializeObject<DiningBillViewModel>(requestObject.Content);
                var result = await diningBillBLL.UpdateDiningBill(diningBill, response, token);
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
                var result = await diningBillBLL.DeleteDiningBill(requestObject, response, token);
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
                var result = await diningBillBLL.SearchDiningBill(requestObject, response, token);
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
                var result = await diningBillBLL.SearchDetails(requestObject, response, token);
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
        public async Task<ResponseMessage> SearchTableData(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningBillBLL.searchDiningBillForTable(response, token);
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
        [Route("statement")]
        public async Task<ResponseMessage> SearchDiningBoarderStatement(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                DiningBoarderStatementSearchVM searchVM = JsonConvert.DeserializeObject<DiningBoarderStatementSearchVM>(requestObject.Content);
                var result = await diningBillBLL.GetDiningBoarderStatement(searchVM, response, token);
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
        [Route("expenseStatement")]
        public async Task<ResponseMessage> SearchDiningExpenseStatement(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                var result = await diningBillBLL.GetDiningExpenseStatement(requestObject, response, token);
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
        [Route("diningBoarderBillDetails")]
        public async Task<ResponseMessage> GetDiningBoarderBillDetails(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                SearchDiningBoarderBillDetails searchVM = JsonConvert.DeserializeObject<SearchDiningBoarderBillDetails>(requestObject.Content);
                var result = await diningBillBLL.GetDiningBoarderBillDetails(searchVM, response, token);
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