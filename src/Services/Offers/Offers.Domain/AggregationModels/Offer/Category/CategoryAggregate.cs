﻿using Offers.Domain.Base;

namespace Offers.Domain.AggregationModels.Offer.Category;

public class CategoryAggregate: BaseEntity
{
    public string Name { get; protected set; }

    public CategoryAggregate(string name)
    {
        Name = name;
    }
}
