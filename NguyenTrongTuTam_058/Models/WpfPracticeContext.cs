using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnThi.Models;

public partial class WpfPracticeContext : DbContext
{
    public WpfPracticeContext()
    {
    }

    public WpfPracticeContext(DbContextOptions<WpfPracticeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }

    public virtual DbSet<LoaiSp> LoaiSps { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DEVTAM;Initial Catalog=wpf_practice;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HoaDonChiTiet>(entity =>
        {
            entity.HasKey(e => new { e.MaHd, e.MaSp });

            entity.ToTable("HoaDonChiTiet");

            entity.Property(e => e.MaHd)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("maHD");
            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("maSP");
            entity.Property(e => e.NgayBan)
                .HasColumnType("date")
                .HasColumnName("ngayBan");
            entity.Property(e => e.SoLuongMua).HasColumnName("soLuongMua");

            entity.HasOne(d => d.MaSpNavigation).WithMany(p => p.HoaDonChiTiets)
                .HasForeignKey(d => d.MaSp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HoaDonChiTiet_SanPham");
        });

        modelBuilder.Entity<LoaiSp>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiSP__E5A6B22882839FE4");

            entity.ToTable("LoaiSP");

            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("maLoai");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("tenLoai");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.MaSp).HasName("PK__SanPham__7A227A7ADE10BA83");

            entity.ToTable("SanPham");

            entity.Property(e => e.MaSp)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("maSP");
            entity.Property(e => e.DonGia)
                .HasColumnType("money")
                .HasColumnName("donGia");
            entity.Property(e => e.MaLoai)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("maLoai");
            entity.Property(e => e.SoLuongCo).HasColumnName("soLuongCo");
            entity.Property(e => e.TenSp)
                .HasMaxLength(50)
                .HasColumnName("tenSP");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.MaLoai)
                .HasConstraintName("FK__SanPham__maLoai__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
