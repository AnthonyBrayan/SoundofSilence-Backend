﻿// <auto-generated />
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ServiceContext))]
    [Migration("20231010114308_Tables")]
    partial class Tables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.AudioFiles", b =>
                {
                    b.Property<int>("Id_AudioFiles")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_AudioFiles"));

                    b.Property<int>("Id_category")
                        .HasColumnType("int");

                    b.Property<string>("audioSrc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("videoSrc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_AudioFiles");

                    b.ToTable("AudioFiles", (string)null);
                });

            modelBuilder.Entity("Entities.Category", b =>
                {
                    b.Property<int>("Id_category")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_category"));

                    b.Property<string>("name_category")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_category");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("Entities.Rol", b =>
                {
                    b.Property<int>("Id_rol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_rol"));

                    b.Property<string>("Name_rol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_rol");

                    b.ToTable("Rol", (string)null);
                });

            modelBuilder.Entity("Entities.UserAudio", b =>
                {
                    b.Property<int>("Id_UserAudio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_UserAudio"));

                    b.Property<int>("Id_AudioFiles")
                        .HasColumnType("int");

                    b.Property<int>("Id_user")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_UserAudio");

                    b.HasIndex("Id_AudioFiles");

                    b.HasIndex("Id_user");

                    b.ToTable("UserAudio", (string)null);
                });

            modelBuilder.Entity("Entities.Users", b =>
                {
                    b.Property<int>("Id_user")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id_user"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Id_rol")
                        .HasColumnType("int");

                    b.Property<string>("Name_user")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_user");

                    b.HasIndex("Id_rol");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Entities.UserAudio", b =>
                {
                    b.HasOne("Entities.AudioFiles", "AudioFiles")
                        .WithMany()
                        .HasForeignKey("Id_AudioFiles")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Entities.Users", "Users")
                        .WithMany("UserAudio")
                        .HasForeignKey("Id_user")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AudioFiles");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Entities.Users", b =>
                {
                    b.HasOne("Entities.Rol", "Rol")
                        .WithMany("Users")
                        .HasForeignKey("Id_rol")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("Entities.Rol", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("Entities.Users", b =>
                {
                    b.Navigation("UserAudio");
                });
#pragma warning restore 612, 618
        }
    }
}