﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SplitTheBill.Persistence;

#nullable disable

namespace SplitTheBill.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Expense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PaidByMemberId")
                        .HasColumnType("uuid");

                    b.Property<byte>("SplitType")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("PaidByMemberId");

                    b.ToTable("Expenses", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.ExpenseParticipant", b =>
                {
                    b.Property<Guid>("ExpenseId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<decimal?>("ExactShare")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.Property<int?>("PercentualShare")
                        .HasColumnType("integer");

                    b.HasKey("ExpenseId", "MemberId");

                    b.ToTable("ExpenseParticipants", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.GroupMember", b =>
                {
                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupId", "MemberId");

                    b.HasIndex("MemberId");

                    b.ToTable("GroupMembers", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 6)
                        .HasColumnType("numeric(18,6)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ReceivingMemberId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SendingMemberId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("ReceivingMemberId");

                    b.HasIndex("SendingMemberId");

                    b.ToTable("Payments", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Members.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.HasKey("Id");

                    b.ToTable("Members", (string)null);
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Expense", b =>
                {
                    b.HasOne("SplitTheBill.Domain.Models.Groups.Group", null)
                        .WithMany("Expenses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitTheBill.Domain.Models.Members.Member", null)
                        .WithMany()
                        .HasForeignKey("PaidByMemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.ExpenseParticipant", b =>
                {
                    b.HasOne("SplitTheBill.Domain.Models.Groups.Expense", null)
                        .WithMany("Participants")
                        .HasForeignKey("ExpenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.GroupMember", b =>
                {
                    b.HasOne("SplitTheBill.Domain.Models.Groups.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SplitTheBill.Domain.Models.Members.Member", null)
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Payment", b =>
                {
                    b.HasOne("SplitTheBill.Domain.Models.Groups.Group", null)
                        .WithMany("Payments")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SplitTheBill.Domain.Models.Members.Member", null)
                        .WithMany()
                        .HasForeignKey("ReceivingMemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SplitTheBill.Domain.Models.Members.Member", null)
                        .WithMany()
                        .HasForeignKey("SendingMemberId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Expense", b =>
                {
                    b.Navigation("Participants");
                });

            modelBuilder.Entity("SplitTheBill.Domain.Models.Groups.Group", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
