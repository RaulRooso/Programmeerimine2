using System;
using System.Collections.Generic;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly IList<User> _users = new List<User>();
        private readonly IList<BeerSort> _beerSorts = new List<BeerSort>();
        private readonly IList<BeerBatch> _beerBatches = new List<BeerBatch>();

        public SeedData(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public void Generate()
        {
            // If any data exists, skip seeding.
            if (_dbContext.Users.Any() ||
                _dbContext.BeerSorts.Any() ||
                _dbContext.BeerBatches.Any())
            {
                return;
            }

            // 1. Generate parent entities
            GenerateUsers();
            GenerateBeerSorts();
            GenerateBeerBatches();

            // Save so IDs get generated
            _dbContext.SaveChanges();

            // 2. Generate child entities (require IDs)
            GenerateIngredients();
            GenerateBatchLogs();
            GenerateTasteLogs();
            GeneratePhotos();

            _dbContext.SaveChanges();
        }

        // USERS
        private void GenerateUsers()
        {
            for (int i = 0; i < 10; i++)
            {
                _users.Add(new User
                {
                    Username = $"user{i + 1}",
                    Email = $"user{i + 1}@example.com"
                });
            }

            _dbContext.Users.AddRange(_users);
        }

        // BEER SORTS
        private void GenerateBeerSorts()
        {
            for (int i = 0; i < 10; i++)
            {
                _beerSorts.Add(new BeerSort
                {
                    Name = $"BeerSort {i + 1}",
                    Description = $"Description for BeerSort {i + 1}"
                });
            }

            _dbContext.BeerSorts.AddRange(_beerSorts);
        }

        // BEER BATCHES
        private void GenerateBeerBatches()
        {
            var random = new Random();

            foreach (var sort in _beerSorts)
            {
                // each BeerSort gets 1–2 batches
                int batchCount = random.Next(1, 3);

                for (int i = 0; i < batchCount; i++)
                {
                    _beerBatches.Add(new BeerBatch
                    {
                        Date = DateTime.Now.AddDays(-(i * 5)),
                        Description = $"Brewed batch #{i + 1} for {sort.Name}",
                        Conclusion = $"Conclusion for batch #{i + 1}",
                        BeerSort = sort   // navigation property (safe before SaveChanges)
                    });
                }
            }

            // Ensure at least 10 batches
            while (_beerBatches.Count < 10)
            {
                _beerBatches.Add(new BeerBatch
                {
                    Date = DateTime.Now.AddDays(-_beerBatches.Count),
                    Description = "Extra generated batch",
                    Conclusion = "Extra batch conclusion",
                    BeerSort = _beerSorts.First()
                });
            }

            _dbContext.BeerBatches.AddRange(_beerBatches);
        }

        // INGREDIENTS
        private void GenerateIngredients()
        {
            var rnd = new Random();
            var ingredientsList = new List<Ingredient>();

            foreach (var batch in _beerBatches)
            {
                for (int i = 0; i < 3; i++)
                {
                    ingredientsList.Add(new Ingredient
                    {
                        Name = $"Ingredient {i + 1} for batch {batch.Id}",
                        Unit = "kg",
                        UnitPrice = (decimal)(rnd.NextDouble() * 10 + 1),
                        Quantity = (decimal)(rnd.NextDouble() * 5 + 0.5),
                        BeerBatchId = batch.Id  // ID is now valid
                    });
                }
            }

            // Ensure 10 minimum
            if (ingredientsList.Count < 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    ingredientsList.Add(new Ingredient
                    {
                        Name = $"Extra Ingredient {i + 1}",
                        Unit = "kg",
                        UnitPrice = 5,
                        Quantity = 1,
                        BeerBatchId = _beerBatches.First().Id
                    });
                }
            }

            _dbContext.Ingredients.AddRange(ingredientsList);
        }

        // BATCH LOGS
        private void GenerateBatchLogs()
        {
            var rnd = new Random();
            var logs = new List<BatchLog>();

            foreach (var batch in _beerBatches)
            {
                for (int i = 0; i < 2; i++)
                {
                    logs.Add(new BatchLog
                    {
                        Date = batch.Date.AddHours(i * 3),
                        Description = $"Log entry {i + 1} for batch {batch.Id}",
                        UserId = _users[rnd.Next(_users.Count)].Id,
                        BeerBatchId = batch.Id
                    });
                }
            }

            _dbContext.BatchLogs.AddRange(logs);
        }

        // TASTE LOGS
        private void GenerateTasteLogs()
        {
            var rnd = new Random();
            var tasteLogs = new List<TasteLog>();

            foreach (var batch in _beerBatches)
            {
                for (int i = 0; i < 2; i++)
                {
                    tasteLogs.Add(new TasteLog
                    {
                        Date = batch.Date.AddDays(i + 1),
                        Description = $"Taste test {i + 1} for batch {batch.Id}",
                        Rating = rnd.Next(1, 10),
                        UserId = _users[rnd.Next(_users.Count)].Id,
                        BeerBatchId = batch.Id
                    });
                }
            }

            _dbContext.TasteLogs.AddRange(tasteLogs);
        }

        // PHOTOS
        private void GeneratePhotos()
        {
            var photos = new List<Photo>();
            int counter = 1;

            foreach (var batch in _beerBatches)
            {
                photos.Add(new Photo
                {
                    Description = $"Photo of batch {batch.Id}",
                    FilePath = $"images/photo_{counter++}.jpg",
                    BeerBatchId = batch.Id
                });
            }

            _dbContext.Photos.AddRange(photos);
        }
    }
}
