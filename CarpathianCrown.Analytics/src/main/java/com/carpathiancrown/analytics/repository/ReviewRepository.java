package com.carpathiancrown.analytics.repository;

import com.carpathiancrown.analytics.model.Review;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;

public interface ReviewRepository extends JpaRepository<Review, Long> {

    @Query(value = "select coalesce(avg(\"Rating\")::float8, 0) from \"Reviews\"", nativeQuery = true)
    double avgRatingNative();

    @Query(value = "select count(*) from \"Reviews\"", nativeQuery = true)
    long totalReviewsNative();
}