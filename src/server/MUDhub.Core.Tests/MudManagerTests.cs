using Microsoft.EntityFrameworkCore;
using Moq;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class MudManagerTests : IDisposable
    {
        public readonly MudDbContext _context;
        public readonly IUserManager _userManager;
        public readonly IMudManager _mudManager;
        private readonly RegisterResult _user;

        public MudManagerTests()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_MudManagment", opt => { })
                .Options;

            _context = new MudDbContext(options);
            _mudManager = new MudManager(_context);
            _userManager = new UserManager(_context, Mock.Of<IEmailService>());
            _user = _userManager.RegisterUserAsync(new RegistrationUserArgs
            {
                Password = "1234",
                Email = "mail@mail.de",
                Lastname = "last",
                Firstname = "first"
            }).Result;
            _userManager.AddRoleToUserAsync(_user.User!.Id, Models.Users.Roles.Master);

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task AddMudSuccessfully()
        {
            var mudName = "MyMud1";
            var mud = await _mudManager.CreateMudAsync(mudName, new MudCreationArgs() { OwnerId = _user.User!.Id });
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.NotNull(mud);
            Assert.NotNull(mudgame);
            Assert.Equal(mudgame.Id, mud.Id);
            Assert.Equal(mudName, mudgame.Name);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithDescription()
        {

            var mudDescrption = "TestDescription";
            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { Description = mudDescrption, OwnerId = _user.User!.Id });
            Assert.NotNull(mud);
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.Equal(mudDescrption, mudgame.Description);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithIamgeKey()
        {
            var imageKey = "TestImageKey";

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { ImageKey = imageKey, OwnerId = _user.User!.Id });
            Assert.NotNull(mud);
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.Equal(imageKey, mudgame.ImageKey);
        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithTrue()
        {

            var autoRestart = true;

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart, OwnerId = _user.User!.Id });
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.NotNull(mud);
            Assert.Equal(autoRestart, mudgame.AutoRestart);

        }

        [Fact]
        public async Task AddMudSuccessfullyWithAutoRestartWithFalse()
        {

            var autoRestart = false;

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { AutoRestart = autoRestart, OwnerId = _user.User!.Id });
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.NotNull(mud);
            Assert.Equal(autoRestart, mudgame.AutoRestart);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithTrue()
        {

            var isPublic = true;

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic, OwnerId = _user.User!.Id });
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.NotNull(mud);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyIsPublicWithFalse()
        {

            var isPublic = false;

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = isPublic, OwnerId = _user.User!.Id });
            Assert.NotNull(mud);
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.Equal(isPublic, mudgame.IsPublic);
        }

        [Fact]
        public async Task AddMudSuccessfullyUpdateName()
        {
            var newmudName = "MyMud9";

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var res2 = await _mudManager.UpdateMudAsync(mud!.Id, new MudUpdateArgs { Name = newmudName, OwnerId = _user.User!.Id });
            var mudgame = await _context.MudGames.FindAsync(mud!.Id);
            Assert.NotNull(res2);
            Assert.Equal(newmudName, mudgame.Name);
        }
        [Fact]
        public async Task AddMudSuccessfullyUpdateNameShouldFail()
        {
            var newmudName = "MyMud9";

            var res2 = await _mudManager.UpdateMudAsync(Guid.NewGuid().ToString(), new MudUpdateArgs { Name = newmudName, OwnerId = _user.User!.Id });
            Assert.Null(res2);
        }

        [Fact]
        public async Task RemoveMudAfterCreating()
        {

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var res2 = await _mudManager.RemoveMudAsync(mud!.Id);
            var mudgame = await _context.MudGames.FindAsync(mud.Id);
            Assert.True(res2);
            Assert.Null(mudgame);
        }

        [Fact]
        public async Task RemoveMudAfterShouldFailNotExist()
        {
            var res2 = await _mudManager.RemoveMudAsync(Guid.NewGuid().ToString());
            Assert.False(res2);
        }

        [Fact]
        public async Task RequestUserToJoin()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var result = await _mudManager.RequestUserForJoinAsync(userid, mud!.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Requested, join.State);
        }

        [Fact]
        public async Task RequestUserToJoinDoubleShouldFail()
        {
            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var userId = Guid.NewGuid().ToString();

            var result = await _mudManager.RequestUserForJoinAsync(userId, mud!.Id);
            Assert.True(result);
            result = await _mudManager.RequestUserForJoinAsync(userId, mud.Id);
            Assert.False(result);
        }

        [Fact]
        public async Task RequestUserToJoinInPublicMudShouldFail()
        {

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { IsPublic = true, OwnerId = _user.User!.Id });
            var userId = Guid.NewGuid().ToString();

            var result = await _mudManager.RequestUserForJoinAsync(userId, mud!.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userId);
            Assert.False(result);
            Assert.Null(join);
        }

        [Fact]
        public async Task RequestUserToJoinWithUnknowMudShouldFail()
        {
            var userId = Guid.NewGuid().ToString();
            var result = await _mudManager.RequestUserForJoinAsync(userId, Guid.NewGuid().ToString());
            Assert.False(result);
        }

        [Fact]
        public async Task ApproveUserToJoin()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var result = await _mudManager.RequestUserForJoinAsync(userid, mud!.Id);
            result = await _mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Accepted, join.State);

        }

        [Fact]
        public async Task ApproveUserToJoinAlreadyApprovedShouldFail()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            _ = await _mudManager.RequestUserForJoinAsync(userid, mud!.Id);
            _ = await _mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var result = await _mudManager.ApproveUserToJoinAsync(userid, mud.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Equal(MudJoinState.Accepted, join.State);
        }


        [Fact]
        public async Task ApproveUserToJoinSchouldFail()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var result = await _mudManager.ApproveUserToJoinAsync(userid, mud!.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Null(join);
        }

        [Fact]
        public async Task ApproveUserToJoinSchouldFailMudNotFound()
        {
            var userid = Guid.NewGuid().ToString();

            var MudId = Guid.NewGuid().ToString();
            var result = await _mudManager.ApproveUserToJoinAsync(userid, MudId);
            var join = await _context.MudJoinRequests.FindAsync(MudId, userid);
            Assert.False(result);
            Assert.Null(join);
        }


        [Fact]
        public async Task RejectUserToJoinAfterRequest()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            _ = await _mudManager.RequestUserForJoinAsync(userid, mud!.Id);
            var result = await _mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
        }

        [Fact]
        public async Task RejectUserToJoinWithoutRequest()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            var result = await _mudManager.RejectUserToJoinAsync(userid, mud!.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.True(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
        }

        [Fact]
        public async Task RejectUserToJoinDoubleShouldFail()
        {
            var userid = Guid.NewGuid().ToString();

            var mud = await _mudManager.CreateMudAsync("", new MudCreationArgs() { OwnerId = _user.User!.Id });
            _ = await _mudManager.RejectUserToJoinAsync(userid, mud!.Id);
            var result = await _mudManager.RejectUserToJoinAsync(userid, mud.Id);
            var join = await _context.MudJoinRequests.FindAsync(mud.Id, userid);
            Assert.False(result);
            Assert.Equal(MudJoinState.Rejected, join.State);
        }


    }
}
