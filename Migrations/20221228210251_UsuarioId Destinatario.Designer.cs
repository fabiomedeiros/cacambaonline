﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using cacambaonline.Data;

#nullable disable

namespace cacambaonline.Migrations
{
    [DbContext(typeof(MeuDbContext))]
    [Migration("20221228210251_UsuarioId Destinatario")]
    partial class UsuarioIdDestinatario
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("cacambaonline.Models.Cacambas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Obs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransportadoresId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransportadoresId");

                    b.ToTable("Cacambas");
                });

            modelBuilder.Entity("cacambaonline.Models.CTR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CacambasId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DestinatariosId")
                        .HasColumnType("int");

                    b.Property<bool?>("Excluido")
                        .HasColumnType("bit");

                    b.Property<bool?>("Fechada")
                        .HasColumnType("bit");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Localizacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TransportadoresId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CacambasId");

                    b.HasIndex("DestinatariosId");

                    b.HasIndex("TransportadoresId");

                    b.ToTable("CTR");
                });

            modelBuilder.Entity("cacambaonline.Models.Destinatarios", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cnpj")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnderecoEletronico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeEmpresarial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeFantasia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Destinatarios");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosDestinatario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bairro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CEP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Complemento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DestinatariosId")
                        .HasColumnType("int");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logradouro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DestinatariosId")
                        .IsUnique();

                    b.ToTable("EnderecosDestinatario");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosGerador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bairro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CEP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Complemento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("GeradoresId")
                        .HasColumnType("int");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logradouro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GeradoresId")
                        .IsUnique();

                    b.ToTable("EnderecosGerador");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosTransportador", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bairro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CEP")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cidade")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Complemento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Estado")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Logradouro")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Numero")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TransportadoresId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransportadoresId")
                        .IsUnique();

                    b.ToTable("EnderecosTransportador");
                });

            modelBuilder.Entity("cacambaonline.Models.Geradores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cpf")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Geradores");
                });

            modelBuilder.Entity("cacambaonline.Models.Infracoes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CTRId")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Obs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Prazo")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioFiscalId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CTRId");

                    b.ToTable("Infracoes");
                });

            modelBuilder.Entity("cacambaonline.Models.LogCTR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CTRId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Operacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CTRId");

                    b.ToTable("LogCTR");
                });

            modelBuilder.Entity("cacambaonline.Models.Notificacoes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CTRId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descricao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Foto2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Latitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Localizacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Longitude")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Multa")
                        .HasColumnType("bit");

                    b.Property<string>("Obs")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UsuarioFiscalId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("CTRId");

                    b.ToTable("Notificacoes");
                });

            modelBuilder.Entity("cacambaonline.Models.Transportadores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Cnpj")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnderecoEletronico")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeEmpresarial")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomeFantasia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telefone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Transportadores");
                });

            modelBuilder.Entity("cacambaonline.Models.Cacambas", b =>
                {
                    b.HasOne("cacambaonline.Models.Transportadores", "Transportadores")
                        .WithMany()
                        .HasForeignKey("TransportadoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transportadores");
                });

            modelBuilder.Entity("cacambaonline.Models.CTR", b =>
                {
                    b.HasOne("cacambaonline.Models.Cacambas", "Cacambas")
                        .WithMany()
                        .HasForeignKey("CacambasId");

                    b.HasOne("cacambaonline.Models.Destinatarios", "Destinatarios")
                        .WithMany()
                        .HasForeignKey("DestinatariosId");

                    b.HasOne("cacambaonline.Models.Transportadores", "Transportadores")
                        .WithMany()
                        .HasForeignKey("TransportadoresId");

                    b.Navigation("Cacambas");

                    b.Navigation("Destinatarios");

                    b.Navigation("Transportadores");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosDestinatario", b =>
                {
                    b.HasOne("cacambaonline.Models.Destinatarios", "Destinatarios")
                        .WithOne("EnderecosDestinatario")
                        .HasForeignKey("cacambaonline.Models.EnderecosDestinatario", "DestinatariosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Destinatarios");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosGerador", b =>
                {
                    b.HasOne("cacambaonline.Models.Geradores", "Geradores")
                        .WithOne("EnderecosGerador")
                        .HasForeignKey("cacambaonline.Models.EnderecosGerador", "GeradoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Geradores");
                });

            modelBuilder.Entity("cacambaonline.Models.EnderecosTransportador", b =>
                {
                    b.HasOne("cacambaonline.Models.Transportadores", "Transportadores")
                        .WithOne("EnderecosTransportador")
                        .HasForeignKey("cacambaonline.Models.EnderecosTransportador", "TransportadoresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transportadores");
                });

            modelBuilder.Entity("cacambaonline.Models.Infracoes", b =>
                {
                    b.HasOne("cacambaonline.Models.CTR", "Ctr")
                        .WithMany()
                        .HasForeignKey("CTRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ctr");
                });

            modelBuilder.Entity("cacambaonline.Models.LogCTR", b =>
                {
                    b.HasOne("cacambaonline.Models.CTR", "CTR")
                        .WithMany()
                        .HasForeignKey("CTRId");

                    b.Navigation("CTR");
                });

            modelBuilder.Entity("cacambaonline.Models.Notificacoes", b =>
                {
                    b.HasOne("cacambaonline.Models.CTR", "Ctr")
                        .WithMany()
                        .HasForeignKey("CTRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ctr");
                });

            modelBuilder.Entity("cacambaonline.Models.Destinatarios", b =>
                {
                    b.Navigation("EnderecosDestinatario");
                });

            modelBuilder.Entity("cacambaonline.Models.Geradores", b =>
                {
                    b.Navigation("EnderecosGerador");
                });

            modelBuilder.Entity("cacambaonline.Models.Transportadores", b =>
                {
                    b.Navigation("EnderecosTransportador");
                });
#pragma warning restore 612, 618
        }
    }
}
