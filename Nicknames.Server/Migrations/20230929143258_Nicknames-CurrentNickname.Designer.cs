﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nicknames.Server.Storage;

#nullable disable

namespace Nicknames.Server.Migrations
{
    [DbContext(typeof(NicknamesDbContext))]
    [Migration("20230929143258_Nicknames-CurrentNickname")]
    partial class NicknamesCurrentNickname
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Nicknames.Shared.Entities.Nickname", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Nicknames");
                });

            modelBuilder.Entity("Nicknames.Shared.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("CurrentNicknameId")
                        .HasColumnType("int");

                    b.Property<string>("GameId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Platform")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CurrentNicknameId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Nicknames.Shared.Entities.Nickname", b =>
                {
                    b.HasOne("Nicknames.Shared.Entities.User", null)
                        .WithMany("Nicknames")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Nicknames.Shared.Entities.User", b =>
                {
                    b.HasOne("Nicknames.Shared.Entities.Nickname", "CurrentNickname")
                        .WithMany()
                        .HasForeignKey("CurrentNicknameId");

                    b.Navigation("CurrentNickname");
                });

            modelBuilder.Entity("Nicknames.Shared.Entities.User", b =>
                {
                    b.Navigation("Nicknames");
                });
#pragma warning restore 612, 618
        }
    }
}