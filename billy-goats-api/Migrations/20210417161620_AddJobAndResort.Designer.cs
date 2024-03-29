﻿// <auto-generated />
using System;
using BillyGoats.Api.Models.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BillyGoats.Api.Migrations
{
    [DbContext(typeof(BillyGoatsDb))]
    [Migration("20210417161620_AddJobAndResort")]
    partial class AddJobAndResort
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("BillyGoats.Api.Models.Guest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("last_name");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_guests");

                    b.HasIndex("UserId");

                    b.ToTable("guests");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Guide", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("last_name");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_guides");

                    b.HasIndex("UserId");

                    b.ToTable("guides");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Job", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("Completed")
                        .HasMaxLength(75)
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<DateTime>("Date")
                        .HasMaxLength(75)
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<long>("GuestId")
                        .HasColumnType("bigint")
                        .HasColumnName("guest_id");

                    b.Property<long>("GuideId")
                        .HasColumnType("bigint")
                        .HasColumnName("guide_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("name");

                    b.Property<long>("ResortId")
                        .HasColumnType("bigint")
                        .HasColumnName("resort_id");

                    b.HasKey("Id")
                        .HasName("pk_jobs");

                    b.HasIndex("GuestId");

                    b.HasIndex("GuideId");

                    b.HasIndex("ResortId");

                    b.ToTable("jobs");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Resort", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_resorts");

                    b.ToTable("resorts");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("last_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.Property<int>("UserType")
                        .HasColumnType("integer")
                        .HasColumnName("user_type");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("character varying(75)")
                        .HasColumnName("username");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Guest", b =>
                {
                    b.HasOne("BillyGoats.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_guests_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Guide", b =>
                {
                    b.HasOne("BillyGoats.Api.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_guides_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BillyGoats.Api.Models.Job", b =>
                {
                    b.HasOne("BillyGoats.Api.Models.Guest", "Guest")
                        .WithMany()
                        .HasForeignKey("GuestId")
                        .HasConstraintName("fk_jobs_guests_guest_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillyGoats.Api.Models.Guide", "Guide")
                        .WithMany()
                        .HasForeignKey("GuideId")
                        .HasConstraintName("fk_jobs_guides_guide_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BillyGoats.Api.Models.Resort", "Resort")
                        .WithMany()
                        .HasForeignKey("ResortId")
                        .HasConstraintName("fk_jobs_resorts_resort_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Guide");

                    b.Navigation("Resort");
                });
#pragma warning restore 612, 618
        }
    }
}
