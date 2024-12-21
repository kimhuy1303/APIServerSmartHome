﻿// <auto-generated />
using System;
using APIServerSmartHome.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIServerSmartHome.Migrations
{
    [DbContext(typeof(SmartHomeDbContext))]
    [Migration("20241220170707_updatedb-2")]
    partial class updatedb2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APIServerSmartHome.Entities.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DeviceName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.IrrigationSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("TimeWorking")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("IrrigationSchedule");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.OperateTimeWorking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DeviceId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("OperatingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("OperateTimeWorking");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.PowerDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("DeviceId")
                        .HasColumnType("int");

                    b.Property<int>("PowerValue")
                        .HasColumnType("int");

                    b.Property<float>("TimeUsing")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.ToTable("PowerDevice");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.RFIDCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<string>("CardUID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RFIDCard");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("RoomName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.TempHumidValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Humidity")
                        .HasColumnType("int");

                    b.Property<float>("Temperature")
                        .HasColumnType("real");

                    b.Property<DateTime?>("TimeSpan")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("TempHumidValue");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phonenumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.UserDevices", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "DeviceId");

                    b.HasIndex("DeviceId");

                    b.ToTable("UserDevices");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.UserFaces", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FaceImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserFaces");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.Device", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.Room", "Room")
                        .WithMany("Devices")
                        .HasForeignKey("RoomId");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.OperateTimeWorking", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.Device", "Device")
                        .WithMany("OperateTimeWorkings")
                        .HasForeignKey("DeviceId");

                    b.Navigation("Device");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.PowerDevice", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.Device", "Device")
                        .WithMany("PowerDevices")
                        .HasForeignKey("DeviceId");

                    b.Navigation("Device");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.RFIDCard", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.User", "User")
                        .WithMany("RFIDCards")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.UserDevices", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.Device", "Device")
                        .WithMany("UserDevices")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("APIServerSmartHome.Entities.User", "User")
                        .WithMany("UserDevices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.UserFaces", b =>
                {
                    b.HasOne("APIServerSmartHome.Entities.User", "User")
                        .WithMany("UserFaces")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.Device", b =>
                {
                    b.Navigation("OperateTimeWorkings");

                    b.Navigation("PowerDevices");

                    b.Navigation("UserDevices");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.Room", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("APIServerSmartHome.Entities.User", b =>
                {
                    b.Navigation("RFIDCards");

                    b.Navigation("UserDevices");

                    b.Navigation("UserFaces");
                });
#pragma warning restore 612, 618
        }
    }
}
