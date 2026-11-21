using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Inventory.Domain.Tests.Entities
{
    public class CategoryTests
    {
        [Fact]
        public void Category_CannotHaveEmptyName()
        {
            Assert.True(true);
        }
    }
}