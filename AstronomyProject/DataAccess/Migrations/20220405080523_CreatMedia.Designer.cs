﻿// <auto-generated />
using System;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(AstronomyContext))]
    [Migration("20220405080523_CreatMedia")]
    partial class CreatMedia
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.CloseApproach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CloseApproachDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MissDistance")
                        .HasColumnType("float");

                    b.Property<int>("NearAsteroidId")
                        .HasColumnType("int");

                    b.Property<string>("OrbitingBody")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("RelativeVelocity")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("NearAsteroidId");

                    b.ToTable("CloseApproachs");
                });

            modelBuilder.Entity("Models.ImageOfTheDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Explanation")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("ImageOfTheDayGallery");
                });

            modelBuilder.Entity("Models.ImaggaTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Confidence")
                        .HasColumnType("float");

                    b.Property<int?>("MediaGroupeId")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("MediaGroupeId");

                    b.ToTable("ImaggaTags");
                });

            modelBuilder.Entity("Models.MediaGroupe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MediaType")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("PreviewUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("Models.MediaItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MediaGroupeId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(700)
                        .HasColumnType("nvarchar(700)");

                    b.HasKey("Id");

                    b.HasIndex("MediaGroupeId");

                    b.ToTable("MediaItems");
                });

            modelBuilder.Entity("Models.NearAsteroid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("AbsoluteMagnitudeH")
                        .HasColumnType("float");

                    b.Property<double>("EstimatedDiameterMax")
                        .HasColumnType("float");

                    b.Property<double>("EstimatedDiameterMin")
                        .HasColumnType("float");

                    b.Property<bool>("IsPotentiallyHazardousAsteroid")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSentryObject")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("NearAsteroids");
                });

            modelBuilder.Entity("Models.SearchWordModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MediaGroupeId")
                        .HasColumnType("int");

                    b.Property<string>("SearchWord")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("MediaGroupeId");

                    b.ToTable("SearchWords");
                });

            modelBuilder.Entity("Models.CloseApproach", b =>
                {
                    b.HasOne("Models.NearAsteroid", null)
                        .WithMany("CloseApproachs")
                        .HasForeignKey("NearAsteroidId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.ImaggaTag", b =>
                {
                    b.HasOne("Models.MediaGroupe", null)
                        .WithMany("Tags")
                        .HasForeignKey("MediaGroupeId");
                });

            modelBuilder.Entity("Models.MediaItem", b =>
                {
                    b.HasOne("Models.MediaGroupe", null)
                        .WithMany("MediaItems")
                        .HasForeignKey("MediaGroupeId");
                });

            modelBuilder.Entity("Models.SearchWordModel", b =>
                {
                    b.HasOne("Models.MediaGroupe", null)
                        .WithMany("SearchWords")
                        .HasForeignKey("MediaGroupeId");
                });

            modelBuilder.Entity("Models.MediaGroupe", b =>
                {
                    b.Navigation("MediaItems");

                    b.Navigation("SearchWords");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Models.NearAsteroid", b =>
                {
                    b.Navigation("CloseApproachs");
                });
#pragma warning restore 612, 618
        }
    }
}
