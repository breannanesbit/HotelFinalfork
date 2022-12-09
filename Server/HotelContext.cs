using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using HotelFinal.Shared;

namespace HotelFinal.Server;

public partial class HotelContext : DbContext
{
    public HotelContext()
    {
    }

    public HotelContext(DbContextOptions<HotelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CleaningType> CleaningTypes { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<RentalPayment> RentalPayments { get; set; }

    public virtual DbSet<RentalRoom> RentalRooms { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<ReservationRoom> ReservationRooms { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomCleaning> RoomCleanings { get; set; }

    public virtual DbSet<RoomCleaningInfo> RoomCleaningInfos { get; set; }

    public virtual DbSet<RoomServiceCharge> RoomServiceCharges { get; set; }

    public virtual DbSet<RoomServiceCharngeItem> RoomServiceCharngeItems { get; set; }

    public virtual DbSet<RoomServiceItem> RoomServiceItems { get; set; }

    public virtual DbSet<RoomType> RoomTypes { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CleaningType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cleaning_type_pkey");

            entity.ToTable("cleaning_type", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('cleaning_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("guest_pkey");

            entity.ToTable("guest", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('guest_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasColumnType("character varying")
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasColumnType("character varying")
                .HasColumnName("last_name");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rental_pkey");

            entity.ToTable("rental", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('rental_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Checkin).HasColumnName("checkin");
            entity.Property(e => e.Checkout).HasColumnName("checkout");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");
            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity.HasOne(d => d.Guest).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_guest_id_fkey");

            entity.HasOne(d => d.Reservation).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.ReservationId)
                .HasConstraintName("rental_reservation_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_staff_id_fkey");
        });

        modelBuilder.Entity<RentalPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rental_payment_pkey");

            entity.ToTable("rental_payment", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('rental_payment_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.PaymentDate).HasColumnName("payment_date");
            entity.Property(e => e.RentalId).HasColumnName("rental_id");

            entity.HasOne(d => d.Rental).WithMany(p => p.RentalPayments)
                .HasForeignKey(d => d.RentalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_payment_rental_id_fkey");
        });

        modelBuilder.Entity<RentalRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("rental_room_pkey");

            entity.ToTable("rental_room", "hotel");

            entity.HasIndex(e => e.RoomCleaningId, "rental_room_room_cleaning_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('rental_room_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.RentalId).HasColumnName("rental_id");
            entity.Property(e => e.RentalRate).HasColumnName("rental_rate");
            entity.Property(e => e.RoomCleaningId).HasColumnName("room_cleaning_id");

            entity.HasOne(d => d.Rental).WithMany(p => p.RentalRooms)
                .HasForeignKey(d => d.RentalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_room_rental_id_fkey");

            entity.HasOne(d => d.RoomCleaning).WithOne(p => p.RentalRoom)
                .HasForeignKey<RentalRoom>(d => d.RoomCleaningId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("rental_room_room_cleaning_id_fkey");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservation_pkey");

            entity.ToTable("reservation", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('reservation_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ExpectedCheckin).HasColumnName("expected_checkin");
            entity.Property(e => e.ExpectedCheckout).HasColumnName("expected_checkout");
            entity.Property(e => e.GuestId).HasColumnName("guest_id");

            entity.HasOne(d => d.Guest).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservation_guest_id_fkey");
        });

        modelBuilder.Entity<ReservationRoom>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("reservation_room_pkey");

            entity.ToTable("reservation_room", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('reservation_room_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ReservationId).HasColumnName("reservation_id");
            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");

            entity.HasOne(d => d.Reservation).WithMany(p => p.ReservationRooms)
                .HasForeignKey(d => d.ReservationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservation_room_reservation_id_fkey");

            entity.HasOne(d => d.RoomType).WithMany(p => p.ReservationRooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservation_room_room_type_id_fkey");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_pkey");

            entity.ToTable("room", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.RoomTypeId).HasColumnName("room_type_id");

            entity.HasOne(d => d.RoomType).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.RoomTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_room_type_id_fkey");
        });

        modelBuilder.Entity<RoomCleaning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_cleaning_pkey");

            entity.ToTable("room_cleaning", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_cleaning_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CleaningTypeId).HasColumnName("cleaning_type_id");
            entity.Property(e => e.DateCleaned).HasColumnName("date_cleaned");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.StaffId).HasColumnName("staff_id");

            entity.HasOne(d => d.CleaningType).WithMany(p => p.RoomCleanings)
                .HasForeignKey(d => d.CleaningTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_cleaning_cleaning_type_id_fkey");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomCleanings)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_cleaning_room_id_fkey");

            entity.HasOne(d => d.Staff).WithMany(p => p.RoomCleanings)
                .HasForeignKey(d => d.StaffId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_cleaning_staff_id_fkey");
        });

        modelBuilder.Entity<RoomCleaningInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("room_cleaning_info", "hotel");

            entity.Property(e => e.DateCleaned).HasColumnName("date_cleaned");
            entity.Property(e => e.RoomId).HasColumnName("room_id");
            entity.Property(e => e.RoomNumber).HasColumnName("room_number");
            entity.Property(e => e.RoomType).HasColumnName("room_type");
            entity.Property(e => e.Staff).HasColumnName("staff");
        });

        modelBuilder.Entity<RoomServiceCharge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_service_charge_pkey");

            entity.ToTable("room_service_charge", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_service_charge_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.Decription)
                .HasColumnType("character varying")
                .HasColumnName("decription");
            entity.Property(e => e.RentalRoomId).HasColumnName("rental_room_id");

            entity.HasOne(d => d.RentalRoom).WithMany(p => p.RoomServiceCharges)
                .HasForeignKey(d => d.RentalRoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_service_charge_rental_room_id_fkey");
        });

        modelBuilder.Entity<RoomServiceCharngeItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_service_charnge_item_pkey");

            entity.ToTable("room_service_charnge_item", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_service_charnge_item_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.ActualCost).HasColumnName("actual_cost");
            entity.Property(e => e.Quanity).HasColumnName("quanity");
            entity.Property(e => e.RoomServiceChargeId).HasColumnName("room_service_charge_id");
            entity.Property(e => e.RoomServiceItemId).HasColumnName("room_service_item_id");

            entity.HasOne(d => d.RoomServiceCharge).WithMany(p => p.RoomServiceCharngeItems)
                .HasForeignKey(d => d.RoomServiceChargeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_service_charnge_item_room_service_charge_id_fkey");

            entity.HasOne(d => d.RoomServiceItem).WithMany(p => p.RoomServiceCharngeItems)
                .HasForeignKey(d => d.RoomServiceItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_service_charnge_item_room_service_item_id_fkey");
        });

        modelBuilder.Entity<RoomServiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_service_item_pkey");

            entity.ToTable("room_service_item", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_service_item_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.CurrentCost).HasColumnName("current_cost");
            entity.Property(e => e.ItemName).HasColumnName("item_name");
        });

        modelBuilder.Entity<RoomType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("room_type_pkey");

            entity.ToTable("room_type", "hotel");

            entity.HasIndex(e => new { e.RType, e.Smoking }, "room_type_r_type_smoking_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('room_type_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.BaseRentalRate).HasColumnName("base_rental_rate");
            entity.Property(e => e.RType).HasColumnName("r_type");
            entity.Property(e => e.Smoking).HasColumnName("smoking");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("staff_pkey");

            entity.ToTable("staff", "hotel");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("nextval('staff_id_seq'::regclass)")
                .HasColumnName("id");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.LastName).HasColumnName("last_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
