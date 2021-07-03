package com.example.localserver.Entity;

import javax.persistence.*;
import java.time.LocalDateTime;

@Entity
@Table(name = "recorded_event")
public class RecordedEvent {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String registryContent;
    private LocalDateTime dateTime;

    @ManyToOne
    @JoinColumn(name="user_id")
    private RegisteredUser registeredUser;

    public RecordedEvent(){}

    public RecordedEvent(String registryContent, LocalDateTime dateTime, RegisteredUser registeredUser) {
        this.registryContent = registryContent;
        this.dateTime = dateTime;
        this.registeredUser = registeredUser;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }

    public LocalDateTime getDateTime() {
        return dateTime;
    }

    public void setDateTime(LocalDateTime dateTime) {
        this.dateTime = dateTime;
    }

    public RegisteredUser getRegisteredUser() {
        return registeredUser;
    }

    public void setRegisteredUser(RegisteredUser registeredUser) {
        this.registeredUser = registeredUser;
    }
}