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
    [Migration("20230927060617_JWTToken")]
    partial class JWTToken
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

                    b.Property<string>("GameId")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Platform")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

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
                    b.Navigation("Nicknames");
                });
#pragma warning restore 612, 618
        }
    }
}
