// <auto-generated />
using System;
using AtaRK.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AtaRK_Back.Migrations
{
    [DbContext(typeof(ServerDbContext))]
    [Migration("20211118130420_UpdateSystemAdmin_AddName")]
    partial class UpdateSystemAdmin_AddName
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AtaRK.Models.ClimateDevice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DeviceNumber")
                        .HasColumnType("int");

                    b.Property<int?>("FranchiseShopId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOnline")
                        .HasColumnType("bit");

                    b.Property<string>("PicturePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FranchiseShopId");

                    b.ToTable("ClimateDevices");
                });

            modelBuilder.Entity("AtaRK.Models.ClimateState", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClimateDeviceId")
                        .HasColumnType("int");

                    b.Property<int?>("FranchiseShopId")
                        .HasColumnType("int");

                    b.Property<double>("Huumidity")
                        .HasColumnType("float");

                    b.Property<double>("Temperature")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ClimateDeviceId");

                    b.HasIndex("FranchiseShopId");

                    b.ToTable("ClimateStates");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseContactInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FastFoodFranchiseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsEmail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPhone")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUrl")
                        .HasColumnType("bit");

                    b.Property<string>("UrlType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FastFoodFranchiseId");

                    b.ToTable("FranchiseContactInfos");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("FastFoodFranchiseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsBanner")
                        .HasColumnType("bit");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FastFoodFranchiseId");

                    b.ToTable("FranchiseImages");
                });

            modelBuilder.Entity("AtaRK.Models.ShopApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FastFoodFranchiseId")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FastFoodFranchiseId");

                    b.ToTable("ShopApplications");
                });

            modelBuilder.Entity("AtaRK.Models.TechMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ClimateDeviceId")
                        .HasColumnType("int");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ShopAdminId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int?>("TechMessageAnswerId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ClimateDeviceId");

                    b.HasIndex("ShopAdminId");

                    b.HasIndex("TechMessageAnswerId")
                        .IsUnique()
                        .HasFilter("[TechMessageAnswerId] IS NOT NULL");

                    b.ToTable("TechMessages");
                });

            modelBuilder.Entity("AtaRK.Models.TechMessageAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SystemAdminId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SystemAdminId");

                    b.ToTable("TechMessageAnswers");
                });

            modelBuilder.Entity("AtaRK.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FranchiseShopShopAdmin", b =>
                {
                    b.Property<int>("FranchiseShopsId")
                        .HasColumnType("int");

                    b.Property<int>("ShopAdminsId")
                        .HasColumnType("int");

                    b.HasKey("FranchiseShopsId", "ShopAdminsId");

                    b.HasIndex("ShopAdminsId");

                    b.ToTable("FranchiseShopShopAdmin");
                });

            modelBuilder.Entity("AtaRK.Models.FastFoodFranchise", b =>
                {
                    b.HasBaseType("AtaRK.Models.User");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MaxHuumidity")
                        .HasColumnType("float");

                    b.Property<double>("MaxTemperature")
                        .HasColumnType("float");

                    b.Property<double>("MinHuumidity")
                        .HasColumnType("float");

                    b.Property<double>("MinTemperature")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("FastFoodFranchises");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseShop", b =>
                {
                    b.HasBaseType("AtaRK.Models.User");

                    b.Property<string>("BuildingNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FastFoodFranchiseId")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("FastFoodFranchiseId");

                    b.ToTable("FranchiseShops");
                });

            modelBuilder.Entity("AtaRK.Models.ShopAdmin", b =>
                {
                    b.HasBaseType("AtaRK.Models.User");

                    b.ToTable("ShopAdmins");
                });

            modelBuilder.Entity("AtaRK.Models.SystemAdmin", b =>
                {
                    b.HasBaseType("AtaRK.Models.User");

                    b.Property<bool>("IsMaster")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SystemAdmins");
                });

            modelBuilder.Entity("AtaRK.Models.ClimateDevice", b =>
                {
                    b.HasOne("AtaRK.Models.FranchiseShop", "FranchiseShop")
                        .WithMany("ClimateDevices")
                        .HasForeignKey("FranchiseShopId");

                    b.Navigation("FranchiseShop");
                });

            modelBuilder.Entity("AtaRK.Models.ClimateState", b =>
                {
                    b.HasOne("AtaRK.Models.ClimateDevice", "ClimateDevice")
                        .WithMany("ClimateStates")
                        .HasForeignKey("ClimateDeviceId");

                    b.HasOne("AtaRK.Models.FranchiseShop", "FranchiseShop")
                        .WithMany()
                        .HasForeignKey("FranchiseShopId");

                    b.Navigation("ClimateDevice");

                    b.Navigation("FranchiseShop");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseContactInfo", b =>
                {
                    b.HasOne("AtaRK.Models.FastFoodFranchise", "FastFoodFranchise")
                        .WithMany("FranchiseContactInfos")
                        .HasForeignKey("FastFoodFranchiseId");

                    b.Navigation("FastFoodFranchise");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseImage", b =>
                {
                    b.HasOne("AtaRK.Models.FastFoodFranchise", "FastFoodFranchise")
                        .WithMany("FranchiseImages")
                        .HasForeignKey("FastFoodFranchiseId");

                    b.Navigation("FastFoodFranchise");
                });

            modelBuilder.Entity("AtaRK.Models.ShopApplication", b =>
                {
                    b.HasOne("AtaRK.Models.FastFoodFranchise", "FastFoodFranchise")
                        .WithMany("ShopApplications")
                        .HasForeignKey("FastFoodFranchiseId");

                    b.Navigation("FastFoodFranchise");
                });

            modelBuilder.Entity("AtaRK.Models.TechMessage", b =>
                {
                    b.HasOne("AtaRK.Models.ClimateDevice", "ClimateDevice")
                        .WithMany("TechMessages")
                        .HasForeignKey("ClimateDeviceId");

                    b.HasOne("AtaRK.Models.ShopAdmin", "ShopAdmin")
                        .WithMany("TechMessages")
                        .HasForeignKey("ShopAdminId");

                    b.HasOne("AtaRK.Models.TechMessageAnswer", "TechMessageAnswer")
                        .WithOne("TechMessage")
                        .HasForeignKey("AtaRK.Models.TechMessage", "TechMessageAnswerId");

                    b.Navigation("ClimateDevice");

                    b.Navigation("ShopAdmin");

                    b.Navigation("TechMessageAnswer");
                });

            modelBuilder.Entity("AtaRK.Models.TechMessageAnswer", b =>
                {
                    b.HasOne("AtaRK.Models.SystemAdmin", "SystemAdmin")
                        .WithMany("TechMessageAnswers")
                        .HasForeignKey("SystemAdminId");

                    b.Navigation("SystemAdmin");
                });

            modelBuilder.Entity("FranchiseShopShopAdmin", b =>
                {
                    b.HasOne("AtaRK.Models.FranchiseShop", null)
                        .WithMany()
                        .HasForeignKey("FranchiseShopsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AtaRK.Models.ShopAdmin", null)
                        .WithMany()
                        .HasForeignKey("ShopAdminsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AtaRK.Models.FastFoodFranchise", b =>
                {
                    b.HasOne("AtaRK.Models.User", null)
                        .WithOne()
                        .HasForeignKey("AtaRK.Models.FastFoodFranchise", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseShop", b =>
                {
                    b.HasOne("AtaRK.Models.FastFoodFranchise", "FastFoodFranchise")
                        .WithMany("FranchiseShops")
                        .HasForeignKey("FastFoodFranchiseId");

                    b.HasOne("AtaRK.Models.User", null)
                        .WithOne()
                        .HasForeignKey("AtaRK.Models.FranchiseShop", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("FastFoodFranchise");
                });

            modelBuilder.Entity("AtaRK.Models.ShopAdmin", b =>
                {
                    b.HasOne("AtaRK.Models.User", null)
                        .WithOne()
                        .HasForeignKey("AtaRK.Models.ShopAdmin", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AtaRK.Models.SystemAdmin", b =>
                {
                    b.HasOne("AtaRK.Models.User", null)
                        .WithOne()
                        .HasForeignKey("AtaRK.Models.SystemAdmin", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AtaRK.Models.ClimateDevice", b =>
                {
                    b.Navigation("ClimateStates");

                    b.Navigation("TechMessages");
                });

            modelBuilder.Entity("AtaRK.Models.TechMessageAnswer", b =>
                {
                    b.Navigation("TechMessage");
                });

            modelBuilder.Entity("AtaRK.Models.FastFoodFranchise", b =>
                {
                    b.Navigation("FranchiseContactInfos");

                    b.Navigation("FranchiseImages");

                    b.Navigation("FranchiseShops");

                    b.Navigation("ShopApplications");
                });

            modelBuilder.Entity("AtaRK.Models.FranchiseShop", b =>
                {
                    b.Navigation("ClimateDevices");
                });

            modelBuilder.Entity("AtaRK.Models.ShopAdmin", b =>
                {
                    b.Navigation("TechMessages");
                });

            modelBuilder.Entity("AtaRK.Models.SystemAdmin", b =>
                {
                    b.Navigation("TechMessageAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
