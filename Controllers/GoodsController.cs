using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentaPhotoServer.Abstractions;
using System;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentaPhotoServer.Controllers
{
    // TODO Add logging
    [Route("api/[controller]")]
    [ApiController]
    public class GoodsController : ControllerBase
    {
        private IGoods _goods;
        public GoodsController(IGoods goods)
        {
            _goods = goods;
        }

        // GET: api/<GoodsController>
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            try
            {
                return _goods.GetAllProducts();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(GetAllProducts)} failure. {ex}"
                });
            }
        }

        // GET api/<GoodsController>/5
        [HttpGet("{id}")]
        public ActionResult GetProductByArticle(sbyte id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest(new
                    {
                        SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                        Description = $"Article Id is incorrect."
                    });
                }

                return _goods.GetProductByArticle(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(GetProductByArticle)} failure. {ex}"
                });
            }
        }
    }
}
