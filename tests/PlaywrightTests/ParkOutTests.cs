using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaywrightTests
{

    [TestFixture]
    public class ParkOutTests : PageTest
    {
        [SetUp]
        public async Task SetUp()
        {
            await Page.GotoAsync("http://localhost/CIP");
        }

        [Test]
        public async Task Validate_ParkOut_Success()
        {
            var outB = Page.GetByRole(AriaRole.Button, new() { Name = "Out" });

            await outB.ClickAsync();

            await Expect(Page.Locator("#TagNumber-error")).ToHaveTextAsync("Tag Number is required.");

            await Page.Locator("#TagNumber").FillAsync("02201018");

            await outB.ClickAsync();
            await outB.ClickAsync();

            await Expect(Page.GetByText("This Car is not in the Park.")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

        }

        [Test]
        public async Task Validate_ParkOut_TotalDue_Less_Than_Minus_One_Success()
        {
            await Page.Locator("#TagNumber").FillAsync("02201018");

            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "In" });

            await inB.ClickAsync();

            await Page.Locator("#TagNumber").FillAsync("02201018");

            var outB = Page.GetByRole(AriaRole.Button, new() { Name = "Out" });

            await outB.ClickAsync();

            await Page.Locator("#Total").FillAsync("-1");

            await outB.ClickAsync();

            await Expect(Page.Locator("#frmInOut").GetByText("This field is must be positive.")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

        }

        [Test]
        public async Task ParkOut_Success()
        {
            string? svailableSpotString = await Page.Locator("#sAvailableSpot").TextContentAsync();

            int svailableSpot = Convert.ToInt32(svailableSpotString?.Split(":")[1].Trim());

            string? sSpotTokenString = await Page.Locator("#sSpotToken").TextContentAsync();

            int stokenSpot = Convert.ToInt32(sSpotTokenString?.Split(":")[1].Trim());

            await Page.Locator("#TagNumber").FillAsync("02201018");

            var inB = Page.GetByRole(AriaRole.Button, new() { Name = "In" });

            await inB.ClickAsync();

            await Expect(Page.Locator(".table").GetByText("02201018")).ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            await Page.Locator("#TagNumber").FillAsync("02201018");

            var outB = Page.GetByRole(AriaRole.Button, new() { Name = "Out" });

            await outB.ClickAsync();

            await outB.ClickAsync();

            await Expect(Page.Locator(".table").GetByText("02201018")).Not.ToBeVisibleAsync(new LocatorAssertionsToBeVisibleOptions { Timeout = 10000 });

            await Expect(Page.Locator("#sAvailableSpot")).ToHaveTextAsync(string.Format("Available spots: {0}", svailableSpot));

            await Expect(Page.Locator("#sSpotToken")).ToHaveTextAsync(string.Format("Spots taken: {0}", stokenSpot));
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
