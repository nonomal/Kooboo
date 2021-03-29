﻿using System;
using System.Collections.Generic;
using System.Text;
using static Kooboo.Sites.Commerce.Entities.Promotion;
using static Kooboo.Sites.Commerce.ViewModels.Promotion.PromotionViewModel;

namespace Kooboo.Sites.Commerce.ViewModels.Promotion
{
    public class PromotionMatchViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PromotionType Type { get; set; }
        public int Priority { get; set; }
        public bool Exclusive { get; set; }
        public decimal Discount { get; set; }
        public PromotionRules Rules { get; set; }
        public PromotionTarget Target { get; set; }
        public DateTime StartTime { get; set; }
    }
}
