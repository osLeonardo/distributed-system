﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using distributed_system.Context;

#nullable disable

namespace distributedsystem.Data.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20241111230627_DefaultLocation")]
    partial class DefaultLocation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("distributed_system.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentCapacity")
                        .HasColumnType("integer");

                    b.Property<bool>("IsMatriz")
                        .HasColumnType("boolean");

                    b.Property<int>("MaxCapacity")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CurrentCapacity = 0,
                            IsMatriz = true,
                            MaxCapacity = 100,
                            Name = "Default Matriz"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}