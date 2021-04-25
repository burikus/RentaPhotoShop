using Microsoft.AspNetCore.Mvc;
using RentaPhotoDbConnector.EFModels;
using System;

namespace RentaPhotoServer.Abstractions
{
    public interface IOrders
    {
        ActionResult GetAllRegisteredOrders();
        ActionResult GetOrderDetailsById(sbyte orderId);
        ActionResult GetRegisteredOrdersByDate(DateTime date);
        ActionResult AddOrder(OrderEntity order, int maxOrderSum);
        ActionResult UpdateOrder(OrderEntity order);
        ActionResult CancelOrder(Int16 orderId);
    }
}
