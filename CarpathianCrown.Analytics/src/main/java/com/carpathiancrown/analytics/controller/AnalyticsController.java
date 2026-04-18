package com.carpathiancrown.analytics.controller;

import com.carpathiancrown.analytics.service.AnalyticsService;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.util.Map;

@RestController
@RequestMapping("/api/analytics")
public class AnalyticsController {

    private final AnalyticsService analyticsService;

    public AnalyticsController(AnalyticsService analyticsService) {
        this.analyticsService = analyticsService;
    }

    @GetMapping("/summary")
    public Map<String, Object> summary() {
        return Map.of(
                "javaRevenueConfirmedOrCompleted", analyticsService.revenueConfirmedOrCompleted(),
                "javaTotalBookings", analyticsService.totalBookings(),
                "javaAvgRating", analyticsService.avgRating()
        );
    }

    @GetMapping("/dashboard")
    public Map<String, Object> dashboard() {
        return Map.of(
                "javaTotalBookings", analyticsService.totalBookings(),
                "javaPendingBookings", analyticsService.pendingBookings(),
                "javaConfirmedBookings", analyticsService.confirmedBookings(),
                "javaCompletedBookings", analyticsService.completedBookings(),
                "javaCancelledBookings", analyticsService.cancelledBookings(),
                "javaRevenueConfirmedOrCompleted", analyticsService.revenueConfirmedOrCompleted(),
                "javaAvgRating", analyticsService.avgRating()
        );
    }
}