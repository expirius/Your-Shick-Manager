﻿// <auto-generated />
using System;
using MFASeekerServer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MFASeekerServer.Migrations
{
    [DbContext(typeof(SeekerDbContext))]
    [Migration("20241113130628_AddUserImageToiletoptional")]
    partial class AddUserImageToiletoptional
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("MFASeekerServer.Core.Entities.ImageFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ByteBase64")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ImageFiles");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.Toilet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<double>("Rating")
                        .HasColumnType("REAL");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("UserID");

                    b.ToTable("Toilets");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceInfo")
                        .HasColumnType("TEXT");

                    b.Property<string>("Guid")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedDateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasColumnName("Username");

                    b.HasKey("Id");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.UserImageToilet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ToiletID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageID")
                        .IsUnique();

                    b.HasIndex("ToiletID");

                    b.HasIndex("UserID");

                    b.ToTable("ToiletImages");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.Toilet", b =>
                {
                    b.HasOne("MFASeekerServer.Core.Entities.User", "User")
                        .WithMany("Toilets")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.SetNull)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.UserImageToilet", b =>
                {
                    b.HasOne("MFASeekerServer.Core.Entities.ImageFile", "ImageFile")
                        .WithOne()
                        .HasForeignKey("MFASeekerServer.Core.Entities.UserImageToilet", "ImageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MFASeekerServer.Core.Entities.Toilet", "Toilet")
                        .WithMany("UserImageToilets")
                        .HasForeignKey("ToiletID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MFASeekerServer.Core.Entities.User", "User")
                        .WithMany("UserImageToilets")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("ImageFile");

                    b.Navigation("Toilet");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.Toilet", b =>
                {
                    b.Navigation("UserImageToilets");
                });

            modelBuilder.Entity("MFASeekerServer.Core.Entities.User", b =>
                {
                    b.Navigation("Toilets");

                    b.Navigation("UserImageToilets");
                });
#pragma warning restore 612, 618
        }
    }
}
