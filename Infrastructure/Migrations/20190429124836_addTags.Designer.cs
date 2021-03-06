﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20190429124836_addTags")]
    partial class addTags
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Infrastructure.Entities.Developer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("developer_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnName("developer_fullname")
                        .HasMaxLength(150);

                    b.Property<string>("Nickname")
                        .HasColumnName("developer_nickname")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("developers");
                });

            modelBuilder.Entity("Infrastructure.Entities.DeveloperTag", b =>
                {
                    b.Property<int>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.Property<int>("TagId")
                        .HasColumnName("tag_id");

                    b.HasKey("DeveloperId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("developer_tag");
                });

            modelBuilder.Entity("Infrastructure.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("project_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnName("project_description")
                        .HasMaxLength(450);

                    b.Property<DateTime>("EndDate")
                        .HasColumnName("project_end_date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("project_name")
                        .HasMaxLength(150);

                    b.Property<DateTime>("StartDate")
                        .HasColumnName("project_start_date");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("projects");
                });

            modelBuilder.Entity("Infrastructure.Entities.ProjectAssignment", b =>
                {
                    b.Property<int>("ProjectId")
                        .HasColumnName("project_id");

                    b.Property<int>("DeveloperId")
                        .HasColumnName("developer_id");

                    b.HasKey("ProjectId", "DeveloperId");

                    b.HasIndex("DeveloperId");

                    b.ToTable("project_developer");
                });

            modelBuilder.Entity("Infrastructure.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("tag_id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("tag_name")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("tags");
                });

            modelBuilder.Entity("Infrastructure.Entities.DeveloperTag", b =>
                {
                    b.HasOne("Infrastructure.Entities.Developer", "Developer")
                        .WithMany("DeveloperTags")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Entities.Tag", "Tag")
                        .WithMany("DeveloperTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Infrastructure.Entities.ProjectAssignment", b =>
                {
                    b.HasOne("Infrastructure.Entities.Developer", "Developer")
                        .WithMany("ProjectAssignments")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Infrastructure.Entities.Project", "Project")
                        .WithMany("ProjectAssignments")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
