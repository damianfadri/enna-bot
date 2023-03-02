﻿// <auto-generated />
using System;
using Enna.Bot.Infrastructure.Mssql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Enna.Bot.Infrastructure.Mssql.Migrations
{
    [DbContext(typeof(StreamerContext))]
    [Migration("20230302104440_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Enna.Discord.Domain.TextChannelFeed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Channel")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("Guild")
                        .HasColumnType("decimal(20,0)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("TextChannelFeeds");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Link")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StreamEndedUtc")
                        .HasColumnType("datetime2");

                    b.Property<string>("StreamLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StreamStartedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("StreamerId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Feed", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("LastNotifiedUtc")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("StreamerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StreamerId");

                    b.ToTable("Feeds");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Streamer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Streamers");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Channel", b =>
                {
                    b.HasOne("Enna.Streamers.Domain.Streamer", "Streamer")
                        .WithMany("Channels")
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Feed", b =>
                {
                    b.HasOne("Enna.Streamers.Domain.Streamer", "Streamer")
                        .WithMany("Feeds")
                        .HasForeignKey("StreamerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Streamer");
                });

            modelBuilder.Entity("Enna.Streamers.Domain.Streamer", b =>
                {
                    b.Navigation("Channels");

                    b.Navigation("Feeds");
                });
#pragma warning restore 612, 618
        }
    }
}
