using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class MudManagerTests
    {
        [Fact]
        public async Task AddMudSuccessfully()
        {
            var mudName = "MyMud1";
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync(mudName, new MudCreationArgs());
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.NotNull(mudgame);
            Assert.Equal(mudgame.Id, MudId);
            Assert.Equal(mudName, mudgame.Name);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithDescription()
        {

            var mudDescrption = "TestDescription";
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { Description = mudDescrption });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(mudDescrption, mudgame.Description);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithIamgeKey()
        {
            var imageKey = "TestImageKey";
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { ImageKey = imageKey });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(imageKey, mudgame.ImageKey);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithTrue()
        {

            var autoRestart = true;
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(autoRestart, mudgame.AutoRestart);

        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithFalse()
        {

            var autoRestart = false;
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(autoRestart, mudgame.AutoRestart);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithTrue()
        {

            var isPublic = true;
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithFalse()
        {

            var isPublic = false;
            var (mudManager, context) = CreateMudManager();
            var (Success, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(Success);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyUpdateName()
        {
            var newmudName = "MyMud9";
            var (mudManager, context) = CreateMudManager();
            var (_, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var res2 = await mudManager.UpdateMudAsync(MudId, new MudUpdateArgs { Name = newmudName });
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(res2);
            Assert.Equal(newmudName, mudgame.Name);
        }
        [Fact]
        public async Task AddMudSuccessfullyUpdateNameShouldFail()
        {
            var newmudName = "MyMud9";
            var (mudManager, context) = CreateMudManager();
            var res2 = await mudManager.UpdateMudAsync(Guid.NewGuid().ToString(), new MudUpdateArgs { Name = newmudName });
            Assert.False(res2);
        }

        [Fact]
        public async Task RemoveMudAfterCreating()
        {
            var (mudManager, context) = CreateMudManager();
            var (_, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var res2 = await mudManager.RemoveMudAsync(MudId);
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(res2);
            Assert.Null(mudgame);
        }

        [Fact]
        public async Task RemoveMudAfterShouldFailNotExist()
        {
            var (mudManager, context) = CreateMudManager();
            var res2 = await mudManager.RemoveMudAsync(Guid.NewGuid().ToString());
            Assert.False(res2);
        }

        [Fact]
        public async Task RequestUserToJoin()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var (_, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var result = await mudManager.RequestUserForJoinAsync(userid, MudId);
            var mudgame = await context.MudGames.FindAsync(MudId);
            Assert.True(result);
            Assert.Contains(mudgame.JoinRequests, mjr =>
                    mjr.MudId == MudId &&
                    mjr.UserId == userid &&
                    mjr.State == MudJoinState.Requested);
        }

        [Fact]
        public async Task RequestUserToJoinDoubleShouldFail()
        {
            var (mudManager, context) = CreateMudManager();
            var (_, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var userId = Guid.NewGuid().ToString();

            var result = await mudManager.RequestUserForJoinAsync(userId, MudId);
            Assert.True(result);
            result = await mudManager.RequestUserForJoinAsync(userId, MudId);
            Assert.False(result);
        }

        [Fact]
        public async Task RequestUserToJoinInPublicMudShouldFail()
        {
            var (mudManager, context) = CreateMudManager();
            var (_, MudId) = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = true });
            var userId = Guid.NewGuid().ToString();

            var result = await mudManager.RequestUserForJoinAsync(userId, MudId);
            var join = await context.MudJoinRequests.FindAsync(MudId, userId);
            Assert.False(result);
            Assert.Null(join);
        }



        private static (IMudManager, MudDbContext) CreateMudManager()
        {
            var context = CreateInMemoryDbContext();
            var mudmanager = new MudManager(context);
            return (mudmanager, context);
        }

        private static MudDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase")
                .Options;

            return new MudDbContext(options);
        }
    }
}
