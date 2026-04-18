package com.carpathiancrown.analytics.model;

import jakarta.persistence.*;

@Entity
@Table(name = "Reviews", schema = "public")
public class Review {

    @Id
    @Column(name = "Id")
    private Long id;

    @Column(name = "rRating")
    private Integer rating;

    public Long getId() { return id; }
    public Integer getRating() { return rating; }
}