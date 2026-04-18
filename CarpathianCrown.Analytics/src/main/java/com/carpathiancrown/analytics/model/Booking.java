package com.carpathiancrown.analytics.model;

import java.math.BigDecimal;
import java.time.LocalDateTime;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "Bookings", schema = "public")
public class Booking {

    @Id
    @Column(name = "Id")
    private Long id;

    @Column(name = "Status")
    private String status;

    @Column(name = "total_price")
    private BigDecimal totalPrice;

    @Column(name = "check_in")
    private LocalDateTime checkIn;

    @Column(name = "check_out")
    private LocalDateTime checkOut;

    @Column(name = "user_id")
    private Long userId;

    @Column(name = "room_id")
    private Long roomId;

    public Long getId() { return id; }
    public Integer getRating() { return getRating(); }
    public String getStatus() { return status; }
    public BigDecimal getTotalPrice() { return totalPrice; }
    public LocalDateTime getCheckIn() { return checkIn; }
    public LocalDateTime getCheckOut() { return checkOut; }
    public Long getUserId() { return userId; }
    public Long getRoomId() { return roomId; }
    public Object getTotaPrice() {
        // TODO Auto-generated method stub
        throw new UnsupportedOperationException("Unimplemented method 'getGrandTotal'");
    }
}