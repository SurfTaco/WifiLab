﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(LibContext))]
    [Migration("20220225160836_ReinitForCourse")]
    partial class ReinitForCourse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DAL.BAL.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Firstname = "John Ronald Reuen",
                            Surname = "Tolkien"
                        },
                        new
                        {
                            Id = 2,
                            Firstname = "Joanne Kathleen",
                            Surname = "Rowling"
                        });
                });

            modelBuilder.Entity("DAL.BAL.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AuthorID")
                        .HasColumnType("int");

                    b.Property<string>("CoverPicture")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("ReleaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AuthorID");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorID = 1,
                            CoverPicture = "https://images-na.ssl-images-amazon.com/images/I/71KG561kbYL.jpg",
                            Name = "The Hobbit or there and back again",
                            ReleaseDate = new DateTime(1937, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            AuthorID = 2,
                            CoverPicture = "https://pictures.abebooks.com/isbn/9780545582926-us.jpg",
                            Name = "Harry Potter and the chamber of secrets",
                            ReleaseDate = new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("DAL.BAL.Credential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("Credentials");
                });

            modelBuilder.Entity("DAL.BAL.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "my@testemail.at",
                            Firstname = "John",
                            IsAdmin = false,
                            RegistrationDate = new DateTime(2022, 2, 25, 17, 8, 36, 607, DateTimeKind.Local).AddTicks(5170),
                            Surname = "Trapper"
                        },
                        new
                        {
                            Id = 2,
                            Email = "Johnboy@waltons.at",
                            Firstname = "Johnboy",
                            IsAdmin = false,
                            RegistrationDate = new DateTime(2022, 2, 25, 17, 8, 36, 607, DateTimeKind.Local).AddTicks(5190),
                            Surname = "Walton"
                        });
                });

            modelBuilder.Entity("DAL.BAL.Rent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfRent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateOfReturn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Rents");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            CustomerId = 1,
                            DateOfRent = new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOfReturn = new DateTime(2022, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            CustomerId = 1,
                            DateOfRent = new DateTime(2022, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("DAL.BAL.Book", b =>
                {
                    b.HasOne("DAL.BAL.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("DAL.BAL.Credential", b =>
                {
                    b.HasOne("DAL.BAL.Customer", "Customer")
                        .WithOne("Credential")
                        .HasForeignKey("DAL.BAL.Credential", "CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DAL.BAL.Rent", b =>
                {
                    b.HasOne("DAL.BAL.Book", "Book")
                        .WithMany("Rents")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.BAL.Customer", "Customer")
                        .WithMany("Rents")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("DAL.BAL.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("DAL.BAL.Book", b =>
                {
                    b.Navigation("Rents");
                });

            modelBuilder.Entity("DAL.BAL.Customer", b =>
                {
                    b.Navigation("Credential");

                    b.Navigation("Rents");
                });
#pragma warning restore 612, 618
        }
    }
}
