using MUDhub.Core.Configurations;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class MailServiceTests
    {
        [Fact]
        public async Task CheckEmail()
        {
            var emailservice = new EmailService(new MailConfiguration());
            var isSended = await emailservice.SendAsync("moriskotsch@gmail.com", "sdfsdf");
            Assert.True(isSended);
        }
    }
}
