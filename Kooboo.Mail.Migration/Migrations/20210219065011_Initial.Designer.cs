﻿// <auto-generated />
using System;
using Kooboo.Mail;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kooboo.Mail.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20210219065011_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Kooboo.Mail.DbHistory", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.HasKey("Id");

                    b.ToTable("__DbHistory");
                });

            modelBuilder.Entity("Kooboo.Mail.EmailAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<int>("AddressType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ForwardAddress")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("EmailAddress");
                });

            modelBuilder.Entity("Kooboo.Mail.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<bool>("Subscribed")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("Kooboo.Mail.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AddressId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Answered")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Bcc")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<long>("BodyPosition")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Cc")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<long>("CreationTimeTick")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Flagged")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FolderId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("From")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<string>("MailFrom")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<bool>("OutGoing")
                        .HasColumnType("INTEGER");

                    b.Property<string>("RcptTo")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<bool>("Read")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Recent")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SmtpMessageId")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<string>("Subject")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<string>("Summary")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<string>("To")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("Kooboo.Mail.TargetAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT COLLATE NOCASE");

                    b.HasKey("Id");

                    b.ToTable("TargetAddress");
                });
#pragma warning restore 612, 618
        }
    }
}
