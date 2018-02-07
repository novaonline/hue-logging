﻿// <auto-generated />
using HueLogging.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HueLogging.DAL.Migrations
{
    [DbContext(typeof(HueLoggingContext))]
    partial class HueLoggingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HueLogging.Models.HueConfigStates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddDate");

                    b.Property<string>("IpAddress");

                    b.Property<string>("Key");

                    b.HasKey("Id");

                    b.HasIndex("AddDate");

                    b.ToTable("HueConfigStates");
                });

            modelBuilder.Entity("HueLogging.Models.HueSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("EndDate");

                    b.Property<string>("LightId");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("LightId");

                    b.HasIndex("StartDate");

                    b.ToTable("HueSessions");
                });

            modelBuilder.Entity("HueLogging.Models.Light", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HueType");

                    b.Property<string>("ModelId");

                    b.Property<string>("Name");

                    b.Property<string>("SWVersion");

                    b.HasKey("Id");

                    b.ToTable("Light");
                });

            modelBuilder.Entity("HueLogging.Models.LightEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddDate");

                    b.Property<string>("LightId");

                    b.Property<int?>("StateId");

                    b.HasKey("Id");

                    b.HasIndex("AddDate");

                    b.HasIndex("LightId");

                    b.HasIndex("StateId");

                    b.ToTable("LightEvent");
                });

            modelBuilder.Entity("HueLogging.Models.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<short>("Brightness");

                    b.Property<int>("Hue");

                    b.Property<bool>("On");

                    b.Property<bool>("Reachable");

                    b.Property<short>("Saturation");

                    b.HasKey("Id");

                    b.ToTable("State");
                });

            modelBuilder.Entity("HueLogging.Models.HueSession", b =>
                {
                    b.HasOne("HueLogging.Models.Light", "Light")
                        .WithMany()
                        .HasForeignKey("LightId");
                });

            modelBuilder.Entity("HueLogging.Models.LightEvent", b =>
                {
                    b.HasOne("HueLogging.Models.Light", "Light")
                        .WithMany()
                        .HasForeignKey("LightId");

                    b.HasOne("HueLogging.Models.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId");
                });
#pragma warning restore 612, 618
        }
    }
}
