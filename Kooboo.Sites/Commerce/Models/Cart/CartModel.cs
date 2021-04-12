﻿using Kooboo.Sites.Commerce.Services;
using Kooboo.Sites.Commerce.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Lib.Helper;

namespace Kooboo.Sites.Commerce.Models.Cart
{
    public class CartModel
    {
        readonly List<PromotionMatchModel> _promotions = new List<PromotionMatchModel>();

        public decimal Amount => Items.Where(w => w.Selected).Sum(s => s.Amount);
        public decimal DiscountAmount
        {
            get
            {
                var result = Items.Where(w => w.Selected).Sum(s => s.DiscountAmount);

                foreach (var promotion in _promotions)
                {
                    switch (promotion.Type)
                    {
                        case Entities.Promotion.PromotionType.MoneyOff:
                            result -= promotion.Discount;
                            break;
                        case Entities.Promotion.PromotionType.PercentOff:
                            result = result * promotion.Discount / 100;
                            break;
                        default:
                            break;
                    }

                    if (result < 0) result = 0;
                }

                return result;
            }
        }
        public KeyValuePair<Guid, string>[] Promotions => _promotions.Select(s => new KeyValuePair<Guid, string>(s.Id, s.Name)).ToArray();
        public CartItemModel[] Items { get; set; }
        public int Quantity { get; set; }

        public void Discount(IEnumerable<PromotionMatchModel> promotions)
        {

            foreach (var promotion in promotions)
            {
                var order = new MatchRule.TargetModels.Order
                {
                    Amount = DiscountAmount,
                    Quantity = Items.Where(w => w.Selected).Sum(s => s.Quantity)
                };

                var matchOrder = order.Match(promotion.Rules.Order);
                if (!matchOrder) continue;

                foreach (var item in Items)
                {
                    var orderItem = new MatchRule.TargetModels.OrderItem
                    {
                        Amount = item.DiscountAmount,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        SkuId = item.SkuId
                    };

                    var matchOrderItem = orderItem.Match(promotion.Rules.OrderItem);

                    if (matchOrderItem)
                    {
                        var shouldBreack = false;

                        switch (promotion.Target)
                        {
                            case Entities.Promotion.PromotionTarget.Order:
                                _promotions.Add(promotion);
                                shouldBreack = true;
                                break;
                            case Entities.Promotion.PromotionTarget.OrderItem:
                                item.AddPromotion(promotion);
                                break;
                            default:
                                break;
                        }

                        if (shouldBreack) break;
                        if (promotion.Exclusive) return;
                    }
                }
            }

        }

        public class CartItemModel
        {
            readonly List<PromotionMatchModel> _promotions = new List<PromotionMatchModel>();

            public void AddPromotion(PromotionMatchModel promotion)
            {
                _promotions.Add(promotion);
            }

            public Guid Id { get; set; }
            public Guid ProductId { get; set; }
            public Guid SkuId { get; set; }
            public bool Selected { get; set; }
            public decimal Price { get; set; }
            public string ProductName { get; set; }
            public KeyValuePair<string, string>[] Specifications { get; set; }
            public int Quantity { get; set; }
            public decimal Amount => Price * Quantity;
            public decimal DiscountAmount
            {
                get
                {
                    var result = Amount;

                    foreach (var promotion in _promotions)
                    {
                        switch (promotion.Type)
                        {
                            case Entities.Promotion.PromotionType.MoneyOff:
                                result -= promotion.Discount;
                                break;
                            case Entities.Promotion.PromotionType.PercentOff:
                                result = result * promotion.Discount / 100;
                                break;
                            default:
                                break;
                        }

                        if (result < 0) result = 0;
                    }

                    return result;
                }
            }
            public KeyValuePair<Guid, string>[] Promotions => _promotions.Select(s => new KeyValuePair<Guid, string>(s.Id, s.Name)).ToArray();
            public int Stock { get; set; }

            public Entities.OrderItem ToOrderItem(Guid orderId, CartModel cart)
            {
                return new Entities.OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = orderId,
                    Price = Helpers.GetCartItemPrice(cart, DiscountAmount, Quantity),
                    Quantity = Quantity,
                    ProductId = ProductId,
                    ProductName = ProductName,
                    Promotions = JsonHelper.Serialize(Promotions),
                    SkuId = SkuId,
                    Specifications = JsonHelper.Serialize(Specifications),
                    State = Entities.OrderItem.OrderItemState.WaitingPay,
                    Tax = 0
                };
            }
        }
    }
}