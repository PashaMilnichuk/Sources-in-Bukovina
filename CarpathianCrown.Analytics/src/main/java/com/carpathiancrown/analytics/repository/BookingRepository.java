package com.carpathiancrown.analytics.repository;

import com.carpathiancrown.analytics.model.Booking;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.math.BigDecimal;

public interface BookingRepository extends JpaRepository<Booking, Long> {

    @Query(value = "select count(*) from \"Bookings\"", nativeQuery = true)
    long totalBookingsNative();

    @Query(value = "select count(*) from \"Bookings\" where \"Status\" = :status", nativeQuery = true)
    long countByStatusNative(@Param("status") String status);

    @Query(value = "select coalesce(sum(\"GrandTotal\"), 0) from \"Bookings\" where \"Status\" = :status", nativeQuery = true)
    BigDecimal sumGrandTotalByStatusNative(@Param("status") String status);
}