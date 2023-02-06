using cacambaonline.Migrations;
using cacambaonline.Models;
using Microsoft.EntityFrameworkCore;

namespace cacambaonline.Data;

public class MeuDbContext : DbContext
{
    public MeuDbContext(DbContextOptions options)
        : base(options)
    {
    }
    public DbSet<Cacambas>? Cacambas { get; set; }
    public DbSet<Destinatarios>? Destinatarios { get; set; }
    public DbSet<EnderecosDestinatario>? EnderecosDestinatario { get; set; }
    public DbSet<Geradores>? Geradores { get; set; }
    public DbSet<EnderecosGerador>? EnderecosGerador { get; set; }
    public DbSet<Transportadores>? Transportadores { get; set; }
    public DbSet<EnderecosTransportador>? EnderecosTransportador { get; set; }
    public DbSet<Notificacoes>? Notificacoes { get; set; }
    public DbSet<CTR> CTR { get; set; }
    public DbSet<Models.LogCTR>? LogCTR { get; set; }        
    public DbSet<Infracoes>? Infracoes { get; set; }
    public DbSet<UsuarioTransportador>? UsuarioTransportadores { get; set; }
    public DbSet<UsuarioDestinatario>? UsuarioDestinatarios { get; set; }
    public DbSet<Models.Roles> Roles { get; set; }


    public DbSet<Models.Notificacoes_Antigo> Notificacoes_Antigas { get; set; }
    public DbSet<Models.Notificacoes_Novo> Notificacoes_Novas { get; set; }
    public DbSet<Models.LogNotificacao> LogNotificacoes { get; set; }

    //Para atribuir mais informações no AspNetUsers
    public DbSet<Models.Pessoas> Pessoas { get; set; }




}
