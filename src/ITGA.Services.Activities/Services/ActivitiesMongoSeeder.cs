using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ITGA.Common.Mongo;
using ITGA.Services.Activities.Domain.Models;
using ITGA.Services.Activities.Domain.Repositories;
using MongoDB.Driver;

namespace ITGA.Services.Activities.Services
{
    public class ActivitiesMongoSeeder : MongoSeeder
    {
        private readonly ICategoryRepository _categoryRepository;

        public ActivitiesMongoSeeder(IMongoDatabase database, ICategoryRepository categoryRepository) : base(database)
        {
            _categoryRepository = categoryRepository;
        }

        protected override async Task CustomSeedAsync()
        {
            var categories = new List<string>
            {
                "work",
                "sport",
                "hobby"
            };
            await Task.WhenAll(categories.Select(x => _categoryRepository.AddAsync(new Category(x))));
        }
    }
}
