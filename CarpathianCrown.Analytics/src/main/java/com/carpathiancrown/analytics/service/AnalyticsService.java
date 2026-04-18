package com.carpathiancrown.analytics.service;

import com.carpathiancrown.analytics.repository.BookingRepository;
import com.carpathiancrown.analytics.repository.ReviewRepository;
import org.springframework.stereotype.Service;

import java.math.BigDecimal;

@Service
public class AnalyticsService {

    private final BookingRepository bookingRepository;
    private final ReviewRepository reviewRepository;

    public AnalyticsService(BookingRepository bookingRepository, ReviewRepository reviewRepository) {
        this.bookingRepository = bookingRepository;
        this.reviewRepository = reviewRepository;
    }

    public long totalBookings() {
        return bookingRepository.totalBookingsNative();
    }

    public long pendingBookings() {
        return bookingRepository.countByStatusNative("Pending");
    }

    public long confirmedBookings() {
        return bookingRepository.countByStatusNative("Confirmed");
    }

    public long completedBookings() {
        return bookingRepository.countByStatusNative("Completed");
    }

    public long cancelledBookings() {
        return bookingRepository.countByStatusNative("Cancelled");
    }

    public BigDecimal revenueConfirmedOrCompleted() {
        return bookingRepository.sumGrandTotalByStatusNative("Confirmed")
                .add(bookingRepository.sumGrandTotalByStatusNative("Completed"));
    }

    public BigDecimal confirmedRevenue() {
        return bookingRepository.sumGrandTotalByStatusNative("Confirmed");
    }

    public double avgRating() {
        return reviewRepository.avgRatingNative();
    }
}