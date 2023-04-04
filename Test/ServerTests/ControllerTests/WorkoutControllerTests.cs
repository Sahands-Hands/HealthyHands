///File Name WorkoutControllerTests.cs

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using HealthyHands.Server.Data;
using HealthyHands.Server.Data.Repository.WorkoutsRepository;
using HealthyHands.Server.Controllers;
using HealthyHands.Shared.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using HealthyHands.Server.Models;

namespace HealthyHands.Tests.ServerTests.ControllerTests
{
    internal class WorkoutControllerTests
    {
        private readonly ApplicationDbContext _context;
        private readonly WorkoutsRepository _repository;
        private readonly WorkoutsController _controller;
        private readonly DbContextOptions<ApplicationDbContext> _options;
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        public WorkoutControllerTests()
        {

        }
    }
}
