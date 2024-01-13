﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TestDotnetReact.Server.Model;

#nullable disable

namespace TestDotnetReact.Server.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("TestDotnetReact.Server.Model.Plant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("PortfolioId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.ToTable("Plants");
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Portfolio", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TenantId");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Tenant", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Plant", b =>
                {
                    b.HasOne("TestDotnetReact.Server.Model.Portfolio", null)
                        .WithMany("Plants")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Portfolio", b =>
                {
                    b.HasOne("TestDotnetReact.Server.Model.Tenant", null)
                        .WithMany("Portfolios")
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Portfolio", b =>
                {
                    b.Navigation("Plants");
                });

            modelBuilder.Entity("TestDotnetReact.Server.Model.Tenant", b =>
                {
                    b.Navigation("Portfolios");
                });
#pragma warning restore 612, 618
        }
    }
}
