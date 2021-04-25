using Microsoft.AspNetCore.Mvc;

namespace RentaPhotoServer.Abstractions
{
    public interface IGoods
    {
        ActionResult GetAllProducts();
        ActionResult GetProductByArticle(sbyte article);
    }
}
