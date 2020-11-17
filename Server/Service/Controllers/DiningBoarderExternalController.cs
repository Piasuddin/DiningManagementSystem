using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonBLL;
using MERP.EntityFramworkDAL.Models;
using MERP.FinanceBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MERP.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiningBoarderExternalController : ControllerBase
    {
        private readonly DiningBoarderExternalBLL diningBorderExternalBLL;
        UserAuthorization token; 
        private readonly TokenExtracter tokenExtracter;
        public DiningBoarderExternalController(TokenExtracter tokenExtracter, DiningBoarderExternalBLL diningBorderExternalBLL)
        {
            this.tokenExtracter = tokenExtracter;
            this.diningBorderExternalBLL = diningBorderExternalBLL;
        }
        [HttpPost]
        [Route("save")]
        public async Task<ResponseMessage> Save(RequestMessage requestObject)
        {
            ResponseMessage response = new ResponseMessage();
            try
            {
                token = await tokenExtracter.ExtractTokenAsync(User);
                MERP.EntityFramworkDAL.ViewModels.DiningBorderExternalViewModel diningBorderExternalViewModel =
                    JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningBorderExternalViewModel>(requestObject.Content);
                var result = await diningBorderExternalBLL.CreateDiningBorderExternal(diningBorderExternalViewModel, response, token);
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
                MERP.EntityFramworkDAL.ViewModels.DiningBorderExternalViewModel diningBorderExternalViewModel =
                   JsonConvert.DeserializeObject<MERP.EntityFramworkDAL.ViewModels.DiningBorderExternalViewModel>(requestObject.Content);
                var result = await diningBorderExternalBLL.UpdateDiningBorderExternal(diningBorderExternalViewModel, response, token);
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
                var result = await diningBorderExternalBLL.SearchDiningBorderExternal(requestObject, response, token);
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