using Microsoft.EntityFrameworkCore;
using Moq;

namespace WebApiTests.Extensions
{
    public static class DbSetExtensions
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(this List<T> sourceList) where T : class
        {
            var queryableList = sourceList.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            // Configurando o IQueryable
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableList.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableList.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableList.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableList.GetEnumerator());

            // Configurando métodos adicionais
            dbSetMock.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(item => sourceList.Add(item));
            dbSetMock.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(item => sourceList.Remove(item));
            dbSetMock.Setup(m => m.AddRange(It.IsAny<IEnumerable<T>>()))
                     .Callback<IEnumerable<T>>(items => sourceList.AddRange(items));

            dbSetMock.Setup(m => m.RemoveRange(It.IsAny<IEnumerable<T>>()))
                     .Callback<IEnumerable<T>>(items =>
                     {
                         foreach (var item in items)
                         {
                             sourceList.Remove(item);
                         }
                     });

            return dbSetMock;
        }
    }
}
