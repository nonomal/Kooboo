﻿using Kooboo.Data.Attributes;
using Kooboo.Lib.Helper;
using Kooboo.Sites.Commerce.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kooboo.Sites.Commerce.Models.Type
{
    public class ProductTypeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ItemDefineModel[] Attributes { get; set; }
        public ItemDefineModel[] Specifications { get; set; }

        [KIgnore]
        public ProductType ToEntity()
        {
            return new ProductType
            {
                Id = Id,
                Name = Name,
                Attributes = JsonHelper.Serialize(Attributes),
                Specifications = JsonHelper.Serialize(Specifications)
            };
        }
    }
}