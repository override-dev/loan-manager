using FastEndpoints;

namespace Server.Products.EndPoints
{
    internal class ProductsGroup : Group
    {
        public ProductsGroup()
        {
            Configure(nameof(Products).ToLower(), _ =>
            {

            });
        }
    }
}
