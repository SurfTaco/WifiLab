using System;
using DAL.BAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL
{
    public class LibContext : DbContext
    {
        public LibContext() { }
        // Zweiter Ctor um die Instanz aus der Factory entgegennehmen zu können.
        public LibContext(DbContextOptions<LibContext> options) : base(options){ }

        // Pro Entitaet benoetigen wir ein DbSet
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rent> Rents { get; set; }

        // Hier können wir standardmaeßig unsere Conventions bzw.
        // Umwandlungen / Zwischenschritte von DAtentypen vornehmen
        // damit EF auch weiß was er mit unbekannten Datentypen
        // machen soll bzw. wir erstellen kompatible SQL Datentypen zum parsen.
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            // Hier ruht der DateOnly convert um in der Sql aus DateOnly c# ein Date SQL Datentyp
            // zu erzeugen
            /*
            configurationBuilder
                .Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
            */
        }

        // Konfiguration beim Migrieren
        // Hier kann man auch den Seed platzieren
        // bzw. ist das der Ort für die Konfig via Fluent-API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Hier platzieren wir (vorerst) den Constring damit wir
            // über das Terminal die Migrations usw. adden können.
            optionsBuilder.UseSqlServer(
                "Constring here"
                );
        }
        */

        // Methode um Daten zu platzieren (Testdaten)
        void SeedData(ModelBuilder builder)
        {
            // Seed Authors
            builder.Entity<Author>()
                .HasData(
                    new Author()
                    {
                        Id = 1,
                        Firstname = "John Ronald Reuen",
                        Surname = "Tolkien"
                    },
                    new Author()
                    {
                        Id = 2,
                        Firstname = "Joanne Kathleen",
                        Surname = "Rowling"
                    });

            // Seed Books
            builder.Entity<Book>()
                .HasData(
                    new Book()
                    {
                        Id = 1,
                        AuthorID = 1,
                        Name = "The Hobbit or there and back again",
                        ReleaseDate = new DateTime(1937, 9, 21),
                        CoverPicture =
                        new Uri("https://images-na.ssl-images-amazon.com/images/I/71KG561kbYL.jpg")
                    },
                    new Book()
                    {
                        Id = 2,
                        AuthorID = 2,
                        Name = "Harry Potter and the chamber of secrets",
                        ReleaseDate = new DateTime(1998, 7, 2),
                        CoverPicture =
                        new Uri("https://pictures.abebooks.com/isbn/9780545582926-us.jpg")
                    });

            // Seed Customer
            builder.Entity<Customer>()
                .HasData(
                    new Customer()
                    {
                        Id = 1,
                        Firstname = "John",
                        Surname = "Trapper",
                        Email = "my@testemail.at",
                        IsAdmin = false,
                        RegistrationDate = DateTime.Now
                    },
                    new Customer()
                    {
                        Id = 2,
                        Firstname = "Johnboy",
                        Surname = "Walton",
                        Email = "Johnboy@waltons.at",
                        IsAdmin = false,
                        RegistrationDate = DateTime.Now
                    });

            // Seed Rent
            builder.Entity<Rent>()
                .HasData(
                    new Rent()
                    {
                        Id = 1,
                        BookId = 1,
                        CustomerId = 1,
                        DateOfRent = new DateTime(2022, 2, 1),
                        DateOfReturn = new DateTime(2022, 2, 4)
                    },
                    new Rent()
                    {
                        Id = 2,
                        BookId = 2,
                        CustomerId = 1,
                        DateOfRent = new DateTime(2022, 2, 1)
                    });
        }


        // Genestete Klasse um DateOnly kompatibel zu machen
        // Wir konvertieren DateOnly zu DateTime und nehmen uns darauf
        // dann lediglich das Date
        class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() :base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d)) { }
        }

        // Wir haben ja den Connection String aus dem OnConfig gelöscht und
        // erstellen daher nun über das FactoryPattern die Möglichkeit,
        // dass die DAL dennoch in der Lage ist zu ihrer Datenbank zu "telefonieren"
        public class DesignTimeDBContextFactory : IDesignTimeDbContextFactory<LibContext>
        {
            public LibContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<LibContext>();
                builder.UseSqlServer(
                    "Server=93.90.193.178; Database=LibraryDominik; User Id = MyTempLogin; Password = Test1234!;TrustServerCertificate=True" // Hier kommt der Constring rein
                    );

                return new LibContext(builder.Options);
            }
        }

    }
}
