﻿// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConfigurationSaver_API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230518144039_Add Polymorphism Device entity")]
    partial class AddPolymorphismDeviceentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Models.Credential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("Models.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CredentialId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CredentialId");

                    b.ToTable("Devices");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Device");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Models.DeviceScheduleTask", b =>
                {
                    b.Property<Guid>("ScheduleTaskId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DeviceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ScheduleTaskId", "DeviceId");

                    b.HasIndex("DeviceId");

                    b.ToTable("DeviceScheduleTasks");
                });

            modelBuilder.Entity("Models.ScheduleTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Hour")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastRun")
                        .HasColumnType("datetime2");

                    b.Property<int>("Minute")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NextRun")
                        .HasColumnType("datetime2");

                    b.Property<int>("Second")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ScheduleTasks");
                });

            modelBuilder.Entity("ConfigurationSaver_API.Models.EsxiServer", b =>
                {
                    b.HasBaseType("Models.Device");

                    b.HasDiscriminator().HasValue("EsxiServer");
                });

            modelBuilder.Entity("Models.Device", b =>
                {
                    b.HasOne("Models.Credential", "Credential")
                        .WithMany("Devices")
                        .HasForeignKey("CredentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Credential");
                });

            modelBuilder.Entity("Models.DeviceScheduleTask", b =>
                {
                    b.HasOne("Models.Device", "Device")
                        .WithMany("DeviceScheduleTasks")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.ScheduleTask", "ScheduleTask")
                        .WithMany("DeviceScheduleTasks")
                        .HasForeignKey("ScheduleTaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("ScheduleTask");
                });

            modelBuilder.Entity("Models.Credential", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("Models.Device", b =>
                {
                    b.Navigation("DeviceScheduleTasks");
                });

            modelBuilder.Entity("Models.ScheduleTask", b =>
                {
                    b.Navigation("DeviceScheduleTasks");
                });
#pragma warning restore 612, 618
        }
    }
}