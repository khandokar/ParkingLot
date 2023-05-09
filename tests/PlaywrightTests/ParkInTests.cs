using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace PlaywrightTests
{
    [TestFixture]
    public class UnitTest1 : PageTest
    {
        [SetUp]
        public async Task SetUp()
        {
            await Page.GotoAsync("http://localhost/CIP");
        }

        [Test]
        public async Task Validate_ParkIn_Success()
        {
            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "In" });

            await inB.ClickAsync();

            //
            await Expect(Page.Locator("#TagNumber-error")).ToHaveTextAsync("Tag Number is required.");

            await Page.Locator("#TagNumber").FillAsync("02201018-02201018");

            await Expect(Page.Locator("#TagNumber")).ToHaveValueAsync("02201018-0", new LocatorAssertionsToHaveValueOptions { Timeout = 10000 });

        }

        [Test]
        public async Task ParkIn_Success()
        {
            string? svailableSpotString = await Page.Locator("#sAvailableSpot").TextContentAsync();

            int svailableSpot = Convert.ToInt32(svailableSpotString?.Split(":")[1].Trim());

            string? sSpotTokenString = await Page.Locator("#sSpotToken").TextContentAsync();

            int stokenSpot = Convert.ToInt32(sSpotTokenString?.Split(":")[1].Trim());

            await Page.Locator("#TagNumber").FillAsync("02201018");

            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "In" });

            await inB.ClickAsync();

            await Expect(Page.Locator(".table").GetByText("02201018")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            await Expect(Page.Locator("#sAvailableSpot")).ToHaveTextAsync(string.Format("Available spots: {0}", svailableSpot - 1));

            await Expect(Page.Locator("#sSpotToken")).ToHaveTextAsync(string.Format("Spots taken: {0}", stokenSpot + 1));
        }

        [Test]
        public async Task Park_With_Existing_TagNumber_Failed()
        {
            await Page.Locator("#TagNumber").FillAsync("02201018");

            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "In" });

            await inB.ClickAsync();

            await Page.Locator("#TagNumber").FillAsync("02201018");

            await inB.ClickAsync();

            await Expect(Page.GetByText("This Car is already Parked.")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

        }

        [TearDown]
        public async Task TearDown()
        {
            await Page.ReloadAsync();

            await Page.Locator("#TagNumber").FillAsync("02201018");

            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "Out" });

            await inB.ClickAsync();

            await inB.ClickAsync();

            await Page.CloseAsync();

        }
    }
}