﻿// <auto-generated />
using System;
using Learnz.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Learnz.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220622114326_TogetherAnswer")]
    partial class TogetherAnswer
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Learnz.Entities.TogetherAsk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool?>("Answer")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TogetherAsk");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherConnection", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TogetherConnections");
                });

            modelBuilder.Entity("Learnz.Entities.TogetherSwipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Choice")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId2")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TogetherSwipes");
                });

            modelBuilder.Entity("Learnz.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BadSubject1")
                        .HasColumnType("int");

                    b.Property<int>("BadSubject2")
                        .HasColumnType("int");

                    b.Property<int>("BadSubject3")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GoodSubject1")
                        .HasColumnType("int");

                    b.Property<int>("GoodSubject2")
                        .HasColumnType("int");

                    b.Property<int>("GoodSubject3")
                        .HasColumnType("int");

                    b.Property<int>("Grade")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("ProfileImage")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("RefreshExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
