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
            var mud = await mudManager.CreateMudAsync(mudName, new MudCreationArgs());
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.NotNull(mud);
            Assert.NotNull(mudgame);
            Assert.Equal(mudgame.Id, mud.Id);
            Assert.Equal(mudName, mudgame.Name);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithDescription()
        {

            var mudDescrption = "TestDescription";
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { Description = mudDescrption });
            Assert.NotNull(mud);
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.Equal(mudDescrption, mudgame.Description);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithIamgeKey()
        {
            var imageKey = "TestImageKey";
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { ImageKey = imageKey });
            Assert.NotNull(mud);
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.Equal(imageKey, mudgame.ImageKey);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithTrue()
        {

            var autoRestart = true;
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart });
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.NotNull(mud);
            Assert.Equal(autoRestart, mudgame.AutoRestart);

        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithFalse()
        {

            var autoRestart = false;
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart });
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.NotNull(mud);
            Assert.Equal(autoRestart, mudgame.AutoRestart);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithTrue()
        {

            var isPublic = true;
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic });
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.NotNull(mud);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithFalse()
        {

            var isPublic = false;
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic });
            Assert.NotNull(mud);
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyUpdateName()
        {
            var newmudName = "MyMud9";
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var res2 = await mudManager.UpdateMudAsync(mud.Id, new MudUpdateArgs { Name = newmudName });
            var mudgame = await context.MudGames.FindAsync(mud.Id);
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
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var res2 = await mudManager.RemoveMudAsync(mud.Id);
            var mudgame = await context.MudGames.FindAsync(mud.Id);
            Assert.True(res2);
            Assert.Null(mudgame);
        }

        [Fact]
        public async Task RemoveMudAfterShouldFailNotExist()
        {
            var (mudManager, _) = CreateMudManager();
            var res2 = await mudManager.RemoveMudAsync(Guid.NewGuid().ToString());
            Assert.False(res2);
        }

        [Fact]
        public async Task RequestUserToJoin()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var result = await mudManager.RequestUserForJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Requested, join.State);
        }

        [Fact]
        public async Task RequestUserToJoinDoubleShouldFail()
        {
            var (mudManager, _) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var userId = Guid.NewGuid().ToString();

            var result = await mudManager.RequestUserForJoinAsync(userId, mud.Id);
            Assert.True(result);
            result = await mudManager.RequestUserForJoinAsync(userId, mud.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task RequestUserToJoinInPublicMudShouldFail()
        {
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = true });
            var userId = Guid.NewGuid().ToString();

            var result = await mudManager.RequestUserForJoinAsync(userId, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userId);
            Assert.False(result);
            Assert.Null(join);
        }

        [Fact]
        public async Task RequestUserToJoinWithUnknowMudShouldFail()
        {
            var (mudManager, _) = CreateMudManager();
            var userId = Guid.NewGuid().ToString();
            var result = await mudManager.RequestUserForJoinAsync(userId, Guid.NewGuid().ToString());
            Assert.False(result);
        }

        [Fact]
        public async Task ApproveUserToJoin()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var result = await mudManager.RequestUserForJoinAsync(userid, mud.Id);
            result = await mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Accepted, join.State);

        }

        [Fact]
        public async Task ApproveUserToJoinAlreadyApprovedShouldFail()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            _ = await mudManager.RequestUserForJoinAsync(userid, mud.Id);
            _ = await mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var result = await mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Equal(MudJoinState.Accepted, join.State);
        }


        [Fact]
        public async Task ApproveUserToJoinSchouldFail()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var result = await mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Null(join);
        }

        [Fact]
        public async Task ApproveUserToJoinSchouldFailMudNotFound()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var MudId = Guid.NewGuid().ToString();
            var result = await mudManager.ApproveUserToJoinAsync(userid, MudId);
            var join = await context.MudJoinRequests.FindAsync(MudId, userid);
            Assert.False(result);
            Assert.Null(join);
        }


        [Fact]
        public async Task RejectUserToJoinAfterRequest()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            _ = await mudManager.RequestUserForJoinAsync(userid, mud.Id);
            var result = await mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
        }

        [Fact]
        public async Task RejectUserToJoinWithoutRequest()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            var result = await mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
        }

        [Fact]
        public async Task RejectUserToJoinDoubleShouldFail()
        {
            var userid = Guid.NewGuid().ToString();
            var (mudManager, context) = CreateMudManager();
            var mud = await mudManager.CreateMudAsync("", new MudCreationArgs());
            _ = await mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var result = await mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var join = await context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
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
                .UseInMemoryDatabase("Testdatabase_MudManagment", opt => { })
                .Options;

            return new MudDbContext(options, useInUnitTests: true);
        }
    }
}
