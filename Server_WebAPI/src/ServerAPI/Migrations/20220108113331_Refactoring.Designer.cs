﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerAPI.Repositories;

namespace ServerAPI.Migrations
{
    [DbContext(typeof(ServerDBContext))]
    [Migration("20220108113331_Refactoring")]
    partial class Refactoring
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ServerAPI.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("EmployeeItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.ExecutedCommand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExecutionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegisteredUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegisteredUserId");

                    b.ToTable("ExecutedCommandsItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.Laboratory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LabName")
                        .IsRequired()
                        .HasMaxLength(35)
                        .HasColumnType("nvarchar(35)");

                    b.Property<string>("LabOrganizer")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("LaboratoryItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.LaboratoryRequirement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LaboratoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LaboratoryId");

                    b.ToTable("LaboratoryRequirementsItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.RecordedEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RegisteredUserId")
                        .HasColumnType("int");

                    b.Property<string>("RegistryContent")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("RegisteredUserId");

                    b.ToTable("RecordedEventItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.RegisteredUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("LaboratoryId")
                        .HasColumnType("int");

                    b.Property<bool>("NoWarning")
                        .HasColumnType("bit");

                    b.Property<string>("UniqueCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Id");

                    b.HasIndex("LaboratoryId");

                    b.ToTable("RegisteredUserItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleName")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("RoleItems");
                });

            modelBuilder.Entity("ServerAPI.Entities.Employee", b =>
                {
                    b.HasOne("ServerAPI.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("ServerAPI.Entities.ExecutedCommand", b =>
                {
                    b.HasOne("ServerAPI.Entities.RegisteredUser", "RegisteredUser")
                        .WithMany("ExecutedCommands")
                        .HasForeignKey("RegisteredUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegisteredUser");
                });

            modelBuilder.Entity("ServerAPI.Entities.LaboratoryRequirement", b =>
                {
                    b.HasOne("ServerAPI.Entities.Laboratory", "Laboratory")
                        .WithMany("LaboratoryRequirements")
                        .HasForeignKey("LaboratoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laboratory");
                });

            modelBuilder.Entity("ServerAPI.Entities.RecordedEvent", b =>
                {
                    b.HasOne("ServerAPI.Entities.RegisteredUser", "RegisteredUser")
                        .WithMany("EventRegistries")
                        .HasForeignKey("RegisteredUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RegisteredUser");
                });

            modelBuilder.Entity("ServerAPI.Entities.RegisteredUser", b =>
                {
                    b.HasOne("ServerAPI.Entities.Laboratory", "Laboratory")
                        .WithMany()
                        .HasForeignKey("LaboratoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Laboratory");
                });

            modelBuilder.Entity("ServerAPI.Entities.Laboratory", b =>
                {
                    b.Navigation("LaboratoryRequirements");
                });

            modelBuilder.Entity("ServerAPI.Entities.RegisteredUser", b =>
                {
                    b.Navigation("EventRegistries");

                    b.Navigation("ExecutedCommands");
                });
#pragma warning restore 612, 618
        }
    }
}
