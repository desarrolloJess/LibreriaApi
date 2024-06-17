﻿// <auto-generated />
using System;
using BibliotecaApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BibliotecaApi.Migrations
{
    [DbContext(typeof(LibrosContext))]
    partial class LibrosContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BibliotecaApi.Models.Autor", b =>
                {
                    b.Property<int>("AutorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AutorId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Pais")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AutorId");

                    b.ToTable("autor", (string)null);

                    b.HasData(
                        new
                        {
                            AutorId = 1,
                            Nombre = "J.K. Rowling",
                            Pais = "Mexico"
                        },
                        new
                        {
                            AutorId = 2,
                            Nombre = "Stephen King",
                            Pais = "Mexico"
                        });
                });

            modelBuilder.Entity("BibliotecaApi.Models.Editorial", b =>
                {
                    b.Property<int>("EditorialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EditorialId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("EditorialId");

                    b.ToTable("editorial", (string)null);

                    b.HasData(
                        new
                        {
                            EditorialId = 1,
                            Nombre = "Oxford University Press"
                        },
                        new
                        {
                            EditorialId = 2,
                            Nombre = "Pearson Education"
                        });
                });

            modelBuilder.Entity("BibliotecaApi.Models.Libro", b =>
                {
                    b.Property<int>("LibroId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LibroId"));

                    b.Property<string>("Edicion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EditorialId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("FechaPublicacion")
                        .HasColumnType("date");

                    b.Property<int>("NumPaginas")
                        .HasColumnType("int");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("LibroId");

                    b.HasIndex("EditorialId");

                    b.ToTable("libro", (string)null);
                });

            modelBuilder.Entity("BibliotecaApi.Models.LibroAutor", b =>
                {
                    b.Property<int>("LibroId")
                        .HasColumnType("int");

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.HasKey("LibroId", "AutorId");

                    b.HasIndex("AutorId");

                    b.ToTable("libroAutor", (string)null);
                });

            modelBuilder.Entity("BibliotecaApi.Models.Libro", b =>
                {
                    b.HasOne("BibliotecaApi.Models.Editorial", "Editorial")
                        .WithMany("Libros")
                        .HasForeignKey("EditorialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Editorial");
                });

            modelBuilder.Entity("BibliotecaApi.Models.LibroAutor", b =>
                {
                    b.HasOne("BibliotecaApi.Models.Autor", "Autor")
                        .WithMany("LibrosAutores")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BibliotecaApi.Models.Libro", "Libro")
                        .WithMany("LibrosAutores")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Autor");

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("BibliotecaApi.Models.Autor", b =>
                {
                    b.Navigation("LibrosAutores");
                });

            modelBuilder.Entity("BibliotecaApi.Models.Editorial", b =>
                {
                    b.Navigation("Libros");
                });

            modelBuilder.Entity("BibliotecaApi.Models.Libro", b =>
                {
                    b.Navigation("LibrosAutores");
                });
#pragma warning restore 612, 618
        }
    }
}
