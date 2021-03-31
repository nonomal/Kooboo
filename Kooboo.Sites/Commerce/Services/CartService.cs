﻿using Dapper;
using Kooboo.Data.Context;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using Kooboo.Sites.Commerce.ViewModels;
using Kooboo.Sites.Commerce.ViewModels.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Sites.Commerce.Services
{
    public class CartService : ServiceBase
    {
        public CartService(RenderContext context) : base(context)
        {
        }

        public void SaveItem(CartItem cartItem)
        {
            using (var con = DbConnection)
            {
                var existEntity = con.QueryFirstOrDefault<CartItem>("select * from CartItem where CustomerId=@CustomerId and SkuId=@SkuId", cartItem);

                if (existEntity != null)
                {
                    existEntity.Selected = cartItem.Selected;
                    existEntity.Quantity = cartItem.Quantity;
                    con.Update(existEntity);
                }
                else
                {
                    cartItem.Id = Guid.NewGuid();
                    con.Insert(cartItem);
                }
            }
        }

        public CartViewModel GetCart(Guid customerId)
        {
            using (var con = DbConnection)
            {
                var list = con.Query(@"
SELECT CI.ProductId,
       CI.SkuId,
       CI.Id,
       CI.Quantity,
       CI.Selected,
       PS.Price,
       P.Title           AS ProductName,
       P.Specifications  AS ProductSpecifications,
       PS.Specifications AS ProductSkuSpecifications,
       PT.Specifications AS ProductTypeSpecifications,
       SUM(S.Quantity)   AS Stock
FROM CartItem CI
         LEFT JOIN ProductSku PS ON CI.SkuId = PS.Id
         LEFT JOIN Product P ON P.Id = PS.ProductId
         LEFT JOIN ProductType PT ON PT.Id = P.TypeId
         LEFT JOIN ProductStock S ON PS.Id = S.SkuId
WHERE CustomerId = @CustomerId
GROUP BY CI.SkuId
", new { CustomerId = customerId });

                var cart = new CartViewModel();
                var items = new List<CartViewModel.CartItemViewModel>();

                foreach (var item in list)
                {
                    var typeSpecifications = JsonHelper.Deserialize<ItemDefineViewModel[]>(item.ProductTypeSpecifications);
                    var skuSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, Guid>[]>(item.ProductSkuSpecifications);
                    var productSpecifications = JsonHelper.Deserialize<KeyValuePair<Guid, string>[]>(item.ProductSpecifications);

                    items.Add(new CartViewModel.CartItemViewModel()
                    {
                        Id = item.Id,
                        Price = (decimal)item.Price,
                        Quantity = item.Quantity,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        SkuId = item.SkuId,
                        Selected = Convert.ToBoolean(item.Selected),
                        Specifications = Helpers.GetSpecifications(typeSpecifications, productSpecifications, skuSpecifications),
                        Stock = Convert.ToInt32(item.Stock)
                    });
                }

                var promotions = new PromotionService(Context).MatchList;
                cart.Items = items.ToArray();
                cart.Discount(promotions);
                return cart;
            }
        }

        public void DeleteItems(Guid[] ids)
        {
            using (var con = DbConnection)
            {
                con.DeleteList<CartItem>(ids);
            }
        }
    }
}
