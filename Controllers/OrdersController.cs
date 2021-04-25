using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RentaPhotoDbConnector.EFModels;
using RentaPhotoServer.Abstractions;
using RentaPhotoServer.Helpers;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentaPhotoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IOrders _orders;
        private readonly OrdersSettings _ordersConfig;
        public OrdersController(IOptions<OrdersSettings> ordersConfig, IOrders orders)
        {
            _ordersConfig = ordersConfig.Value;
            _orders = orders;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        public ActionResult GetAllRegisteredOrders()
        {
            try
            {
                return _orders.GetAllRegisteredOrders();
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(GetAllRegisteredOrders)} failure. {ex}"
                });
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public ActionResult GetOrderDetailsById(sbyte id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest(new
                    {
                        SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                        Description = $"Order Id is incorrect."
                    });
                }

                return _orders.GetOrderDetailsById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(GetOrderDetailsById)} failure. {ex}"
                });
            }
        }

        // GET api/<OrdersController>/5
        [HttpGet("{date}")]
        public ActionResult GetRegisteredOrdersByDate(DateTime date) // string date
        {
            // DateTime.TryParse(date, out DateTime _date);
            try
            {
                return _orders.GetRegisteredOrdersByDate(date);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(GetRegisteredOrdersByDate)} failure. {ex}"
                });
            }
        }

        // POST api/<OrdersController>/5
        [HttpPost("{id}")]
        public async Task<ActionResult> AddOrder(Int16 id)
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var request = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(request))
                    {
                        var result = JsonConvert.DeserializeObject<OrderEntity>(request);
                        result.OrderId = id;

                        int.TryParse(_ordersConfig.MaxGoodsAmount, out int maxGoodsAmount);
                        if (result.Goods.Count > maxGoodsAmount)
                        {
                            return BadRequest(new
                            {
                                SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                                Description = $"Number of goods exceeds allowed maximum of {maxGoodsAmount}"
                            });
                        }
                        int.TryParse(_ordersConfig.MaxOrderSum, out int maxOrderSum);

                        return _orders.AddOrder(result, maxOrderSum);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(AddOrder)} failure. {ex}"
                });
            }

            return BadRequest(new
            {
                SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                Description = $"Order {id} wasn't added."
            });
        }

        // PUT api/<OrdersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrder(int id)
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var request = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(request))
                    {
                        var result = JsonConvert.DeserializeObject<OrderEntity>(request);

                        return _orders.UpdateOrder(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(UpdateOrder)} failure. {ex}"
                });
            }

            return BadRequest(new
            {
                SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                Description = $"Order {id} wasn't updated."
            });
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchOrder(int id)
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var request = await reader.ReadToEndAsync();

                    if (!string.IsNullOrEmpty(request))
                    {
                        var result = JsonConvert.DeserializeObject<OrderEntity>(request);

                        return _orders.UpdateOrder(result);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(UpdateOrder)} failure. {ex}"
                });
            }

            return BadRequest(new
            {
                SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                Description = $"Order {id} wasn't updated."
            });
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public ActionResult CancelOrder(Int16 id)
        {
            try
            {
                return _orders.CancelOrder(id);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    SatusCode = $"{StatusCodes.Status400BadRequest} {HttpStatusCode.BadRequest}",
                    Description = $"Executing {nameof(CancelOrder)} failure. {ex}"
                });
            }
        }
    }
}
