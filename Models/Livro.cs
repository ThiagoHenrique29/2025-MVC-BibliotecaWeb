using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Biblioteca.Models;

public class Livro
{
    public int LivroID { get; set; }
    public string LivroTituloOriginal { get; set; }
    public string? LivroTituloNacional { get; set; }
    public ushort LivroPaginas { get; set; }
    public string LivroISBN { get; set; }
    public ushort LivroAnoPublicacao { get; set; }
    //public byte[] LivroCapa { get; set; }
    public int EditoraID { get; set; }
    public Editora LivroEditora { get; set; }
    public ICollection<OperacaoCompraVenda> LivroOperacoes { get; set; }
    public ICollection<AutorLivro> AutoresDoLivro { get; set; }
}

public class LivroConfiguration : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.HasKey(p => p.LivroID);

        builder.HasIndex(p => p.LivroTituloNacional);
        builder.HasIndex(p => p.LivroTituloOriginal);

        builder.Property(p => p.LivroTituloOriginal).HasMaxLength(120).IsRequired();
        builder.Property(p => p.LivroTituloNacional).HasMaxLength(120);
        builder.Property(p => p.LivroPaginas).HasDefaultValue(0).IsRequired();
        builder.Property(p => p.LivroISBN).HasMaxLength(13).IsRequired();

        builder.HasOne<Editora>(p => p.LivroEditora)
            .WithMany(p => p.EditoraLivros);
        builder.HasMany<OperacaoCompraVenda>(p => p.LivroOperacoes)
            .WithOne(p => p.OperacaoLivro);
    }
}