using Microsoft.EntityFrameworkCore;
using MUDhub.Core.Abstracts.Models.Characters;
using MUDhub.Core.Models;
using MUDhub.Core.Models.Characters;
using MUDhub.Core.Models.Muds;
using MUDhub.Core.Models.Rooms;
using MUDhub.Core.Models.Users;
using MUDhub.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class CharacterManagerTests : IDisposable
    {

        private CharacterManager _characterManager;
        private MudDbContext _context;

        private User _user1, _user2;
        private MudGame _mudGame1, _mudGame2, _mudGame3;
        private CharacterRace _race1, _race2, _race3;
        private CharacterClass _class1, _class2, _class3;
        private Character _character1, _character2, _character3;

        private CharacterArgs _createCharacterArgs, _createCharacterArgsNullClass;
        private CharacterArgs _createCharacterArgsNullRace, _createCharacterArgsWrongClass;
        private CharacterArgs _createCharacterArgsWrongRace;

        private CharacterClassArgs _createCharacterClassArgs, _updateCharacterClassArgs;
        private CharacterRaceArgs _createCharacterRaceArgs, _updateCharacterRaceArgs;

        public CharacterManagerTests()
        {
            InitializeTestDb();
        }
        public void Dispose()
        {
            _context.Users.Remove(_user1);
            _context.Users.Remove(_user2);
            _context.SaveChanges();
            _context.Dispose();
        }

        //*********************************************************//

        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.CreateCharacterAsync("99", "1", _createCharacterArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_ClassNull()
        {
            var result = await _characterManager.CreateCharacterAsync("1", "1", _createCharacterArgsNullClass);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_ClassInWrongMud()
        {
            var result = await _characterManager.CreateCharacterAsync("1", "1", _createCharacterArgsWrongClass);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_RaceNull()
        {
            var result = await _characterManager.CreateCharacterAsync("1", "1", _createCharacterArgsNullRace);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_RaceInWrongMud()
        {
            var result = await _characterManager.CreateCharacterAsync("1", "1", _createCharacterArgsWrongRace);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnFalse_UserIsNotOwner()
        {
            var result = await _characterManager.CreateCharacterAsync("2", "1", _createCharacterArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateCharacterAsync_ReturnTrue()
        {
            var result = await _characterManager.CreateCharacterAsync("1", "1", _createCharacterArgs);
            Assert.True(result.Success);

            var newCharacter = _context.Characters.FirstOrDefault(
                a => a.Name.Equals("New Character 1"));
            Assert.True(newCharacter.Race.Id == "1");
        }

        //*********************************************************//

        [Fact]
        public async Task CreateClassAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.CreateClassAsync("99", "1", _createCharacterClassArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateClassAsync_ReturnFalse_MudGameNull()
        {
            var result = await _characterManager.CreateClassAsync("1", "99", _createCharacterClassArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateClassAsync_ReturnTrue()
        {
            var result = await _characterManager.CreateClassAsync("1", "1", _createCharacterClassArgs);
            Assert.True(result.Success);

            var newClass = _context.Classes.FirstOrDefault(
                a => a.Name.Equals("New Class"));
            Assert.True(newClass.Description == "Beschreibung New Class");
        }

        //*********************************************************//

        [Fact]
        public async Task CreateRaceAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.CreateRaceAsync("99", "1", _createCharacterRaceArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRaceAsync_ReturnFalse_MudGameNull()
        {
            var result = await _characterManager.CreateRaceAsync("1", "99", _createCharacterRaceArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task CreateRaceAsync_ReturnTrue()
        {
            var result = await _characterManager.CreateRaceAsync("1", "1", _createCharacterRaceArgs);
            Assert.True(result.Success);

            var newRace = _context.Races.FirstOrDefault(
                a => a.Name.Equals("New Race"));
            Assert.True(newRace.Description == "Beschreibung New Race");
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateClassAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.UpdateClassAsync("99", "1", _updateCharacterClassArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateClassAsync_ReturnFalse_MudGameNull()
        {
            var result = await _characterManager.UpdateClassAsync("1", "99", _updateCharacterClassArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateClassAsync_ReturnTrue()
        {
            var result = await _characterManager.UpdateClassAsync("1", "1", _updateCharacterClassArgs);
            Assert.True(result.Success);

            var newRace = _context.Classes.FirstOrDefault(
                a => a.Name.Equals("Updated New Class"));
            Assert.True(newRace.Description == "Updated Beschreibung New Class");
        }

        //*********************************************************//

        [Fact]
        public async Task UpdateRaceAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.UpdateRaceAsync("99", "1", _updateCharacterRaceArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateRaceAsync_ReturnFalse_MudGameNull()
        {
            var result = await _characterManager.UpdateRaceAsync("1", "99", _updateCharacterRaceArgs);
            Assert.False(result.Success);
        }
        [Fact]
        public async Task UpdateRaceAsync_ReturnTrue()
        {
            var result = await _characterManager.UpdateRaceAsync("1", "1", _updateCharacterRaceArgs);
            Assert.True(result.Success);

            var newRace = _context.Races.FirstOrDefault(
                a => a.Name.Equals("Updated New Race"));
            Assert.True(newRace.Description == "Updated Beschreibung New Race");
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveCharacterAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.RemoveCharacterAsync("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveCharacterAsync_ReturnFalse_CharacterNull()
        {
            var result = await _characterManager.RemoveCharacterAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveCharacterAsync_ReturnTrue()
        {
            var result = await _characterManager.RemoveCharacterAsync("1", "1");
            Assert.True(result.Success);

            var newRace = _context.Characters.FirstOrDefault(
                a => a.Id == "1");
            Assert.Null(newRace);
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveClassAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.RemoveClassAsync("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveClassAsync_ReturnFalse_CharacterNull()
        {
            var result = await _characterManager.RemoveClassAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveClassAsync_ReturnTrue()
        {
            var result = await _characterManager.RemoveClassAsync("1", "1");
            Assert.True(result.Success);

            var newRace = _context.Classes.FirstOrDefault(
                a => a.Id == "1");
            Assert.Null(newRace);
        }

        //*********************************************************//

        [Fact]
        public async Task RemoveRaceAsync_ReturnFalse_UserNull()
        {
            var result = await _characterManager.RemoveRaceAsync("99", "1");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRaceAsync_ReturnFalse_CharacterNull()
        {
            var result = await _characterManager.RemoveRaceAsync("1", "99");
            Assert.False(result.Success);
        }
        [Fact]
        public async Task RemoveRaceAsync_ReturnTrue()
        {
            var result = await _characterManager.RemoveRaceAsync("1", "1");
            Assert.True(result.Success);

            var newRace = _context.Races.FirstOrDefault(
                a => a.Id == "1");
            Assert.Null(newRace);
        }

        //*********************************************************//

        private void InitializeTestDb()
        {
            var options = new DbContextOptionsBuilder<MudDbContext>()
                .UseInMemoryDatabase("Testdatabase_CharacterManager")
                .Options;
            _context = new MudDbContext(options, useNotInUnitests: false);
            _characterManager = new CharacterManager(_context);

            _user1 = new User("1")
            {
                Name = "User 1",
                Lastname = "Mustermann",
                Role = Roles.Admin,
                Email = "User1@mustermann.de"
            };

            _user2 = new User("2")
            {
                Name = "User 2",
                Lastname = "Musterfrau",
                Role = Roles.Admin,
                Email = "User2@musterfrau.de"
            };

            _mudGame1 = new MudGame("1")
            {
                Name = "MudGame 1",
                Description = "Beschreibung MudGame 1",
                IsPublic = true,
                Owner = _user1
            };

            _mudGame2 = new MudGame("2")
            {
                Name = "MudGame 2",
                Description = "Beschreibung MudGame 2",
                IsPublic = true,
                Owner = _user1
            };

            _mudGame3 = new MudGame("3")
            {
                Name = "MudGame 3",
                Description = "Beschreibung MudGame 3",
                IsPublic = true,
                Owner = _user1
            };
            _mudGame1.Areas.Add(new Area()
            {
                Rooms = new Collection<Room>
                {
                    new Room()
                    {
                        Game = _mudGame1,
                        Name = "Roomname",
                        IsDefaultRoom = true,
                        X = 1,
                        Y = 1,
                        Description = "test descrption"
                    }
                }
            });
            _mudGame3.Areas.Add(new Area()
            {
                Rooms = new Collection<Room>
                {
                    new Room()
                    {
                        Game = _mudGame1,
                        Name = "Roomname",
                        IsDefaultRoom = true,
                        X = 1,
                        Y = 1,
                        Description = "test descrption"
                    }
                }
            });
            _mudGame2.Areas.Add(new Area()
            {
                Rooms = new Collection<Room>
                {
                    new Room()
                    {
                        Game = _mudGame1,
                        Name = "Roomname",
                        IsDefaultRoom = true,
                        X = 1,
                        Y = 1,
                        Description = "test descrption"
                    }
                }
            });

            _user1.MudGames.Add(_mudGame1);
            _user1.MudGames.Add(_mudGame2);
            _user2.MudGames.Add(_mudGame3);

            _class1 = new CharacterClass("1")
            {
                Name = "Class 1",
                Description = "Beschreibung Class 1",
                Game = _mudGame1
            };
            _class2 = new CharacterClass("2")
            {
                Name = "Class 2",
                Description = "Beschreibung Class 2",
                Game = _mudGame2
            }; 
            _class3 = new CharacterClass("3")
            {
                Name = "Class 3",
                Description = "Beschreibung Class 3",
                Game = _mudGame3
            };

            _race1 = new CharacterRace("1")
            {
                Name = "Race 1",
                Description = "Beschreibung Race 1",
                Game = _mudGame1
            };
            _race2 = new CharacterRace("2")
            {
                Name = "Race 2",
                Description = "Beschreibung Race 2",
                Game = _mudGame2
            };
            _race3 = new CharacterRace("3")
            {
                Name = "Race 3",
                Description = "Beschreibung Race 3",
                Game = _mudGame3
            };

            _character1 = new Character("1")
            {
                Name = "Character 1",
                Owner = _user1,
                Class = _class1,
                Race = _race1
            }; 
            _character2 = new Character("2")
            {
                Name = "Character 2",
                Owner = _user1,
                Class = _class2,
                Race = _race2
            }; 
            _character3 = new Character("3")
            {
                Name = "Character 3",
                Owner = _user2,
                Class = _class3,
                Race = _race3
            };

            _mudGame1.Characters.Add(_character1);
            _mudGame2.Characters.Add(_character2);
            _mudGame3.Characters.Add(_character3);

            _character1.Class = _class1;
            _character1.Race = _race1;
            _character2.Class = _class2;
            _character2.Race = _race2;
            _character3.Class = _class3;
            _character3.Race = _race3;

            _context.Users.Add(_user1);
            _context.Users.Add(_user2);
            _context.SaveChanges();


            _createCharacterArgs = new CharacterArgs()
            {
                ClassId = "1",
                RaceId = "1",
                Name = "New Character 1"
            };
            _createCharacterArgsWrongClass = new CharacterArgs()
            {
                ClassId = "2",
                RaceId = "1",
                Name = "New Character 1"
            };
            _createCharacterArgsNullClass = new CharacterArgs()
            {
                ClassId = "99",
                RaceId = "1",
                Name = "New Frodo Null Class"
            }; 
            _createCharacterArgsWrongRace = new CharacterArgs()
            {
                ClassId = "1",
                RaceId = "2",
                Name = "New Character 1"
            };
            _createCharacterArgsNullRace = new CharacterArgs()
            {
                ClassId = "1",
                RaceId = "99",
                Name = "New Frodo Null Race"
            };
            _createCharacterClassArgs = new CharacterClassArgs()
            {
                Name = "New Class",
                Desctiption = "Beschreibung New Class"
            };
            _createCharacterRaceArgs = new CharacterRaceArgs()
            {
                Name = "New Race",
                Desctiption = "Beschreibung New Race"
            };
            _updateCharacterClassArgs = new CharacterClassArgs()
            {
                Name = "Updated New Class",
                Desctiption = "Updated Beschreibung New Class"
            };
            _updateCharacterRaceArgs = new CharacterRaceArgs()
            {
                Name = "Updated New Race",
                Desctiption = "Updated Beschreibung New Race"
            };
        }
    }
}
