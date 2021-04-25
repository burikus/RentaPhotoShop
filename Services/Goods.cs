using Microsoft.AspNetCore.Mvc;
using RentaPhotoDbConnector.Abstractions;
using RentaPhotoServer.Abstractions;
using System.Threading;

namespace RentaPhotoServer.Services
{
    public class Goods : IGoods
    {
        IDbGoods _db;
        private Semaphore _semaphore = new Semaphore(1, 1);

        public Goods(IDbGoods db)
        {
            _db = db;
        }

        public ActionResult GetAllProducts()
        {
            _semaphore.WaitOne();

            try
            {
                return _db.GetListOfProducts();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public ActionResult GetProductByArticle(sbyte article)
        {
            _semaphore.WaitOne();

            try
            {
                return _db.GetProductByArticle(article);
            }
            finally
            {
                _semaphore.Release();
            }

        }
    }
}
