using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using PassCrypter.DB;

namespace PassCrypter.Migrations
{
    [DbContext(typeof(PassManagerContext))]
    [Migration("20170821100057_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("PassCrypter.Models.Account", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("email");

                    b.Property<string>("iv")
                        .IsRequired();

                    b.Property<string>("password");

                    b.Property<string>("tag")
                        .IsRequired();

                    b.Property<int>("userID");

                    b.Property<string>("username");

                    b.Property<string>("websiteUrl")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PassCrypter.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("email")
                        .IsRequired();

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<string>("passHash")
                        .IsRequired();

                    b.Property<string>("settings")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
        }
    }
}
