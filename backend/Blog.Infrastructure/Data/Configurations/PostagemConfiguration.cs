using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infrastructure.Data.Configurations;

public class PostagemConfiguration : IEntityTypeConfiguration<Postagem>
{
    public void Configure(EntityTypeBuilder<Postagem> builder)
    {
        builder.ToTable("Postagens");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Conteudo)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(p => p.DataCriacao)
            .IsRequired();

        builder.Property(p => p.DataAtualizacao);

        builder.HasOne(p => p.Usuario)
            .WithMany(u => u.Postagens)
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(p => p.DataCriacao);
        builder.HasIndex(p => p.UsuarioId);
    }
}
