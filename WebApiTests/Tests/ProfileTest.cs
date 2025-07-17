using Xunit;
using BizModels = Business.Models;


namespace WebApiTests.Tests
{
    public class ProfileTests
    {
        [Fact]
        public void Create_ShouldInitializeProfileWithCorrectValues()
        {
            var name = "Admin";
            var permissions = new List<string> { "Read", "Write", "Delete" };
            var profile = BizModels.Profile.Create(name, permissions);
            Assert.Equal(name, profile.Name);
            Assert.Equal(permissions, profile.Permissions);
            Assert.True(profile.Active);
        }

        [Fact]
        public void ToCommons_ShouldConvertToCommonsProfileWithCorrectValues()
        {
            var profile = new BizModels.Profile
            {
                Id = Guid.NewGuid(),
                Name = "Manager",
                Active = true,
                Permissions = new List<string> { "Read", "Approve" }
            };
            var commonsProfile = profile.ToCommons();
            Assert.Equal(profile.Id, commonsProfile.Id);
            Assert.Equal(profile.Name, commonsProfile.Name);
            Assert.Equal(profile.Permissions, commonsProfile.Permissions);
        }
    }
}


