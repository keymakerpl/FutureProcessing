using Xunit;
using System.Linq;

namespace WebDataAccess.Tests
{
    public class WebContextTests
    {
        [Fact()]
        public void GetCandidatesShouldReturnNonEmptyIEnumerable()
        {
            var context = new WebContext();
            var candidates = context.GetCandidates();

            Assert.True(candidates.Any());
        }

        [Fact()]
        public void GetDisallowedPersonsTest()
        {
            var context = new WebContext();
            var candidates = context.GetDisallowedPersons();

            Assert.True(candidates.Any());
        }
    }
}