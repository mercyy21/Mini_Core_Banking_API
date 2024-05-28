using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace API.Test.Services
{
    public static class MockDBContext
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList)where T : class
        {
            var queryable = sourceList.AsQueryable().BuildMockDbSet();
            return queryable.Object;
        }
    }
}
