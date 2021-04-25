using Microsoft.AspNetCore.Mvc;
using RentaPhotoDbConnector.Abstractions;
using RentaPhotoDbConnector.EFModels;
using RentaPhotoServer.Abstractions;
using System;
using System.Threading;

namespace RentaPhotoServer.Services
{
    public class Orders : IOrders
    {
        IDbOrders _db;
        private Semaphore _semaphore = new Semaphore(1, 1);

        public Orders(IDbOrders db)
        {
            _db = db;
        }

        public ActionResult GetAllRegisteredOrders()
        {
            _semaphore.WaitOne();

            try
            {
                return _db.GetRegisteredOrders();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult GetOrderDetailsById(sbyte orderId)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.GetOrderDetailsById(orderId);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult GetRegisteredOrdersByDate(DateTime date)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.GetRegisteredOrdersByDate(date);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult AddOrder(OrderEntity order, int maxOrderSum)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.AddOrder(order, maxOrderSum);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult UpdateOrder(OrderEntity order)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.UpdateOrder(order);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult CancelOrder(Int16 orderId)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.CancelOrder(orderId);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
