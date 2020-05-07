﻿// <auto-generated />
using MUDhub.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MUDhub.Server.Migrations
{
    [DbContext(typeof(MudDbContext))]
    partial class MudDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("MUDhub.Core.Models.Area", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.Character", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClassId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RaceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Starvation")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.HasIndex("GameId");

                    b.HasIndex("OwnerId");

                    b.HasIndex("RaceId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterBoost", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("CharacterId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CharacterId");

                    b.ToTable("CharacterBoost");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterClass", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterRace", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Races");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Muds.MudGame", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("AutoRestart")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DefaultRoomId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageKey")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DefaultRoomId");

                    b.HasIndex("OwnerId");

                    b.ToTable("MudGames");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Muds.MudJoinRequest", b =>
                {
                    b.Property<string>("MudId")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("MudId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MudJoinRequests");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.Room", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("AreaId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AreaId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.RoomConnection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LockAssociatedId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LockDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("LockType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Room1Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Room2Id")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Room1Id");

                    b.HasIndex("Room2Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomConnections");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.RoomInteraction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ExecutionMessage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RelatedId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomInteractions");
                });

            modelBuilder.Entity("MUDhub.Core.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetKey")
                        .HasColumnType("TEXT");

                    b.Property<int>("Role")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Area", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Muds.MudGame", "Game")
                        .WithMany("Areas")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.Character", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Characters.CharacterClass", "Class")
                        .WithMany("Characters")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.Muds.MudGame", "Game")
                        .WithMany("Characters")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.User", "Owner")
                        .WithMany("Characters")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.Characters.CharacterRace", "Race")
                        .WithMany("Characters")
                        .HasForeignKey("RaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterBoost", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Characters.Character", "Character")
                        .WithMany("ActiveBoosts")
                        .HasForeignKey("CharacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterClass", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Muds.MudGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Characters.CharacterRace", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Muds.MudGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Muds.MudGame", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Rooms.Room", "DefaultRoom")
                        .WithMany()
                        .HasForeignKey("DefaultRoomId");

                    b.HasOne("MUDhub.Core.Models.User", "Owner")
                        .WithMany("MudGames")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Muds.MudJoinRequest", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Muds.MudGame", "MudGame")
                        .WithMany("JoinRequests")
                        .HasForeignKey("MudId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.User", "User")
                        .WithMany("Joins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.Room", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Area", "Area")
                        .WithMany("Rooms")
                        .HasForeignKey("AreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.RoomConnection", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Rooms.Room", "Room1")
                        .WithMany()
                        .HasForeignKey("Room1Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.Rooms.Room", "Room2")
                        .WithMany()
                        .HasForeignKey("Room2Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MUDhub.Core.Models.Rooms.Room", null)
                        .WithMany("Connections")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("MUDhub.Core.Models.Rooms.RoomInteraction", b =>
                {
                    b.HasOne("MUDhub.Core.Models.Rooms.Room", "Room")
                        .WithMany("Interactions")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
